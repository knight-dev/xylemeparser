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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xy="http://xyleme.com/xylink" xmlns:zip="http://apache.org/cocoon/zip-archive/1.0" exclude-result-prefixes="xy zip">

	<!-- 
		background template
	-->
	<xsl:template match="@*|node()" priority="-9">
		<xsl:copy copy-namespaces="no"> <!-- NOTICE copy-namespaces="no" this removes not required namespace declarations -->
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:variable name="defs0">
		<!-- paste special -->
		<def name="RichText" definition="Core/Definitions/RichTextDef.xml" />
		<def name="List" definition="Core/Definitions/ListDef.xml" />
		<def name="Table" definition="Core/Definitions/TableDef.xml" />
		<def name="ParaBlock" definition="Core/Definitions/ParaBlockDef.xml" />
		<!-- common import -->
		<def name="NativePPT" definition="Core/Definitions/NativePPTDef.xml" />
		<def name="SlideDeck" definition="Core/Definitions/SlideDeckDef.xml" />
		<def name="Topic" definition="Core/Definitions/TopicTypes/TopicDef.xml" />
		<def name="Lesson" definition="IA/Definitions/LessonDef.xml" />
		<def name="Module" definition="IA/Definitions/ModuleDef.xml" />
		<def name="IA" definition="IA/Definitions/IADef.xml" />
		<def name="WebCourse" definition="Core/Definitions/WebCourseDef.xml" />
		<def name="Page" definition="Core/Definitions/PageDef.xml" />
		<def name="TopicBook" definition="TopicBook/Definitions/TopicBookDef.xml" />
		<!-- bpp -->
		<def name="QuestionBank" definition="IA/Definitions/QuestionBankDef.xml" />
		<def name="iPass" definition="IA/Definitions/iPassDef.xml" />
		<!-- DITA -->
		<def name="Procedure" definition="Core/Definitions/TopicTypes/ProcedureDef.xml" />
	</xsl:variable>

	<!-- check if two different elements have the same guid -->
	<xsl:template match="/">
		<xsl:for-each-group select="//*[@xy:guid]" group-by="@xy:guid">
			<xsl:variable name="suspect" select="distinct-values(current-group()/node-name(.))"/>
			<xsl:if test="count($suspect)>1">
				<xsl:message select="concat('guid clash, element1: ', $suspect[1], ', element2: ', $suspect[2], ', guid: ', current-grouping-key())"/>
			</xsl:if>
		</xsl:for-each-group>
		<xsl:next-match/>
	</xsl:template>
	
	<xsl:template match="/sdoc-backup/sdoc/content/* | /zip:archive/zip:entry/* | /*">
		<xsl:variable name="def" select="$defs0/def[@name=current()/name()]" />
		<xsl:copy>
			<xsl:if test="$def">
				<xsl:attribute name="xy:type" select="$def/@definition" />
			</xsl:if>
			<xsl:apply-templates select="@* except @xy:type[$def] | node()" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>