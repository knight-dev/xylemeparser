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

	<xsl:template match="center">
		<p align="center" text-align="center">
			<xsl:apply-templates select="@*|node()" />
		</p>
	</xsl:template>

	<!-- 
		break inline/br:
		<b>text1<br/>text2<b> ==> <b>text1</b><br/><b>text2</b> 
	-->
	<xsl:template match="*[name()=('b', 'i', 'u', 'strong', 'em')][br or sup or sub]">
		<xsl:variable name="self" select="." />
		<xsl:for-each-group select="node()" group-adjacent="name()=('br', 'sup', 'sub')">
			<xsl:choose>
				<xsl:when test="current-grouping-key()">
					<xsl:apply-templates select="current-group()" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:element name="{$self/name()}">
						<xsl:apply-templates select="$self/@*,current-group()" />
					</xsl:element>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each-group>
	</xsl:template>

	<xsl:template match="*[name()=('b', 'i', 'u', 'strong', 'em')][count(node())=1 and span]">
		<xsl:variable name="pass0">
			<xsl:copy>
				<xsl:apply-templates select="@*|span/@*|span/node()"/>
			</xsl:copy>
		</xsl:variable>
		<xsl:apply-templates select="$pass0" />
	</xsl:template>

</xsl:stylesheet>