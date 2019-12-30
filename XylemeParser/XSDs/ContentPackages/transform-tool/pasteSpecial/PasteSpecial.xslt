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

	<xsl:output method="xml" omit-xml-declaration="yes" indent="no" encoding="utf-8" />

	<xsl:include href="config.xslt" />
	<xsl:include href="../html2xml/pass0.xslt?mode=pass0" />
	<xsl:include href="../html2xml/pass1.xslt?mode=pass1" />
	<xsl:include href="pass1.xslt?mode=pass1" />
	<xsl:include href="../html2xml/pass2.xslt?mode=pass2" />
	<xsl:include href="../_2xml/struct.xslt?mode=struct" />
	<xsl:include href="../_2xml/struct-ListItem.xslt?mode=struct-ListItem" />
	<xsl:include href="../_2xml/merge-ListItem.xslt?mode=merge-ListItem" />
	<xsl:include href="../html2xml/pass3.xslt?mode=pass3" />
	<xsl:include href="pass3.xslt?mode=pass3" />
	<xsl:include href="../_2xml/merge-InLine.xslt?mode=merge-InLine" />
	<xsl:include href="../html2xml/pass4.xslt?mode=pass4" />
	<xsl:include href="../_2xml/main_def.xslt?mode=def" />

	<xsl:template mode="def" match="RichText/@* | ListPreamble/@* | Item/@* | ItemPara/@* | Cell/Topic" />

	<xsl:template match="/">
		<xsl:variable name="pass0">
			<xsl:apply-templates mode="pass0" select="/node()" />
		</xsl:variable>
		<xsl:variable name="pass1">
			<xsl:apply-templates mode="pass1" select="$pass0/node()" />
		</xsl:variable>
		<xsl:variable name="struct">
			<xsl:apply-templates mode="struct" select="$pass1/node()" />
		</xsl:variable>
		<xsl:variable name="pass2">
			<xsl:apply-templates mode="pass2" select="$struct/node()" />
		</xsl:variable>
		<xsl:variable name="struct-ListItem">
			<xsl:apply-templates mode="struct-ListItem" select="$pass2" />
		</xsl:variable>
		<xsl:variable name="merge-ListItem">
			<xsl:apply-templates mode="merge-ListItem" select="$struct-ListItem" />
		</xsl:variable>
		<xsl:variable name="pass3">
			<xsl:apply-templates mode="pass3" select="$merge-ListItem/node()" />
		</xsl:variable>
		<xsl:variable name="pass4">
			<xsl:apply-templates mode="pass4" select="$pass3/node()" />
		</xsl:variable>
		<xsl:variable name="result" select="$pass4" />

		<xsl:variable name="xy_result">
			<xsl:choose>
				<xsl:when test="$result/text()[normalize-space(translate(., '&#13;&#10;&#160;', ' '))!='']">
					<RichText>
						<xsl:copy-of select="$result/node()" />
					</RichText>
				</xsl:when>
				<xsl:when test="count($result/*)=1">
					<xsl:copy-of select="$result/*" />
				</xsl:when>
				<xsl:when test="$result/Topic">
					<Topic>
						<Title />
						<xsl:copy-of select="$result/*" />
					</Topic>
				</xsl:when>
				<xsl:otherwise>
					<ParaBlock>
						<xsl:copy-of select="$result/*" />
					</ParaBlock>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:apply-templates mode="def" select="$xy_result/node()" />
	</xsl:template>

</xsl:stylesheet>