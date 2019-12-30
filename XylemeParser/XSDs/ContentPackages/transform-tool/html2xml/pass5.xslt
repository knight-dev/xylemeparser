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
	xmlns:my="urn:my.namespace" exclude-result-prefixes="my">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:function name="my:TableRowSpec">
		<xsl:param name="self"/>
		<xsl:value-of select="count($self/Cell/RichText/(*|text()[translate(., ' &#13;&#10;&#9;&#160;', '')!=''])|$self/Cell/(* except RichText)|$self/Cell[@rowspan&gt;1])"/>
	</xsl:function>
	
	<xsl:template match="TableRow[my:TableRowSpec(.)=0]"/> <!-- and preceding-sibling::TableRow/my:TableRowSpec(.)=0 -->
	
	<xsl:template match="Item/List">
		<SubList>
			<xsl:apply-templates select="@*|node()"/>
		</SubList>
	</xsl:template>
	
	<xsl:template match="Item[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<ItemPara />
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>