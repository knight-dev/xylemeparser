using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;
using msoTriState = Microsoft.Office.Core.MsoTriState;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Security;

namespace LessonParser
{

    class Shape
    {
        public string name
        {
            get; set;
        }

        public int x1
        {
            get; set;
        }

        public int x2
        {
            get; set;
        }

        public int midX
        {
            get; set;
        }

        public int y1
        {
            get; set;
        }

        public int y2
        {
            get; set;
        }

        public int midY
        {
            get; set;
        }

        public int width
        {
            get; set;
        }

        public int height
        {
            get; set;
        }
    }

    class ListItem
    {
        public string value
        {
            get; set;
        }

        public int level
        {
            get; set;
        }
    }


    class SlideUtil
    {
        public string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
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
        }

        public string RemoveLineEndings(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            var xml = value.Replace(@"\s+", "5");
            xml = xml.Replace("\f", string.Empty);
            xml = xml.Replace("\r\n", string.Empty);
            xml = xml.Replace("\n", string.Empty);
            xml = xml.Replace("\r", string.Empty);
            xml = xml.Replace(lineSeparator, string.Empty);
            xml = xml.Replace(paragraphSeparator, string.Empty);
            return xml;
        }

        private static LayoutXML LayoutXML = new LayoutXML();

        public string GetSegueTitle(PowerPoint.Shapes shapesObject)
        {
            string Title = null;

            // get single shape item presumed to be the title
            PowerPoint.Shape shapeItem = shapesObject[1];

            // ensure title isn't empty
            if (shapeItem.HasTextFrame == msoTriState.msoTrue)
            {
                if (shapeItem.TextFrame.HasText == msoTriState.msoTrue)
                {
                    // slide title and type
                    Title = shapeItem.TextFrame.TextRange.Text;
                }
            }

            return Title;
        }

        public string GetSlideTitle(PowerPoint.Shapes shapesObject)
        {
            
            if (shapesObject.HasTitle == msoTriState.msoTrue)
            {
                if (shapesObject.Title.HasTextFrame == msoTriState.msoTrue)
                {
                    if (shapesObject.Title.TextFrame.HasText == msoTriState.msoTrue)
                    {
                        return shapesObject.Title.TextFrame.TextRange.Text;
                    }
                }
            }
            else
            {
                foreach(PowerPoint.Shape Shape in shapesObject)
                {
                    if (Shape.Name.Contains("Title"))
                    {
                        return Shape.TextFrame.TextRange.Text;
                    }
                }
            }

            return "";
        }

        public string GetSlideName(String SegueTitle, String SlideTitle)
        {
            string SlideName = "";
            if (SegueTitle.Length > 0 && SlideTitle.Length > 0)
            {
                SlideName = RemoveInvalidCharacters(UpperCase(SegueTitle)) + "_" + RemoveInvalidCharacters(UpperCase(SlideTitle));
                //SlideName = RemoveInvalidCharacters(SlideTitle);
            }
            return SlideName;
        }

        public string GetSlideNameV4(String SlideTitle)
        {
            string SlideName = "";
            if (SlideTitle.Length > 0)
            {
                SlideName = RemoveInvalidCharacters(SlideTitle);
            }
            return SlideName;
        }

        public string GetFullFileName(String Acronym, String Version, String FileName, int Increment)
        {
            if (Increment == 2) {
                Increment = 1;
                return Acronym + "_" + Version + "_" + FileName + "_" + "Cont" + "_" + Increment.ToString().PadLeft(3, '0') + "-sketch";
            }
            // else
            return Acronym + "_" + Version + "_" + FileName + "_" + Increment.ToString().PadLeft(3, '0') + "-sketch";
        }

        public string Truncate(string value, int maxLength)
        {
            Console.WriteLine("/**truncate: ***/");
            Console.WriteLine(value);
            Console.WriteLine("maxlength: " + maxLength);
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public bool isContSlide(String FileName, String PrevFileName)
        {
            // check for continuation slides
            if (FileName == PrevFileName)
            {
                return true;
            }
            return false;
        }
        public string RemoveInvalidCharacters(String text)
        {
            string output = text;
            // remove (cont) from name
            if (Regex.IsMatch(text, "(Cont.)", RegexOptions.IgnoreCase))
            {
                text = Regex.Replace(text, "(Cont.)", "Cont", RegexOptions.IgnoreCase);
            }

            text = text.Trim();
            // remove extraneous symbols
            if (Regex.IsMatch(text, "[^a-zA-Z0-9]"))
            {
                string outp = Regex.Replace(text, "[^a-zA-Z0-9]", " ");
                string txt = outp.Trim();
                string tx = Regex.Replace(txt, "[^a-zA-Z0-9]", "_");
                output = Regex.Replace(tx, "_+", "_");
            }

            /*// replace spaces
            if (Regex.IsMatch(text, "\\s"))
            {
                output = Regex.Replace(text.Trim(), "\\s", "_");
            }*/

            return output;
        }

        public string UpperCase(string text)
        {
            string[] words = text.Split(' ');
            string txt = "";

            // Make first letter of each word uppercase
            foreach (var word in words)
            {
                if (!String.IsNullOrEmpty(word))
                {
                    txt += word.First().ToString().ToUpper() + word.Substring(1);
                }

            }
            return txt;
        }

        public int CountMatches(List<string> list, string value)
        {
            int matches = 0;
            for (int i = 0; i < list.Count; i++)
            {
                // compare value to list item
                if (list[i].ToLower() == value.ToLower())
                {
                    matches++;
                }
            }
            return matches;
        }

        public bool isList(PowerPoint.TextRange paragraphs)
        {
            if (paragraphs.ParagraphFormat.Bullet.Visible == msoTriState.msoTrue)
            {
                return true;
            }
            return false;
        }
        // quick code identification
        public bool isCode(PowerPoint.TextRange TextRange)
        {
            if (TextRange != null && TextRange.Font != null)
            {
                if (TextRange.Font.Name != null && TextRange.Font.Name.Trim().Length > 0)
                {
                    // identify code
                    if (TextRange.Font.Name.ToLower().Contains("courier"))
                    {
                        return true;
                    }
                }
                else
                {
                    
                    foreach(PowerPoint.TextRange run in TextRange.Runs(-1, -1))
                    {
                        if (run.Font.Name != null && run.Font.Name.Trim().Length > 0)
                        {
                            // identify code
                            if (run.Font.Name.ToLower().Contains("courier"))
                            {
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }

        // table data to xml
        public string GetTableXML(PowerPoint.Shape Shape)
        {
            string tablehead = "";
            string tablebody = "";
            string tablecols = "";
            float colWidthSum = 0;
            float colWidth = 0;

            // find total col widths
            for (var col = 1; col <= Shape.Table.Columns.Count; col++)
            {
                colWidthSum = Shape.Table.Columns[col].Width + colWidthSum;
            }
            // important to add correct number of cols or table won't show in xyleme
            for (var v = 1; v <= Shape.Table.Columns.Count; v++)
            {
                colWidth = Shape.Table.Columns[v].Width / colWidthSum;
                tablecols += "<TblCol width=\"" + Math.Round(colWidth * 100) + "\"/>";
            }

            // single row table
            if(Shape.Table.Rows.Count == 1)
            {
                tablehead = "";

                // get data in each cell
                string cells = "";
                //string code = "";

                for (var x = 1; x <= Shape.Table.Rows[1].Cells.Count; x++)
                {
                    // is this code?
                    if (isCode(Shape.Table.Rows[1].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1)))
                    {
                        // replace line spaces
                        var formattedText = Regex.Replace(Shape.Table.Rows[1].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1).Text, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", Environment.NewLine);
                        cells += LayoutXML.TableCodeCell(SecurityElement.Escape(formattedText));
                    }
                    else
                    {
                        string itemText = Program.applyFormatting(Shape.Table.Rows[1].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1));
                        cells += LayoutXML.TableCell(itemText);
                    }

                    
                }
                tablebody += LayoutXML.TableRow(cells);
            }
            else // multi row tables
            {
                // get table data
                for (var a = 1; a <= Shape.Table.Rows.Count; a++)
                {
                    // get data in each cell
                    string cells = "";
                    for (var x = 1; x <= Shape.Table.Rows[a].Cells.Count; x++)
                    {
                        // is this code?
                        if (isCode(Shape.Table.Rows[a].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1)))
                        {
                            // replace line spaces
                            var formattedText = Regex.Replace(Shape.Table.Rows[a].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1).Text, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", Environment.NewLine);
                            cells += LayoutXML.TableCodeCell(SecurityElement.Escape(formattedText));
                        }
                        else
                        {
                            string itemText = Program.applyFormatting(Shape.Table.Rows[a].Cells[x].Shape.TextFrame.TextRange.Runs(-1, -1));
                            cells += LayoutXML.TableCell(itemText);
                        }
                    }

                    // insert row data
                    if (a == 1)
                    {
                        tablehead = LayoutXML.TableRow(cells);
                    }
                    else
                    {
                        tablebody += LayoutXML.TableRow(cells);
                    }
                }
            }
            

            // return table xml with data
            return LayoutXML.Table(tablecols, tablehead, tablebody);            
        }

        public string RemoveEmptyItems(String ListXML)
        {
            // parse list xml
            XElement Xml = XElement.Parse(ListXML);
            XName SubList = XName.Get("SubList");
            //Xml.Descendants(SubList).Where(e => e.HasAttributes)

            // error var
            bool hasErrors = false;

            // remove all empty items
            Xml.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();

            // parse new xml string
            var x = XDocument.Parse(Xml.ToString());
            
            // Create schema set
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", @"..\..\XSDs\ContentPackages\Core\Definitions\List.xsd");

            // Validate
            x.Validate(schemas, (o, e) =>
            {
                hasErrors = true;
                // log errors
                Console.WriteLine("{0}", e.Message);
            });

            // avoid returning invalid xml
            if (hasErrors)
            {
                return ListXML;
            }

            return x.ToString(SaveOptions.DisableFormatting);
                        
        }

        public string SetListMarker(String ListXML)
        {
            // parse list xml
            XElement Xml = XElement.Parse(ListXML);
            XName SubList = XName.Get("SubList");
            XName ListMarker = XName.Get("ListMarker");
            //Xml.Descendants(SubList).Where(e => e.HasAttributes)

            // nesting level of sublist
            int nesting = 0;

            // error var
            bool hasErrors = false;

            // remove all empty items
            IEnumerable<XElement> Sublists = Xml.Descendants(SubList).Where(e => e.HasAttributes);

            foreach(XElement SubLi in Sublists)
            {
                // find out how deeply nested sublist is
                var ParentLists = SubLi.Ancestors(SubList);
                nesting = ParentLists.Count();

                // assign sublist marker based on nesting
                //var Marker = nesting % 2 == 0 ? "Bullet" : "nDash";
                var Marker = nesting % 2 == 0 ? "nDash" : "Bullet";
                SubLi.Attribute(ListMarker).Value = Marker;

                //
                //Console.WriteLine("parent elements: " + nesting);
                //Console.WriteLine(SubLi);
            }

            
            // parse new xml string
            var x = XDocument.Parse(Xml.ToString());

            // Create schema set
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", @"..\..\XSDs\ContentPackages\Core\Definitions\List.xsd");

            // Validate
            x.Validate(schemas, (o, e) =>
            {
                hasErrors = true;
                // log errors
                Console.WriteLine("{0}", e.Message);
            });

            // avoid returning invalid xml
            if (hasErrors)
            {
                return ListXML;
            }

            return x.ToString(SaveOptions.DisableFormatting);

        }

        public List<ListItem> GenerateList(PowerPoint.TextRange Content)
        {
            // list variable            
            List<ListItem> List = new List<ListItem>();

            // start from high to low
            int start = Content.Count;
            int stop = 1;

            List<string> paragraphs = new List<string>();
            List<string> ListOrphans = new List<string>();
            // loop through content
            for (var x = start; x >= stop; x--)
            {
                // new list item for each paragraph
                ListItem ListItem = new ListItem();

                // Ensure paragraph is text
                if (Content.Paragraphs(x, 1) is PowerPoint.TextRange)
                {
                    // variables
                    var thisParagraph = Content.Paragraphs(x, 1);

                    // format runs
                    var itemText = thisParagraph.Runs(-1, -1);
                    var FormattedText = Program.applyFormatting(itemText);

                    if (!String.IsNullOrWhiteSpace(itemText.Text))
                    {
                        // store list data
                        ListItem.value = RemoveLineEndings(FormattedText);
                        ListItem.level = thisParagraph.IndentLevel;

                        // add to main list
                        List.Add(ListItem);
                    }                                        
                }

            }

            // restore normal paragraph order
            List.Reverse();

            // List
            return List;
        }

        public string GetList(List<ListItem> List)
        {
            // start from high to low
            int start = List.Count - 1;
            int stop = 0;

            List<string> paragraphs = new List<string>();
            List<string> ListOrphans = new List<string>();

            for (var x = start; x >= stop; x--)
            {
                // variables
                ListItem thisParagraph = List.ElementAt(x);
                // format runs
                var FormattedText = thisParagraph.value;
                //var FormattedText = Program.applyFormatting(itemText);

                //
                if (x == start)
                {
                    // save list item
                    paragraphs.Add(LayoutXML.ListItem(FormattedText));

                }
                else
                {
                    // previous paragraph
                    ListItem prevParagraph = List.ElementAt(x + 1);

                    // compare list levels to handle sublists
                    if (thisParagraph.level < prevParagraph.level)
                    {
                        // assign sublist marker based orphan count (a reflection of sublist number)
                        var ListMarker = "Bullet"; // SetListMarker function handles this issue
                                                   //var ListMarker = ListOrphans.Count % 2 == 0 ? "Bullet" : "nDash";

                        // restore order before conversion to string
                        paragraphs.Reverse();
                        var items = string.Join("", paragraphs.ToArray());

                        // create sublist
                        var sublist = LayoutXML.SubList(FormattedText, items, ListMarker);


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
                    else if (thisParagraph.level > prevParagraph.level)
                    {
                        // restore order before conversion to string
                        paragraphs.Reverse();
                        var items = string.Join("", paragraphs.ToArray());

                        // add to orphan list for later use
                        ListOrphans.Add(items);

                        // reset paragraphs to prevent duplicates
                        paragraphs.Clear();

                        // save list item
                        paragraphs.Add(LayoutXML.ListItem(FormattedText));


                    }
                    else
                    {

                        // save list item
                        paragraphs.Add(LayoutXML.ListItem(FormattedText));

                    }


                }
            }
                // restore normal paragraph order
                paragraphs.Reverse();
                var ListItems = string.Join("", paragraphs.ToArray());
                //Console.WriteLine(string.Join("", paragraphs.ToArray()) + Environment.NewLine);
                return ListItems;
            
        }

        public List<string> GetListXML(PowerPoint.TextRange paragraphs)
        {
            List<string> ListArray = new List<string>();
            string subListMarker = "nDash";
            int inSublist = 0;
            int count = 0;

            //loop
            foreach (PowerPoint.TextRange thisParagraph in paragraphs)
            {
                count++;
                //
                // format runs
                var itemText = Program.applyFormatting(thisParagraph.Runs(-1, -1));

                // first bullet item
                if(count == 1)
                {
                    // next paragraph
                    PowerPoint.TextRange nextParagraph = paragraphs.Paragraphs(count + 1, 1);
                    //
                    if (nextParagraph.IndentLevel == thisParagraph.IndentLevel) {
                        // add list item to array
                        ListArray.Add(LayoutXML.ListItem(itemText));
                    }
                    else if (nextParagraph.IndentLevel > thisParagraph.IndentLevel)
                    {
                        // first sublist marker always ndash
                        // add sublist item to array
                        ListArray.Add(LayoutXML.SubListStart(itemText, subListMarker));
                        inSublist++;
                    }
                }
                else if(count == paragraphs.Count) // last bullet item
                {
                    // previous paragraph
                    PowerPoint.TextRange prevParagraph = paragraphs.Paragraphs(count - 1, 1);
                    //
                    if (prevParagraph.IndentLevel == thisParagraph.IndentLevel)
                    {
                        // add list item to array
                        ListArray.Add(LayoutXML.ListItem(itemText));
                        // check for unclosed sublists and close
                        if(inSublist > 0)
                        {
                            for (var x = 0; x < inSublist; x++)
                            {
                                ListArray.Add(LayoutXML.SubListEnd());
                            }                                
                        }
                    }
                    else if (prevParagraph.IndentLevel > thisParagraph.IndentLevel)
                    {
                        // end sublist and add item to array
                        ListArray.Add(LayoutXML.ListItem(itemText));
                        // check for unclosed sublists and close
                        if (inSublist > 0)
                        {
                            for (var x = 0; x < inSublist; x++)
                            {
                                ListArray.Add(LayoutXML.SubListEnd());
                            }
                        }
                    }
                    else if (prevParagraph.IndentLevel < thisParagraph.IndentLevel)
                    {
                        // add list item to array and end sublist
                        ListArray.Add(LayoutXML.ListItem(itemText));
                        // check for unclosed sublists and close
                        if (inSublist > 0)
                        {
                            for (var x = 0; x < inSublist; x++)
                            {
                                ListArray.Add(LayoutXML.SubListEnd());
                            }
                        }
                        
                    }
                }
                else
                {
                    // next paragraph
                    PowerPoint.TextRange nextParagraph = paragraphs.Paragraphs(count + 1, 1);
                    //
                    if (nextParagraph.IndentLevel == thisParagraph.IndentLevel)
                    {
                        // add list item to array
                        ListArray.Add(LayoutXML.ListItem(itemText));
                    }
                    else if (nextParagraph.IndentLevel > thisParagraph.IndentLevel)
                    {
                        // choose sublist marker based on indent level
                        if (inSublist % 2 == 0) { subListMarker = "nDash"; } else if (inSublist % 2 == 1) { subListMarker = "Bullet"; }
                        // add sublist item to array
                        ListArray.Add(LayoutXML.SubListStart(itemText, subListMarker));
                        inSublist++;
                    }
                    else if (nextParagraph.IndentLevel < thisParagraph.IndentLevel)
                    {
                        // find the difference in indent levels between the current and next paragraph
                        var levelDiff = thisParagraph.IndentLevel - nextParagraph.IndentLevel;
                        // add list item to array and end sublist
                        ListArray.Add(LayoutXML.ListItem(itemText));
                        // close sublists based on level difference
                        for (var x = 0; x < levelDiff; x++)
                        {
                            ListArray.Add(LayoutXML.SubListEnd());
                            inSublist--;
                        }
                        
                    }
                }
            }

            return ListArray;
        }

        public List<string> GetListXMLold(PowerPoint.TextRange paragraphs)
        {
            List<string> arr = new List<string>();
            List<string> pr = new List<string>();
            int x = 0;
            int lastIndentLevel = 1;
            int currentIndentLevel = 1;
            string sublist = "";
            string list = "";
            int inSublist = 0;
            int t = 0;
            string lastListItem;

            //loop
            foreach (PowerPoint.TextRange paragraph in paragraphs)
            {
                t++;
                // only insert paragraphs with actual text
                if (paragraph.TrimText().Characters(-1, -1).Count > 0)
                {
                    // trim whitespace, make safe for xml
                    //var itemText = paragraph.TrimText().Text;
                    var itemText = Program.applyFormatting(paragraph.Runs(-1, -1));
                    bool wasBullet = false;

                    // account for the possibility of lists not starting at one (1)
                    if (x == 0 && paragraph.IndentLevel > 1)
                    {
                        currentIndentLevel = paragraph.IndentLevel - 1;
                    }
                    else
                    {
                        currentIndentLevel = paragraph.IndentLevel;
                    }

                    if (lastIndentLevel < currentIndentLevel && currentIndentLevel % 2 == 0) // currentIndentLevel % 2 == 0
                    {
                        //string substring;
                        // start ndash sublist                                     
                        //var substring = arr[x - 1].Substring(0, arr[x - 1].Length - 7);
                        var substring = arr[x - 1].Substring(0, arr[x - 1].Length - 7);
                        arr[x - 1] = substring + "<SubList ListMarker=\"nDash\"><ItemBlock>" + LayoutXML.ListItem(itemText);
                        inSublist++;
                    }
                    else if (lastIndentLevel < currentIndentLevel && currentIndentLevel % 2 == 1) // currentIndentLevel % 2 == 1
                    {
                        //string substring;
                        // start bullet sublist
                        //if (x == 0) { substring = arr[0].Substring(0, arr[0].Length - 7); } else { substring = arr[x - 1].Substring(0, arr[x - 1].Length - 7); }
                        var substring = arr[x - 1].Substring(0, arr[x - 1].Length - 7);
                        arr[x - 1] = substring + "<SubList ListMarker=\"Bullet\"><ItemBlock>" + LayoutXML.ListItem(itemText);
                        inSublist++;
                    }
                    else if (lastIndentLevel > currentIndentLevel)
                    {
                        //
                        //for (var l = 0; l < inSublist; l++) {
                        arr[x - 1] += "</ItemBlock></SubList></Item>";
                        //}
                        // add list item    
                        arr.Add(LayoutXML.ListItem(itemText));
                        x++;
                        inSublist--;
                    }
                    else if (lastIndentLevel == currentIndentLevel)
                    {
                        // add list item
                        arr.Add(LayoutXML.ListItem(itemText));
                        x++;
                    }


                    lastIndentLevel = currentIndentLevel;
                    lastListItem = itemText;
                    //insertP(wDoc, paragraph);
                }

                // check for any unclosed sublists
                if (paragraphs.Count == t && inSublist > 0)
                {
                    //Console.WriteLine(inSublist);
                    for (var l = 0; l < inSublist; l++)
                    {
                        arr[x - 1] += "</ItemBlock></SubList></Item>";
                    }

                    //Console.WriteLine("last item: "+paragraph.Text);
                }
            }

            return arr;
        }
    }
}
