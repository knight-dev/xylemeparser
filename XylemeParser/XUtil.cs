using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using P = DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using V = DocumentFormat.OpenXml.Vml;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using D = DocumentFormat.OpenXml.Drawing;
using Wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;

namespace LessonParser
{
    class XUtil
    {
        public string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                if (input != null)
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
                else {
                    return "No Slide Id";
                }
            }
        }

        public bool isImage(Paragraph Paragraph)
        {
            // check if paragraph is image
            V.Shape VShape = Paragraph.Descendants<V.Shape>().FirstOrDefault();
            PIC.BlipFill BlipFill = Paragraph.Descendants<PIC.BlipFill>().FirstOrDefault();

            // vml images
            if (VShape != null)
            {
                V.ImageData image = VShape.Descendants<V.ImageData>().FirstOrDefault();
                if (image != null)
                {
                    return true;
                }
            }

            // blips
            if (BlipFill != null)
            {
                D.Blip image = BlipFill.Descendants<D.Blip>().FirstOrDefault();
                if (image != null)
                {
                    return true;
                }
            }
            
            return false;
        }

        public string GetSlideId(Paragraph Paragraph, String Type)
        {
            // check if paragraph contains an image
            V.Shape VShape = Paragraph.Descendants<V.Shape>().FirstOrDefault();
            Drawing Drawing = Paragraph.Descendants<Drawing>().FirstOrDefault();

            // handle vml images (older office versions)
            if (VShape != null && VShape.Alternate != null)
            {                
                if (VShape.Alternate.Value.Contains(Type))
                {
                    var array = VShape.Alternate.Value.Split(' ');
                    var match = array.First(x => x.Contains(Type));
                    Console.WriteLine(match);
                    return match;
                }
                /*if (VShape.Title != null && VShape.Title.Value.Contains(Type))
                {
                    return VShape.Title.Value;
                }*/
            }

            // handle drawing images (newer office versions)
            if (Drawing != null)
            {
                if(Drawing.Inline != null && Drawing.Inline.DocProperties != null)
                {
                    var DrawingProperties = Drawing.Inline.DocProperties;
                    if (DrawingProperties.Title != null && DrawingProperties.Title.Value.Contains(Type))
                    {
                        Console.WriteLine(DrawingProperties.Title.Value);
                        return DrawingProperties.Title.Value;
                    }
                }
            }
            return null;
        }
        
        public string GetSlideTitle(Paragraph Paragraph)
        {
            // check if paragraph contains an image
            V.Shape VShape = Paragraph.Descendants<V.Shape>().FirstOrDefault();
            Drawing Drawing = Paragraph.Descendants<Drawing>().FirstOrDefault();

            // handle vml images (older office versions)
            if (VShape != null)
            {
                if (VShape.Alternate.Value.Contains("Description:"))
                {
                    var array = VShape.Alternate.Value.Split(':');
                    var match = array.Last();
                    return match.Trim();
                }
                /*if (VShape.Title != null && VShape.Title.Value.Contains(Type))
                {
                    return VShape.Title.Value;
                }*/
            }

            // handle drawing images (newer office versions)
            if (Drawing != null)
            {
                if (Drawing.Inline != null && Drawing.Inline.DocProperties != null)
                {
                    var DrawingProperties = Drawing.Inline.DocProperties;
                    if (DrawingProperties.Description != null)
                    {
                        return DrawingProperties.Description.Value;
                    }
                }
            }
            return null;
        }

        public string GetV4_1SlideTitle(Paragraph Paragraph)
        {
            // check if paragraph contains an image
            V.Shape VShape = Paragraph.Descendants<V.Shape>().FirstOrDefault();
            Drawing Drawing = Paragraph.Descendants<Drawing>().FirstOrDefault();

            // handle vml images (older office versions)
            if (VShape != null)
            {
                if (VShape.Alternate.Value.Contains("Description:"))
                {
                    var array = VShape.Alternate.Value.Split(':');
                    var match = array.Last();
                    return match.Trim();
                }
                /*if (VShape.Title != null && VShape.Title.Value.Contains(Type))
                {
                    return VShape.Title.Value;
                }*/
            }

            // handle drawing images (newer office versions)
            if (Drawing != null)
            {
                if (Drawing.Inline != null && Drawing.Inline.DocProperties != null)
                {
                    var DrawingProperties = Drawing.Inline.DocProperties;
                    if (DrawingProperties.Description != null)
                    {
                        return DrawingProperties.Description.Value;
                    }
                }
            }
            return null;
        }

        // Determines whether the shape is a title shape.
        private static bool IsTitleShape(P.Shape shape)
        {
            var placeholderShapes = shape.NonVisualShapeProperties.ApplicationNonVisualDrawingProperties.Descendants<PlaceholderShape>();
            var placeholderShape = placeholderShapes.FirstOrDefault();
            var NonVisualDrawingProperties = shape.NonVisualShapeProperties.NonVisualDrawingProperties;

            if (NonVisualDrawingProperties.Name != null && NonVisualDrawingProperties.Name.Value.ToLower().Contains("title")) {
                return true;
            }
            if (placeholderShape != null && placeholderShape.Type != null)
            {
                Console.WriteLine("ph: " + placeholderShape);
                switch ((PlaceholderValues)placeholderShape.Type)
                {
                    // Any title shape.
                    case PlaceholderValues.Title:

                    // A centered title.
                    case PlaceholderValues.CenteredTitle:
                        return true;

                    default:
                        return false;
                }
            }
            return false;
        }

        public string GetSlideLayoutName(SlidePart slidePart)
        {
            if (slidePart == null)
            {
                throw new ArgumentNullException("slide part null");
            }
            
            if (slidePart.SlideLayoutPart != null)
            {
                SlideLayoutPart SlideLayoutPart = slidePart.SlideLayoutPart;
                if (SlideLayoutPart.SlideLayout != null && SlideLayoutPart.SlideLayout.CommonSlideData != null) {
                    SlideLayout SlideLayout = SlideLayoutPart.SlideLayout;

                    if (SlideLayout.CommonSlideData.Name != null) {
                        return SlideLayout.CommonSlideData.Name;
                    }

                }
            }

            return string.Empty;
        }

        public string GetV4SlideText(SlidePart slidePart) {
            if (slidePart == null)
            {
                throw new ArgumentNullException("slide part null");
            }

            // Declare a paragraph separator.
            string paragraphSeparator = null;

            if (slidePart.Slide != null)
            {
                // Find all the title shapes.
                var shapes = from shape in slidePart.Slide.Descendants<P.Shape>()
                             where IsTitleShape(shape)
                             select shape;
                //Console.WriteLine(shapes.Count());
                StringBuilder paragraphText = new StringBuilder();

                foreach (P.Shape shape in shapes)
                {
                    // Get the text in each paragraph in this shape.
                    foreach (var paragraph in shape.TextBody.Descendants<D.Paragraph>())
                    {
                        // Add a line break.
                        paragraphText.Append(paragraphSeparator);

                        foreach (var text in paragraph.Descendants<D.Text>())
                        {
                            //Console.WriteLine(paragraph.InnerText);
                            paragraphText.Append(text.Text);
                        }

                        paragraphSeparator = "\n";
                    }
                }

                return paragraphText.ToString();
            }

            return string.Empty;
        }

        public string GetV4SlideTitle(Paragraph Paragraph, MainDocumentPart DocumentPart)
        {
            // check if paragraph is ole slide
            EmbeddedObject Object = Paragraph.Descendants<EmbeddedObject>().FirstOrDefault();


            // handle drawing images (newer office versions)
            /*if (Object != null)
            {*/
                V.Office.OleObject OleObject = Paragraph.Descendants<V.Office.OleObject>().FirstOrDefault();
                if(OleObject != null && OleObject.Id != null)
                {
                    EmbeddedPackagePart embeddedSlide = (EmbeddedPackagePart)DocumentPart.GetPartById(OleObject.Id);
                    Stream stream = embeddedSlide.GetStream();

                    using (PresentationDocument presentationDocument = PresentationDocument.Open(stream, false))
                    {
                        // Get the presentation part of the document.
                        PresentationPart presentationPart = presentationDocument.PresentationPart;
                        if (presentationPart != null)
                        {
                            int slidesCount = presentationPart.SlideParts.Count();
                            SlideId sid = presentationPart.Presentation.SlideIdList.Descendants<SlideId>().FirstOrDefault();

                            SlidePart slidePart = presentationPart.SlideParts.FirstOrDefault();

                            string t = GetV4SlideText(slidePart);
                            Console.WriteLine("Slide: " + t);
                            //Console.WriteLine("sid: " + sid.Id);
                            return t;
                        }
                    }
                    //ImagePart imagePart = e.Current;
                    //Stream stream = imagePart.GetStream();
                }

                return "";
            /*}*/
            return null;
        }

        public bool isSlideObject(Paragraph Paragraph)
        {
            // check if paragraph is ole slide
            EmbeddedObject Object = Paragraph.Descendants<EmbeddedObject>().FirstOrDefault();


            // image objects
            if (Object != null)
            {
                V.Office.OleObject OleObject = Object.Descendants<V.Office.OleObject>().FirstOrDefault();
                return true;                
            }
            return false;
        }

        public bool isSlideType(Paragraph Paragraph, String Type)
        {
            // check if paragraph contains an image
            V.Shape VShape = Paragraph.Descendants<V.Shape>().FirstOrDefault();
            Drawing Drawing = Paragraph.Descendants<Drawing>().FirstOrDefault();

            // handle vml images (older office versions)
            if (VShape != null && VShape.Alternate != null)
            {
                if (VShape.Alternate.Value.Contains(Type))
                {
                    var array = VShape.Alternate.Value.Split(' ');
                    var match = array.First(x => x.Contains(Type));
                    return true;
                }
                /*if (VShape.Title != null && VShape.Title.Value.Contains(Type))
                {
                    return true;
                }*/
            }

            // handle drawing images (newer office versions)
            if (Drawing != null)
            {
                if (Drawing.Inline != null && Drawing.Inline.DocProperties != null)
                {
                    var DrawingProperties = Drawing.Inline.DocProperties;
                    if (DrawingProperties.Title != null && DrawingProperties.Title.Value.Contains(Type))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isV4SlideType(Paragraph Paragraph, MainDocumentPart DocumentPart, String Type)
        {
            // check if paragraph is ole slide
            EmbeddedObject Object = Paragraph.Descendants<EmbeddedObject>().FirstOrDefault();
            V.Office.OleObject OleObject = Paragraph.Descendants<V.Office.OleObject>().FirstOrDefault();

            // handle drawing images (newer office versions)
           /* if (Object != null)
            {*/
                //V.Office.OleObject OleObject = Object.Descendants<V.Office.OleObject>().FirstOrDefault();
                if (OleObject != null && OleObject.Id != null)
                {
                Console.WriteLine("oleobj: "+OleObject.Id);
                EmbeddedPackagePart embeddedSlide = (EmbeddedPackagePart)DocumentPart.GetPartById(OleObject.Id);
                    Stream stream = embeddedSlide.GetStream();

                    using (PresentationDocument presentationDocument = PresentationDocument.Open(stream, false))
                    {
                        // Get the presentation part of the document.
                        PresentationPart presentationPart = presentationDocument.PresentationPart;
                        if (presentationPart != null)
                        {
                            int slidesCount = presentationPart.SlideParts.Count();
                            SlideId sid = presentationPart.Presentation.SlideIdList.Descendants<SlideId>().FirstOrDefault();

                            SlidePart slidePart = presentationPart.SlideParts.FirstOrDefault();
                            
                            string LayoutName = GetSlideLayoutName(slidePart);
                            //Console.WriteLine("Type: " + Type);
                            Console.WriteLine("LayoutName: " + LayoutName);
                            if (LayoutName.ToLower().Contains(Type))
                            {
                                return true;
                            }
                        }
                    }
                    //ImagePart imagePart = e.Current;
                    //Stream stream = imagePart.GetStream();
                }
                
           /* }*/
            return false;
        }

        public string GetV4SlideType(Paragraph Paragraph, MainDocumentPart DocumentPart, String Type)
        {
            // check if paragraph is ole slide
            EmbeddedObject Object = Paragraph.Descendants<EmbeddedObject>().FirstOrDefault();
            V.Office.OleObject OleObject = Paragraph.Descendants<V.Office.OleObject>().FirstOrDefault();

            // handle drawing images (newer office versions)
            /*if (Object != null)
            {*/
                //V.Office.OleObject OleObject = Object.Descendants<V.Office.OleObject>().FirstOrDefault();
                if (OleObject != null && OleObject.Id != null)
                {
                    EmbeddedPackagePart embeddedSlide = (EmbeddedPackagePart)DocumentPart.GetPartById(OleObject.Id);
                    Stream stream = embeddedSlide.GetStream();

                    using (PresentationDocument presentationDocument = PresentationDocument.Open(stream, false))
                    {
                        // Get the presentation part of the document.
                        PresentationPart presentationPart = presentationDocument.PresentationPart;
                        if (presentationPart != null)
                        {
                            int slidesCount = presentationPart.SlideParts.Count();
                            SlideId sid = presentationPart.Presentation.SlideIdList.Descendants<SlideId>().FirstOrDefault();

                            SlidePart slidePart = presentationPart.SlideParts.FirstOrDefault();

                            string LayoutName = GetSlideLayoutName(slidePart);
                            return LayoutName;
                            //Console.WriteLine("Slide: " + t);
                            //Console.WriteLine("sid: " + sid.Id);
                            
                        }
                    }
                    //ImagePart imagePart = e.Current;
                    //Stream stream = imagePart.GetStream();
                }

            /*}*/
            return "";
        }

        public string GetSlideXML(string dir, string name)
        {
            string file = dir + name + ".xml";
            if (File.Exists(file))
            {
                // Use StreamReader to consume the entire text file.
                using (StreamReader reader = new StreamReader(file))
                {
                    return reader.ReadToEnd();
                }
            }
            return null;
        }

        /**
        * Challenge identifiers
        */
        public bool isChallenge(String Text)
        {
            // check if previous paragraphs were intro
            if (Text.ToLower().Contains("challenge"))
            {
                return true;
            }
            return false;
        }

        public bool isQuestion(Level ListFormat)
        {
            if (ListFormat != null)
            {
                if (ListFormat.NumberingFormat != null)
                {
                    if(ListFormat.NumberingFormat.Val == "decimal")
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public bool isAnswer(Level ListFormat)
        {
            if (ListFormat != null)
            {
                if (ListFormat.NumberingFormat != null)
                {
                    if (ListFormat.NumberingFormat.Val == "lowerLetter" || ListFormat.NumberingFormat.Val == "upperLetter")
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public bool isFeedback(String Text)
        {
            // check if paragraphs is negative feedback
            if (Text.ToLower().Contains("negative feedback"))
            {
                return true;
            }
            return false;
        }

        public bool isCorrect(String Text)
        {
            // check if paragraphs is correct answer
            if (Text.ToLower().Contains("correct answer"))
            {
                return true;
            }
            return false;
        }

        /**
        * Lesson identifiers
        */
        public bool isLesson(Paragraph Paragraph)
        {
            /*// check if paragraph is lesson
            ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
            if (Paragraph.ParagraphProperties != null)
            {
                ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
            }

            // checking heading name
            if (ParagraphStyleId != null && ParagraphStyleId.Val == "Heading1")
            {
                return true;
            }*/

            // check image name
            if(isSlideType(Paragraph, "Lesson"))
            {
                return true;
            }

            return false;
        }

        public bool isLessonV4(Paragraph Paragraph, MainDocumentPart DocumentPart)
        {
            /*// check if paragraph is lesson
            ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
            if (Paragraph.ParagraphProperties != null)
            {
                ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
            }

            // checking heading name
            if (ParagraphStyleId != null && ParagraphStyleId.Val == "Heading1")
            {
                return true;
            }*/
            // check image name
            if (isSlideType(Paragraph, "Lesson"))
            {
                return true;
            }

            // check image name
            if (isV4SlideType(Paragraph, DocumentPart, "lessontitleslide"))
            {
                return true;
            }

            // check image name
            if (Paragraph.InnerText.ToLower().Contains("slide 1"))
            {
                //return true;
            }            

            return false;
        }

        public bool isTopic(Paragraph Paragraph)
        {
           
            /*// check if paragraph is topic
            ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
            if (Paragraph.ParagraphProperties != null)
            {
                ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
            }

            // checking heading name
            if (ParagraphStyleId != null && ParagraphStyleId.Val == "Heading2")
            {
                return true;
            }*/

            // check image name
            if (isSlideType(Paragraph, "Segue"))
            {
                return true;
            }

            return false;
        }

        public bool isTopicV4(Paragraph Paragraph, MainDocumentPart DocumentPart)
        {

            /*// check if paragraph is topic
            ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
            if (Paragraph.ParagraphProperties != null)
            {
                ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
            }

            // checking heading name
            if (ParagraphStyleId != null && ParagraphStyleId.Val == "Heading2")
            {
                return true;
            }*/

            // check image name
            if (isSlideType(Paragraph, "Segue"))
            {
                return true;
            }

            // check image name
            if (isV4SlideType(Paragraph, DocumentPart, "lessontitleslide"))
            {
                return true;
            }

            if (Paragraph.InnerText.ToLower().Contains("slide 1")) {
                //return true;
            }            

            return false;
        }

        public bool isV4Slide(Paragraph Paragraph)
        {
            
            // check image name
            if (isImage(Paragraph))
            {
                //return true;
            }

            // check image name
            if (isSlideType(Paragraph, "Slide"))
            {
                return true;
            }

            return false;
        }

        public bool isSubTopic(Paragraph Paragraph)
        {
            
            /*// check if paragraph is subtopic
            ParagraphStyleId ParagraphStyleId = new ParagraphStyleId();
            if (Paragraph.ParagraphProperties != null)
            {
                ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
            }

            // checking heading name
            if (ParagraphStyleId != null && ParagraphStyleId.Val == "Heading3")
            {
                return true;
            }*/

            // check image name
            if (isSlideType(Paragraph, "Slide"))
            {
                return true;
            }

            return false;
        }

        public bool isName(String Text)
        {
            // check if previous paragraphs were intro
            if (Text.ToLower().Contains("topic name:") || Text.ToLower().Contains("topic title:") || Text.ToLower().Contains("lesson name:") || Text.ToLower().Contains("lesson title:") || Text.ToLower().Trim() == "lesson title" || Text.ToLower().Trim() == "topic title")
            {
                return true;
            }
            return false;
        }

        public bool isIntro(String Text)
        {
            // check if previous paragraphs were intro
            if (Text.ToLower().Contains("introduction:") || Text.ToLower().Contains("lesson introduction:") || Text.ToLower().Contains("lesson intro:") || Text.ToLower().Contains("topic introductory statement:") || Text.ToLower().Contains("slide introductory statement:") || Text.ToLower().Contains("topic introduction:") || Text.ToLower().Trim() == "lesson intro" || Text.ToLower().Trim() == "section introduction:" || Text.ToLower().Trim() == "topic intro statement" || Text.ToLower().Trim() == "slide intro statement" || Text.ToLower().Trim() == "topic introductory statement" || Text.ToLower().Trim() == "slide introductory statement" || Text.ToLower().Trim() == "slide preamble:" || Text.ToLower().Trim() == "introduction")
            {
                return true;
            }
            return false;
        }

        public bool isObjective(String Text)
        {
            // check if previous paragraphs were intro
            if (Text.ToLower().Contains("topic objective:") || Text.ToLower().Contains("topic objectives:") || Text.ToLower().Contains("lesson objective:") || Text.ToLower().Contains("lesson objectives:") || Text.ToLower().Contains("educational objective:") || Text.ToLower().Trim() == "lesson objective" || Text.ToLower().Trim() == "topic objective" || Text.ToLower().Trim() == "lesson objectives" || Text.ToLower().Trim() == "topic objectives")
            {
                return true;
            }
            return false;
        }

        public bool hasNotes(String Text)
        {
            // check if paragraph has notes
            if (Text.ToLower().Contains("notes:") || Text.ToLower().Contains("note:"))
            {
                return true;
            }
            return false;
        }

        public bool hasNotesV4(String Text, Paragraph Paragraph)
        {
            // check if paragraph has notes
            if (Text.ToLower().Contains("notes:") || Text.ToLower().Contains("note:"))
            {
                return true;
            }

            // check image exists
            if (isSlideObject(Paragraph))
            {
                return true;
            }
            return false;
        }

        /**
        * Lab identifiers
        */

        public bool isTask(Level ListFormat)
        {
            if (ListFormat != null)
            {
                if (ListFormat.LevelText != null)
                {
                    if (ListFormat.LevelText.Val.Value.ToLower().Contains("task"))
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        
        public bool isTask(String text)
        {
            Regex regex = new Regex(@"task\s+\d+");
            Match match = regex.Match(text.Trim().ToLower().ToString());

            if (match.Success && match.Index == 0) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

            return false;
        }
        public bool isTaskActivity(String text)
        {
            Regex regex = new Regex(@"activity");
            //Regex regex = new Regex(@"activity");
            Match match = regex.Match(text.Trim().ToLower().ToString());

            if (match.Success && match.Index == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

            return false;
        }

        public bool isStep(String text)
        {
            
            Regex regex = new Regex(@"step\s+\d+");
            Match match = regex.Match(text.Trim().ToLower().ToString());
            
            if (match.Success && match.Index == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(text.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

            return false;
        }

        public bool isLab(String text)
        {
            //Console.WriteLine(text.ToString());
            //Regex regex = new Regex(@"discovery\s+\d+");
            Regex regex = new Regex(@"lab\s+\d+");
            Match match = regex.Match(text.Trim().ToLower().ToString());

            if (match.Success && match.Index == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

            return false;
        }

        public String getPlainText(IEnumerable<Run> Runs) {
            StringBuilder text = new StringBuilder();
            // loop
            foreach (var run in Runs)
            {
                // has text
                int textChild = run.Descendants<Word.Text>().Count();

                // group children
                if (textChild > 0)
                {
                    text.Append(run.InnerText);
                }

            }

            return text.ToString();
        }

        public bool isStep(Level ListFormat)
        {
            if (ListFormat != null)
            {
                if (ListFormat.LevelText != null)
                {
                    if (ListFormat.LevelText.Val.Value.ToLower().Contains("step"))
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public bool isNote(Level ListFormat)
        {
            if (ListFormat != null)
            {
                if (ListFormat.ParagraphStyleIdInLevel != null)
                {
                    //Console.WriteLine(ListFormat.ParagraphStyleIdInLevel.Val);                    
                }

                if (ListFormat.LevelText != null)
                {
                    if (ListFormat.LevelText.Val.Value.ToLower().Contains("note"))
                    {
                        return true;
                    }
                    /*if (ListFormat.ParagraphStyleIdInLevel != null)
                    {
                        //Console.WriteLine(ListFormat.ParagraphStyleIdInLevel.Val);
                        
                    }*/
                    
                }

            }
            return false;
        }

        public bool isNote(Paragraph Paragraph)
        {
            if (Paragraph.ParagraphProperties != null)
            {
                
                if (Paragraph.ParagraphProperties.ParagraphStyleId != null)
                {
                    //Console.WriteLine(Paragraph.ParagraphProperties.ParagraphStyleId.Val);
                    if (Paragraph.ParagraphProperties.ParagraphStyleId.Val.Value.ToLower().Contains("note"))
                    {
                        return true;
                    }
                }
                
                /*if (ListFormat.LevelText != null)
                {
                    //Console.WriteLine(ListFormat.ParagraphStyleIdInLevel.Val.Value.ToLower());
                    if (ParagraphProperties.ParagraphStyleId.Val.Value.ToLower().Contains("note"))
                    {
                        return true;
                    }
                }*/

            }
            return false;
        }

        public bool hasTopology(String Text)
        {
            // check if paragraph has notes
            if (Text.ToLower().Trim() == "topology")
            {
                return true;
            }
            return false;
        }

        public bool isLabProcedure(String Text)
        {
            // check if paragraph has notes
            if (Text.ToLower().Trim() == "lab procedures" || Text.ToLower().Trim() == "lab procedure")
            {
                return true;
            }
            return false;
        }

        public bool isRequiredResources(String Text)
        {
            // check if paragraph has notes
            if (Text.ToLower().Trim() == "required resources")
            {
                return true;
            }
            return false;
        }

        public void SaveImage(Paragraph Paragraph)
        {
            /*foreach (V.Shape pic in Paragraph.Descendants<V.Shape>())
            {
                //pic.ge
                //pic
                i++;
                Console.WriteLine(pic.Alternate);
                V.ImageData image = pic.Descendants<V.ImageData>().FirstOrDefault();
                if (image != null)
                {
                    ImagePart p = MainDocumentPart.GetPartById(image.RelationshipId) as ImagePart;
                    //p.GetStream();
                    Bitmap img = new Bitmap(p.GetStream());
                    img.Save(i + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    Console.WriteLine(image.RelationshipId + Environment.NewLine);
                }

            }*/
        }
    }
}
