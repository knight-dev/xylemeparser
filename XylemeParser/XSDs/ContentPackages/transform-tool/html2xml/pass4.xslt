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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:my="urn:my.namespace" exclude-result-prefixes="my">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:template match="Cell[not(RichText|List|Code|Figure|Table|Topic)]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<RichText>
				<xsl:apply-templates select="node()" />
			</RichText>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="Sup/Emph | Sub/Emph | Sup/Italic | Sub/Italic | Emph/Italic | Emph/Underline | Italic/Emph | Underline/Emph | Underline/Italic | Italic/Emph | Italic/Underline | Caption/Italic | TblTitle/Italic">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:variable name="whitespaces" select="string-join($config/whitespace, '')" />
	<xsl:variable name="whitespaces-regexp" select="concat('[', $whitespaces, ']')" />

	<xsl:template match="text()[parent::RichText or parent::ItemPara or parent::Caption or parent::TblTitle]">
		<xsl:variable name="text0" select="translate(., '&#10;&#13;', '  ')" />
		<xsl:variable name="text1" select="if (not(preceding-sibling::*)) then replace($text0, concat('^', $whitespaces-regexp, '+'), '', 's') else $text0" />
		<xsl:variable name="text2" select="if (not(following-sibling::*)) then replace($text1, concat($whitespaces-regexp, '+$'), '', 's') else $text1" />
		<xsl:variable name="text3" select="replace($text2, concat('(', $whitespaces-regexp, ')\1+'), '$1', 's')" />
		<xsl:variable name="text4" select="replace($text3, concat('(', $whitespaces-regexp, ') '), '$1', 's')" />
		<xsl:variable name="text5" select="replace($text4, concat(' (', $whitespaces-regexp, ')'), '$1', 's')" />
		<xsl:value-of select="$text5" />
	</xsl:template>

	<xsl:template match="*[my:IsInLine(.)]/text()">
		<xsl:variable name="text0" select="translate(., '&#10;&#13;', '  ')" />
		<xsl:variable name="text1" select="replace($text0, concat('(', $whitespaces-regexp, ')\1+'), '$1', 's')" />
		<xsl:variable name="text2" select="replace($text1, concat('(', $whitespaces-regexp, ') '), '$1', 's')" />
		<xsl:variable name="text3" select="replace($text2, concat(' (', $whitespaces-regexp, ')'), '$1', 's')" />
		<xsl:value-of select="$text3" />
	</xsl:template>
		
</xsl:stylesheet>