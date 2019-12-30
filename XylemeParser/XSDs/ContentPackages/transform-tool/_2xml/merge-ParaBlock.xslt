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

	<xsl:function name="my:ParaBlockSpec" as="xs:string">
		<xsl:param name="ParaBlock" as="element()" />
		<xsl:value-of select="string-join(for $v in $ParaBlock/@*[name()='boundingRect'] return concat(name($v), ': ', $v, ';'), ' ')" />
	</xsl:function>

	<xsl:function name="my:CanMergeParaBlock" as="xs:boolean">
		<xsl:param name="ParaBlock1" as="element()" />
		<xsl:param name="ParaBlock2" as="element()" />
		<xsl:value-of select="my:ParaBlockSpec($ParaBlock1)=my:ParaBlockSpec($ParaBlock2)" />
	</xsl:function>
	
	<!-- merge ParaBlock -->
	<xsl:template match="ParaBlock">
		<xsl:if test="not(preceding-sibling::node()[1]/self::ParaBlock[my:CanMergeParaBlock(., current())])">
			<xsl:copy>
				<xsl:apply-templates mode="__attr" select="." />
				<xsl:variable name="pass0">
					<xsl:apply-templates mode="__node" select="." />
				</xsl:variable>
				<xsl:apply-templates select="$pass0" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template mode="__attr" match="ParaBlock">
		<xsl:apply-templates select="@*" />
		<xsl:apply-templates mode="__attr" select="following-sibling::node()[1]/self::ParaBlock[my:CanMergeParaBlock(current(), .)]" />
	</xsl:template>

	<xsl:template mode="__node" match="ParaBlock">
		<xsl:copy-of select="node()" />
		<xsl:apply-templates mode="__node" select="following-sibling::node()[1]/self::ParaBlock[my:CanMergeParaBlock(current(), .)]" />
	</xsl:template>

</xsl:stylesheet>