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

    class XParseLessonV4
    {
        //
        int fromSegue = 0;
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
        public string SourceDirectory = "";

        public MainDocumentPart OpenDocument(string documentFile)
        {
            // xyleme xml template object
            V4Template.CourseCode = "CIS";
            V4Template.ModuleCode = "M04";
            V4Template.LessonCode = "L04";
            
            string MediaDirectory = SourceDirectory + @"media\";

            OpenSettings OpenSettings = new OpenSettings();
            //OpenSettings.

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
                List<string> SubTopics = new List<string>();
                List<string> Topics = new List<string>();
                List<string> ListOrphan = new List<string>();

                string txt = "";
                //Console.WriteLine(Body.ChildElements.Count);
                var i = 0;
                var Elements = Body.ChildElements;
                int count = Elements.Count();
                int imgCount = 0;
                // check imgs in doc
                for (var x = count - 1; x >= 0; x--)
                {
                    var Element = Elements[x];

                    if (Element is Paragraph) {

                        Paragraph Paragraph = (Paragraph)Element;
                        if (XUtil.isSlideObject(Paragraph)) {
                            imgCount++;
                        }
                    }
                }

                for (var x = count - 1; x >= 0; x--)
                {
                    var Element = Elements[x];
                    //Console.WriteLine(Element);
                    
                    if (Element is Paragraph)
                    {
                        // create paragraphs
                        Paragraph Paragraph = (Paragraph)Element;
                        Paragraph prevParagraph = new Paragraph();
                        ParagraphStyleId prevParagraphStyleId = new ParagraphStyleId();
                        

                        if (x < count && Elements[x + 1] is Paragraph)
                        {
                            prevParagraph = (Paragraph)Elements[x + 1];
                            prevParagraphStyleId = prevParagraph.ParagraphProperties.ParagraphStyleId;
                        }

                        // style
                        //ParagraphStyleId ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                        //Paragraph.
                        //ParagraphProperties ParagraphProperties = Paragraph.ParagraphProperties

                        
                        // save every paragraph
                        paragraphs.Add(SecurityElement.Escape(Paragraph.InnerText));
                        //Console.WriteLine(Paragraph.InnerText);
                        //var ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);

                        // detect subtopic
                        if (XUtil.isV4Slide(Paragraph))
                        {
                            
                            fromSegue = Math.Abs(imgCount - 1);
                            imgCount--;

                            string title = XUtil.GetV4_1SlideTitle(Paragraph);
                            Console.WriteLine(title);
                            Console.WriteLine("=================================================================== " + x + "+" + paragraphs.Count);
                            // x + 1 is important to ignore heading in count 
                            string content = GetSubTopicV4(x + paragraphs.Count - 2, x, Elements, title);
                            //Console.WriteLine("com: "+content);
                            paragraphs.Clear();
                            //string content = V4Template.RichText(Slide.Intro) + V4Template.RichText(Slide.Notes);
                            //ignore closing slides
                            if (!XUtil.isV4SlideType(Paragraph, MainDocumentPart, "closing"))
                            {
                                SubTopics.Add(V4Template.TitledBlock(SecurityElement.Escape(title), content));
                            }
                            
                            
                        }
                        
                        // detect subtopic
                        /*if (XUtil.isSubTopic(Paragraph))
                        {
                            string title = XUtil.GetSlideTitle(Paragraph);
                            Console.WriteLine(title);
                            Console.WriteLine("=================================================================== " + x + "+" + paragraphs.Count);
                            // x + 1 is important to ignore heading in count 
                            string content = GetSubTopic(x + paragraphs.Count - 2, x, Elements);
                            paragraphs.Clear();
                            //string content = V4Template.RichText(Slide.Intro) + V4Template.RichText(Slide.Notes);
                            SubTopics.Add(V4Template.TitledBlock(title, content));
                        }*/
                        
                        // detect topic
                        if (XUtil.isTopicV4(Paragraph, MainDocumentPart))
                        {
                            paragraphs.Reverse();
                            SubTopics.Reverse();
                            string title = XUtil.GetV4_1SlideTitle(Paragraph);
                            Console.WriteLine(title);
                            Console.WriteLine("///////////////////////////////////////////////////////////////////// " + x + "+" + paragraphs.Count);
                            string content = string.Join("", SubTopics.ToArray());
                            // x + 1 is important to ignore heading in count 
                            // count should be 1 less since the array starts at 0
                            var Topic = GetV4Topic(x + paragraphs.Count - 1, x, Elements, content, title);
                            SubTopics.Clear();

                            // if topic and lesson slide are the same then dont clear
                            
                            //paragraphs.Clear();
                            
                            
                            
                            Topics.Add(Topic);
                        }

                        // detect lesson
                        if (XUtil.isLessonV4(Paragraph, MainDocumentPart))
                        {
                            paragraphs.Reverse();
                            Topics.Reverse();
                            string title = XUtil.GetV4SlideTitle(Paragraph, MainDocumentPart);
                            Console.WriteLine(title);
                            Console.WriteLine("********************************************************************** " + x + "+" + paragraphs.Count);
                            string content = string.Join("", Topics.ToArray());
                            string challenge = "";

                            try
                            {
                                // get challenge ques
                                var XParse = new XParseChallenge();
                                challenge = XParse.OpenDocument(SourceDirectory + @"challenge.docx");
                            }
                            catch (Exception ex)
                            {
                                challenge = "";
                                Console.WriteLine("No challenge.docx file found. Skipping challenge.");
                            }

                            // x + 1 is important to ignore heading in count 
                            // count should be 1 less since the array starts at 0
                            var Lesson = GetSection(x + paragraphs.Count - 1, x, Elements, content + challenge, title);
                            Topics.Clear();
                            paragraphs.Clear();

                            try
                            {
                                //Topics.Add(Topic);
                                File.WriteAllText(SourceDirectory + "lesson.xml", Lesson);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        
                        //Console.WriteLine(Paragraph.InnerText);
                        
                    }
                    
                }

                //Topics.Reverse();
                //txt = V4Template.Lesson("ttl", string.Join("", Topics.ToArray()), "obj");
                //File.WriteAllText("text.txt",txt);

                /*foreach (Paragraph Paragraph in Body.Descendants<Paragraph>())
                {
                    i++;
                    
                    //Console.WriteLine(Paragraph.InnerText);
                }*/
                //Console.WriteLine(i);
                Console.WriteLine("done");
                return MainDocumentPart;
            }
            
        }

        public string GetLesson(int start, int stop, OpenXmlElementList Elements, string topics)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Name = "";
            string Objective = "";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);
                    //Console.WriteLine(Paragraph.InnerText);
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(string.Join("", paragraphs.ToArray()));
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isObjective(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(Paragraph.InnerText);
                        Objective = GetObjective(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isName(Paragraph.InnerText))
                    {
                        //Console.WriteLine(paragraphs.Count);
                        //paragraphs.Reverse();
                        //Console.WriteLine(Paragraph.InnerText);
                        paragraphs.Remove(paragraphs.Last());
                        Name = GetName(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                        //
                    }
                }
            }

            string LessonSlide;
            if (Intro == "") { LessonSlide = V4Template.LessonSlide(Name); } else { LessonSlide = V4Template.LessonSlide(Name, Intro); }

            return V4Template.Lesson(Name, LessonSlide + topics, Objective);
        }

        public string GetSection(int start, int stop, OpenXmlElementList Elements, string topics, string title)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Name = "";
            string Objective = "";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);
                    //Console.WriteLine(Paragraph.InnerText);
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(string.Join("", paragraphs.ToArray()));
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isObjective(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(Paragraph.InnerText);
                        Objective = GetObjective(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isName(Paragraph.InnerText))
                    {
                        //Console.WriteLine(paragraphs.Count);
                        //paragraphs.Reverse();
                        //Console.WriteLine(Paragraph.InnerText);
                        paragraphs.Remove(paragraphs.Last());
                        Name = GetName(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                        //
                    }
                }
            }
            //Console.WriteLine(title);
            string LessonSlide;
            if (Intro == "") { LessonSlide = V4Template.LessonSlide(Name); } else { LessonSlide = V4Template.LessonSlide(Name, Intro); }

            return V4Template.Lesson(Name, LessonSlide + topics, Objective);
        }

        public string GetTopic(int start, int stop, OpenXmlElementList Elements, string subtopics)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Name = "";
            string Objective = "";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);
                    //Console.WriteLine(Paragraph.InnerText);
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(string.Join("", paragraphs.ToArray()));
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isObjective(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(Paragraph.InnerText);
                        Objective = GetObjective(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isName(Paragraph.InnerText))
                    {
                        //Console.WriteLine(paragraphs.Count);
                        //paragraphs.Reverse();
                        //Console.WriteLine(Paragraph.InnerText);
                        paragraphs.Remove(paragraphs.Last());
                        Name = GetName(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                        //
                    }
                }
            }

            // create segue slide xml
            string SegueSlide;
            if (Intro == "") { SegueSlide = V4Template.Segue(Name);  } else { SegueSlide = V4Template.Segue(Name, Intro); }
            
            // return appended xml
            return V4Template.Topic(Name, SegueSlide + subtopics, Objective);
        }
        public string GetV4Topic(int start, int stop, OpenXmlElementList Elements, string subtopics, string title)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Name = "";
            string Objective = "";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);
                    //Console.WriteLine(Paragraph.InnerText);
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(string.Join("", paragraphs.ToArray()));
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isObjective(Paragraph.InnerText))
                    {
                        //paragraphs.Reverse();
                        paragraphs.Remove(paragraphs.Last());
                        //Console.WriteLine(Paragraph.InnerText);
                        Objective = GetObjective(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    if (XUtil.isName(Paragraph.InnerText))
                    {
                        //Console.WriteLine(paragraphs.Count);
                        //paragraphs.Reverse();
                        //Console.WriteLine(Paragraph.InnerText);
                        paragraphs.Remove(paragraphs.Last());
                        Name = GetName(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                        //
                    }
                }
            }

            // create segue slide xml
            string SegueSlide;
            if (Intro == "") { SegueSlide = V4Template.Segue(Name); } else { SegueSlide = V4Template.Segue(Name, Intro); }

            // return appended xml
            return V4Template.Topic(Name, SegueSlide + subtopics, Objective);
        }

        public string GetSubTopic(int start, int stop, OpenXmlElementList Elements)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Notes = "";
            string SlideXml = "";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {
                
                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);

                    // detect and handle slide images
                    if (XUtil.isImage(Paragraph))
                    {
                        var SlideId = XUtil.GetSlideId(Paragraph, "Slide");
                        Console.WriteLine("sid: " + SlideId);
                        SlideXml = XUtil.GetSlideXML(XmlDirectory, SlideId);
                    }

                    // detect and handle intro statements
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        paragraphs.Remove(paragraphs.Last());
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }

                    // detect and handle notes
                    if (XUtil.hasNotesV4(Paragraph.InnerText, Paragraph))
                    {
                        paragraphs.Remove(paragraphs.Last());
                        Notes = GetNotes(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                        //
                    }
                }
                    
                
            }

            // return appended xml
                return Intro + SlideXml + Notes;
        }

        public string GetSubTopicV4(int start, int stop, OpenXmlElementList Elements, string title)
        {
            List<string> paragraphs = new List<string>();
            string Intro = "";
            string Notes = "";
            string SlideXml = "<Slide><Title></Title><Body></Body><FilterMetadata><TargetAudiences><TargetAudience>v4 PPT ILT</TargetAudience><TargetAudience>v4 PPT Lab</TargetAudience></TargetAudiences></FilterMetadata></Slide>";
            string XmlDirectory = SourceDirectory + @"xml\";

            for (var x = start; x >= stop; x--)
            {

                if (Elements[x] is Paragraph)
                {
                    var Paragraph = (Paragraph)Elements[x];
                    paragraphs.Add(Paragraph.InnerText);

                    // detect and handle slide images
                    if (XUtil.isImage(Paragraph))
                    {
                        var sId = XUtil.GetSlideId(Paragraph, "Slide");
                        var SlideId = XUtil.MD5(sId);
                        //var SlideId = XUtil.MD5(fromSegue + title);
                        //Console.WriteLine("from segue: " + fromSegue);
                        Console.WriteLine("title: " + title);
                        Console.WriteLine("sid: " + SlideId);
                        SlideXml = XUtil.GetSlideXML(XmlDirectory, SlideId);
                    }

                    // detect and handle intro statements
                    if (XUtil.isIntro(Paragraph.InnerText))
                    {
                        paragraphs.Remove(paragraphs.Last());
                        Intro = GetIntro(x + paragraphs.Count, x + 1, Elements);
                        paragraphs.Clear();
                    }
                    
                    // detect and handle notes with "notes:" marker
                    if (XUtil.hasNotes(Paragraph.InnerText))
                    {
                        paragraphs.Remove(paragraphs.Last());
                        Notes = GetNotes(x + paragraphs.Count, x, Elements);
                        paragraphs.Clear();
                        //
                    }// else
                    else if (XUtil.hasNotesV4(Paragraph.InnerText, Paragraph))
                    {
                        paragraphs.Remove(paragraphs.Last());
                        Notes = GetNotes(x + paragraphs.Count, x, Elements);
                        paragraphs.Clear();
                        //
                    }
                }


            }

            // return appended xml
            return Intro + SlideXml + Notes;
        }

        public string GetIntro(int start, int stop, OpenXmlElementList Elements)
        {
            string intro = "";
            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var p = (Paragraph)Elements[x];
                    var FormattedText = applyFormatting(p).Trim();
                    intro = V4Template.RichText(FormattedText);
                    //Console.WriteLine(p.InnerText);
                }                    
            }
            return intro;
        }

        public string GetObjective(int start, int stop, OpenXmlElementList Elements)
        {
            List<string> Objectives = new List<string>();
            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var p = (Paragraph)Elements[x];
                    var FormattedText = applyFormatting(p).Trim();
                    if (p.InnerText.TrimEnd() != "" && p.InnerText.TrimEnd().Last() != ':')
                    {
                        Objectives.Add(FormattedText);
                    }
                }
            }
            //Console.WriteLine(Objective);
            Objectives.Reverse();
            return string.Join("", Objectives.ToArray());
        }

        public string GetName(int start, int stop, OpenXmlElementList Elements)
        {
            string Name = "";
            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var p = (Paragraph)Elements[x];
                    var FormattedText = applyFormatting(p).Trim();
                    Name = FormattedText;
                    //Console.WriteLine(p.InnerText);
                }
            }
            return Name;
        }

        public string GetNotes(int start, int stop, OpenXmlElementList Elements)
        {
            List<string> paragraphs = new List<string>();
            List<string> notes = new List<string>();

            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    //
                    var Paragraph = (Paragraph)Elements[x];
                    var FormattedText = applyFormatting(Paragraph).Trim();

                    Paragraph prevParagraph = new Paragraph();
                    ParagraphStyleId prevParagraphStyleId = new ParagraphStyleId();
                    
                    //
                    if (x < stop && Elements[x + 1] is Paragraph)
                    {
                        prevParagraph = (Paragraph)Elements[x + 1];
                        prevParagraphStyleId = prevParagraph.ParagraphProperties.ParagraphStyleId;
                    }
                    
                    // check if paragraph is subtopic
                    ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
                    if (Paragraph.ParagraphProperties != null)
                    {
                        ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                    }

                    // differentiate between list and richtext and log list items
                    var ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);
                    if (ListFormat != null)
                    {
                        // log paragraph to track number of list items
                        paragraphs.Add("list item");
                        var prevListFormat = new Level();

                        //
                        if (prevParagraph != null && prevParagraph.ParagraphProperties != null)
                        {
                            prevListFormat = GetListFormat(prevParagraph, Styles, Numbering, prevParagraphStyleId);
                            if (prevListFormat == null)
                            {
                                //paragraphs.Add(SecurityElement.Escape(Paragraph.InnerText));
                                //paragraphs.Clear();
                                
                            }
                        }
                        
                    }
                    else
                    {
                        
                        //
                        var prevListFormat = new Level();
                        if (prevParagraph != null && prevParagraph.ParagraphProperties != null)
                        {
                            prevListFormat = GetListFormat(prevParagraph, Styles, Numbering, prevParagraphStyleId);
                        }

                        if (prevListFormat != null)
                        {
                            // reverse list                            
                            paragraphs.Reverse();
                            var list = GetList(x + paragraphs.Count, x + 1, Elements);
                            paragraphs.Clear();

                            // identify preamble
                            if(Paragraph.InnerText.Trim() != "" && Paragraph.InnerText.TrimEnd().Last() == ':')
                            {
                                if (list != "")
                                {
                                    notes.Add(V4Template.List(list, FormattedText));
                                }
                            }
                            else
                            {
                                if (list != "")
                                {
                                    notes.Add(V4Template.List(list));
                                }
                                notes.Add(V4Template.RichText(FormattedText));
                            }                                                       
                            
                        }
                        else
                        {
                            //
                            notes.Add(V4Template.RichText(FormattedText));
                            paragraphs.Clear();
                        }
                        
                    }
                }

            }
            notes.Reverse();
            var data = string.Join("", notes.ToArray());
            return data;
        }

        public string GetList(int start, int stop, OpenXmlElementList Elements)
        {
            List<string> paragraphs = new List<string>();
            List<string> ListOrphans = new List<string>();
            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    // variables
                    var Paragraph = (Paragraph)Elements[x];
                    var FormattedText = applyFormatting(Paragraph).Trim();
                    //
                    Paragraph prevParagraph = new Paragraph();
                    // style
                    ParagraphStyleId ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                    ParagraphStyleId prevParagraphStyleId = new ParagraphStyleId();

                    // gets list info, null if not list
                    var ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);
                    var prevListFormat = new Level();
                    
                    //
                    if(x == start)
                    {
                        // save list item
                        paragraphs.Add(V4Template.ListItem(FormattedText));
                        //Console.WriteLine(V4Template.ListItem(Paragraph.InnerText) + Environment.NewLine);               
                        
                    }
                    else
                    {
                        if (Elements[x + 1] is Paragraph)
                        {
                            prevParagraph = (Paragraph)Elements[x + 1];
                            prevParagraphStyleId = prevParagraph.ParagraphProperties.ParagraphStyleId;
                            if (prevParagraph != null && prevParagraph.ParagraphProperties != null)
                            {
                                prevListFormat = GetListFormat(prevParagraph, Styles, Numbering, prevParagraphStyleId);
                            }
                        }
                        
                        if (prevListFormat != null && prevListFormat.LevelIndex != null && ListFormat.LevelIndex < prevListFormat.LevelIndex)
                        {
                            // assign sublist marker based orphan count (a reflection of sublist number)
                            var ListMarker = ListOrphans.Count % 2 == 0 ? "Bullet" : "nDash";
                            // restore order before conversion to string
                            paragraphs.Reverse();
                            var items = string.Join("", paragraphs.ToArray());
                            // create sublist
                            var sublist = V4Template.SubList(FormattedText, items, ListMarker);
                            // reset paragraphs to prevent duplicates
                            paragraphs.Clear();
                            // save sublist
                            paragraphs.Add(sublist);

                            // check if orphans are present
                            if(ListOrphans.Count > 0)
                            {
                                // add orphans to end of sublist
                                paragraphs.Add(ListOrphans.Last());
                                ListOrphans.Remove(ListOrphans.Last());
                                // reverse again to maintain flow
                                paragraphs.Reverse();
                                // reset orphan list
                                //ListOrphans = new List<string>();
                            }
                        }
                        else if (prevListFormat != null && prevListFormat.LevelIndex != null && ListFormat.LevelIndex > prevListFormat.LevelIndex)
                        {
                            // restore order before conversion to string
                            paragraphs.Reverse();
                            var items = string.Join("", paragraphs.ToArray());

                            // add to orphan list for later use
                            ListOrphans.Add(items);

                            // reset paragraphs to prevent duplicates
                            paragraphs.Clear();

                            // save list item
                            paragraphs.Add(V4Template.ListItem(FormattedText));
                        }
                        else
                        {
                            // save list item
                            paragraphs.Add(V4Template.ListItem(FormattedText));
                        }
                    }
                }

            }

            // restore normal paragraph order
            paragraphs.Reverse();
            var ListItems = string.Join("", paragraphs.ToArray());
            //Console.WriteLine(string.Join("", paragraphs.ToArray()) + Environment.NewLine);
            return ListItems;
        }

        // apply text formatting...bold, underline, italics, code, etc
        public string applyFormatting(Paragraph Paragraph)
        {
            // var
            string Para = "";

            // loop through items
            foreach (var Element in Paragraph.ChildElements)
            {
                if(Element is Run)
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
            NumberingProperties NumberingProperties = new NumberingProperties();
            if(Paragraph.ParagraphProperties != null)
            {
                NumberingProperties = Paragraph.ParagraphProperties.NumberingProperties;
            }          

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
                if(NumberingInstance != null)
                {
                    var AbstractNumId = NumberingInstance.AbstractNumId;
                    AbstractNum AbstractNum = Numbering.Descendants<AbstractNum>().FirstOrDefault(tag => tag.AbstractNumberId == (int)AbstractNumId.Val);
                    Level Level = AbstractNum.Descendants<Level>().FirstOrDefault(Tag => Tag.LevelIndex == (int)IndentLevel.Val);
                    if (Level != null)
                    {
                        return Level;
                    }
                }                                
            }

            return null;
        }
    }
}
