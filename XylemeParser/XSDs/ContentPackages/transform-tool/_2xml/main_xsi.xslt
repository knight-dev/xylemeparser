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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xmlns:xy="http://xyleme.com/xylink" xmlns:zip="http://apache.org/cocoon/zip-archive/1.0" exclude-result-prefixes="xsi xy zip">

	<xsl:import href="identity.xslt" />

	<xsl:output method="xml" omit-xml-declaration="no" indent="yes" encoding="UTF-8" />

	<xsl:param name="SchemaFolder" />
	<xsl:variable name="ContentPackages" select="if (ends-with($SchemaFolder, '/') or ends-with($SchemaFolder, '\')) then $SchemaFolder else concat($SchemaFolder, '/')" />

	<xsl:template match="@xy:type | @xy:guid | @xy:root" />

	<xsl:template match="/sdoc-backup">
		<xsl:apply-templates select="sdoc/content/node()" />
	</xsl:template>

	<xsl:variable name="defs">
		<!-- paste special -->
		<def name="RichText" schema="Core/Definitions/RichText.xsd" />
		<def name="List" schema="Core/Definitions/List.xsd" />
		<def name="Table" schema="Core/Definitions/Table.xsd" />
		<def name="ParaBlock" schema="Core/Definitions/Paragraph.xsd" />
		<!-- common import -->
		<def name="NativePPT" schema="Core/Definitions/NativePPT.xsd" />
		<def name="SlideDeck" schema="Core/Definitions/SlideDeck.xsd" />
		<def name="Topic" schema="Core/Definitions/TopicTypes/Topic.xsd" />
		<def name="Lesson" schema="IA/Definitions/IA.xsd" />
		<def name="Module" schema="IA/Definitions/IA.xsd" />
		<def name="IA" schema="IA/Definitions/IA.xsd" />
		<def name="WebCourse" schema="Core/Definitions/WebCourse.xsd" />
		<def name="Page" schema="Core/Definitions/WebCourse.xsd" />
		<def name="TopicBook" schema="TopicBook/Definitions/TopicBook.xsd" />
		<!-- bpp -->
		<def name="QuestionBank" schema="IA/Definitions/QuestionBank.xsd" />
		<def name="iPass" schema="IA/Definitions/iPass.xsd" />
		<!-- DITA -->
		<def name="Procedure" schema="Core/Definitions/TopicTypes/Procedure.xsd" />
	</xsl:variable>

	<xsl:template match="/sdoc-backup/sdoc/content/* | /zip:archive/zip:entry/* | /*">
		<xsl:variable name="def" select="$defs/def[@name=current()/name()]" />
		<xsl:copy>
			<xsl:if test="$def">
				<xsl:attribute name="xsi:noNamespaceSchemaLocation" select="concat($ContentPackages, $def/@schema)" />
			</xsl:if>
			<xsl:apply-templates select="@* except @xsi:noNamespaceSchemaLocation[$def] | node()" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="/zip:archive" priority="2">
		<xsl:copy>			
			<xsl:attribute name="xsi:schemaLocation" select="concat('http://apache.org/cocoon/zip-archive/1.0 ', resolve-uri('../common/src.schemas/schemas/apache/cocoon/zip-archive.xsd'))" /> <!-- DIRTY HACK --> 
			<xsl:apply-templates select="@* except @xsi:noNamespaceSchemaLocation | node()" />
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>