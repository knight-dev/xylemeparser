using System;
using System.Xml;
using System.Security;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonParser
{
    class LayoutXML
    {
        public int ExpectedElements = 1;
        public int ListElements = 0;
        public int TableElements = 0;
        //public Form1 ui = new Form1();
        public String CourseAcronym, vNumber, moduleNumber, lessonNumber;
                
        public string List(String Content, String Preamble = null)
        {
            //NB!! v4 hides ppt lists!! check filter
            string data = "";
            if (Preamble == null)
            {
                data = "<List ListMarker=\"Bullet\"><ItemBlock>" + Content + "</ItemBlock></List>";
            }
            else
            {
                data = "<List ListMarker=\"Bullet\"><ListPreamble>" + Preamble + "</ListPreamble><ItemBlock>" + Content + "</ItemBlock></List>";
            }
            return data;
        }
        public string ListItem(String Data)
        {
            string data = "<Item><ItemPara>" + Data + "</ItemPara></Item>";
            return data;
        }
        public string SubListStart(String ListText, String ListMarker = "nDash")
        {
            string data = "<Item><ItemPara>" + ListText + "</ItemPara><SubList ListMarker=\"" + ListMarker + "\"><ItemBlock>";
            return data;
        }
        public string SubListEnd()
        {
            string data = "</ItemBlock></SubList></Item>";
            return data;
        }
        public string SubList(String Data, String Content, String ListMarker = "nDash")
        {
            string data = "<Item><ItemPara>" + Data + "</ItemPara><SubList ListMarker=\"" + ListMarker + "\"><ItemBlock>" + Content + "</ItemBlock></SubList></Item>";
            return data;
        }
        public string Figure(String Filename)
        {             
            string data = "<Figure><MediaObject><Renditions><Web uri=\"Cloud/ProductTraining/" + CourseAcronym + "/v2.0" +  "/Images/Concepts/" + moduleNumber + "/"+ lessonNumber + "/" + Filename + ".png\"" + " thumbWidth = \"50\"/><Source>Cloud/ProductTraining/" + CourseAcronym + "/v2.0" + "/Images/Concepts/" + moduleNumber + "/" + lessonNumber + "/" + Filename + ".pptx" + "</Source></Renditions></MediaObject></Figure>";
            return data;
        }
        public string FigureV4(String Filename)
        {
            string data = "<Figure><MediaObject><Renditions><Web thumbWidth=\"50\" uri=\"_PORTFOLIO_2.0/Security/Level_200/Concentrations/" + CourseAcronym + "/v1.0/" + Filename + ".png\"/></Renditions></MediaObject></Figure>";
            return data;
        }
        public string Table(String Columns, String HeaderContent, String BodyContent)
        {
            string data = "<Table align=\"center\" border=\"true\" width=\"100\"><TblGroup>" + Columns + TableHeader(HeaderContent) + TableBody(BodyContent) + "<TblFooter/></TblGroup></Table>";
            return data;
        }
        public string TableHeader(String TableRow)
        {
            string data = "<TblHeader>" + TableRow + "</TblHeader>";
            return data;
        }
        public string TableBody(String TableRows)
        {
            string data = "<TblBody>" + TableRows + "</TblBody>";
            return data;
        }
        public string TableRow(String TableCells)
        {
            string data = "<TableRow>"+ TableCells + "</TableRow>";
            return data;
        }
        public string TableCell(String Content)
        {
            string data = "<Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\">" + RichText(Content) + "</Cell>";
            return data;
        }
        public string TableCodeCell(String Content)
        {
            string data = "<Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><Code>" + Content + "</Code></Cell>";
            return data;
        }
        public string RichText(String Content)
        {
            string data = "<RichText>" + Content + "</RichText>";
            return data;
        }

        // text formatting
        public string Bold(String Content)
        {
            string data = "<Emph>" + Content + "</Emph>";
            return data;
        }
        public string Italic(String Content)
        {
            string data = "<Italic>" + Content + "</Italic>";
            return data;
        }
        public string Underline(String Content)
        {
            string data = "<Underline>" + Content + "</Underline>";
            return data;
        }
        public string InLineCode(String Content)
        {
            string data = "<InLineCode>" + Content + "</InLineCode>";
            return data;
        }

        public string Hyperlink(String Url, String Name)
        {
            string data = "<Href UrlTarget=\"" + Url + "\">" + Name + "</Href>";
            return data;
        }

        public string CodeTable(String Content)
        {
            string data = "<Table align=\"left\" border=\"false\" width=\"100\"><TblGroup><TblCol width=\"\"/><TblHeader/><TblBody><TableRow><Cell align=\"left\" cell-bg=\"white\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><Code>" + Content + "</Code></Cell></TableRow></TblBody><TblFooter/></TblGroup></Table>";
            return data;
        }

        public string MasterSlide(String Theme, int Lists, int Tables, int Images, int ExpectedElements, String Title,  String XMLContent, String currentSlideName)
        {
            string data = "";
            Console.WriteLine("lists: {0}, tables: {1}, exp: {2}", Lists, Tables, ExpectedElements);
            Console.WriteLine(Program.defaultPlaceholderCount);
            // calculate expected elements, make sure its greater than one in case the wrong layouts are used
            //ExpectedElements = ExpectedElements - Program.defaultPlaceholderCount;
            ExpectedElements = ExpectedElements - 1;
            /*if (ExpectedElements > 2 && Theme.Contains("BodySlide")) {
                ExpectedElements = ExpectedElements - 3;
            }else if (ExpectedElements == 2 && Theme.Contains("BodySlide"))
            {
                ExpectedElements = ExpectedElements - 1;
            }
            else if (ExpectedElements > 1)
            {
                ExpectedElements = ExpectedElements - 3;
            }*/

            int FoundElements = (Lists + Tables + Images);
            //

            if (ExpectedElements == FoundElements)
            {
                // add an extra image in case slide layout is wrong
                /*if (!XMLContent.Contains("<Figure>") && !Theme.Contains("BodySlide")) {
                    XMLContent += Figure(currentSlideName);
                }*/
                // final xml
                data = "<Slide slideTheme=\"" + Theme.Replace("Title Only", "BodySlide").Replace("Title and Content", "BodySlide") + "\"><Title>" + Title + "</Title><Body>" + XMLContent + "</Body></Slide>";

            }
            else if(ExpectedElements > FoundElements)
            {
                // calculate and add images
                //int nImages = ExpectedElements - FoundElements;
                for(var x=0; x < Images; x++)
                {
                    XMLContent += FigureV4(currentSlideName);
                }
                // final xml
                data = "<Slide slideTheme=\"" + Theme.Replace("Title Only", "BodySlide").Replace("Title and Content", "BodySlide") + "\"><Title>" + Title + "</Title><Body>" + XMLContent + "</Body></Slide>";

            }
            else if (ExpectedElements < FoundElements)
            {
                // calculate and add images
                //int nImages = ExpectedElements - (Lists + Tables);
                /*XMLContent += Figure(currentSlideName);

                // add an extra image in case slide layout is wrong
                if (!XMLContent.Contains("<Figure>") && !Theme.Contains("BodySlide"))
                {
                    XMLContent += Figure(currentSlideName);
                }*/

                // final xml
                data = "<Slide slideTheme=\"" + Theme.Replace("Title Only", "BodySlide").Replace("Title and Content", "BodySlide") + "\"><Title>" + Title + "</Title><Body>" +  XMLContent + "</Body></Slide>";

            }

            //return final outptut
            //data = "<Slide slideTheme=\"" + Theme + "\"><Title>" + Title + "</Title><Body>" + XMLContent + "</Body></Slide>";
            return data;
        }
        /*public string ShortTall(String Title, int Lists, int Tables, String Content)
        {
            ExpectedElements = 2;
            string data = "<Slide slideTheme=\"ShortTall\" xy:guid=\"\"><Title>" + Title + "</Title><Body>" + Content + "</Body></Slide>";
            return data;
        }*/
    }
}
