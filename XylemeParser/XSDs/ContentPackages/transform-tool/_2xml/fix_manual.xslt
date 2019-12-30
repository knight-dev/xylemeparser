<?xml version="1.0" encoding="utf-8"?>

<!--
	
	Copyright c 2015 Xyleme, Inc., 1881 9th Street, Suite 300, Boulder, Colorado 80302 USA.
	All rights reserved.
	
	This file and related documentation are protected by copyright and
	are distributed under licenses regarding their use, copying, distribution,
	and decompilation. No part of this product or related documentation may
	be reproduced or transmitted in any form or by any means, electronic or
	mechanical, for any purpose, without the express written permission of
	Xyleme, Inc.
	
-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:import href="identity.xslt" />

	<xsl:template match="Figure/@* | IA/@*
		| Table/@*[not(name()=('width','align','border'))] | Passage/CompoundQuestion
		| ParaBlock/@*[not(name()='boundingRect' and $customer='bpp')] 
		| Title/@* | TblTitle/@* | Caption/@* | RichText/@* | CustomNote/@* | InstructorNote/@* | MarginNote/@*
		| ListPreamble/@* | Item/@* | Distractor/@* | MultipleChoice/@*[not(name()=('Identifier', 'CognitionLevel'))] | Term/@*
		| EducationalObjective/@* | EnablingObjective/@* | FillInBlank/@* | FillInContent/Text/@*
		| EssayActivity/@*[not(name()='name')] | CompoundEssay/@*[not(name()='name')] | CompoundQuestion/@*[not(name()='name')]" />

	<xsl:template match="Title/InLineKeyboard">
		<InLineKeyword>
			<xsl:apply-templates select="@*|node()" />
		</InLineKeyword>
	</xsl:template>
			
	<xsl:template match="TblBody[../TblHeader/TableRow/Cell][not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<TableRow>
				<xsl:for-each select="../TblHeader/TableRow/Cell">
					<Cell>
						<xsl:apply-templates select="@*" />
						<RichText />
					</Cell>
				</xsl:for-each>
			</TableRow>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="Advice[not(@*|node())]" />
	
	<xsl:template match="ParaBlock[not(node())] | Cell[not(node())] | SimpleBlock[not(node())] | QuestionStem[not(node())] | Distractor[not(node())] | Solution[not(node())] | Passage[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<RichText />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Topic[not(node() except Title)] | ContentBlock[not(node() except Title)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<ParaBlock />
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Topic[Topic]/ParaBlock[not(@*|node())]" priority="2" />

	<xsl:template match="Lesson[not(node() except Title)] | Chapter[not(node() except Title)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<Topic>
					<Title />
				</Topic>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Lessons[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="pass0">
				<Lesson>
					<Title />
				</Lesson>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Module[not(Lesson)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<Lesson>
					<Title />
				</Lesson>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Modules[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="pass0">
				<Module>
					<Title />
				</Module>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="WebCourse[not(node() except (Title|WebPublishingProperties))]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<Page>
					<ContentBlock>
						<Title />
					</ContentBlock>
				</Page>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="IA[not(Lessons|Modules|FrontMatter)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node() except (Lesson|Module|Chapter)" />
			<xsl:if test="Chapter">
				<FrontMatter>
					<xsl:apply-templates select="Chapter" />
				</FrontMatter>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="Module">
					<Modules>
						<xsl:for-each-group select="Module|Lesson" group-starting-with="Module">
							<xsl:variable name="pass0">
								<Module>
									<xsl:if test="not(current-group()/self::Module)">
										<Title />
									</xsl:if>
									<xsl:apply-templates select="current-group()/(self::Module/(@*|node()), self::Lesson)" />
								</Module>
							</xsl:variable>
							<xsl:apply-templates select="$pass0" />
						</xsl:for-each-group>
					</Modules>
				</xsl:when>
				<xsl:otherwise>
					<Lessons>
						<xsl:apply-templates select="Lesson" />
					</Lessons>
				</xsl:otherwise>
			</xsl:choose>			
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="QuestionBank[not(Parts|Chapters|FrontMatter)]"> <!-- for BPP -->
		<xsl:copy>
			<xsl:apply-templates select="@*|node() except (MockExam|QuestionBankChapter|QuestionBankPart|Chapter)" />
			<xsl:if test="Chapter">
				<FrontMatter>
					<xsl:apply-templates select="Chapter" />
				</FrontMatter>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="QuestionBankPart">
					<Parts>
						<xsl:for-each-group select="QuestionBankPart|QuestionBankChapter|MockExam" group-starting-with="QuestionBankPart">
							<xsl:variable name="pass0">
								<QuestionBankPart>
									<xsl:if test="not(current-group()/self::QuestionBankPart)">
										<Title />
									</xsl:if>
									<xsl:apply-templates select="current-group()/(self::QuestionBankPart/(@*|node()), self::QuestionBankChapter|self::MockExam)" />
								</QuestionBankPart>
							</xsl:variable>
							<xsl:apply-templates select="$pass0" />
						</xsl:for-each-group>
					</Parts>
				</xsl:when>
				<xsl:otherwise>
					<Chapters>
						<xsl:apply-templates select="QuestionBankChapter|MockExam" />
					</Chapters>
				</xsl:otherwise>
			</xsl:choose>			
		</xsl:copy>
	</xsl:template>

	<xsl:template match="QuestionBlock[not(node() except (Title|ParaBlock))]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<MultipleChoice>
					<QuestionStem />
					<Option>
						<Distractor />
					</Option>
				</MultipleChoice>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="QuestionBankChapter[not(QuestionBlock)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<QuestionBlock />
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="QuestionBankPart[not(QuestionBankChapter|MockExam)]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="pass0">
				<QuestionBankChapter>
					<Title />
				</QuestionBankChapter>
			</xsl:variable>
			<xsl:apply-templates select="$pass0" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="EssayActivity[not($customer='bpp') and not(ResponseLines)]/QuestionStem">
		<xsl:next-match />
		<ResponseLines>1</ResponseLines>
	</xsl:template>
	
	<xsl:template match="CompoundEssay[not(Passage)]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="pass0">
				<Passage />
			</xsl:variable>
			<xsl:apply-templates select="$pass0,node()" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="FillInContent/Blank[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<CorrectResponse />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="TitlePage1[not(FileName)]/Title">
		<Title />
		<FileName>
			<xsl:apply-templates select="@*|node()" />
		</FileName>
	</xsl:template>

	<xsl:template match="iPass[not(iPassParts|iPassChapters|FrontMatter)]"> <!-- for BPP -->
		<xsl:copy>
			<xsl:apply-templates select="@*|node() except (iPassChapter|iPassPart|Chapter)" />
			<xsl:if test="Chapter">
				<FrontMatter>
					<xsl:apply-templates select="Chapter" />
				</FrontMatter>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="iPassPart">
					<iPassParts>
						<xsl:for-each-group select="iPassPart|iPassChapter" group-starting-with="iPassPart">
							<xsl:variable name="pass0">
								<iPassPart>
									<xsl:if test="not(current-group()/self::iPassPart)">
										<Title />
									</xsl:if>
									<xsl:apply-templates select="current-group()/(self::iPassPart/(@*|node()))" />
										<ChapterGroup>
											<xsl:apply-templates select="current-group()/self::iPassChapter"/>
									<xsl:if test="not(current-group()/self::iPassChapter)">
										<iPassChapter>
											<Title/>
										</iPassChapter>
									</xsl:if>
										</ChapterGroup>
								</iPassPart>
							</xsl:variable>
							<xsl:apply-templates select="$pass0" />
						</xsl:for-each-group>
					</iPassParts>
				</xsl:when>
				<xsl:otherwise>
					<iPassChapters>
						<xsl:apply-templates select="iPassChapter" />
					</iPassChapters>
				</xsl:otherwise>
			</xsl:choose>			
		</xsl:copy>
	</xsl:template>

	<xsl:template match="AnswerPassage//RichText[$customer='bpp']|AnswerPassage//ItemPara[$customer='bpp']">
		<xsl:if test="not(preceding-sibling::node()[1]/(self::RichText|self::ItemPara))">
			<xsl:copy>
				<xsl:apply-templates select="@*"/>
				<xsl:apply-templates mode="bpp-654__attr" select="following-sibling::node()[1]/self::RichText" />
				<xsl:apply-templates select="node()"/>
				<xsl:apply-templates mode="bpp-654__node" select="following-sibling::node()[1]/self::RichText" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template mode="bpp-654__attr" match="RichText">
		<xsl:apply-templates select="@*" />
		<xsl:apply-templates mode="bpp-654__attr" select="following-sibling::node()[1]/self::RichText" />
	</xsl:template>

	<xsl:template mode="bpp-654__node" match="RichText">
		<xsl:if test="node()">
			<xsl:value-of select="'&#10;&#10;'"/>
			<xsl:copy-of select="node()" />
		</xsl:if>
		<xsl:apply-templates mode="bpp-654__node" select="following-sibling::node()[1]/self::RichText" />
	</xsl:template>

	<xsl:template match="Assessments/*/MultipleChoice[not($customer='bpp')]">
		<MultipleChoice-Remediation>
			<xsl:next-match />
		</MultipleChoice-Remediation>
	</xsl:template>

</xsl:stylesheet>