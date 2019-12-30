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

	<xsl:template mode="regexp" match="*">
		<xsl:variable name="style" select="string-join(for $v in @* return concat(name($v), ': ', $v, ';'), ' ')" />
		<xsl:variable name="text" select="string(.)" />

		<xsl:variable name="elements" select="$config/*[local-name()=current()/local-name() and namespace-uri()=current()/namespace-uri()]"/>
		<xsl:variable name="element1"
			select="$elements
			[@style and matches($style, @style, 's') 
				and @text and matches($text, @text, 's')][last()]" />
		<xsl:variable name="element2"
			select="$elements[not($element1)]
			[@style and matches($style, @style, 's') 
				and (not(@text) or matches($text, @text, 's'))][last()]" />
		<xsl:variable name="element3"
			select="$elements[not($element1) and not($element2)]
			[(not(@style) or matches($style, @style, 's')) 
				and (not(@text) or matches($text, @text, 's'))][last()]" />
		<xsl:variable name="element" select="($element1, $element2, $element3)[1]" />

		<xsl:choose>
			<xsl:when test="$element">
				<xsl:apply-templates mode="placeholder" select="$element/node()">
					<xsl:with-param name="regexp" select="$element/@text" tunnel="yes" />
					<xsl:with-param name="content" select="." tunnel="yes" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="." />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- 
		background copy template 
	-->
	<xsl:template mode="placeholder" priority="-1" match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates mode="placeholder" select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template mode="placeholder" match="comment()"/>
		
	<xsl:template mode="placeholder" match="placeholder">
		<xsl:param name="regexp" tunnel="yes"/>
		<xsl:param name="content" tunnel="yes"/>
		<xsl:choose>
			<xsl:when test="$regexp!='' and not(@no_replace)">
				<xsl:attribute name="replace" select="substring($content, 1, string-length($content)-string-length(replace($content, $regexp, '', 's')))"/>				
				<xsl:copy-of select="$content[not(current()/@no_attr)]/@*"/>
				<xsl:apply-templates mode="replace" select="$content/node()">
					<xsl:with-param name="regexp" select="$regexp" tunnel="yes"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$content/(@*[not(current()/@no_attr)]|node())"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template mode="placeholder" match="@placeholder">
		<xsl:param name="content" tunnel="yes"/>
		<xsl:copy-of select="$content/@*"/>
	</xsl:template>
	
	<xsl:template mode="placeholder" match="text()">
		<xsl:value-of select="normalize-space()"/>
	</xsl:template>

	<xsl:template mode="placeholder" match="text"> <!-- text w/o normalize-space() -->
		<xsl:value-of select="."/>
	</xsl:template>

	<!-- 
		background copy template 
	-->
	<xsl:template mode="replace" priority="-1" match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates mode="replace" select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template mode="replace" match="text()">
		<xsl:param name="regexp" tunnel="yes"/>
		<xsl:param name="flags" select="'s'" tunnel="yes"/>
		<xsl:choose>
			<xsl:when test="starts-with($regexp, '^') and ends-with($regexp, '$')">
				<!-- no action -->
			</xsl:when>
			<xsl:when test="starts-with($regexp, '^')">
				<xsl:variable name="preceding-text" select="string-join(preceding::text(), '')"/>
				<xsl:choose>
					<xsl:when test="matches($preceding-text, $regexp, $flags)">
						<xsl:value-of select="."/>
					</xsl:when>
					<xsl:when test="matches(concat($preceding-text, .), $regexp, $flags)">
						<xsl:value-of select="replace(concat($preceding-text, .), $regexp, '', $flags)" />
					</xsl:when>
					<xsl:otherwise>
						<!-- no match exists -->
						<xsl:variable name="following-text" select="string-join(following::text(), '')"/>
						<xsl:if test="not(matches(concat($preceding-text, ., $following-text), $regexp, $flags))">
							<xsl:value-of select="."/>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="ends-with($regexp, '$')">
				<xsl:variable name="following-text" select="string-join(following::text(), '')"/>
				<xsl:choose>
					<xsl:when test="matches($following-text, $regexp, $flags)">
						<xsl:value-of select="."/>
					</xsl:when>
					<xsl:when test="matches(concat(., $following-text), $regexp, $flags)">
						<xsl:value-of select="replace(concat(., $following-text), $regexp, '', $flags)" />
					</xsl:when>
					<xsl:otherwise>
						<!-- no match exists -->
						<xsl:variable name="preceding-text" select="string-join(preceding::text(), '')"/>
						<xsl:if test="not(matches(concat($preceding-text, ., $following-text), $regexp, $flags))">
							<xsl:value-of select="."/>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes" select="concat('regexp w/o ^ or $ is not supported: ', $regexp)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>