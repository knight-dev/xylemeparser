<?xml version="1.0" encoding="UTF-8"?>

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

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xy="http://xyleme.com/xylink"
	exclude-result-prefixes="xy">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:template match="Topic/ParaBlock[count(node())=0]" />

	<xsl:template
		match="Emph/Emph | Italic/Italic | Underline/Underline">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template
		match="TableRow/Cell/Table[count(TblGroup/TblCol)=1]">
		<xsl:apply-templates
			select="TblGroup/*/TableRow/Cell/node()" />
	</xsl:template>

	<xsl:template match="TableRow/Cell/h1">
		<RichText>
			<xsl:apply-templates select="node()" />
		</RichText>
	</xsl:template>

	<xsl:template match="Cell/@width | Cell/@index |
		Cell/text()[normalize-space(translate(., '&#13;&#10;&#160;', ' '))='']"/>

	<xsl:template match="Table/TblGroup[not(TblCol)]">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:variable name="coltr0">
				<xsl:for-each select="TblBody/TableRow/Cell">
					<xsl:sort select="@index"/>
					<xsl:sort select="@colspan"/>
					<xsl:copy>
						<xsl:copy-of select="@*"/>
					</xsl:copy>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="coltr1">
				<xsl:for-each select="$coltr0/Cell[not(@index=preceding-sibling::Cell/@index)]">
					<xsl:variable name="index" select="@index"/>
					<xsl:choose>
						<xsl:when test="@colspan='1'">
							<xsl:copy>
								<xsl:copy-of select="@*"/>
							</xsl:copy>
						</xsl:when>
						<xsl:when test="@colspan='2'">
							<xsl:copy>
								<xsl:copy-of select="@*[name()!='width']"/>
								<xsl:attribute name="width">
									<xsl:value-of select="@width - following-sibling::Cell[@colspan='1' and @index=$index+1]/@width"/>
								</xsl:attribute>
							</xsl:copy>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message select="'misaligned colspan&gt;2 not supported'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="coltr" select="$coltr1/Cell"/>
			<xsl:variable name="table-width" select="sum($coltr/@width)" />
			<xsl:for-each select="$coltr">
				<TblCol 
					width="{format-number(@width div $table-width * 100, '#')}" />
			</xsl:for-each>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>
		
	<xsl:template
		match="Cell[not(RichText|List|MediaObject|Code|Figure)]">
		<Cell>
			<xsl:apply-templates select="@*" />
			<RichText>
				<xsl:apply-templates select="node()" />
			</RichText>
		</Cell>
	</xsl:template>

	<xsl:template match="comment() | if" />

</xsl:stylesheet>