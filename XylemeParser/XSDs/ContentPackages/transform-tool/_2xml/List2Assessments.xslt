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

	<xsl:template match="List[parent::Cell]" priority="1.1">
		<xsl:copy-of select="." />
	</xsl:template>

	<xsl:template match="List">
		<xsl:variable name="pass0">
			<xsl:apply-templates mode="__preprocess" select="." />
		</xsl:variable>
		<xsl:apply-templates select="$pass0/List/node()" />
	</xsl:template>

	<xsl:template match="List/ListPreamble">
		<RichText>
			<xsl:apply-templates select="@*|node()" />
		</RichText>
	</xsl:template>
	
	<xsl:template match="List/ItemBlock">
		<xsl:apply-templates select="@*|node()" />
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item[not(SubList) and not(descendant::Blank)]">
		<xsl:param name="answer-List" tunnel="yes" />
		<EssayActivity>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="Solution" select="node()[@pStyle='Answer']" />
			<xsl:variable name="QuestionStem" select="node() except $Solution" />
			<QuestionStem>
				<xsl:apply-templates select="$QuestionStem" />
			</QuestionStem>
			<Solution>
				<xsl:apply-templates select="$Solution" />
				<xsl:if test="$answer-List">
					<xsl:variable name="question-number" select="../../@StartAtNumber+count(preceding-sibling::Item)" />
					<xsl:variable name="answer" select="$answer-List/ItemBlock/Item[(../../@StartAtNumber+count(preceding-sibling::Item))=$question-number]" />
					<xsl:choose>
						<xsl:when test="$answer">
							<xsl:variable name="pass0">
								<xsl:copy-of select="$answer/node()" />
							</xsl:variable>
							<xsl:apply-templates mode="__solution" select="$pass0"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message select="concat('answer not found for question #', $question-number, '&#10;&#9;question stem: ', $QuestionStem)" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</Solution>
		</EssayActivity>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item[not(SubList) and descendant::Blank]">
		<xsl:param name="answer-List" tunnel="yes" />
		<FillInBlank>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="FillInContent" select="ItemPara[Blank]|RichText[Blank]" />
			<xsl:variable name="QuestionStem" select="node() except $FillInContent" />
			<xsl:if test="$QuestionStem">
				<QuestionStem>
					<xsl:apply-templates select="$QuestionStem" />
				</QuestionStem>
			</xsl:if>
			<FillInContent>
				<xsl:apply-templates select="$FillInContent" />
			</FillInContent>
			<xsl:if test="$answer-List">
				<xsl:variable name="question-number" select="../../@StartAtNumber+count(preceding-sibling::Item)" />
				<xsl:variable name="answer" select="$answer-List/ItemBlock/Item[(../../@StartAtNumber+count(preceding-sibling::Item))=$question-number]" />
				<Advice>
					<xsl:variable name="pass0">
						<xsl:copy-of select="$answer/node()" />
					</xsl:variable>
					<xsl:apply-templates mode="__solution" select="$pass0"/>
				</Advice>
			</xsl:if>
		</FillInBlank>
	</xsl:template>
	
	<xsl:template match="ItemPara[Blank]|RichText[Blank]">
		<xsl:variable name="self" select="." />
		<xsl:for-each-group select="node()" group-adjacent="name()=('Blank')">
			<xsl:choose>
				<xsl:when test="current-grouping-key()">
					<xsl:apply-templates select="current-group()"/>
				</xsl:when>
				<xsl:otherwise>
					<Text>
						<xsl:apply-templates select="$self/@*,current-group()"/>
					</Text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each-group>
		<xsl:if test="following-sibling::ItemPara[Blank]|following-sibling::RichText[Blank]">
			<LineBreak/>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="ItemPara/Blank|RichText/Blank">
		<xsl:param name="answer-List" tunnel="yes" />
		<!-- xsl:variable name="regexp0">
			<xsl:text>^</xsl:text>
			<xsl:for-each select="../node()">
				<xsl:choose>
					<xsl:when test="self::Blank">
						<xsl:text>(.*)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="replace(., '([\.])', '\\$1', 's')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
			<xsl:text>$</xsl:text>
		</xsl:variable>
		<xsl:variable name="regexp" select="replace($regexp0, ' ? Fill in the blanks\\\.', '', 's')"/>
		<xsl:message select="$regexp"/-->
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
			<!-- xsl:if test="$answer-List">
				<xsl:variable name="question-number" select="../../../../@StartAtNumber+count(../../preceding-sibling::Item)" />
				<xsl:variable name="answer" select="normalize-space($answer-List/ItemBlock/Item[(../../@StartAtNumber+count(preceding-sibling::Item))=$question-number])" />
				<xsl:if test="matches($answer, $regexp, 's')">
					<CorrectResponse>
						<xsl:value-of select="$answer" />
					</CorrectResponse>
				</xsl:if>
			</xsl:if-->
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="List/ItemBlock/Item/ItemPara[not(Blank)]">
		<RichText>
			<xsl:apply-templates select="@*|node()" />
		</RichText>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item[SubList]">
		<xsl:param name="answer-List" tunnel="yes" />
		<MultipleChoice>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="QuestionStem" select="node() except SubList[last()]|SubList[last()]/ListPreamble"/>
			<QuestionStem>
				<xsl:apply-templates select="$QuestionStem" />
			</QuestionStem>
			<xsl:variable name="question-number" select="../../@StartAtNumber+count(preceding-sibling::Item)" />
			<xsl:variable name="answer0">
				<xsl:if test="$answer-List">
					<xsl:copy-of select="$answer-List/ItemBlock/Item[(../../@StartAtNumber+count(preceding-sibling::Item))=$question-number]/node()" />
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="answer">
				<xsl:apply-templates mode="__option" select="$answer0" />
			</xsl:variable>
			<xsl:apply-templates select="SubList[last()]">
				<xsl:with-param name="answer" select="$answer" tunnel="yes" />
			</xsl:apply-templates>
			<xsl:if test="$answer-List">
				<xsl:choose>
					<xsl:when test="not($answer)">
						<xsl:message select="concat('answer not found for question #', $question-number, '&#10;&#9;question stem: ', $QuestionStem)" />
					</xsl:when>
					<xsl:when test="$answer[not(SubList)]">
						<xsl:message select="concat('answer found but correct option not found for question #', $question-number, 
							'&#10;&#9;question stem: ', $QuestionStem, '&#10;&#9;answer text: ', $answer)" />
						<Advice>
							<xsl:variable name="pass0">
								<xsl:copy-of select="$answer/node()" />
							</xsl:variable>
							<xsl:apply-templates mode="__solution" select="$pass0"/>
						</Advice>
					</xsl:when>
					<xsl:otherwise>
						<Advice>
							<xsl:variable name="pass0">
								<xsl:copy-of select="$answer/SubList/ItemBlock/Item/node()|$answer/(node() except SubList)" />
							</xsl:variable>
							<xsl:apply-templates mode="__solution" select="$pass0"/>
						</Advice>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</MultipleChoice>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList" priority="-1">
		<List>
			<xsl:apply-templates select="@*|node()" />
		</List>
	</xsl:template>
	
	<xsl:template match="List/ItemBlock/Item/SubList[last()]">
		<xsl:apply-templates select="node() except ListPreamble" />
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList[last()]/ListPreamble">
		<RichText>
			<xsl:apply-templates select="@*|node()" />
		</RichText>
	</xsl:template>
	
	<xsl:template match="List/ItemBlock/Item/SubList[last()]/ItemBlock">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList[last()]/ItemBlock/Item">
		<xsl:param name="answer" tunnel="yes" />
		<Option>
			<xsl:choose>
				<xsl:when test="$answer">
					<xsl:variable name="option-number" select="../../@StartAtNumber+count(preceding-sibling::Item)" />
					<xsl:variable name="option" select="$answer/SubList/ItemBlock/Item[(../../@StartAtNumber+count(preceding-sibling::Item))=$option-number]
						| $answer/SubList[not(@StartAtNumber)]/ItemBlock/Item[@text=lower-case(normalize-space(current()))]" />
					<xsl:attribute name="IsCorrect" select="if ($option) then 'true' else 'false'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="IsCorrect" select="'false'" />
				</xsl:otherwise>
			</xsl:choose>
			<Distractor>
				<xsl:apply-templates select="@*|node()" />
			</Distractor>
		</Option>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList[last()]/ItemBlock/Item/ItemPara">
		<RichText>
			<xsl:apply-templates select="@*|node()" />
		</RichText>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList[last()]/ItemBlock/Item/SubList">
		<List>
			<xsl:apply-templates select="@*|node()" />
		</List>
	</xsl:template>

	<!-- 
		background copy template
	-->
	<xsl:template priority="-1" mode="__preprocess" match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates mode="__preprocess" select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:variable name="truefalse-mcq-regexp" select="' True ?(/|or) ?False[\?.][ \t]{0,2}$'" />
	
	<xsl:template mode="__preprocess" match="List/ItemBlock/Item[count(node())=1]/ItemPara[matches(., $truefalse-mcq-regexp, 'si')]" priority="2">
		<xsl:copy>
			<xsl:variable name="pass0">
				<xsl:copy-of select="node()"/>
			</xsl:variable>
			<xsl:apply-templates mode="replace" select="$pass0">
				<xsl:with-param name="regexp" select="$truefalse-mcq-regexp" tunnel="yes"/>
				<xsl:with-param name="flags" select="'si'" tunnel="yes"/>
			</xsl:apply-templates>
		</xsl:copy>
		<SubList>
			<ItemBlock>
				<Item>
					<ItemPara>True</ItemPara>
				</Item>
				<Item>
					<ItemPara>False</ItemPara>
				</Item>
			</ItemBlock>
		</SubList>
	</xsl:template>
	
	<xsl:template mode="__preprocess" match="List/ItemBlock/Item[not(SubList)]/*[self::ItemPara|self::RichText]/Underline[normalize-space(translate(., '&#160;', ' '))='' and string-length()>=5]">
		<Blank />
	</xsl:template>

	<xsl:template mode="__preprocess" match="List/ItemBlock/Item[not(SubList)]/*[self::ItemPara|self::RichText][not(*)]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="regexp" select="'([ ])\1{4,}|([\.])\2{4,}'" />
			<xsl:analyze-string select="." regex="{$regexp}">
				<xsl:matching-substring>
					<xsl:choose>
						<xsl:when test="regex-group(1)">
							<xsl:text> </xsl:text>
							<Blank />
							<xsl:text> </xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<Blank />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:matching-substring>
				<xsl:non-matching-substring>
					<xsl:value-of select="." />
				</xsl:non-matching-substring>
			</xsl:analyze-string>
		</xsl:copy>
	</xsl:template>
	
	<!-- 
		background copy template
	-->
	<xsl:template priority="-1" mode="__solution" match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates mode="__solution" select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template mode="__solution" match="/ItemPara">
		<RichText>
			<xsl:apply-templates mode="__solution" select="@*|node()" />
		</RichText>
	</xsl:template>
	
	<xsl:template mode="__solution" match="/SubList">
		<List>
			<xsl:apply-templates mode="__solution" select="@*|node()" />
		</List>
	</xsl:template>
	
	<xsl:template mode="__solution" match="/ItemPara[not(node())]" priority="2" />

	<!-- 
		background copy template
	-->
	<xsl:template priority="-1" mode="__option" match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates mode="__option" select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:variable name="option-regexp" select="'^ {0,2}\(?([a-zA-Z])\)?( {0,2}(and|,) {0,2}\(?([a-zA-Z])\)?)?( {0,2}(and|,) {0,2}\(?([a-zA-Z])\)?)? {0,2}([\.:] {0,2}|[-&#x2013;&#x2014;] {0,2}|&#9; {0,2}|$)'" />
	<xsl:variable name="option-regexp-replace" select="'^ {0,2}\(?([a-zA-Z])\)?( {0,2}(and|,) {0,2}\(?([a-zA-Z])\)?)?( {0,2}(and|,) {0,2}\(?([a-zA-Z])\)?)? {0,2}([\.:] {0,2}|[-&#x2013;&#x2014;] {0,2}|&#9; {0,2}|)?.*$'" />
	
	<xsl:template mode="__option" match="/ItemPara[matches(., $option-regexp, 's')]">
		<xsl:variable name="StartAtNumber" select="replace(., $option-regexp-replace, '$1', 's')" />
		<xsl:variable name="ListMarker" select="if (matches($StartAtNumber, '[a-z]', 's')) then 'LowercaseAlpha' else 'UppercaseAlpha'" />
		<SubList ListMarker="{$ListMarker}" StartAtNumber="{string-to-codepoints($StartAtNumber)[1]-string-to-codepoints(if($ListMarker='LowercaseAlpha')then'a'else'A')[1]+1}">
			<ItemBlock>
				<Item />
			</ItemBlock>
		</SubList>
		<xsl:variable name="StartAtNumber2" select="replace(., $option-regexp-replace, '$4', 's')" />
		<xsl:variable name="ListMarker2" select="if (matches($StartAtNumber2, '[a-z]', 's')) then 'LowercaseAlpha' else 'UppercaseAlpha'" />
		<xsl:if test="$StartAtNumber2">
			<SubList ListMarker="{$ListMarker2}" StartAtNumber="{string-to-codepoints($StartAtNumber2)[1]-string-to-codepoints(if($ListMarker2='LowercaseAlpha')then'a'else'A')[1]+1}">
				<ItemBlock>
					<Item />
				</ItemBlock>
			</SubList>
		</xsl:if>
		<xsl:variable name="StartAtNumber3" select="replace(., $option-regexp-replace, '$7', 's')" />
		<xsl:variable name="ListMarker3" select="if (matches($StartAtNumber3, '[a-z]', 's')) then 'LowercaseAlpha' else 'UppercaseAlpha'" />
		<xsl:if test="$StartAtNumber3">
			<SubList ListMarker="{$ListMarker3}" StartAtNumber="{string-to-codepoints($StartAtNumber3)[1]-string-to-codepoints(if($ListMarker3='LowercaseAlpha')then'a'else'A')[1]+1}">
				<ItemBlock>
					<Item />
				</ItemBlock>
			</SubList>
		</xsl:if>
		<xsl:variable name="replace">
			<xsl:apply-templates mode="replace" select="node()">
				<xsl:with-param name="regexp" select="$option-regexp" tunnel="yes"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:if test="$replace/node()">
			<RichText>
				<xsl:copy-of select="$replace" />
			</RichText>
		</xsl:if>
	</xsl:template>
	
	<xsl:variable name="truefalse-regexp" select="'^ {0,2}(True|False) {0,2}([\.:;] {0,2}|[-&#x2013;&#x2014;] {0,2}|&#9; {0,2}|$)'" />
	<xsl:variable name="truefalse-regexp-replace" select="'^ {0,2}(True|False) {0,2}([\.:;] {0,2}|[-&#x2013;&#x2014;] {0,2}|&#9; {0,2}|)?.*$'" />
	
	<xsl:template mode="__option" match="/ItemPara[matches(., $truefalse-regexp, 'si')]">
		<SubList>
			<ItemBlock>
				<Item text="{lower-case(normalize-space(replace(., $truefalse-regexp-replace, '$1', 'si')))}"/>
			</ItemBlock>
		</SubList>
		<xsl:variable name="replace">
			<xsl:apply-templates mode="replace" select="node()">
				<xsl:with-param name="regexp" select="$truefalse-regexp" tunnel="yes"/>
				<xsl:with-param name="flags" select="'si'" tunnel="yes"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:if test="$replace/node()">
			<RichText>
				<xsl:copy-of select="$replace" />
			</RichText>
		</xsl:if>
	</xsl:template>
	
</xsl:stylesheet>