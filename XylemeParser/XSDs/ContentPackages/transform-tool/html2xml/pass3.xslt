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

	<!--
		lift caption inside the figure or table
	-->
	<xsl:template match="Caption[not(parent::Figure)]">
		<RichText>
			<xsl:apply-templates select="@*|node()"/>
		</RichText>
	</xsl:template>
	
	<xsl:template match="Caption[@after][preceding-sibling::*[1][self::Figure|self::Table]]
		| Caption[@before][following-sibling::*[1][self::Figure|self::Table]]" priority="1.1"/>

	<!--
		lift caption inside the figure
	-->
	<xsl:template match="Figure[following-sibling::*[1][self::Caption[@after]]
		| preceding-sibling::*[1][self::Caption[@before]]]">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
			<xsl:variable name="caption" select="following-sibling::*[1][self::Caption[@after]]
				| preceding-sibling::*[1][self::Caption[@before]]" />
			<Caption>
				<xsl:apply-templates select="$caption/(node())" />
			</Caption>
		</xsl:copy>
	</xsl:template>

	<!--
		lift caption inside the table
	-->
	<xsl:template match="Table[following-sibling::*[1][self::Caption[@after]]
		| preceding-sibling::*[1][self::Caption[@before]]]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:variable name="caption" select="following-sibling::*[1][self::Caption[@after]]
				| preceding-sibling::*[1][self::Caption[@before]]" />
			<TblTitle>
				<xsl:apply-templates select="$caption/(node())" />
			</TblTitle>
			<xsl:apply-templates select="node()" />
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>