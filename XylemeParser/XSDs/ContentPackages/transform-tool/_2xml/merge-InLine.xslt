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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:my="urn:my.namespace" exclude-result-prefixes="my">

	<xsl:import href="identity.xslt" />

	<!--
		compare if two inline elements are the same, i.e. can be merged
		<Emph>text1</Emph><Emph>text2</Emph> ==> <Emph>text1text2</Emph> 
	-->
	<xsl:function name="my:SameInLine">
		<xsl:param name="p1" />
		<xsl:param name="p2" />
		<xsl:value-of select="$p1[namespace-uri()=$p2/namespace-uri() and local-name()=$p2/local-name()]" />
	</xsl:function>

	<!--
		check if element is inline, i.e. can be merged 
	-->
	<xsl:function name="my:IsInLine">
		<xsl:param name="self" />
		<xsl:value-of select="$config/merge-InLine/*[my:SameInLine(., $self)]" />
	</xsl:function>

	<!-- 
		check if whitespace can be merged between same inlines:
		<Emph>text1</Emph> <Emph>text2</Emph> ==> <Emph>text1 text2</Emph> 
	-->
	<xsl:function name="my:CanInLineText">
		<xsl:param name="self" />
		<xsl:value-of select="$self[translate(., ' &#13;&#10;&#9;', '')='']" />
	</xsl:function>

	<!-- 
		drop empty inlines:
		<Emph><Emph> ==> 
	-->
	<xsl:template match="*[my:IsInLine(.)][not(node())]" priority="1.2" />

	<!--
		fix inlines inside inlines:
		<Emph>text1<Emph>text2</Emph>text3</Emph> => <Emph>text1text2text3</Emph>
	-->
	<xsl:template match="*[my:IsInLine(.)]/*[my:SameInLine(.., .)]" priority="1.1">
		<xsl:apply-templates select="node()" />
	</xsl:template>
	
	<!-- merge elements, incl. separated by space -->
	<xsl:template match="*[my:IsInLine(.)]">
		<xsl:if
			test="not(preceding-sibling::node()[1][my:SameInLine(., current()) or
			self::text()[my:CanInLineText(.) and preceding-sibling::node()[1][my:SameInLine(., current())]]])">
			<xsl:copy>
				<xsl:apply-templates mode="__attr" select="." />
				<xsl:apply-templates mode="__node" select="." />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="text()[my:CanInLineText(.) and 
		my:SameInLine(preceding-sibling::node()[1][my:IsInLine(.)], following-sibling::node()[1])]" />

	<xsl:template mode="__node" match="*[my:IsInLine(.)]">
		<xsl:apply-templates select="node()" />
		<xsl:apply-templates mode="__node"
			select="following-sibling::node()[1][my:SameInLine(., current()) or 
				self::text()[my:CanInLineText(.) and following-sibling::node()[1][my:SameInLine(., current())]]]" />
	</xsl:template>

	<xsl:template mode="__node" match="text()">
		<xsl:copy-of select="." />
		<xsl:variable name="current" select="preceding-sibling::node()[1]" />
		<xsl:apply-templates mode="__node" select="following-sibling::node()[1][my:SameInLine(., $current)]" />
	</xsl:template>

	<xsl:template mode="__attr" match="*[my:IsInLine(.)]">
		<xsl:apply-templates select="@*" />
		<xsl:apply-templates mode="__attr" select="following-sibling::node()[1][my:SameInLine(., current())]" />
	</xsl:template>

</xsl:stylesheet>