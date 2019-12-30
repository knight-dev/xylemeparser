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
		<xsl:apply-templates select="ListPreamble" />
		<StepGroup>
			<xsl:apply-templates select="node() except ListPreamble" />
		</StepGroup>
	</xsl:template>

	<xsl:template match="List/ListPreamble">
		<RichText>
			<xsl:apply-templates select="@*|node()" />
		</RichText>
	</xsl:template>

	<xsl:template match="List/ItemBlock">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item">
		<Step>
			<xsl:apply-templates select="@*" />
			<UserAction>
				<xsl:apply-templates select="ItemPara" />
			</UserAction>
			<xsl:if test="node() except ItemPara">
				<ParaBlock>
					<xsl:apply-templates select="node() except ItemPara" />
				</ParaBlock>
			</xsl:if>
		</Step>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/*" priority="-1">
		<xsl:copy-of select="." />
	</xsl:template>
	
	<xsl:template match="List/ItemBlock/Item/ItemPara">
		<ActionDescription>
			<xsl:apply-templates select="@*|node()" />
		</ActionDescription>
	</xsl:template>

	<xsl:template match="List/ItemBlock/Item/SubList">
		<List>
			<xsl:apply-templates select="@*|node()" />
		</List>
	</xsl:template>
	
</xsl:stylesheet>