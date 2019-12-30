using System;
using System.Security;
using System.IO.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using D = DocumentFormat.OpenXml.Drawing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using V = DocumentFormat.OpenXml.Vml;
using System.Drawing;
using System.IO;

namespace LessonParser
{
    class Answer
    {
        public string Key
        {
            get; set;
        }
        
        public string Value
        {
            get; set;
        }
    }
    class XParseChallenge
    {
        // ooxml util
        static XUtil XUtil = new XUtil();
        // xyleme xml template object
        V4LessonTemplate V4Template = new V4LessonTemplate();

        // should be accessible from any function
        //Documents' numbering definition
        Numbering Numbering = new Numbering();
        // styles
        Styles Styles = new Styles();
        // HyperlinkRelationships
        IEnumerable<HyperlinkRelationship> HyperlinkRelationships = new List<HyperlinkRelationship>();
        // file locations
        string SourceDirectory = Directory.GetCurrentDirectory() + @"\";

        public string OpenDocument(string documentFile)
        {
            // xyleme xml template object
            V4Template.CourseCode = "CIS";
            V4Template.ModuleCode = "M04";
            V4Template.LessonCode = "L04";

            string MediaDirectory = SourceDirectory + @"media\";


            // Open the presentation as read-only.
            using (WordprocessingDocument WordDocument = WordprocessingDocument.Open(documentFile, false))
            {
                // Create the Document DOM.
                // Add a new main document part. 
                MainDocumentPart MainDocumentPart = WordDocument.MainDocumentPart;
                //Documents' numbering definition
                Numbering = MainDocumentPart.NumberingDefinitionsPart.Numbering;
                // styles
                Styles = MainDocumentPart.StyleDefinitionsPart.Styles;
                //HyperlinkRelationship
                HyperlinkRelationships = MainDocumentPart.HyperlinkRelationships;

                // root element part of the doc
                Document Document = MainDocumentPart.Document;

                // actual document body
                Body Body = Document.Body;

                // create list to store paragraphs
                List<string> paragraphs = new List<string>();
                List<string> Questions = new List<string>();
                List<Answer> Answers = new List<Answer>();
                List<string> Feedback = new List<string>();
                List<string> Options = new List<string>();

                string[] Correct = {};
                var i = 0;
                var Elements = Body.ChildElements;
                int count = Elements.Count();

                // loop
                for (var x = count - 1; x >= 0; x--)
                {
                    var Element = Elements[x];
                    if (Element is Paragraph)
                    {
                        // create paragraphs
                        Paragraph Paragraph = (Paragraph)Element;
                        var ListFormat = new Level();

                        // save every paragraph
                        paragraphs.Add(applyFormatting(Paragraph));
                        
                        // get list info
                        if(Paragraph.ParagraphProperties != null)
                        {
                            // style
                            ParagraphStyleId ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                            ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);
                        }
                        
                        // detect questions
                        if(XUtil.isQuestion(ListFormat))
                        {
                            // get list of answers for question
                            Answers = GetAnswers(x + 1, x + paragraphs.Count, Elements);
                            foreach(var Answer in Answers)
                            {
                                if (Correct.Contains(Answer.Key))
                                {
                                    // add option as correct
                                    Options.Add(V4Template.Option("true", SecurityElement.Escape(Answer.Value)));
                                }
                                else
                                {
                                    // add option as incorrect
                                    Options.Add(V4Template.Option("false", SecurityElement.Escape(Answer.Value)));
                                }
                            }

                            // convert options to string
                            var options = string.Join("", Options);

                            // store question xml
                            Questions.Add(V4Template.MultipleChoice(V4Template.QuestionStem(SecurityElement.Escape(Paragraph.InnerText)), options, V4Template.Feedback(Feedback.FirstOrDefault())));

                            // reset values
                            Options.Clear();
                            Correct = new string[]{};
                            Answers.Clear();
                            Feedback.Clear();
                            paragraphs.Clear();
                        }

                        // detect negative feedback
                        if (XUtil.isFeedback(Paragraph.InnerText))
                        {
                            // remove the current paragraph with unwanted text
                            paragraphs.Remove(paragraphs.Last());
                            paragraphs.Reverse();
                            // save feedback text
                            string feedback = string.Join(Environment.NewLine, paragraphs);
                            Feedback.Add(feedback);

                            // reset paragraph numbering
                            paragraphs.Clear();
                        }
                        
                        // detect correct answers
                        if (XUtil.isCorrect(Paragraph.InnerText))
                        {
                            // get an array with the correct answers
                            Correct = GetCorrect(Paragraph.InnerText);
                            
                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }
                    }
                }
                
                Questions.Reverse();
                var txt = V4Template.QuestionBlock(string.Join("", Questions.ToArray()));
                string challenge = V4Template.SelfCheckTopic(txt);
                //File.WriteAllText("ques.xml",txt);
                /*foreach (var Question in Questions)
                {
                    //i++;
                    
                    Console.WriteLine(Question);
                }*/
                //Console.WriteLine(i);
                Console.WriteLine("challenge parsed");
                return challenge;
            }

        }

        public List<Answer> GetAnswers(int start, int stop, OpenXmlElementList Elements)
        {
            
            // global answer list
            List<Answer> Answers = new List<Answer>();

            // custom answer key, since actual letters not accessible
            int i = 0;
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();

            for (var x = start; x <= stop; x++)
            {
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    var ListFormat = new Level();

                    //Console.WriteLine(Paragraph.InnerText);
                    if (Paragraph.ParagraphProperties != null)
                    {
                        // style
                        ParagraphStyleId ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                        ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);
                    }
                    if (XUtil.isAnswer(ListFormat))
                    {
                        // save answers
                        Answer Answer = new Answer();
                        Answer.Key = alpha[i].ToString();
                        Answer.Value = Paragraph.InnerText;
                        Answers.Add(Answer);

                        // counter to track letter position
                        i++;
                    }
                }
            }

            return Answers;
        }

        public string[] GetCorrect(string text)
        {
            var array = text.Replace(" ", "").Split(':');
            var values = array.Last();
            array = values.Split(',');

            return array;
        }
        
        // apply text formatting...bold, underline, italics, code, etc
        public string applyFormatting(Paragraph Paragraph)
        {
            // var
            string Para = "";

            // loop through items
            foreach (var Element in Paragraph.ChildElements)
            {
                if (Element is Run)
                {
                    Run Run = (Run)Element;
                    string text = SecurityElement.Escape(Run.InnerText);

                    // identify code
                    if (Run.RunProperties != null && Run.RunProperties.RunFonts != null && Run.RunProperties.RunFonts.Ascii != null && Run.RunProperties.RunFonts.Ascii.Value == "Courier New")
                    {
                        text = V4Template.InLineCode(text);
                    }
                    else if (Run.RunProperties != null && text.Trim() != "") // prevent empty tags, should do this above but...
                    {
                        // Make text italic
                        if (Run.RunProperties.Italic != null)
                        {
                            text = V4Template.Italic(text);
                        }
                        // Make text bold
                        if (Run.RunProperties.Bold != null)
                        {
                            text = V4Template.Bold(text);
                        }
                        // Make text underlined
                        if (Run.RunProperties.Underline != null)
                        {
                            text = V4Template.Underline(text);
                        }

                        // superscript / subscript 
                        if (Run.RunProperties.VerticalTextAlignment != null && Run.RunProperties.VerticalTextAlignment.Val == VerticalPositionValues.Superscript)
                        {
                            text = V4Template.Superscript(text);
                        }
                        else if (Run.RunProperties.VerticalTextAlignment != null && Run.RunProperties.VerticalTextAlignment.Val == VerticalPositionValues.Subscript)
                        {
                            text = V4Template.Subscript(text);
                        }
                    }

                    // append run to paragraph
                    Para += text;
                }
                else if (Element is Hyperlink)
                {
                    // handle links
                    Hyperlink Hyperlink = (Hyperlink)Element;
                    if (Hyperlink.Id != null)
                    {
                        var link = HyperlinkRelationships.FirstOrDefault(Tag => Tag.Id == Hyperlink.Id);
                        Para += V4Template.Hyperlink(SecurityElement.Escape(link.Uri.AbsoluteUri), SecurityElement.Escape(Hyperlink.InnerText));
                    }
                }
            }

            return Para;
        }

        public NumberingLevelReference GetIndentLevel(NumberingProperties NumberingProperties, Style ParagraphStyle)
        {
            if (NumberingProperties != null && NumberingProperties.NumberingId != null)
            {
                return NumberingProperties.NumberingLevelReference;
            }
            else if (ParagraphStyle != null && ParagraphStyle.StyleParagraphProperties != null && ParagraphStyle.StyleParagraphProperties.NumberingProperties != null && ParagraphStyle.StyleParagraphProperties.NumberingProperties.NumberingId != null)
            {
                return ParagraphStyle.StyleParagraphProperties.NumberingProperties.NumberingLevelReference;
            }

            return null;
        }

        public NumberingId GetNumberingId(NumberingProperties NumberingProperties, Style ParagraphStyle)
        {
            if (NumberingProperties != null && NumberingProperties.NumberingId != null)
            {
                return NumberingProperties.NumberingId;
            }
            else if (ParagraphStyle != null && ParagraphStyle.StyleParagraphProperties != null && ParagraphStyle.StyleParagraphProperties.NumberingProperties != null && ParagraphStyle.StyleParagraphProperties.NumberingProperties.NumberingId != null)
            {
                return ParagraphStyle.StyleParagraphProperties.NumberingProperties.NumberingId;
            }

            return null;
        }

        public Level GetListFormat(Paragraph Paragraph, Styles Styles, Numbering Numbering, ParagraphStyleId ParagraphStyleId)
        {
            NumberingProperties NumberingProperties = Paragraph.ParagraphProperties.NumberingProperties;

            // get paragraph style
            Style ParagraphStyle = new Style();
            if (ParagraphStyleId != null)
            {
                ParagraphStyle = Styles.Descendants<Style>().FirstOrDefault(tag => tag.StyleId == (string)ParagraphStyleId.Val);
            }

            NumberingId NumberingId = GetNumberingId(NumberingProperties, ParagraphStyle);
            NumberingLevelReference IndentLevel = GetIndentLevel(NumberingProperties, ParagraphStyle);
            //Console.WriteLine(Paragraph.InnerText);

            var ListData = GetListData(Numbering, NumberingId, IndentLevel);
            /*if (ListData != null)
            {
                txt += ListData.LevelText.Val + " " + Paragraph.InnerText + Environment.NewLine;
                //Console.WriteLine(ListData.LevelText.Val + " "+ Paragraph.InnerText);
            }*/
            return ListData;
        }

        public Level GetListData(Numbering Numbering, NumberingId NumberingId, NumberingLevelReference IndentLevel)
        {

            if (NumberingId != null)
            {
                // use numbering instance and abstract number to get list data
                NumberingInstance NumberingInstance = Numbering.Descendants<NumberingInstance>().FirstOrDefault(tag => tag.NumberID == (int)NumberingId.Val);
                if (NumberingInstance != null)
                {
                    var AbstractNumId = NumberingInstance.AbstractNumId;
                    AbstractNum AbstractNum = Numbering.Descendants<AbstractNum>().FirstOrDefault(tag => tag.AbstractNumberId == (int)AbstractNumId.Val);

                    if(IndentLevel != null)
                    {
                        Level Level = AbstractNum.Descendants<Level>().FirstOrDefault(Tag => Tag.LevelIndex == (int)IndentLevel.Val);
                        if (Level != null)
                        {
                            return Level;
                        }
                    }
                    else
                    {
                        Level Level = AbstractNum.Descendants<Level>().FirstOrDefault();
                        if (Level != null)
                        {
                            return Level;
                        }
                    }
                    
                }
            }

            return null;
        }
    }
}
