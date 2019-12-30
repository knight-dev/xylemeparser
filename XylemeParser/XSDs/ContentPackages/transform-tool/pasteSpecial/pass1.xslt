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

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:java="http://xml.apache.org/xalan/java"
	xmlns:xy="http://xyleme.com/xylink"
	exclude-result-prefixes="java xy">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:template match="p[count(node())=1 and br]" priority="9"/>

	<xsl:template match="span[@mso-tab-count&gt;0 and translate(., '&#160;', '')='']" priority="9">
		<xsl:for-each select="1 to @mso-tab-count">
			<xsl:value-of select="'&#9;'" />
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="p[@mso-list]" priority="9">
		<ListItem ListMarker="Bullet" ListLevel="0">
			<ItemPara>
				<xsl:apply-templates select="node()" />
			</ItemPara>
		</ListItem>
	</xsl:template>

	<xsl:template match="table" priority="9">
		<Table align="left" width="100" border="true">
			<TblGroup>
				<xsl:choose>
					<xsl:when test="col | colgroup">
						<xsl:apply-templates
							select="col | colgroup" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="msotr" select="comment()[starts-with(., '[if !supportMisalignedColumns]&gt;')
									and contains(., '&lt;tr ') and contains(., '&lt;/tr&gt;')]"/>
						<xsl:variable name="coltr0">
							<xsl:choose>
								<xsl:when test="tr[not(td[@colspan&gt;1])][1]">
									<xsl:copy-of select="tr[not(td[@colspan&gt;1])][1]"/>
								</xsl:when>
								<xsl:when test="$msotr">
									<xsl:variable name="xml0" select="concat('&lt;tr ', substring-before(substring-after($msotr, '&lt;tr '), '&lt;/tr&gt;'), '&lt;/tr&gt;')"/>
									<xsl:variable name="xml" select="java:europa.client.sdocfs.modeloperations.PasteSpecialTransferableExtractor.parseDocument($xml0)"/>
									<xsl:copy-of select="$xml/tr"/>								
								</xsl:when>
								<xsl:otherwise>
									<xsl:message select="'there should be at least one row that does not have colspan'" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="coltr" select="$coltr0/node()"/>

						<xsl:if test="$coltr">
							<xsl:variable name="table-width">
								<xsl:choose>
									<xsl:when test="@width">
										<xsl:value-of select="@width" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of
											select="sum($coltr/td/@width)" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
	
							<xsl:for-each select="$coltr/td">
								<TblCol
									width="{format-number(@width div $table-width * 100, '#')}" />
							</xsl:for-each>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
				<TblHeader />
				<xsl:choose>
					<xsl:when test="tbody">
						<xsl:apply-templates select="tbody" />
					</xsl:when>
					<xsl:otherwise>
						<TblBody>
							<xsl:apply-templates select="node()[name()!='col' and name()!='colgroup']" />
						</TblBody>
					</xsl:otherwise>
				</xsl:choose>
				<TblFooter />
			</TblGroup>
		</Table>
	</xsl:template>

</xsl:stylesheet>