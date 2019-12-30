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

	<xsl:param name="customer" />

	<xsl:variable name="config">
		<xsl:variable name="general_config" select="document(('../_2xml/config.xml', '../_2xml/config_lists.xml', 'config.xml'))" />

		<xsl:variable name="customer_config">
			<xsl:if test="$customer!=''">
				<xsl:copy-of select="document(concat('config_', $customer, '.xml'))" />
			</xsl:if>
		</xsl:variable>

		<xsl:for-each select="$general_config/*/* | $customer_config/*/*">
			<xsl:sort select="@priority" />
			<xsl:copy-of select="." />
		</xsl:for-each>
	</xsl:variable>

</xsl:stylesheet>