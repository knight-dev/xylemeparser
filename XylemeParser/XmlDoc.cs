using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;
using System.Xml;
using System.IO;

namespace LessonParser
{
    class LessonFrame
    {
        public string Intro
        {
            get; set;
        }

        public List<TopicFrame> Topics
        {
            get; set;
        }

        public string DesignData
        {
            get; set;
        }
    }

    class TopicFrame
    {
        public string Title
        {
            get; set;
        }

        public string Intro
        {
            get; set;
        }

        public string Segue
        {
            get; set;
        }

        public List<SubTopicFrame> SubTopics
        {
            get; set;
        }

        public string DesignData
        {
            get; set;
        }
    }

    class SubTopicFrame
    {
        public string Title
        {
            get; set;
        }

        public string Slide
        {
            get; set;
        }

        public string Notes
        {
            get; set;
        }

        public List<SubTopicGroup> SubTopicsGroup
        {
            get; set;
        }
    }

    class SubTopicGroup
    {
        public string Title
        {
            get; set;
        }

        public string Slide
        {
            get; set;
        }

        public string Notes
        {
            get; set;
        }
    }

    class XmlDoc
    {
        public void Start()
        {
            // xml template object
            var V4Template = new V4LessonTemplate();
            V4Template.CourseCode = "CIS";
            V4Template.ModuleCode = "M04";
            V4Template.LessonCode = "L04";

            // Use util object to open word doument
            var WordUtil = new WordUtil();
            var WordApplication = WordUtil.CreateApplication();
            Word.Document WordDocument = WordUtil.OpenDocument(WordApplication, @"C:\Users\Niel\Source\Repos\projectx\SplitterReloaded\bin\Debug\ANDMB - L04 - Streaming Analytics_20160812_SB.docx");

            //
            var Lesson = new LessonFrame();
            var Topic = new TopicFrame();
            var SubTopic = new SubTopicFrame();
            var SubTopicGroup = new SubTopicGroup();

            //
            int SegueCount = 1;
            int i = 0;
            

            // loop through sections document
            foreach ( Word.Section WordSection in WordDocument.Sections )
            {
                var Title = WordUtil.GetSlideTitle(WordSection);
                bool isSegue = WordUtil.isSegue(WordSection);

                if (isSegue && SegueCount == 1)
                {
                    SegueCount++;
                    Topic.Title = Title;
                    Topic = new TopicFrame();
                    Console.WriteLine(Environment.NewLine + "Topic");

                }else if (isSegue && SegueCount > 1)
                {
                    SegueCount++;
                    //Lesson.Topics.Add(Topic);
                    Topic = new TopicFrame();
                    Console.WriteLine(Environment.NewLine + "Topic");
                }

                if (i == 0)
                {
                    Console.WriteLine(Environment.NewLine + "Slide");
                    //Console.WriteLine(WordUtil.GetNotes(WordSection));
                    File.WriteAllText("test.txt", WordUtil.GetNotes(WordSection));
                }
                    
                /*if(i == 0)
                {
                    // loop through paragraphs in section
                    foreach (Word.Paragraph Paragraph in WordSection.Range.Paragraphs)
                    {
                        //Word.Range rng = Paragraph.Range.FormattedText;
                        //Console.WriteLine(Paragraph.Range.WordOpenXML);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Paragraph.Range.WordOpenXML);
                        XmlNodeList newNode = doc.GetElementsByTagName("w:r");

                        //Console.WriteLine(Paragraph.Range.XML);
                        foreach (XmlNode rng in newNode)
                        {
                            //Console.WriteLine(rng.HasChildNodes);
                            foreach (XmlNode ng in rng.ChildNodes)
                            {
                                Console.WriteLine(ng.Name);

                                /*foreach (var g in ng.ChildNodes)
                                    {
                                    if(g is XmlElement)
                                    {
                                        XmlElement x = (XmlElement)g;
                                        Console.WriteLine(x.Name +": "+ x.InnerText);
                                    }
                                        
                                    }/
                            }
                        }
                        //Console.WriteLine(Paragraph.Range.Text);
                    }
                }*/
                i++;
            }

            WordApplication.Quit();
        }
    }
}
