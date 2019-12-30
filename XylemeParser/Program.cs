using System;
using System.IO;
using System.Security;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;
using msoTriState = Microsoft.Office.Core.MsoTriState;
using msoShapeType = Microsoft.Office.Core.MsoShapeType;
using msoThemeColor = Microsoft.Office.Core.MsoThemeColorIndex;
using msoTextOrientation = Microsoft.Office.Core.MsoTextOrientation;
using MessageBox = System.Windows.Forms.MessageBox;
using missing = System.Reflection.Missing;
using System.ComponentModel;
using ImageMagick;

namespace LessonParser
{
  
    class Program
    {
        public static Form1 UI = new Form1();
        public static Form2 renameDialog = new Form2();

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            //UI.renameDialog.ShowDialog();
            Application.Run(UI);
            //Application.Run(renameDialog);
            
        }

        public static string renamedFile = "";
        public static string filesLengthList = "";
        public static string ErrorList = "";
        public static bool hasErrors = false;
        public static LayoutXML LayoutXML = new LayoutXML();
        public static SlideUtil SlideUtil = new SlideUtil();
        public static int defaultPlaceholderCount = 1;
        public static bool newExportMethod = false;
        public static bool reverseObjects = true;
        public static int SlideScale = 1;


        public static string ReplaceInvalidChars(string s)
        {
            //s = SecurityElement.Escape(s);
            if (!string.IsNullOrEmpty(s))
                return s.Replace("\u2018", "&#x2018;").Replace("\u2019", "&#x2019;").Replace("\u201c", "&#x201c;").Replace("\u201d", "&#x201d;").Replace('\u000B', ' ').Replace('\u0001', ' ');
            else
                return s;
        }

        public static void PrepareDirectory(string directory)
        {
            // delete files
            if (Directory.Exists(@directory))
            {
                Array.ForEach(Directory.GetFiles(@directory), File.Delete);
                //foreach (System.IO.FileInfo file in Directory.GetFiles(@directory)) { file.Delete(); }
                //Directory.Delete(@SourceDirectory + "temp", true);
                //Directory.CreateDirectory(@SourceDirectory + "temp");
            }
            else {
                // create new directory for files
                Directory.CreateDirectory(@directory);
            }
        }

        public static void RunSplit(string courseCode, string courseVersion, string path) {

            try
            {

                // main code

                PowerPoint.Slides Slides;
                int SlidesCount;
                int SegueDistance = 0;
                string msg;

                // application
                PowerPoint.Application pptApp = new PowerPoint.Application();
                PowerPoint.Presentation pptPres = pptApp.Presentations.Open(path);
                pptApp.Windows[1].WindowState = PowerPoint.PpWindowState.ppWindowMinimized;

                // file locations
                string SourceDirectory = pptApp.ActivePresentation.Path + @"\";
                string MediaDirectory = SourceDirectory + @"media\";
                string XmlDirectory = SourceDirectory + @"xml\";

                Console.WriteLine(SourceDirectory);

                //prepare file folders
                PrepareDirectory(MediaDirectory);
                PrepareDirectory(XmlDirectory);
                //pptApp.StartNewUndoEntry();
                Slides = pptApp.ActivePresentation.Slides;
                SlidesCount = pptApp.ActivePresentation.Slides.Count;
                msg = " Total slides: " + SlidesCount;
                //MessageBox.Show(msg);

                string TopicName = "";
                string SubTopicName = "";

                var SlideNames = new List<String>();              

                for (int i = 1; i <= SlidesCount; i++)
                {
                    var Shapes = new List<Shape>();
                    //Console.WriteLine("\n//* Slide: {0} *//", i);
                    // get slide object
                    PowerPoint.Slide slideObject = pptApp.ActivePresentation.Slides[i];

                    // hide logo and copyright to prevent cropping issues
                    slideObject.DisplayMasterShapes = msoTriState.msoFalse;
                    
                    Console.WriteLine("\n//* Slide: {0}  Layout: {1} shapes: {2} *//", slideObject.SlideIndex, slideObject.CustomLayout.Name, slideObject.CustomLayout.Shapes.Placeholders.Count);
                    //slideObject.Master.Design.SlideMaster.HeadersFooters.Footer.Visible = msoTriState.msoFalse;

                    if (slideObject.CustomLayout.Name.ToLower().Contains("segue"))
                    {
                        // track empty placeholders
                        defaultPlaceholderCount = slideObject.CustomLayout.Shapes.Placeholders.Count - 1;
                        //Console.WriteLine(defaultPlaceholderCount);
                    }
                        

                    // get shape (object) in the slide
                    PowerPoint.Shapes shapesObject = slideObject.Shapes;
                    //shapesObject.Range(0,1,2).
                    int shapesCount = shapesObject.Count;
                    //shapesObject.Placeholders[0].

                    if (shapesCount == 1 || slideObject.CustomLayout.Name.ToLower().Contains("segue"))
                    {
                        // slide title
                        TopicName = SlideUtil.GetSlideTitle(shapesObject);

                        //reset distance from segue slide
                        SegueDistance = 0;
                    }
                    else
                    {
                        
                        //vars
                        int nLists = 0;
                        int nTables = 0;
                        int nImages = 0;
                        string SlideXML = "";
                        List<string> sXML = new List<string>();
                        //Console.WriteLine("hyperlinks: " + slideObject.Hyperlinks.Count);
                        // SegueDistance
                        SegueDistance++;

                        // get title for slide
                        SubTopicName = SlideUtil.GetSlideTitle(shapesObject);

                        // store original slide title
                        var SlideTitle = ReplaceInvalidChars(SecurityElement.Escape(SubTopicName));

                        // get suitable slide file name
                        //string formattedSlideName = SlideUtil.GetSlideName(TopicName, SubTopicName);
                        string formattedSlideName = SlideUtil.GetSlideNameV4(SubTopicName);

                        // decrease filename length to match 99 char limit
                        int maxlength = 99 - (courseCode.Length + courseVersion.Length + 6);
                        formattedSlideName = SlideUtil.Truncate(formattedSlideName, maxlength);

                        // count the number of slides with the same name
                        int ContSlideCount = SlideUtil.CountMatches(SlideNames, formattedSlideName);

                        // increment the slide count by 1
                        int increment = ContSlideCount + 1;

                        // get full slide name based on naming convention
                        string currentSlideName = SlideUtil.GetFullFileName(courseCode, courseVersion, formattedSlideName, increment);

                        //Console.WriteLine("slide height: " + slideObject.CustomLayout.Height + " || slide width: " + slideObject.CustomLayout.Width);
                        //Console.WriteLine("title height: " + shapesObject.Title.Height);

                        // find the centre point of the slide content
                        int CanvasHeight_MidPoint = (int)(slideObject.CustomLayout.Height) / 2;
                        int CanvasWidth_MidPoint = (int)(slideObject.CustomLayout.Width) / 2;

                        // track the number of shapes
                        int nShapes = 0;
                        //
                        foreach (PowerPoint.Shape shapeItem in shapesObject)
                        {

                            var texFrameText = shapeItem.TextFrame;
                            var texFrameText2 = shapeItem.TextFrame2;
                            // identify title shape based on position from top of slide
                            if (shapesObject.Title.Top != shapeItem.Top)
                            {
                                nShapes++;
                                //Console.WriteLine("shape: " + shapeItem.Name + " || Top: " + shapeItem.Top + " || Left: " + shapeItem.Left + " || Height: " + shapeItem.Height);
                            }                            

                            // shape co-ordinates
                            var Shape = new Shape();
                            Shape.name = shapeItem.Name;
                            Shape.x1 = (int)shapeItem.Left;
                            Shape.y1 = (int)shapeItem.Top;
                            Shape.x2 = (int)shapeItem.Left + (int)shapeItem.Width;
                            Shape.y2 = (int)shapeItem.Top + (int)shapeItem.Height;
                            Shape.width = (int)shapeItem.Width;
                            Shape.height = (int)shapeItem.Height;
                            Shape.midX = Shape.x1 + (Shape.width / 2);
                            Shape.midY = Shape.y1 + (Shape.height / 2);

                            // first non title shape
                            if(nShapes == 1)
                            {
                                // compare midpoint of the shape to midpoint of slide
                                if(Shape.midX <= CanvasWidth_MidPoint || Shape.midY <= CanvasHeight_MidPoint)
                                {
                                    // shape should be on the top or left...should be
                                    // if not, xml should be reversed
                                    Console.WriteLine("\n ------no reversal needed-------");
                                    reverseObjects = false;
                                }
                                else
                                {
                                    reverseObjects = true;
                                }
                            }

                            //shapeItem.t
                            if (shapeItem.HasTextFrame == msoTriState.msoTrue)
                            {
                                if (shapeItem.TextFrame.HasText == msoTriState.msoTrue)
                                {
                                    // get all paragraphs in shape and identify lists
                                    PowerPoint.TextRange paragraphs = shapeItem.TextFrame.TextRange.Paragraphs(-1, -1);
                                    
                                    // get list xml
                                    if (SlideUtil.isList(paragraphs))
                                    {
                                        //var ListXMLArray = SlideUtil.GetListXML(paragraphs);                                        
                                        nLists++;

                                        /*var opt = "";                                        
                                        foreach (String s in ListXMLArray) {
                                            opt += s;
                                        }
                                        sXML.Add(LayoutXML.List(opt));*/

                                        // algorithm to handle lists
                                        // get the list data
                                        var ListData = SlideUtil.GenerateList(paragraphs);

                                        // get the list xml
                                        var ListXML = LayoutXML.List(SlideUtil.GetList(ListData));

                                        // remove empty xml items from list
                                        ListXML = SlideUtil.RemoveEmptyItems(ListXML);

                                        // fix bullet ndash issue
                                        ListXML = SlideUtil.SetListMarker(ListXML);

                                        // store list xml
                                        sXML.Add(ListXML);
                                    }
                                    else // get code xml
                                    {
                                        // xml code block
                                        if (SlideUtil.isCode(paragraphs)) {
                                            nTables++;
                                            // save xml file
                                            //File.WriteAllText(slideObject.SlideID + ".xml", SecurityElement.Escape(paragraphs.Text));
                                            // replace line spaces
                                            var formattedText = Regex.Replace(paragraphs.Text, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", Environment.NewLine);
                                            sXML.Add(LayoutXML.CodeTable(SecurityElement.Escape(formattedText)));
                                        }
                                        
                                    }

                                    

                                }

                            }
                            else // see what other stuff is available apart from text
                            {
                                // check for table
                                if (shapeItem.HasTable == msoTriState.msoTrue)
                                {
                                    //track number of tables on the slide
                                    nTables++;

                                    // get all paragraphs in shape if it exists
                                    PowerPoint.TextRange paragraphs = null;
                                    try
                                    {
                                        paragraphs = shapeItem.TextFrame.TextRange.Paragraphs(-1, -1);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("No valid textrange in this table!");
                                        paragraphs = null;
                                    }
                                    

                                    // check if table is code table
                                    if (paragraphs != null && SlideUtil.isCode(paragraphs))
                                    {
                                        // replace line spaces
                                        var formattedText = Regex.Replace(paragraphs.Text, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", Environment.NewLine);
                                        sXML.Add(LayoutXML.CodeTable(SecurityElement.Escape(formattedText)));
                                    }
                                    else
                                    {
                                        // get xyleme xml for table
                                        var TableXML = SlideUtil.GetTableXML(shapeItem);
                                        sXML.Add(TableXML);
                                    }
                                }
                                else
                                {
                                    // add images to xml
                                    if (nImages == 0)
                                    {
                                        // add formatted slide name to list
                                        SlideNames.Add(formattedSlideName);
                                        //v4 figure vs old format
                                        var ImageXML = LayoutXML.FigureV4(currentSlideName);
                                        sXML.Add(ImageXML);
                                    }
                                    // track potential image shapes
                                    nImages++;                                    
                                    //Console.WriteLine("shape type: "+shapeItem.AutoShapeType);
                                }
                            }

                            // list with shape data
                            Shapes.Add(Shape);
                        }

                        //if (i != 1 && !Regex.IsMatch(SubTopicName, "summary", System.Text.RegularExpressions.RegexOptions.IgnoreCase) && !Regex.IsMatch(SubTopicName, "objectives", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        if (i != 1 && !Regex.IsMatch(SubTopicName, "objectives", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                                                    
                            // check file lengths
                            int fileNameChars = currentSlideName.Length + 5;
                            if (fileNameChars > 99)
                            {
                                // show dialog with current slide filename
                                //renameDialog.richTextBox1.Text = currentSlideName;
                                if (currentSlideName.Length + 5 <= 99)
                                {
                                    //currentSlideName = currentSlideName.Replace("CiscoInformationServer", "CIS");
                                }
                                else
                                {
                                    renameDialog.RenamedFile = currentSlideName;
                                    renameDialog.ShowDialog();

                                    // update slide filename
                                    if (renamedFile.Length > 0)
                                    {
                                        currentSlideName = renameDialog.RenamedFile;
                                    }
                                }
                                                               

                            }

                            if (reverseObjects)
                            {
                                // reverse order of items
                                sXML.Reverse();
                            }
                            
                            // convert list to xml
                            SlideXML = string.Join("", sXML);                           

                            if(slideObject != null) {

                                // get complete slide xml data
                                //var sn = XmlDirectory + "Slide" + slideObject.SlideID + ".xml";
                                string SlideId = SlideUtil.MD5("Slide" + slideObject.SlideID.ToString());
                                //string SlideId = SlideUtil.MD5(SegueDistance + SubTopicName);
                                Console.WriteLine("from segue: " + SegueDistance);
                                Console.WriteLine("title: " + SubTopicName);
                                Console.WriteLine("sid: " + SlideId);
                                var sn = XmlDirectory + SlideId + ".xml";
                                var sdta = LayoutXML.MasterSlide(slideObject.CustomLayout.Name, nLists, nTables, nImages, slideObject.CustomLayout.Shapes.Placeholders.Count, SlideTitle, SlideXML, currentSlideName);
                                
                                // save xml file
                                File.WriteAllText(@sn, sdta);

                                if (nImages > 0)
                                {
                                    // export pptx and png
                                    //Export(@MediaDirectory, currentSlideName, slideObject, pptPres.Name);
                                    ExportNoPublish(@MediaDirectory, currentSlideName, slideObject, pptPres.Name);

                                    // log exported files
                                    fileNameChars = currentSlideName.Length;
                                    filesLengthList += "[" + fileNameChars + "]:" + currentSlideName + ".png" + Environment.NewLine;
                                }
                                    
                            }

                        }
                    }

                    // track progress
                    var progress = Math.Ceiling(((double)i / SlidesCount) * 100);
                    if(progress > 100)
                    {
                        progress = 100;
                    }
                    UI.backgroundWorker1.ReportProgress((int)progress);

                }

                //Console.ReadKey();
                //object saveChanges = false;
                pptPres.Saved = msoTriState.msoTrue;
                pptPres.Close();
                pptApp.Quit();
            }
            catch (Exception ex)
            {
                hasErrors = true;
                ErrorList += "Error: " + ex;
            }
        }

        // bold, underline, italics, code
        public static string applyFormatting( PowerPoint.TextRange Runs ) {

            // var
            string Paragraph = "";
            // find formats
            foreach (PowerPoint.TextRange Run in Runs)
            {
                // get hyperlink
                PowerPoint.Hyperlink Hyperlink = Run.ActionSettings[PowerPoint.PpMouseActivation.ppMouseClick].Hyperlink;
                
                // var
                var tx = Run.Text;
                //tx = Regex.Replace(tx, "\\s", "######");
                string text = ReplaceInvalidChars( SecurityElement.Escape( tx ) );

                // identify code
                if (Run.Font.Name.ToLower().Contains("courier")) {
                    Paragraph += LayoutXML.InLineCode(text);                    
                }
                // identify links
                else if (Hyperlink.Address != null)
                {
                    Paragraph += LayoutXML.Hyperlink(SecurityElement.Escape(Hyperlink.Address), SecurityElement.Escape(Hyperlink.TextToDisplay));
                }
                // other formatted text
                else if (Run.Font.Bold == msoTriState.msoTrue || Run.Font.Underline == msoTriState.msoTrue || Run.Font.Italic == msoTriState.msoTrue) 
                {
                    // bold, italic and underlined
                    if (Run.Font.Bold == msoTriState.msoTrue && Run.Font.Underline == msoTriState.msoTrue && Run.Font.Italic == msoTriState.msoTrue)
                    {
                        Paragraph += LayoutXML.Underline(LayoutXML.Bold(LayoutXML.Italic(text)));
                    }else
                    // bold and underlined
                    if (Run.Font.Bold == msoTriState.msoTrue && Run.Font.Underline == msoTriState.msoTrue) {
                        Paragraph += LayoutXML.Underline(LayoutXML.Bold(text));
                    }
                    else// bold and italic
                    if (Run.Font.Bold == msoTriState.msoTrue && Run.Font.Italic == msoTriState.msoTrue)
                    {
                        Paragraph += LayoutXML.Bold(LayoutXML.Italic(text));
                    }
                    else // italic and underlined
                    if (Run.Font.Italic == msoTriState.msoTrue && Run.Font.Underline == msoTriState.msoTrue)
                    {
                        Paragraph += LayoutXML.Underline(LayoutXML.Italic(text));
                    }
                    else {
                        // bold only
                        if (Run.Font.Bold == msoTriState.msoTrue)
                        {
                            Paragraph += LayoutXML.Bold(text);
                        }
                        // underline only
                        if (Run.Font.Underline == msoTriState.msoTrue)
                        {
                            Paragraph += LayoutXML.Underline(text);
                        }
                        // italic only
                        if (Run.Font.Italic == msoTriState.msoTrue)
                        {
                            Paragraph += LayoutXML.Italic(text);
                        }
                    }
                        
                }
                else // regular text
                {
                    Paragraph += text;
                }
            }
            return Paragraph.Trim();
        }

        public static int removeTextShapes(PowerPoint.Shapes Shapes) {

            int deleted = 0;
            // loop through shapes and remove text
            foreach (PowerPoint.Shape Shape in Shapes)
            {
                // check if text frame exists
                if (Shape.HasTextFrame == msoTriState.msoTrue)
                {
                    /// check if textframe has text in it
                    if (Shape.TextFrame.HasText == msoTriState.msoTrue)
                    {
                        //identify shape
                        //Console.WriteLine("\n"+shp.Type +": "+ shp.TextFrame.TextRange.Paragraphs(-1, -1).Text);

                        // try to delete text thats not linked to images
                        if (Shape.Type != msoShapeType.msoAutoShape && Shape.Type != msoShapeType.msoTextBox || Shape.TextFrame.TextRange.ParagraphFormat.Bullet.Type != PowerPoint.PpBulletType.ppBulletNone)
                        {
                            // delete the shape and log deletion                         
                            Shape.TextFrame.DeleteText();
                            //Shape.TextFrame2.DeleteText();
                            deleted++;
                        }

                    }
                }

            }

            // return the number of items deleted
            return deleted;
        }

        public static string toUpper(string text) {
            string[] wds = text.Split(' ');
            string txt = "";
            //
            foreach(var wd in wds)
            {
                if (!String.IsNullOrEmpty(wd))
                {
                    txt += wd.First().ToString().ToUpper() + wd.Substring(1);
                }
                
            }
            return txt;
        }

        public static string SlideXML(string data)
        {

            // return xml text
            return data;
        }

        public static string formatText( string data )
        {
            // capitalize first letter of each word
            data = toUpper(data);
            //data = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(data);

            //remove unwanted symbols
            Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            data = rgx.Replace(data, "");

            // return formatted text
            return data;
        }

        public static string CleanInvalidXmlChars(string StrInput)
        {
            //Returns same value if the value is empty.
            if (string.IsNullOrWhiteSpace(StrInput))
            {
                return StrInput;
            }
            // From xml spec valid chars:
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]    
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.
            string RegularExp = @"\s+";
            return Regex.Replace(StrInput, RegularExp, " ");
        }

        public static void Export(string url, string filename, PowerPoint.Slide slide, string pptName)
        {
            string pngFile = url + filename + ".png";
            string pptxFile = url + filename + ".pptx";
            string[] pName = pptName.Split('.');
            string publishedName = url + pName[0] + "_" + slide.SlideIndex.ToString().PadLeft(3, '0') + "." + pName[1];

            // publish current slide
            slide.PublishSlides(url, true, true);
            // rename slide and export image
            if (File.Exists(publishedName)) {
                // rename published slide
                File.Move(publishedName, pptxFile);

                if (newExportMethod)
                {
                    // new approach
                    newExportImage(pngFile, slide, pptxFile);
                }
                else {
                    // old approach
                    ExportImage(pngFile, slide, pptxFile);
                }
                
            }
        }

        public static void ExportNoPublish(string url, string filename, PowerPoint.Slide slide, string pptName)
        {
            string pngFile = url + filename + ".png";

            // export png img
            ExportImage2019(pngFile, slide);
        }

        public static void ExportImage(string file, PowerPoint.Slide slide, string pptxFile)
        {
            // remove text from slide
            int removed = removeTextShapes(slide.Shapes);

            // low quality image
            //slide.Export(file, "PNG");

            var scaleWidth = (int)slide.CustomLayout.Width * SlideScale;
            var scaleHeight = (int)slide.CustomLayout.Height * SlideScale;

            // high quality image
            //slide.Export(file, "PNG", scaleWidth, scaleHeight);
            slide.Export(file, "PNG", scaleWidth, scaleHeight);

            // trim white space
            trimImage(file, pptxFile);
        }

        public static void ExportImage2019(string file, PowerPoint.Slide slide)
        {
            // remove text from slide
            int removed = removeTextShapes(slide.Shapes);

            // low quality image
            //slide.Export(file, "PNG");

            var scaleWidth = (int)slide.CustomLayout.Width * SlideScale;
            var scaleHeight = (int)slide.CustomLayout.Height * SlideScale;

            // high quality image
            slide.Export(file, "PNG", scaleWidth, scaleHeight);

            // trim white space
            trimImage2019(file);
        }

        public static void newExportImage(string file, PowerPoint.Slide slide, string pptxFile)
        {
            // remove text from slide
            //int removed = removeTextShapes(slide.Shapes);
            //Console.WriteLine(Environment.NewLine+file );
            //slide.Shapes.Range(0).exp
            foreach (PowerPoint.Shape shapeItem in slide.Shapes)
            {
                //PowerPoint.SlideRange rng = 
                //shapeItem.Select();
                //slide.Select();
                //slide.Shapes.SelectAll();
                //slide.ran
                Console.WriteLine(shapeItem.Name);
                Console.WriteLine(shapeItem.Type + Environment.NewLine); 
                //shapeItem.               
            }
            //Console.WriteLine(slide.Shapes.Count);
            //slide.Export(file, "PNG");
            //trimImage(file, pptxFile);
        }

        public static void trimImage(string ImageFile, string pptxFile)
        {
            // Read from file.
            using (MagickImage img = new MagickImage(ImageFile))
            {
                //chop footer 16:9 slides, may chop images overflowing in footer
                //img.ChopVertical(509, 26);
                //4:3
                //img.ChopVertical(700, 20);

                // trim
                img.Trim();   
                            
                // Save the result
                img.Write(ImageFile);
            }
            
            // open trimmed image and check dimensions
            var newimg = new MagickImage(ImageFile);
            // check if image is blank and delete
            if (newimg.BaseHeight == 1 && newimg.BaseWidth == 1) {
                //Console.WriteLine(ImageFile + ": " + newimg.BaseHeight + "x" + newimg.BaseWidth);
                File.Delete(ImageFile);
                File.Delete(pptxFile);
            }
                       
        }

        public static void trimImage2019(string ImageFile)
        {
            // Read from file.
            using (MagickImage img = new MagickImage(ImageFile))
            {
                //chop footer 16:9 slides, may chop images overflowing in footer
                //img.ChopVertical(509, 26);
                //4:3
                //img.ChopVertical(700, 20);

                // trim
                img.Trim();

                // Save the result
                img.Write(ImageFile);
            }

            // open trimmed image and check dimensions
            var newimg = new MagickImage(ImageFile);
            // check if image is blank and delete
            if (newimg.BaseHeight == 1 && newimg.BaseWidth == 1)
            {
                //Console.WriteLine(ImageFile + ": " + newimg.BaseHeight + "x" + newimg.BaseWidth);
                File.Delete(ImageFile);
            }

        }

        /*public string XmlSlide(String ImageName)
        {
            //string data = "<Figure xy:guid=\"\" xy:schema-version=\"Core/Definitions/Figure.xsd#core-3.9-0/cisco-3.9-0\"><MediaObject xy:guid=\"\" xy:schema-version=\"Core/Definitions/Figure.xsd#core-3.9-0/cisco-3.9-0\"><Renditions><Web thumbWidth=\"50\" uri=\"DataCenter/ProductTraining/" + this.CourseCode + "/SG/" + this.ModuleCode + "/" + this.LessonCode + "/" + this.CourseCode + "_1-0_" + ImageName + ".png\"/><Source>DataCenter/ProductTraining/" + this.CourseCode + "/SG/" + this.ModuleCode + "/" + this.LessonCode + "/" + this.CourseCode + "_1-0_" + ImageName + ".pptx</Source></Renditions></MediaObject></Figure>";
            return data;
        }*/
    }
}

