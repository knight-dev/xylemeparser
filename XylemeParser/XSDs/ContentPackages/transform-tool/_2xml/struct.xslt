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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:my="urn:mynamespace" exclude-result-prefixes="my">

	<xsl:import href="identity.xslt" />

	<xsl:template match="@level | @ender" />

	<xsl:function name="my:level">
		<xsl:param name="self" />
		<xsl:value-of select="if ($self/@level !='') then $self/@level else '999'" />
	</xsl:function>

	<!-- should work only on elements, no text() because it makes formatted xml not process -->
	<xsl:template match="*">
		<xsl:variable name="level" select="my:level(.)" />
		<xsl:variable name="pass0">
			<xsl:apply-templates mode="__level" select="following-sibling::*[1][my:level(.)>$level]">
				<xsl:with-param name="level" select="$level" tunnel="yes" />
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:if test="not(self::ender)">
			<xsl:copy>
				<xsl:apply-templates select="@*" />
				<xsl:choose>
					<xsl:when test="@ender">
						<xsl:apply-templates select="node()" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="pass1">
							<xsl:copy-of select="node(),$pass0" />
						</xsl:variable>
						<xsl:apply-templates select="$pass1" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:copy>
		</xsl:if>
		<xsl:if test="@ender or self::ender">
			<xsl:apply-templates select="$pass0" />
		</xsl:if>
	</xsl:template>

	<xsl:template mode="__level" match="*">
		<xsl:param name="level" tunnel="yes" />
		<xsl:copy-of select="." />
		<xsl:apply-templates mode="__level" select="following-sibling::*[1][my:level(.)>$level]" />
	</xsl:template>

	<xsl:template priority="1.1" match="*[my:level(.)>preceding-sibling::*/my:level(.)]" />

</xsl:stylesheet> 