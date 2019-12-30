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

	<xsl:template name="PrintProperties">
		<PrintProperties>
			<PublishingOptions BodyPageStyle="StandardMarginBodyPage" CoverPageStyle="CenteredTitleStyle" GenerateTOC="true" TOC_HierarchyDepth="3">
				<xsl:if test="not($customer='bpp')">
					<xsl:attribute name="GenerateGlossary" select="'true'" />
					<xsl:attribute name="GenerateIndex" select="if (not($customer='capgemini') or Topic/Title='Index') then 'true' else 'false'" />
					<xsl:attribute name="GlossaryStyle" select='"Both"' />
				</xsl:if>
			</PublishingOptions>
		</PrintProperties>
	</xsl:template>

	<xsl:template name="WebPublishingProperties">
		<xsl:if test="not($customer=('allstate1', 'esi','forum-pg','forum-fg'))">
			<WebPublishingProperties>
				<WebCourseOptions RandomizeChoices="true" ShowMarginNotes="true" />
			</WebPublishingProperties>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>