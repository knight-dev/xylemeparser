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

	<!-- sort attributes alphabetically for easy compare -->
	<xsl:template match="*" priority="-1">
		<xsl:copy>
			<xsl:apply-templates select="@*">
				<xsl:sort select="name()" />
			</xsl:apply-templates>
			<xsl:apply-templates select="node()" />
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>