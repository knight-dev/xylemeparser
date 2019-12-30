using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;

namespace LessonParser
{
    class NotesFrame
    {
        public List<String> Name
        {
            get; set;
        }

        public List<String> Intro
        {
            get; set;
        }

        public List<String> Notes
        {
            get; set;
        }

        public List<String> Objectives
        {
            get; set;
        }
    }
    class WordUtil
    {
        public Word.Application CreateApplication()
        {
            try
            {
                Word.Application WordApp = new Word.Application();

                return WordApp;
                //Console.WriteLine("done");

            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        public Word.Document CreateDocument(Word.Application WordApp)
        {
            try
            {
                WordApp.Visible = true;
                Word.Document WordDoc = WordApp.Documents.Add();
                WordDoc.Activate();

                return WordDoc;
                //Console.WriteLine("done");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        public Word.Document OpenDocument(Word.Application WordApp, String url)
        {
            try
            {

                Word.Document WordDoc = WordApp.Documents.Open(url, ReadOnly: false, Visible: true);
                WordDoc.Activate();

                return WordDoc;
                //Console.WriteLine("done");

            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        public void SaveDocument(Word.Application WordApp)
        {
            try
            {
                WordApp.Documents.Save(NoPrompt: true, OriginalFormat: true);
                //WordApp.Documents.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        public static string getSelfCheckXML(Word.Document qDoc)
        {
            bool isFeedBack = false;
            string feedback = "";
            string SelfCheckXML = "";
            var ListString = "";
            var V4Template = new V4LessonTemplate();

            // prepare document for easy traversal
            foreach (Word.Paragraph Paragraph in qDoc.Paragraphs)
            {

                // questions
                if (Paragraph.Range.ListFormat.ListString.Length > 0 && Regex.IsMatch(Paragraph.Range.ListFormat.ListString, "\\d.", RegexOptions.IgnoreCase))
                {
                    // add section
                    qDoc.Sections.Add(Paragraph.Range);
                    ListString = Paragraph.Range.ListFormat.ListString;
                }
                // correct answer(s)
                if (Paragraph.Range.ListFormat.ListString.Length == 0 && Regex.IsMatch(Paragraph.Range.Text, "Correct\\s+answer:", RegexOptions.IgnoreCase))
                {
                    // add bookmark
                    string bkName = "Answer" + ListString.Replace(".", "");
                    qDoc.Bookmarks.Add(bkName, Paragraph.Range);
                }
            }

            //
            string MultipleChoice = "";
            string QuestionStem = "";
            int count = 0;

            // traverse document and create xml
            foreach (Word.Section Section in qDoc.Sections)
            {
                count++;
                // xml vars                
                string Options = "";
                string Feedback = "";
                //
                string bkName = "";
                foreach (Word.Paragraph p in Section.Range.Paragraphs)
                {
                    string text = StripIncompatableQuotes(p.Range.Text.Trim());
                    // questions
                    if (p.Range.ListFormat.ListString.Length > 0 && Regex.IsMatch(p.Range.ListFormat.ListString, "\\d.", RegexOptions.IgnoreCase))
                    {
                        //Console.WriteLine("\n\nType: {0}, String: {1}", p.Range.ListFormat.ListType, p.Range.ListFormat.ListString);
                        //Console.WriteLine(p.Range.Text.Trim());

                        // create stem
                        if (text.Length > 0)
                        {
                            QuestionStem = V4Template.QuestionStem(text);
                            //Console.WriteLine(QuestionStem);
                        }
                        // bookmark name with correct answer                        
                        bkName = "Answer" + p.Range.ListFormat.ListString.Replace(".", "");

                    }

                    // answers
                    if (p.Range.ListFormat.ListString.Length > 0 && Regex.IsMatch(p.Range.ListFormat.ListString, "[a-z].", RegexOptions.IgnoreCase))
                    {
                        var isCorrect = false;

                        // check if correct
                        foreach (Word.Bookmark Bookmark in Section.Range.Bookmarks)
                        {
                            // get answers as an array, important to remove spaces before
                            string[] answers = Bookmark.Range.Text.Substring(15).Trim().Replace(@"\s+", "").Split(',');
                            foreach (string answer in answers)
                            {
                                // match answer
                                if (Regex.IsMatch(p.Range.ListFormat.ListString, answer))
                                {
                                    isCorrect = true;
                                }
                            }

                        }

                        // handle if true/false
                        if (isCorrect)
                        {
                            Options += V4Template.Option("true", text);
                            //Console.WriteLine("{0}\ntrue", p.Range.ListFormat.ListString + p.Range.Text);
                        }
                        else
                        {
                            Options += V4Template.Option("false", text);
                            //Console.WriteLine(p.Range.ListFormat.ListString + p.Range.Text);
                        }
                    }

                    // feedback
                    if (p.Range.ListFormat.ListString.Length == 0 && Regex.IsMatch(p.Range.Text, "Negative\\s+feedback:", RegexOptions.IgnoreCase))
                    {
                        isFeedBack = true;
                    }

                    // save feedback and reset
                    if (p.Range.ListFormat.ListString.Length == 0 && Regex.IsMatch(p.Range.Text, "Correct\\s+answer:", RegexOptions.IgnoreCase))
                    {
                        isFeedBack = false;
                        // get feedback xml
                        Feedback = V4Template.Feedback(feedback.Substring(18).Trim());
                        feedback = "";
                    }

                    // append feedback paragraphs
                    if (isFeedBack && p.Range.Text.Trim().Length > 0)
                    {
                        feedback += text;
                    }

                }

                if (count > 1)
                    // create list
                    MultipleChoice += V4Template.MultipleChoice(QuestionStem, Options, Feedback);
            }
            // get full xml
            SelfCheckXML = V4Template.SelfCheckTopic(V4Template.QuestionBlock(MultipleChoice));
            return SelfCheckXML;
        }

        public static string getBoldText(Word.Range rng)
        {
            int w = 0;
            var ntxt = "";
            var btxt = "";

            foreach (Word.Range wd in rng.Words)
            {
                w++;

                // counter to prevent out of range
                if (w > 1)
                {
                    if (wd.Bold == 0 && rng.Words[w - 1].Bold == -1)
                    {
                        if (btxt.Trim().Length > 0)
                        {
                            // prevent empty bold tags
                            ntxt += "<Emph>" + StripIncompatableQuotes(btxt) + "</Emph>";
                        }
                        if (wd.Text.Trim().Length > 0)
                        {
                            ntxt += StripIncompatableQuotes(wd.Text);
                        }
                        btxt = "";
                    }
                    else if (wd.Bold == -1 && rng.Words[w - 1].Bold == -1)
                    {
                        btxt += wd.Text;
                    }
                    else if (wd.Bold == -1 && rng.Words[w - 1].Bold == 0)
                    {
                        btxt += wd.Text;
                    }
                    else {
                        if (wd.Text.Trim().Length > 0)
                        {
                            ntxt += StripIncompatableQuotes(wd.Text);
                        }
                    }
                }
                else
                {
                    if (wd.Bold == -1)
                    {
                        btxt += wd.Text;
                    }
                    else {
                        if (wd.Text.Trim().Length > 0)
                        {
                            ntxt += StripIncompatableQuotes(wd.Text);
                        }
                    }
                }

                //Console.WriteLine("bold: {0}, text: {1}", wd.Bold, rng.Words[w].Text);
            }

            //if(wd.Text)
            return ntxt;
        }

        public static string StripIncompatableQuotes(string s)
        {
            s = SecurityElement.Escape(s);
            if (!string.IsNullOrEmpty(s))
                //return s.Replace("\u2018", "&#8216;").Replace("\u2019", "&#8217;").Replace("\u201c", "&#8220;").Replace("\u201d", "&#8221;").Replace('\u000B', ' ');
                return s.Replace("\u2018", "&#x2018;").Replace("\u2019", "&#x2019;").Replace("\u201c", "&#x201c;").Replace("\u201d", "&#x201d;").Replace('\u000B', ' ').Replace('\u0001', ' ');
            else
                return s.Replace(@"\s+", " ");
        }

        public bool isSegue(Word.Section WordSection)
        {
            // check if images are present
            if(WordSection.Range.InlineShapes.Count > 0)
            {
                // check if image title contains segue
                if (WordSection.Range.InlineShapes[1].Title.Contains("Segue"))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetSlideTitle(Word.Section WordSection)
        {
            // check if images are present
            if (WordSection.Range.InlineShapes.Count > 0)
            {
                // set title
                string Title = WordSection.Range.InlineShapes[1].AlternativeText;
                // check if image title contains segue
                if (Title != null)
                {
                    return Title;
                }
            }

            return null;
        }

        public bool isNotes(Word.Range Range)
        {
            // check if previous paragraphs were notes
            if (Range.Text.ToLower().Contains("notes:") || Range.Text.ToLower().Contains("note:"))
            {
                return true;
            }
            return false;
        }

        public bool isIntro(Word.Range Range)
        {
            // check if previous paragraphs were intro
            if (Range.Text.ToLower().Contains("lesson introduction:") || Range.Text.ToLower().Contains("slide introductory statement:"))
            {
                return true;
            }
            return false;
        }

        public string GetNotes(Word.Section WordSection)
        {
            List<string> paras = new List<string>();
            NotesFrame Notes = new NotesFrame();
            var txt = "";

            Word.Paragraphs Paragraphs = WordSection.Range.Paragraphs;
            /*foreach (Word.Paragraph Paragraph in WordSection.Range.Paragraphs)
            {
                Word.Range ParagraphRange = Paragraph.Range;
                Word.Style ParagraphStyle = ParagraphRange.ParagraphStyle;
                //Console.WriteLine(ParagraphStyle.NameLocal);
                if (ParagraphStyle.NameLocal.Contains("Heading") == false && ParagraphRange.InlineShapes.Count == 0)
                {
                    txt += ParagraphRange.Text;
                }
            }*/
            for (var x = Paragraphs.Count; x >= 1; x--)
            {
                Word.Range ParagraphRange = Paragraphs[x].Range;
                //txt += ParagraphRange.Text;

                paras.Add(ParagraphRange.Text);
                if (isIntro(ParagraphRange))
                {
                    paras.RemoveAt(x);
                    paras.Reverse();
                    Notes.Intro = paras;
                    paras = new List<string>();
                }
            }

            txt = string.Join("", Notes.Intro.ToArray());
            return txt;
        }
    }
}
