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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:my="urn:my.namespace" exclude-result-prefixes="xs my">

	<xsl:import href="identity.xslt" />

	<xsl:function name="my:CanMergeListItem" as="xs:boolean?">
		<xsl:param name="ListItem1" />
		<xsl:param name="ListItem2" />
		<xsl:if test="$ListItem1/@ListMarker=$ListItem2/@ListMarker">
			<xsl:variable name="StartAtNumber1" select="my:StartAtNumber($ListItem1)" />
			<xsl:variable name="StartAtNumber2" select="my:StartAtNumber($ListItem2)" />
			<xsl:value-of select="not($StartAtNumber1) and not($StartAtNumber2) or ($StartAtNumber1+1)=$StartAtNumber2" />
		</xsl:if>
	</xsl:function>
	
	<!-- merge ListItem into List -->

	<xsl:template match="ListItem">
		<xsl:if test="not(preceding-sibling::*[1]/self::ListItem[my:CanMergeListItem(., current())])">
			<List>
				<xsl:apply-templates select="@ListMarker | @StartAtNumber | @Build" />
				<xsl:if test="$config/ListPreamble[matches(current()/preceding-sibling::*[1]/self::RichText/normalize-space(), @text, 's')]">
					<ListPreamble>
						<xsl:apply-templates select="preceding-sibling::*[1]/self::RichText/(@*|node())" />
					</ListPreamble>
				</xsl:if>
				<ItemBlock>
					<xsl:apply-templates mode="__next" select="." />
				</ItemBlock>
			</List>
		</xsl:if>
	</xsl:template>

	<xsl:template mode="__next" match="ListItem">
		<Item>
			<xsl:apply-templates select="@* except (@ListMarker|@StartAtNumber|@Build) | node()" />
		</Item>
		<xsl:apply-templates mode="__next" select="following-sibling::*[1]/self::ListItem[my:CanMergeListItem(current(), .)]" />
	</xsl:template>

	<!-- list preamble -->
	<xsl:template match="RichText[following-sibling::*[1]/self::ListItem][$config/ListPreamble[matches(current()/normalize-space(), @text, 's')]]" />

	<!-- list markers -->
	<xsl:template match="ListItem/@StartAtNumber">
		<xsl:variable name="StartAtNumber" select="my:StartAtNumber(..)" />
		<xsl:if test="$StartAtNumber">
			<xsl:attribute name="{name()}" select="$StartAtNumber" />
		</xsl:if>
	</xsl:template>

	<xsl:function name="my:StartAtNumber">
		<xsl:param name="ListItem" />
		<xsl:variable name="StartAtNumber" select="normalize-space($ListItem/@StartAtNumber)" />
		<xsl:if test="$StartAtNumber">
			<xsl:choose>
				<xsl:when test="$ListItem[@ListMarker=('LowercaseAlpha','(LowercaseAlpha)')] and matches($StartAtNumber, '^[a-z]$', 's')">
					<xsl:value-of select="string-to-codepoints($StartAtNumber)[1]-string-to-codepoints('a')[1]+1" />
				</xsl:when>
				<xsl:when test="$ListItem[@ListMarker=('UppercaseAlpha','(UppercaseAlpha)')] and matches($StartAtNumber, '^[A-Z]$', 's')">
					<xsl:value-of select="string-to-codepoints($StartAtNumber)[1]-string-to-codepoints('A')[1]+1" />
				</xsl:when>
				<xsl:when test="$ListItem[@ListMarker=('UppercaseRoman', '(UppercaseRoman)', 'LowercaseRoman', '(LowercaseRoman)')]">
					<xsl:variable name="new" select="$config/roman-number[@search=lower-case($StartAtNumber)]/@replace" />
					<xsl:value-of select="if ($new) then $new else $StartAtNumber" />
				</xsl:when>
				<xsl:when test="$ListItem[@ListMarker=('Numeric','(Numeric)')]">
					<xsl:value-of select="$StartAtNumber" />
				</xsl:when>
			</xsl:choose>
		</xsl:if>
	</xsl:function>
	
</xsl:stylesheet>