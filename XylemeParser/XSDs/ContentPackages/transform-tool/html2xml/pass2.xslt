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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:import href="../_2xml/identity.xslt" />
	<xsl:import href="../_2xml/regexp.xslt" />

	<xsl:template match="@ListLevel">
		<xsl:attribute name="level" select="."/>
	</xsl:template>
	
	<xsl:template match="Cell/@width | Cell/@index"/>

	<xsl:template match="Table/TblGroup[not(TblCol)]">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:variable name="col-tr0">
				<xsl:for-each select="TblBody/TableRow/Cell">
					<xsl:sort select="@index"/>
					<xsl:sort select="@colspan"/>
					<xsl:sort select="@width" order="descending"/>
					<xsl:copy>
						<xsl:copy-of select="@*"/>
					</xsl:copy>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="col-tr1">
				<xsl:for-each select="$col-tr0/Cell[not(@index=preceding-sibling::Cell/@index)]">
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
			<xsl:variable name="col-tr" select="$col-tr1/Cell"/>
			<xsl:variable name="table-width" select="sum($col-tr/@width)" />
			<xsl:for-each select="$col-tr">
				<TblCol width="{format-number(@width div $table-width * 100, '#')}" />
			</xsl:for-each>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>
		
	<xsl:template match="ListItem[not(ItemPara)]/*[1][self::RichText]">
		<ItemPara>
			<xsl:apply-templates select="@*|node()" />
		</ItemPara>
	</xsl:template>

	<xsl:template match="ListItem[ListItem]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node() except ListItem"/>
		</xsl:copy>
		<xsl:apply-templates select="ListItem"/>
	</xsl:template>
	
	<!-- convert to InLineImage -->
	<xsl:template match="RichText/Figure">
		<InLineImage>
			<xsl:apply-templates select="MediaObject/Web/(@*|text())" />
		</InLineImage>
	</xsl:template>

	<!-- simplify Figure -->
	<xsl:template match="RichText[count(node())=1 and Figure
		or count(node())=2 and Figure and text()[normalize-space(translate(., $whitespaces, ''))='']
		or count(node())=3 and Figure and text()[1][normalize-space(translate(., $whitespaces, ''))=''] and text()[2][normalize-space(translate(., $whitespaces, ''))='']
		]" priority="1.1">
		<xsl:apply-templates select="Figure" />
	</xsl:template>

	<xsl:template match="RichText[count(node())=1 and Figure
		or count(node())=2 and Figure and text()[normalize-space(translate(., $whitespaces, ''))='']
		or count(node())=3 and Figure and text()[1][normalize-space(translate(., $whitespaces, ''))=''] and text()[2][normalize-space(translate(., $whitespaces, ''))='']
		]/Figure" priority="1.1">
		<xsl:copy>
			<xsl:apply-templates select="../@*|@*|node()" />
		</xsl:copy>
	</xsl:template>

	<!-- regexp -->	
	<xsl:template match="RichText" priority="2">
		<xsl:variable name="pass0">
			<xsl:next-match />
		</xsl:variable>
		<xsl:variable name="pass1">
			<xsl:apply-templates mode="merge-InLine" select="$pass0"/>
		</xsl:variable>
		<xsl:apply-templates mode="regexp" select="$pass1" />
	</xsl:template>

	<xsl:template match="br">
		<xsl:text>
</xsl:text>
	</xsl:template>

	<xsl:template match="Emph[count(node())=1 and Italic] | Italic[count(node())=1 and Emph]">
		<InLineTypeThis>
			<xsl:apply-templates select="@*|*/@*"/>
			<xsl:value-of select="."/>
		</InLineTypeThis>
	</xsl:template>
	
	<xsl:template match="Emph[count(node())=1 and Italic and $customer='esi'] | Italic[count(node())=1 and Emph and $customer='esi']" priority="2">
		<xsl:copy>
			<xsl:apply-templates select="@*|*/@*"/>
			<xsl:value-of select="."/>
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>