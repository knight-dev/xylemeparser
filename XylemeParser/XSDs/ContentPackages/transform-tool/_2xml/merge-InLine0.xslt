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
		break inline/Figure:
		<Emph>text1<Figure/>text2<Emph> ==> <Emph>text1</Emph><Figure/><Emph>text2</Emph> 
	-->
	<xsl:template match="*[my:IsInLine(.) and Figure]">
		<xsl:variable name="self" select="." />
		<xsl:for-each-group select="node()" group-adjacent="name()='Figure'">
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

</xsl:stylesheet>