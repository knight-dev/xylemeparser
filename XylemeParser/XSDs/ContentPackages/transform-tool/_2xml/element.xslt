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

	<xsl:param name="documentTitle" />
	<xsl:param name="element" select="'Topic'" />

	<xsl:template match="/Topic/Title[not(node())]">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
			<xsl:value-of select="$documentTitle" />
		</xsl:copy>
	</xsl:template>

	<xsl:template match="/Topic[$element='Topic']">
		<xsl:next-match />
	</xsl:template>

	<xsl:template match="/Topic[$element='Lesson']">
		<Lesson>
			<xsl:apply-templates select="@*|Title" />
			<Topic>
				<Title />
				<xsl:apply-templates select="node() except Title" />
			</Topic>
		</Lesson>
	</xsl:template>

	<xsl:template match="/Topic[$element='Module']">
		<Module>
			<xsl:apply-templates select="@*|Title" />
			<Lesson>
				<Title />
				<Topic>
					<Title />
					<xsl:apply-templates select="node() except Title" />
				</Topic>
			</Lesson>
		</Module>
	</xsl:template>

	<xsl:template match="/Topic[$element='IA/Lesson']">
		<IA>
			<CoverPage>
				<xsl:apply-templates select="@*|Title" />
			</CoverPage>
			<PrintProperties>
				<PublishingOptions BodyPageStyle="StandardMarginBodyPage" CoverPageStyle="CenteredTitleStyle"
					GenerateGlossary="true" GenerateIndex="true" GenerateTOC="true" TOC_HierarchyDepth="3" GlossaryStyle="Both" />
			</PrintProperties>
			<Lessons>
				<Lesson>
					<Title />
					<Topic>
						<Title />
						<xsl:apply-templates select="node() except Title" />
					</Topic>
				</Lesson>
			</Lessons>
		</IA>
	</xsl:template>

	<xsl:template match="/Topic[$element='IA/Module']">
		<IA>
			<CoverPage>
				<xsl:apply-templates select="@*|Title" />
			</CoverPage>
			<PrintProperties>
				<PublishingOptions BodyPageStyle="StandardMarginBodyPage" CoverPageStyle="CenteredTitleStyle"
					GenerateGlossary="true" GenerateIndex="true" GenerateTOC="true" TOC_HierarchyDepth="3" GlossaryStyle="Both" />
			</PrintProperties>
			<Modules>
				<Module>
					<Title />
					<Lesson>
						<Title />
						<Topic>
							<Title />
							<xsl:apply-templates select="node() except Title" />
						</Topic>
					</Lesson>
				</Module>
			</Modules>
		</IA>
	</xsl:template>

	<xsl:template match="/Topic[$element='SlideDeck']">
		<SlideDeck>
			<xsl:apply-templates select="@*|Title" />
			<DeckWatermarks />
			<xsl:apply-templates select="node() except Title" />
		</SlideDeck>
	</xsl:template>

	<xsl:template match="/Topic[$element='SlideDeck']/ParaBlock">
		<Slide>
			<xsl:apply-templates select="@*" />
			<Title />
			<Body>
				<xsl:apply-templates select="node()" />
			</Body>
		</Slide>
	</xsl:template>

</xsl:stylesheet>