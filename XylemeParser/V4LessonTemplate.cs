using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonParser
{
    class V4LessonTemplate
    {
        public String CourseCode = "VSIMP10";
        public String ModuleCode = "M01";
        public String LessonCode = "L07";

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

        public string Superscript(String Content)
        {
            string data = "<Sup>" + Content + "</Sup>";
            return data;
        }

        public string Subscript(String Content)
        {
            string data = "<Sub>" + Content + "</Sub>";
            return data;
        }

        public string Strikethrough(String Content)
        {
            string data = "<Strike>" + Content + "</Strike>";
            return data;
        }

        // lesson elements
        public string Lesson(String Title, String Content, String Objective)
        {
            string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Lesson xmlns:xy=\"http://xyleme.com/xylink\" xy:type=\"IA/Definitions/LessonDef.xml\"><Title>" + Title + "</Title>" + Content + LessonDD(Objective) +"</Lesson>";
            return data;
        }
        public string LessonDD(String Objective)
        {
            string data = "<DesignData><Notes><RichText>[Lesson Notes]</RichText><RichText/></Notes><EducationalObjective><ObjectiveIntro>Upon completion of this lesson you will be able to:</ObjectiveIntro><TerminalObjective><ObjectiveStatement>" + Objective + "</ObjectiveStatement></TerminalObjective></EducationalObjective><LOM><Educational><TypicalLearningTime/></Educational><AttributeGroup><Attribute><Name>template_version</Name><String>4.0</String></Attribute></AttributeGroup></LOM></DesignData>";
            //v4 dd
            data = "<DesignData><Notes><RichText>[Section Notes]</RichText><RichText/></Notes><EducationalObjective><ObjectiveIntro/><TerminalObjective><ObjectiveStatement>" + Objective + "</ObjectiveStatement></TerminalObjective></EducationalObjective><LOM><Educational><TypicalLearningTime>[minutes]</TypicalLearningTime></Educational><AttributeGroup><Attribute><Name>template_version</Name><String>4.1</String></Attribute></AttributeGroup></LOM></DesignData>";
            return data;
        }
        public string Topic(String Title, String Content, String Objective)
        {
            string data = "<Topic><Title>" + Title + "</Title>" + Content + TopicDD(Objective) + "</Topic>";
            return data;
        }
        public string TopicDD(String Objective)
        {
            string data = "<DesignData><Notes><Table align=\"left\" border=\"true\" width=\"100\"><TblGroup><TblCol width=\"22\"/><TblCol width=\"78\"/><TblHeader><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Item</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Description</RichText></Cell></TableRow></TblHeader><TblBody><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Topic Description</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>" + Objective + "</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Topic Type</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Reusable Content Topic</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Media for ELT and ILT</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>XML Slide or Figure</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Content Mining</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText/></Cell></TableRow></TblBody><TblFooter/></TblGroup></Table></Notes><EducationalObjective><ObjectiveIntro>Upon completion to this topic, you will be able to:</ObjectiveIntro><TerminalObjective><ObjectiveStatement>" + Objective + "</ObjectiveStatement></TerminalObjective></EducationalObjective><LOM><Classification><TaxonPath><Taxon><ID>7e8e2dfb-3d9a-4894-bf58-d27ba03195cf</ID><Entry/><Taxon><ID>daf1ca5e-8b64-4300-afdd-703a56a96da0</ID><Entry/><Taxon><ID>411b6532-8fd8-45cb-bf39-eca26087edc2</ID><Entry/></Taxon></Taxon></Taxon></TaxonPath></Classification><AttributeGroup><Attribute><Name>template_version</Name><String>4.0</String></Attribute></AttributeGroup></LOM></DesignData>";
            // v4.1 DD
            data = "<DesignData xy:type=\"Core/Definitions/Metadata/DesignDataDef.xml\" xmlns:xy=\"http://xyleme.com/xylink\"><Notes><RichText/><Table align=\"left\" border=\"true\" width=\"100\"><TblTitle>Concept Topic Design Table V4.1</TblTitle><TblGroup><TblCol width=\"22\"/><TblCol width=\"78\"/><TblHeader><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Item</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Description</RichText></Cell></TableRow></TblHeader><TblBody><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Objective </RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Insert enabling objective]<Emph>[Note</Emph>: Educational Objective below to link to the Objectives tree]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Difficulty level of Topic</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Rate 1–5, with 5 as most difficult] </RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Topic Type</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Remove all that do not apply: Definitions | Facts | Concepts | Processes | Procedures]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Video</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Video description and type; minimum one video per topic required]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Visual Media </RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Specify photos, graphics and/or diagrams. List update to icons, if required]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Content Mining</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Sources for content or graphics, including previous versions of the course; be specific]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Content Review Question(s) </RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[Specify # and question type (MC-SA, MC-MA, Matching, etc.)][Requirement: Minimum one Content Review question per topic (more as needed to represent the topic). No True/False or two-option MC questions.]</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Downloadable Assets </RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>[List any downloadable assets that should be made available to the student]</RichText></Cell></TableRow></TblBody><TblFooter/></TblGroup></Table></Notes><EducationalObjective><ObjectiveIntro/><EnablingObjective><ObjectiveStatement>"+ Objective +"</ObjectiveStatement></EnablingObjective></EducationalObjective><LOM><Classification><TaxonPath><Taxon><ID>7e8e2dfb-3d9a-4894-bf58-d27ba03195cf</ID><Entry/><Taxon><ID>daf1ca5e-8b64-4300-afdd-703a56a96da0</ID><Entry/><Taxon><ID>411b6532-8fd8-45cb-bf39-eca26087edc2</ID><Entry/></Taxon></Taxon></Taxon></TaxonPath></Classification><AttributeGroup><Attribute><Name>template_version</Name><String>4.1</String></Attribute></AttributeGroup></LOM></DesignData>";
            return data;
        }
        public string SelfCheckTopic(String Content)
        {
            string data = "<Topic><Title>Challenge</Title>" + Content + SelfCheckTopicDD() + "</Topic>";
            return data;
        }
        public string SelfCheckTopicDD()
        {
            string data = "<DesignData xy:type=\"Core/Definitions/Metadata/DesignDataDef.xml\" xmlns:xy=\"http://xyleme.com/xylink\"><Notes><Table align=\"left\" border=\"true\" width=\"100\"><TblGroup><TblCol width=\"\"/><TblCol width=\"\"/><TblHeader><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Item</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Description</RichText></Cell></TableRow></TblHeader><TblBody><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Types of Assessment items</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Questions | Lab</RichText></Cell></TableRow><TableRow><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText>Question work space</RichText></Cell><Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\"><RichText/></Cell></TableRow></TblBody><TblFooter/></TblGroup></Table></Notes><LOM><Classification><TaxonPath><Taxon><ID>7e8e2dfb-3d9a-4894-bf58-d27ba03195cf</ID><Entry/><Taxon><ID>daf1ca5e-8b64-4300-afdd-703a56a96da0</ID><Entry/><Taxon><ID>3b615ebc-5a45-4625-be8f-1aaeb4a384b4</ID><Entry/></Taxon></Taxon></Taxon></TaxonPath></Classification><AttributeGroup><Attribute><Name>template_version</Name><String>4.0.2</String></Attribute></AttributeGroup></LOM></DesignData>";
            return data;
        }
        public string LessonSlide(String Title, String Intro = null)
        {
            string data = "";
            if (Intro == null)
            {
                data = "<IntroBlock><Title>Introduction</Title>" + ParaBlock("<Slide slideTheme=\"LessonTitleSlide\"><Title>" + Title + "</Title><Body>" + RichText("[Module Name Here]") + "</Body></Slide>" + FilterMetadata()) + "</IntroBlock>";
            }
            else
            {
                data = "<IntroBlock><Title>Introduction</Title>" + ParaBlock(Intro) + ParaBlock("<Slide slideTheme=\"LessonTitleSlide\"><Title>" + Title + "</Title><Body>" + RichText("[Module Name Here]") + "</Body></Slide>" + FilterMetadata()) + "</IntroBlock>";
            }
            return data;
        }

        public string Segue(String Title, String Intro = null)
        {
            string data = "";
            if (Intro != null)
            {
                data = ParaBlock(Intro) + ParaBlock("<Slide slideTheme=\"SegueSlide\"><Title>" + Title + "</Title><Body><RichText/></Body></Slide>" + FilterMetadata());
            }
            else
            {
                data = ParaBlock("<Slide slideTheme=\"SegueSlide\"><Title>" + Title + "</Title><Body><RichText/></Body></Slide>" + FilterMetadata());
            }

            return data;
        }

        // keep for backward compatibility
        public string SegueSlide(String Title, String Intro)
        {
            string data;
            if (Intro.Trim().Length > 0)
            {
                data = ParaBlock(RichText(Intro)) + ParaBlock("<Slide slideTheme=\"SegueSlide\"><Title>" + Title + "</Title><Body><RichText/></Body></Slide>" + FilterMetadata());
            }
            else
            {
                data = ParaBlock(RichText("[Topic Intro Here]")) + ParaBlock("<Slide slideTheme=\"SegueSlide\"><Title>" + Title + "</Title><Body><RichText/></Body></Slide>" + FilterMetadata());
            }
            
            return data;
        }

        public string AnchorSlide(String Title, String Intro, String Content = "", String Image = "SampleImage")
        {
            string data = IntroBlock( RichText(Intro) + "<Slide slideTheme=\"BodySlide\"><Title>" + Title + "</Title><Body>" + Figure(Image) + "</Body></Slide>" + RichText(Content) );
            return data;
        }
        public string NormalSlide(String Title, String Intro, String Content = "", String SlideXML = "", String Image = "SampleImage")
        {
            string data = "";
            if (SlideXML.Length > 0)
            {
                data = TitledBlock(Title, RichText(Intro) + SlideXML + Content);
            }
            else
            {
                data = TitledBlock(Title, RichText(Intro) + "<Slide slideTheme=\"BodySlide\"><Title>" + Title + "</Title><Body>" + Figure(Image) + "</Body></Slide>" + Content);
            }
            
            return data;
        }
        public string ParaBlock(String Content)
        {
            string data = "<ParaBlock>" + Content + "</ParaBlock>";
            return data;
        }
        public string IntroBlock(String Content)
        {
            string data = "<ParaBlock>" + Content + "</ParaBlock>";
            return data;
        }
        public string RichText(String Content)
        {
            // var
            string data = "";
            // check for empty strings
            if (Content != null && Content.Replace("\u0001", " ").Length > 0)
            {
                data = "<RichText>" + Content.Trim() + "</RichText>";
            }
            else
            {
                data = "";
            }

            return data;
        }
        public string CustomNote(String Content)
        {
            string data = "<CustomNote><SimpleBlock><RichText>" + Content + "</RichText></SimpleBlock></CustomNote>";
            return data;
        }
        public string List(String Content, String Preamble = null)
        {
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
        public string preSubListItem(String Data)
        {
            string data = "<Item><ItemPara>" + Data + "</ItemPara>";
            return data;
        }
        public string SubList(String Data, String Content, String ListMarker = "nDash")
        {
            string data = "<Item><ItemPara>" + Data + "</ItemPara><SubList ListMarker=\"" + ListMarker + "\"><ItemBlock>" + Content + "</ItemBlock></SubList></Item>";
            return data;
        }
        public string Figure(String ImageName)
        {
            string data = "<Figure><MediaObject><Renditions><Web thumbWidth=\"50\" uri=\"DataCenter/ProductTraining/" + this.CourseCode + "/SG/" + this.ModuleCode + "/" + this.LessonCode + "/" + this.CourseCode + "_1-0_" + ImageName + ".png\"/><Source>DataCenter/ProductTraining/" + this.CourseCode + "/SG/" + this.ModuleCode + "/" + this.LessonCode + "/" + this.CourseCode + "_1-0_" + ImageName + ".pptx</Source></Renditions></MediaObject></Figure>";
            return data;
        }
        public string LabFigure(String ImageName)
        {
            string data = "<Figure><MediaObject><Renditions><Web thumbWidth=\"50\" uri=\"" + ImageName + "\"/></Renditions></MediaObject></Figure>";
            return data;
        }
        public string TitledBlock(String Title, String Content)
        {
            string data = "<TitledBlock><Title>" + Title + "</Title>" + ParaBlock(Content) + "</TitledBlock>";
            return data;
        }
        public string FilterMetadata()
        {
            string data = "<FilterMetadata><TargetAudiences>"+ FilterTarget("v4 PPT ILT") + FilterTarget("v4 PPT Lab") + "</TargetAudiences></FilterMetadata>";
            return data;
        }
        public string FilterTarget(String Target)
        {
            string data = "<TargetAudience>" + Target + "</TargetAudience>";
            return data;
        }

        // table
        public string TableCell(String Content)
        {
            string data = "<Cell align=\"left\" cell-bg=\"none\" colspan=\"1\" isInHeaderColumn=\"0\" rowspan=\"1\">" + Content + "</Cell>";
            return data;
        }
        public string TableRow(String Content)
        {
            string data = "<TableRow>" + Content + "</TableRow>";
            return data;
        }
        public string TableBody(String Content)
        {
            string data = "<TblBody>" + Content + "</TblBody>";
            return data;
        }
        public string TableHeader(String Content)
        {
            string data = "<TblHeader>" + Content + "</TblHeader>";
            return data;
        }
        public string TableGroup(String Content, int Cols)
        {
            string columns = "";
            for(int i = 0; i < Cols; i++)
            {
                columns += "<TblCol width=\"\"/>";
            }
            string data = "<TblGroup>"+ columns + Content + "<TblFooter/></TblGroup>";
            return data;
        }
        public string Table(String title, String Content)
        {
            string data = "<Table align=\"\" border=\"true\" width=\"\"><TblTitle>"+title+"</TblTitle>" + Content + "</Table>";
            return data;
        }

        // labs
        public string Lab(String title, String Content, String Topology, String JobAid, String CommandList)
        {
            string data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Topic xmlns:xy=\"http://xyleme.com/xylink\" xy:type=\"Core/Definitions/TopicTypes/TopicDef.xml\"><Title>"+title+"</Title><Introduction><Title>Introduction</Title><ParaBlock><RichText>Lab intro</RichText></ParaBlock></Introduction>"+ Topology + JobAid + CommandList + Content + "</Topic>";
            return data;
        }
        public string JobAid(String Content)
        {
            string data = "<Topic><Title>Job Aid</Title><ParaBlock>" + Content + "</ParaBlock></Topic>";
            return data;
        }
        public string CommandList(String Content)
        {
            string data = "<Topic><Title>Command List</Title><ParaBlock>" + Content + "</ParaBlock></Topic>";
            return data;
        }
        public string Code(String Content)
        {
            string data = "<Code>" + Content + "</Code>";
            return data;
        }
        public string Procedure(String title, String Content, String Activity = "<RichText>Lab Activity</RichText>", String Verification = "<RichText>Lab Activity verification</RichText>")
        {
            string data = "<Procedure><Title>"+title+"</Title><StepGroup numberedSteps=\"true\"><Title>Activity</Title><ParaBlock>"+Activity+"</ParaBlock>" + Content + "</StepGroup><TitledBlock><Title>Activity Verification</Title><ParaBlock>"+Verification+"</ParaBlock></TitledBlock></Procedure>";
            return data;
        }
        public string Step(String Content)
        {
            string data = "<Step>" + Content + "</Step>";
            return data;
        }
        public string StepActionDescription(String Content)
        {
            string data = "<ActionDescription>" + Content + "</ActionDescription>";
            return data;
        }
        public string StepUserAction(String Content)
        {
            string data = "<UserAction><ActionDescription>" + Content + "</ActionDescription></UserAction>";
            return data;
        }
        public string StepResponseDescription(String Content)
        {
            string data = "<ResponseDescription>" + Content + "</ResponseDescription>";
            return data;
        }
        public string StepResponse(String Content)
        {
            string data = "<SystemResponse>" + Content + "</SystemResponse>";
            return data;
        }

        // challenges
        public string QuestionBlock(String MultipleChoice)
        {
            string data = "<QuestionBlock>" + MultipleChoice + "</QuestionBlock>";
            return data;
        }
        public string MultipleChoice(String QuestionStem, String Options, String Feedback)
        {
            string data = "<MultipleChoice Weight=\"1.0\">" + QuestionStem + Options + Feedback + "</MultipleChoice>";
            return data;
        }
        public string QuestionStem(String Question)
        {
            string data = "<QuestionStem>" + RichText(Question) + "</QuestionStem>";
            return data;
        }
        public string Option(String IsCorrect, String Answer)
        {
            string data = "<Option IsCorrect=\""+ IsCorrect + "\"><Distractor>" + RichText(Answer) + "</Distractor></Option>";
            return data;
        }
        public string Feedback(String nFeedback)
        {
            string data = "<Feedback><PositiveFeedback/><NegativeFeedback>" + nFeedback + "</NegativeFeedback></Feedback>";
            return data;
        }
    }
}
