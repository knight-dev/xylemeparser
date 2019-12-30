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

	<xsl:import href="identity.xslt" />

	<xsl:template match="List">
		<EducationalObjective>
			<xsl:apply-templates select="@*" />
			<xsl:if test="not(ListPreamble)">
				<ObjectiveIntro />
			</xsl:if>
			<xsl:apply-templates select="node()" />
		</EducationalObjective>
	</xsl:template>

	<xsl:template match="List/@ListMarker[.='Bullet']" />

	<xsl:template match="List/ListPreamble">
		<ObjectiveIntro>
			<xsl:apply-templates select="@*|node()" />
		</ObjectiveIntro>
	</xsl:template>

	<xsl:template match="List/ItemBlock">
		<xsl:apply-templates select="@*|node()" />
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item">
		<EnablingObjective>
			<xsl:apply-templates select="@*|node()" />
		</EnablingObjective>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/ItemPara">
		<ObjectiveStatement>
			<xsl:apply-templates select="@*|node()" />
		</ObjectiveStatement>
	</xsl:template>

</xsl:stylesheet>