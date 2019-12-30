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

	<xsl:include href="../html2xml/pass0.xslt?mode=html2xml-pass0" />
	<xsl:include href="../html2xml/pass01.xslt?mode=html2xml-pass01" />
	<xsl:include href="../html2xml/pass1.xslt?mode=html2xml-pass1" />
	<xsl:include href="../html2xml/pass2.xslt?mode=html2xml-pass2" />
	<xsl:include href="../html2xml/pass3.xslt?mode=html2xml-pass3" />
	<xsl:include href="../html2xml/pass4.xslt?mode=html2xml-pass4" />
	<xsl:include href="../html2xml/pass5.xslt?mode=html2xml-pass5" />
	<xsl:include href="../_2xml/struct.xslt?mode=struct" />
	<xsl:include href="../_2xml/struct-ListItem.xslt?mode=struct-ListItem" />
	<xsl:include href="../_2xml/merge-ListItem.xslt?mode=merge-ListItem" />
	<xsl:include href="../_2xml/merge-InLine0.xslt?mode=merge-InLine0" />
	<xsl:include href="../_2xml/merge-InLine.xslt?mode=merge-InLine" />
	<xsl:include href="../_2xml/upload.xslt?mode=upload" />
	
	<xsl:template mode="html2xml" match="node()">
		<xsl:param name="base" tunnel="yes" />
		<xsl:param name="element" tunnel="yes" />
		<xsl:param name="br2p" tunnel="yes" />
		<xsl:variable name="pass0">
			<xsl:apply-templates mode="html2xml-pass0" select="." />
		</xsl:variable>
		<xsl:variable name="pass01">
			<xsl:apply-templates mode="html2xml-pass01" select="$pass0" />
		</xsl:variable>
		<xsl:variable name="pass1">
			<xsl:apply-templates mode="html2xml-pass1" select="$pass01" />
		</xsl:variable>
		<xsl:variable name="struct">
			<xsl:apply-templates mode="struct" select="$pass1/node()" />
		</xsl:variable>
		<xsl:variable name="pass20">
			<xsl:apply-templates mode="merge-InLine0" select="$struct" />
		</xsl:variable>
		<xsl:variable name="pass201">
			<xsl:apply-templates mode="merge-InLine" select="$pass20" />
		</xsl:variable>
		<xsl:variable name="pass21">
			<xsl:apply-templates mode="html2xml-pass2" select="$pass201" />
		</xsl:variable>
		<xsl:variable name="struct-ListItem">
			<xsl:apply-templates mode="struct-ListItem" select="$pass21" />
		</xsl:variable>
		<xsl:variable name="merge-ListItem">
			<xsl:apply-templates mode="merge-ListItem" select="$struct-ListItem" />
		</xsl:variable>
		<xsl:variable name="pass3">
			<xsl:apply-templates mode="html2xml-pass3" select="$merge-ListItem/node()" />
		</xsl:variable>
		<xsl:variable name="pass4">
			<xsl:apply-templates mode="html2xml-pass4" select="$pass3/node()" />
		</xsl:variable>
		<xsl:variable name="pass5">
			<xsl:apply-templates mode="html2xml-pass5" select="$pass4/node()" />
		</xsl:variable>
		<xsl:variable name="merge-InLine">
			<xsl:apply-templates mode="merge-InLine" select="$pass5/node()" />
		</xsl:variable>
		<xsl:apply-templates mode="upload" select="$merge-InLine/node()" />
	</xsl:template>

</xsl:stylesheet> 