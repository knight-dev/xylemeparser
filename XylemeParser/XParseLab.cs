using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using D = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using V = DocumentFormat.OpenXml.Vml;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace LessonParser
{
    class LabImage {
        public int step { get; set; }
        public int task { get; set; }
        public ImagePart ImagePart { get; set; }
        public string increment { get; set; } = "";
    }
    class LabTaskXml {
        public string Title { get; set; }
        public List<string> Activity { get; set; }
        public List<LabStepXml> Steps { get; set; }
    }
    class LabStepXml
    {
        public string Action { get; set; }
        public List<string> Response { get; set; }
    }
    class XParseLab
    {
        // show paragraph details
        public bool showParagraphSchema = true;
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
        public string SourceDirectory = Directory.GetCurrentDirectory() + @"\";

        string imagePrefix = "DEVWBX";
        string labName = "Retrieve_Created_Messages_to_Take_a_Compliance_Action";
        string version = "1-0-0_LAB";

        public MainDocumentPart OpenDocument(string documentFile)
        {
            // xyleme xml template object
            V4Template.CourseCode = "CIS";
            V4Template.ModuleCode = "M04";
            V4Template.LessonCode = "L04";
            //
            string imageName = "";
            int taskNumber = 0;
            int stepNumber = 0;

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

                // root element part of the doc
                Document Document = MainDocumentPart.Document;

                // actual document body
                Body Body = Document.Body;                

                // create list to store paragraphs
                List<string> paragraphs = new List<string>();
                List<string> SubTopics = new List<string>();
                List<string> Topics = new List<string>();
                List<string> ListOrphan = new List<string>();

                // store images
                List<LabImage> labImages = new List<LabImage>();
                List<LabImage> taskImages = new List<LabImage>();
                List<LabImage> stepImages = new List<LabImage>();

                // store xml info
                List<LabStepXml> labSteps = new List<LabStepXml>();
                List<LabTaskXml> labTasks = new List<LabTaskXml>();

                //
                List<string> lastTaskActivity = new List<string>();

                string txt = "";
                //Console.WriteLine(Body.ChildElements.Count);
                var i = 0;
                var Elements = Body.ChildElements;
                int count = Elements.Count();
                for (var x = count - 1; x >= 0; x--)
                {
                    var Element = Elements[x];
                    if (Element is Paragraph)
                    {  
                        // create paragraphs
                        Paragraph Paragraph = (Paragraph)Element;
                        var ListFormat = new Level();

                        // vars
                        int runs = Paragraph.Descendants<Run>().Count();
                        int links = Paragraph.Descendants<Hyperlink>().Count();
                        int drawings = Paragraph.Descendants<Drawing>().Count();
                        IEnumerable<Run> paragraphRuns = Paragraph.Descendants<Run>();
                        String plainText = XUtil.getPlainText(paragraphRuns);

                        // show paragraph details
                        if (showParagraphSchema && runs > 0)
                        {   
                            // pretty print
                            Console.WriteLine("\n//////////////////////////////////////////////////");
                            Console.WriteLine(plainText);
                            Console.WriteLine("==================================================");
                            if (Paragraph.ParagraphProperties != null)
                            {
                                Console.WriteLine("///// has properties: true");
                            }
                            
                            Console.WriteLine("///// runs: {0}", runs);
                            Console.WriteLine("///// links: {0}", links);
                            Console.WriteLine("///// images: {0}", drawings);
                            Console.WriteLine("//////////////////////////////////////////////////**");
                        }

                        Paragraph prevParagraph = new Paragraph();
                        ParagraphStyleId prevParagraphStyleId = new ParagraphStyleId();
                        if (x < count && Elements[x + 1] is Paragraph)
                        {
                            prevParagraph = (Paragraph)Elements[x + 1];
                            prevParagraphStyleId = prevParagraph.ParagraphProperties.ParagraphStyleId;
                        }

                        // get list info
                        if (Paragraph.ParagraphProperties != null)
                        {
                            // style
                            ParagraphStyleId ParagraphStyleId = Paragraph.ParagraphProperties.ParagraphStyleId;
                            ListFormat = GetListFormat(Paragraph, Styles, Numbering, ParagraphStyleId);
                        }

                        // save every paragraph
                        paragraphs.Add(Paragraph.InnerText);


                        // detect tasks
                        /*if (XUtil.isTask(ListFormat))
                        {
                            // get an array with the correct answers
                            //Console.WriteLine(Environment.NewLine + Paragraph.InnerText);

                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }*/
                        if (XUtil.isImage(Paragraph))
                        {
                            //
                            //Console.WriteLine(Environment.NewLine + "<image>");
                            foreach (PIC.BlipFill pic in Paragraph.Descendants<PIC.BlipFill>())
                            {
                                i++;
                                Console.WriteLine("========================================= pic" + i);
                                D.Blip image = pic.Descendants<D.Blip>().FirstOrDefault();
                                if (image != null)
                                {
                                    Console.WriteLine("========================================= blip pic");
                                    ImagePart part = MainDocumentPart.GetPartById(image.Embed.Value) as ImagePart;

                                    // new lab image class
                                    LabImage labImage = new LabImage();
                                    labImage.ImagePart = part;
                                    stepImages.Add(labImage);
                                }

                            }
                            //Console.WriteLine("========================================= new pic");
                            foreach (V.Shape pic in Paragraph.Descendants<V.Shape>())
                            {
                                i++;
                                Console.WriteLine("========================================= new pic");
                                V.ImageData image = pic.Descendants<V.ImageData>().FirstOrDefault();
                                if (image != null)
                                {
                                    Console.WriteLine("========================================= i pic");
                                    ImagePart part = MainDocumentPart.GetPartById(image.RelationshipId) as ImagePart;
                                    // new lab image class
                                    LabImage labImage = new LabImage();
                                    labImage.ImagePart = part;
                                    stepImages.Add(labImage);
                                    Console.WriteLine(image.RelationshipId + Environment.NewLine);
                                }

                            }
                        }

                        
                        // steps
                        if (XUtil.isStep(plainText))
                        {
                            List<string> Notes = new List<string>();
                            int stepEndPosition = 0;
                            LabStepXml labStepXml = new LabStepXml();

                            // store image information
                            if (stepImages.Count > 0) {
                                // get step number
                                Match result = Regex.Match(plainText, @"\d+");
                                // reverse img since in reverse order
                                //stepImages.Reverse();

                                // save step number if success
                                if (result.Success) {
                                    // letters for additional step images
                                    string[] alpha = { "", "a", "b", "c", "d", "e", "f", "g",
                                        "h", "i", "j", "k", "l", "m", "n", "o", "p", "q","r",
                                        "s", "t", "u", "v", "w", "x", "y", "z"
                                    };
                                    // save ending position of step literal
                                    //stepEndPosition = result.Captures[0].Index + result.Captures[0].Length;
                                    for (int c = 0; c < stepImages.Count; c++)
                                    {
                                        stepImages[c].step = int.Parse(result.Groups[0].Value);

                                        if(c > 0)
                                        {
                                            stepImages[c].increment = alpha[c];
                                        }
                                    }

                                    // add to task and clear step
                                    stepImages.Reverse();
                                    taskImages.AddRange(stepImages);
                                    stepImages.Clear();
                                }
                                
                            }

                            // get formatted step text
                            string formattedText = applyFormatting(Paragraph);
                            string t = Regex.Replace(formattedText, @"Step\s+\d+", "");
                            string stepAction = V4Template.StepUserAction(t.Trim());
                            
                            Notes = GetNotes(x + paragraphs.Count-1, x+1, Elements);
                            var data = string.Join("", Notes.ToArray());
                            string responseText = "";

                            int nCount = 0;
                            //Notes.Reverse();
                            foreach(var n in Notes)
                            {
                                if (n != "<imageplaceholder/>" && !n.Contains("<CustomNote>") && !n.Contains("<List>") && !n.Contains("<RichText>") && nCount < 1)
                                {
                                    nCount++;
                                    // get response text and remove from main list
                                    responseText = n;                                    
                                }
                            }
                            if (nCount == 1)
                            {
                                Notes.Remove(responseText);
                            }
                            
                            //responseText = V4Template.StepResponseDescription(responseText);
                            string stepResponse = V4Template.StepResponse(V4Template.StepResponseDescription(data));
                            string fullStep = V4Template.Step(stepAction + stepResponse);
                            labStepXml.Action = stepAction;
                            List<string> res = new List<string>();
                            if (drawings > 0) {
                                res.Add("<imageplaceholder/>");
                            }
                            res.Add(responseText != "" ? V4Template.StepResponseDescription(responseText) : "<ResponseDescription/>");
                            res.AddRange(Notes);
                            labStepXml.Response = res;
                            labSteps.Add(labStepXml); 
                            //Console.WriteLine("* " + fullStep);
                            //paragraphs.Remove(paragraphs.Last());
                            //GetNotes(x + paragraphs.Count, x + 1, Elements, MainDocumentPart);
                            //Console.WriteLine(Environment.NewLine + "Step: " + Notes);

                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }

                        // detect tasks
                        if (XUtil.isTaskActivity(plainText))
                        {
                            List<string> Notes = new List<string>();
                            Notes = GetNotes(x + paragraphs.Count - 1, x + 1, Elements);
                            var data = string.Join("", Notes.ToArray());
                            Console.WriteLine(Environment.NewLine + "Activity: " + data);
                            lastTaskActivity = Notes;
                            // reset paragraph numbering
                            paragraphs.Clear();
                        }

                        // detect tasks
                        if (XUtil.isTask(plainText))
                        {
                            LabTaskXml labTaskXml = new LabTaskXml();
                            // store image information
                            if (taskImages.Count > 0)
                            {
                                // get step number
                                Match result = Regex.Match(plainText, @"\d+");

                                // save step number if success
                                if (result.Success)
                                {
                                    for (int c = 0; c < taskImages.Count; c++)
                                    {
                                        taskImages[c].task = int.Parse(result.Groups[0].Value);
                                    }

                                    // add to task and clear step
                                    labImages.AddRange(taskImages);
                                    taskImages.Clear();
                                }

                            }

                            // get formatted task text
                            string formattedText = applyFormatting(Paragraph);
                            string t = Regex.Replace(formattedText, @"Task\s+\d+:", "");
                            labTaskXml.Title = t.Trim();
                            labTaskXml.Activity = lastTaskActivity;
                            labSteps.Reverse();
                            labTaskXml.Steps = new List<LabStepXml>();
                            labTaskXml.Steps.AddRange(labSteps);
                            labTasks.Add(labTaskXml);
                            labSteps.Clear();
                            // get an array with the correct answers
                            //Console.WriteLine(Environment.NewLine + Paragraph.InnerText);
                            taskNumber++;
                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }

                        labImages.Reverse();
                        if (XUtil.isLab(plainText))
                        {
                            // get formatted lab text
                            string formattedText = applyFormatting(Paragraph);
                            string t = Regex.Replace(formattedText, @"Discovery\s+\d+:", "");
                            labName = t.Trim();
                            string ilabName = Regex.Replace(labName, @"\s+", "_");

                            // get an array with the correct answers
                            Console.WriteLine(Environment.NewLine + "Saving {0} images...", labImages.Count);
                            foreach (var img in labImages)
                            {

                                string imgname = string.Format("{0}_{1}_{2}_Task-{3}_{4}{5}.png", imagePrefix, version, ilabName, img.task.ToString().PadLeft(2, '0'), img.step.ToString().PadLeft(3, '0'), img.increment);
                                Console.WriteLine(imgname);

                                // convert image part to stream and save
                                //Bitmap image = new Bitmap(img.ImagePart.GetStream());
                                //image.Save(SourceDirectory + imgname, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            int u = 0;
                            int h = 0;
                            int k = 0;
                            int imgtrack = 0;
                            labSteps.Reverse();
                            labTasks.Reverse();
                            foreach (var task in labTasks) {
                                h++;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Task: " + h);
                                Console.ForegroundColor = ConsoleColor.White;
                                k = 0;
                                string fullstep = "";
                                string allSteps = "";
                                string allTasks = "";
                                foreach (var step in task.Steps)
                                {
                                    k++;
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("Step: " + k);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    //Console.WriteLine(step.Action);
                                    //fullstep += step.Action;
                                    Console.ForegroundColor = ConsoleColor.White;

                                    string response = "";
                                    foreach (var r in step.Response)
                                    {
                                        if (r.Count() > 0)
                                        {
                                            if (r == "<imageplaceholder/>")
                                            {
                                                LabImage img = labImages[imgtrack];
                                                string imgname = string.Format("_PORTFOLIO_2.0/DevNet/Level_200/Concentrations/DEVWBX/v1.0/{0}_{1}_{2}_Task-{3}_{4}{5}.png", imagePrefix, version, ilabName, img.task.ToString().PadLeft(2, '0'), img.step.ToString().PadLeft(3, '0'), img.increment);
                                                //Console.WriteLine(V4Template.LabFigure(imgname));
                                                response += V4Template.LabFigure(imgname);
                                                imgtrack++;
                                                //Console.WriteLine("...." + r);
                                            }
                                            else
                                            {
                                                //Console.WriteLine("...." + r);
                                                response += r;
                                            }

                                        }
                                    }

                                    //Console.WriteLine(V4Template.StepResponse(response));
                                    fullstep = V4Template.Step(step.Action + V4Template.StepResponse(response));
                                    Console.WriteLine(fullstep);
                                    u++;
                                }
                            }
                            
                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }
                        /*
                        // detect steps -- list step
                        if (XUtil.isStep(ListFormat))
                        {
                            // get an array with the correct answers
                            
                            paragraphs.Remove(paragraphs.Last());
                            GetNotes(x + paragraphs.Count, x + 1, Elements, MainDocumentPart);
                            Console.WriteLine(Environment.NewLine + "Step: " + Paragraph.InnerText);

                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }
                        if (XUtil.isStep(Paragraph.InnerText))
                        {
                            // get an array with the correct answers

                            //paragraphs.Remove(paragraphs.Last());
                            GetNotes(x + paragraphs.Count, x + 1, Elements, MainDocumentPart);
                            Console.WriteLine(Environment.NewLine + "Step: " + Paragraph.InnerText);

                            // reset paragraph numbering 
                            paragraphs.Clear();
                        }*/

                        /*if (XUtil.isNote(ListFormat))
                        {
                            //
                            Console.WriteLine(Environment.NewLine + "<notes>");
                        }
                        else if (XUtil.isNote(Paragraph))
                        {
                            //
                            Console.WriteLine(Environment.NewLine + "<notes>");
                        }*/
                        // save every paragraph
                        //paragraphs.Add(SecurityElement.Escape(Paragraph.InnerText));
                        //Console.WriteLine(Paragraph.InnerText);




                        //Console.WriteLine(Paragraph.InnerText);
                    }
                }
                
                //Console.WriteLine(i);
                Console.WriteLine("done");
                return MainDocumentPart;
            }

        }

        

        public string GetIntro(int start, int stop, OpenXmlElementList Elements)
        {
            string intro = "";
            for (var x = start; x >= stop; x--)
            {
                if (Elements[x] is Paragraph)
                {
                    var p = (Paragraph)Elements[x];
                    intro = V4Template.RichText(SecurityElement.Escape(p.InnerText));
                    //Console.WriteLine(p.InnerText);
                }

            }
            return intro;
        }

        public List<string> GetNotes(int start, int stop, OpenXmlElementList Elements)
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
                    if (ParagraphStyleId != null && ParagraphStyleId.Val != null && ParagraphStyleId.Val.Value.ToLower().Contains("simpleblock"))
                    {
                        // custom note
                        notes.Add(V4Template.CustomNote(FormattedText));
                        paragraphs.Clear();

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
                            if (Paragraph.InnerText.Trim() != "" && Paragraph.InnerText.TrimEnd().Last() == ':')
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
                            // richtext
                            notes.Add(V4Template.RichText(FormattedText));
                            paragraphs.Clear();
                        }

                    }

                    // check for image
                    if (Paragraph != null && Paragraph.Descendants<Drawing>().Count() > 0)
                    {
                        Console.WriteLine("!!!image detected!!!");
                        notes.Add("<imageplaceholder/>");
                    }
                }

            }
            notes.Reverse();
            //var data = string.Join("", notes.ToArray());
            return notes;
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
                    if (x == start)
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
                            if (ListOrphans.Count > 0)
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
                if (Element is Run)
                {
                    Run Run = (Run)Element;
                    // has text
                    int textChild = Run.Descendants<Word.Text>().Count();

                    // group children
                    if (textChild > 0) {
                        string text = SecurityElement.Escape(Run.InnerText);

                        // identify code
                        if (Run.RunProperties != null && Run.RunProperties.RunFonts != null && Run.RunProperties.RunFonts.Ascii != null && Run.RunProperties.RunFonts.Ascii.Value == "Courier New")
                        {
                            text = V4Template.InLineCode(text);
                        }
                        else if (Run.RunProperties != null && text.Trim() != "") // prevent empty tags, should do this above but...
                        {
                            // Make text italic
                            if (Run.RunProperties.Italic != null && Run.RunProperties.Italic.Val != OnOffValue.FromBoolean(false))
                            {
                                text = V4Template.Italic(text);
                            }
                            // Make text bold
                            if (Run.RunProperties.Bold != null && Run.RunProperties.Bold.Val != OnOffValue.FromBoolean(false))
                            {
                                text = V4Template.Bold(text);
                            }
                            // Make text underlined
                            if (Run.RunProperties.Underline != null && Run.RunProperties.Underline.Val != OnOffValue.FromBoolean(false))
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
                        
                }
                else if (Element is Hyperlink)
                {
                    // handle links
                    Hyperlink Hyperlink = (Hyperlink)Element;
                    if (Hyperlink.Id != null)
                    {
                        var link = HyperlinkRelationships.FirstOrDefault(Tag => Tag.Id == Hyperlink.Id);
                        if(HyperlinkRelationships.Count() > 0 && link != null)
                        {
                            Para += V4Template.Hyperlink(SecurityElement.Escape(link.Uri.AbsoluteUri), SecurityElement.Escape(Hyperlink.InnerText));
                        }
                        else
                        {
                            Para += V4Template.Hyperlink(SecurityElement.Escape(Hyperlink.InnerText), SecurityElement.Escape(Hyperlink.InnerText));
                        }                        
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
            if (Paragraph.ParagraphProperties != null)
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
                if (NumberingInstance != null)
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
