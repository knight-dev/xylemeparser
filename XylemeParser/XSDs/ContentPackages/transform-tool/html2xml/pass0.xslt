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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:html40="http://www.w3.org/TR/REC-html40" 
	xmlns:xhtml="http://www.w3.org/1999/xhtml">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:template match="html40:* | xhtml:*">
		<xsl:element name="{local-name()}">
			<xsl:apply-templates select="@*|node()" />
		</xsl:element>
	</xsl:template>

	<xsl:template match="@style">
		<xsl:variable name="that" select=".."/>
		<xsl:for-each select="tokenize(., ';')">
			<xsl:if test=".!=''">
				<xsl:variable name="name" select="normalize-space(substring-before(concat(., ':'), ':'))"/>
				<xsl:if test="matches($name, '^[a-zA-Z0-9-]+$', 's') and not($that/@*[local-name()=$name])">
					<xsl:attribute name="{$name}" select="normalize-space(substring-after(., ':'))" />
				</xsl:if>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="processing-instruction() | comment()" priority="-1" />
	
	<!-- convert mso:if comment node to regular element -->
	<xsl:template
		match="comment[starts-with(., '[if ') and ends-with(., '&lt;![endif]')]">
		<xsl:element name="if">
			<xsl:attribute name="test" select="substring-before(substring-after(., '[if '), ']&gt;')" />
			<xsl:variable name="xml"
				select="substring-after(substring(., 1, string-length(.)-string-length('&lt;![endif]')), ']&gt;')" />
			<xsl:copy-of xmlns:java="http://xml.apache.org/xalan/java"
				select="java:europa.client.sdocfs.modeloperations.PasteSpecialTransferableExtractor.parseDocument($xml)" />
		</xsl:element>
	</xsl:template>

</xsl:stylesheet>