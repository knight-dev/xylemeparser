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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:ImportHelper="java:com.xyleme.transform.poi.ImportOfficeHelper"
	exclude-result-prefixes="xs ImportHelper">

	<xsl:import href="identity.xslt" />
	
	<xsl:param name="imagesFolder" />
	<xsl:param name="imagesPrefix" />

	<xsl:template match="LaunchFlashFile">
		<xsl:param name="base" tunnel="yes"/>
		<xsl:copy>
			<xsl:value-of select="ImportHelper:uploadFile($imagesFolder, $imagesPrefix, concat($base, .))"/>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="Web | Icon[not(starts-with(@uri, 'DemoMedia/'))]">
		<xsl:param name="base" tunnel="yes"/>
		<xsl:if test="self::Web[matches(@uri, '\.[we]m[fz]$', 's')]">
			<Print uri="{ImportHelper:uploadFile($imagesFolder, $imagesPrefix, concat($base, @uri))}" 
				width-cm="{format-number(@origWidth div 96 * 2.54, '#.##')}"
				height-cm="{format-number(@origHeight div 96 * 2.54, '#.##')}" />
		</xsl:if>
		<xsl:copy>
			<xsl:attribute name="uri" select="ImportHelper:uploadImage($imagesFolder, $imagesPrefix, concat($base, @uri), 
				(if (@origWidth>0) then @origWidth else -1) cast as xs:integer, 
				(if (@origHeight>0) then @origHeight else -1) cast as xs:integer)"/>
			<xsl:apply-templates select="@* except @uri | *"/>
			
			<xsl:variable name="text" select="translate(text(), ' &#13;&#10;&#9;', '')"/>
			<xsl:if test="$text!=''">
				<xsl:value-of select="ImportHelper:queueFile($imagesFolder, $imagesPrefix, concat($base, @uri), $text)" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="InLineImage">
		<xsl:param name="base" tunnel="yes"/>
		<xsl:copy>
			<xsl:attribute name="mediaUri" select="ImportHelper:uploadImage($imagesFolder, $imagesPrefix, concat($base, @uri), 
				(if (@origWidth>0) then @origWidth else -1) cast as xs:integer, 
				(if (@origHeight>0) then @origHeight else -1) cast as xs:integer)"/>
			<xsl:apply-templates select="@* except (@uri|@origWidth|@origHeight) | *"/>

			<xsl:variable name="text" select="translate(text(), ' &#13;&#10;&#9;', '')"/>
			<xsl:if test="$text!=''">
				<xsl:value-of select="ImportHelper:queueFile($imagesFolder, $imagesPrefix, concat($base, @uri), $text)" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>