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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:my="urn:my.namespace" exclude-result-prefixes="xs my">

	<xsl:import href="identity.xslt" />

	<xsl:function name="my:IsTitled" as="xs:boolean">
		<xsl:param name="Titled" as="element()" />
		<xsl:value-of select="exists($config/merge-Titled/*[node-name(.)=node-name($Titled)])" />
	</xsl:function>

	<xsl:function name="my:TitledSpec" as="xs:string">
		<xsl:param name="Titled" as="element()" />
		<xsl:variable name="title" select="lower-case(normalize-space(translate($Titled/Title, '&#160;', ' ')))" as="xs:string" />
		<xsl:variable name="regexp" select="$config/merge-Titled/*[node-name(.)=node-name($Titled)][matches($title, @text, 's')]/@text" as="xs:string?" />
		<xsl:value-of select="if ($regexp) then replace($title, $regexp, '', 's') else $title" />
	</xsl:function>

	<xsl:function name="my:CanMergeTitled" as="xs:boolean?">
		<xsl:param name="Titled1" as="element()" />
		<xsl:param name="Titled2" as="element()" />
		<xsl:if test="node-name($Titled1)=node-name($Titled2)">
			<xsl:choose>
				<xsl:when test="$Titled1[count(node())=1 and Title]">
					<xsl:value-of select="'true'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="my:TitledSpec($Titled1)=my:TitledSpec($Titled2) 
						and not($Titled2[count(node())=1 and Title]/following-sibling::node()[1]/self::Titled)" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:function>
	
	<!-- merge Titled by Title -->
	<xsl:template match="*[my:IsTitled(.)]">
		<xsl:if test="not(preceding-sibling::node()[1][my:CanMergeTitled(., current())])">
			<xsl:copy>
				<xsl:apply-templates mode="__attr" select="." />
				<xsl:variable name="pass0">
					<xsl:apply-templates mode="__node" select="." />
				</xsl:variable>
					<xsl:if test="$pass0/Title">
						<Title>
							<xsl:apply-templates select="$pass0/Title/@*" />
							<xsl:for-each select="$pass0/Title">
								<xsl:if test="preceding-sibling::node()[1]/self::Title[normalize-space()!='' or *]">
									<xsl:value-of select="'&#10;'" />
								</xsl:if>
								<xsl:if test="(normalize-space()!='' or *) and (not(preceding-sibling::Title) or preceding-sibling::node()[1]/self::Title)">
									<xsl:apply-templates select="node()" />
								</xsl:if>
							</xsl:for-each>
						</Title>
					</xsl:if>
				<xsl:variable name="pass1">
					<xsl:copy-of select="$pass0/(node() except Title)"/>
				</xsl:variable>
				<xsl:apply-templates select="$pass1" />
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template mode="__attr" match="*">
		<xsl:apply-templates select="@*" />
		<xsl:apply-templates mode="__attr" select="following-sibling::node()[1][my:CanMergeTitled(current(), .)]" />
	</xsl:template>

	<xsl:template mode="__node" match="*">
		<xsl:copy-of select="node()" />
		<xsl:apply-templates mode="__node" select="following-sibling::node()[1][my:CanMergeTitled(current(), .)]" />
	</xsl:template>

</xsl:stylesheet>