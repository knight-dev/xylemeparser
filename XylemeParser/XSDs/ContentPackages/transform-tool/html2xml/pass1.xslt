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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:st1="urn:x-prefix:st1" xmlns:st2="urn:x-prefix:st2" xmlns:o="urn:x-prefix:o" xmlns:O="urn:x-prefix:O"
	xmlns:ST1="urn:x-prefix:ST1" xmlns:ST2="urn:x-prefix:ST2" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:my="urn:my.namespace" exclude-result-prefixes="o O st1 st2 ST1 ST2 xs my">

	<xsl:import href="../_2xml/identity.xslt" />

	<xsl:template match="html">
		<xsl:param name="element" tunnel="yes" />

		<xsl:variable name="content">
			<xsl:apply-templates select="body" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$element='Topic'">
				<Topic level="'0'">
					<Title>
						<xsl:value-of select="head/title"/>
					</Title>
					<ParaBlock level="9" />
					<xsl:copy-of select="$content" />
				</Topic>
			</xsl:when>
			<xsl:when test="$element='ParaBlock' and $content/*[1][not(self::Topic)]">
				<ParaBlock level="9" />
				<xsl:copy-of select="$content" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$content" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="h1 | h2 | h3 | h4 | h5 | h6">
		<Topic level="{substring-after(name(), 'h')}">
			<Title>
				<xsl:apply-templates select="node()" />
			</Title>
		</Topic>
		<ParaBlock level="9" />
	</xsl:template>

	<xsl:variable name="html-block-elements" select="('h1', 'h2', 'h3', 'h4', 'h5', 'h6',
		'div', 'p', 'ul', 'ol', 'li', 'table', 'script', 'noscript', 'object', 'embed', 'hr', 'blockquote',
		'dl', 'dt', 'dd', 'form', 'codeblock', 'note')" /> <!-- codeblock, note is from DITA -->

	<xsl:template match="body | div | noscript | p | li | blockquote | td | th | dl | dt | dd" name="div" priority="-1">
		<xsl:param name="br2p" tunnel="yes" />
		<xsl:variable name="self" select="." />
		<xsl:for-each-group select="node()" group-starting-with="*[name()=$html-block-elements]">
			<xsl:apply-templates select="current-group()[name()=$html-block-elements]" />
			<xsl:if test="current-group()[not(name()=$html-block-elements)]">
				<xsl:variable name="content">
					<xsl:for-each select="current-group()[not(name()=$html-block-elements)]">
						<xsl:apply-templates select="." />
					</xsl:for-each>
				</xsl:variable>
				<xsl:if test="$content/* or $content/text()[normalize-space()!='']">
					<xsl:choose>
						<xsl:when test="$br2p='yes' and $br2p='two' and $content/br">
							<xsl:for-each-group select="$content/node()" group-adjacent="name()='br' and following-sibling::node()[1]/self::br"> <!-- quite odd because self::br does not work -->
								<xsl:choose>
									<xsl:when test="current-grouping-key()"/>
									<xsl:otherwise>
										<RichText br2p="{string-join($br2p, ',')}">
											<xsl:apply-templates select="$self/@*,current-group()"/>
										</RichText>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each-group>
						</xsl:when>
						<xsl:when test="$br2p='yes' and $content/br">
							<xsl:for-each-group select="$content/node()" group-adjacent="name()='br'"> <!-- quite odd because self::br does not work -->
								<xsl:choose>
									<xsl:when test="current-grouping-key()"/>
									<xsl:otherwise>
										<RichText br2p="{string-join($br2p, ',')}">
											<xsl:apply-templates select="$self/@*,current-group()"/>
										</RichText>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each-group>
						</xsl:when>
						<xsl:otherwise>
							<RichText>
								<xsl:apply-templates select="$self/@*" />
								<xsl:copy-of select="$content" />
							</RichText>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:if>
		</xsl:for-each-group>
	</xsl:template>

	<xsl:template match="p1">
		<xsl:variable name="self" select="." />
		<xsl:variable name="content">
			<xsl:apply-templates select="node()" />
		</xsl:variable>
		<xsl:if test="$content/* or $content/text()[normalize-space()!='']">
			<RichText>
				<xsl:apply-templates select="$self/@*" />
				<xsl:copy-of select="$content" />
			</RichText>
		</xsl:if>
	</xsl:template>

	<xsl:template match="br">
		<xsl:param name="br2p" tunnel="yes" />
		<xsl:choose>
			<xsl:when test="$br2p='yes'">
				<xsl:copy>
					<xsl:apply-templates select="@*|node()" />
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>
</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dl[.//dt and .//dd]">
		<Glossary>
			<xsl:apply-templates select="node()" />
		</Glossary>
	</xsl:template>
	
	<xsl:template match="dt[following-sibling::dd]">
		<Term>
			<xsl:apply-templates select="node()" />
		</Term>
	</xsl:template>

	<xsl:template match="dd[preceding-sibling::dt]">
		<Definition>
			<xsl:apply-templates select="node()" />
		</Definition>
	</xsl:template>
		
	<xsl:template match="ul | ol">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:variable name="li_types">
		<!-- ul -->
		<type value="square" xyleme="Bullet" />
		<type value="disc" xyleme="Disk" />
		<type value="circle" xyleme="Bullet" />
		<!-- ol -->
		<type value="a" xyleme="LowercaseAlpha" />
		<type value="A" xyleme="UppercaseAlpha" />
		<type value="i" xyleme="LowercaseRoman" />
		<type value="I" xyleme="UppercaseRoman" />
	</xsl:variable>

	<xsl:template match="li">
		<xsl:variable name="ListMarker" select="$li_types/type[@value=current()/parent::*[self::ol|self::ul]/@type]/@xyleme" />
		<ListItem ListMarker="{if ($ListMarker!='') then $ListMarker else if (parent::ol) then 'Numeric' else 'Bullet'}" ListLevel="{count(ancestor::*[self::ol|self::ul])-1}">
			<xsl:if test="parent::ol">
				<xsl:attribute name="StartAtNumber"
					select="if (@value != '') then @value
					else (if (parent::ol/@start!='') then parent::ol/@start else 1) + count(preceding-sibling::li)" />
			</xsl:if>
			<xsl:next-match />
		</ListItem>
	</xsl:template>

	<xsl:template match="span | font | tt">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template match="b | strong | span[@font-weight='bold']">
		<Emph>
			<xsl:apply-templates select="node()" />
		</Emph>
	</xsl:template>

	<xsl:template match="i | em | span[@font-style='italic'] | font[@font-style='italic']">
		<Italic>
			<xsl:apply-templates select="node()" />
		</Italic>
	</xsl:template>

	<xsl:template match="u | span[@text-decoration='underline']">
		<Underline>
			<xsl:apply-templates select="node()" />
		</Underline>
	</xsl:template>

	<xsl:template match="o:p | O:P | st1:* | st2:* | ST1:* | ST2:*">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<!-- nice for bold superscript styles -->
	<xsl:variable name="sups_search" select="string-join($config/sup/@search, '')"/>
	<xsl:variable name="sups_replace" select="string-join($config/sup/@replace, '')"/>
	
	<xsl:template match="sup">
		<xsl:choose>
			<xsl:when test="translate(., concat($sups_search, ' &#9;&#10;&#13;&#160;'), '')=''">
				<xsl:value-of select="translate(., $sups_search, $sups_replace)" />
			</xsl:when>
			<xsl:otherwise>
				<Sup>
					<xsl:apply-templates select="node()" />
				</Sup>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- nice for bold subscript styles -->
	<xsl:variable name="subs_search" select="string-join($config/sub/@search, '')"/>
	<xsl:variable name="subs_replace" select="string-join($config/sub/@replace, '')"/>

	<xsl:template match="sub">
		<xsl:choose>
			<xsl:when test="translate(., concat($subs_search, ' &#9;&#10;&#13;&#160;'), '')=''">
				<xsl:value-of select="translate(., $subs_search, $subs_replace)" />
			</xsl:when>
			<xsl:otherwise>
				<Sub>
					<xsl:apply-templates select="node()" />
				</Sub>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="a | abbr">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template match="hr | script" />

	<xsl:function name="my:pictZ">
		<xsl:param name="value" />
		<xsl:choose>
			<xsl:when test="ends-with($value, 'px')">
				<xsl:value-of select="number(substring($value, 1, string-length($value)-2))" />
			</xsl:when>
			<xsl:when test="matches($value, '[0-9]$', 's')">
				<xsl:value-of select="number($value)" />
			</xsl:when>
			<xsl:when test="not($value)" />
			<xsl:otherwise>
				<xsl:message select="concat('unknown measure: ', $value)" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:function>

	<xsl:template match="strong[count(node())=1 and img]" priority="2">
		<xsl:apply-templates select="node()" />
	</xsl:template>
	
	<xsl:template match="img">
		<Figure>
			<MediaObject>
				<Web origWidth="{my:pictZ(@width)}" origHeight="{my:pictZ(@height)}" uri="{@src}">
					<xsl:apply-templates select="node()" />
				</Web>
			</MediaObject>
		</Figure>
	</xsl:template>

	<xsl:template match="object[lower-case(@classid)='clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95']">
		<Audio>
			<AudioFile>
				<xsl:value-of select="param[lower-case(@name)='filename']/@value" />
			</AudioFile>
		</Audio>
	</xsl:template>
	
	<xsl:template match="object[lower-case(@classid)=('clsid:d27cdb6e-ae6d-11cf-96b8-444553540000', 'clsid:d27cdb6e-ae6d-11cf-96b8-444553550000')]">
		<Flash>
			<xsl:if test="@width&gt;=0">
				<xsl:attribute name="width" select="@width" />
			</xsl:if>
			<xsl:if test="@height&gt;=0">
				<xsl:attribute name="height" select="@height" />
			</xsl:if>
			<LaunchFlashFile>
				<xsl:value-of select="param[lower-case(@name)='movie']/@value" />
			</LaunchFlashFile>
			<xsl:if test="@title">
				<AltText>
					<xsl:value-of select="@title" />
				</AltText>
			</xsl:if>
		</Flash>
	</xsl:template>

	<xsl:template match="embed">
		<Flash>
			<xsl:if test="@width&gt;=0">
				<xsl:attribute name="width" select="@width" />
			</xsl:if>
			<xsl:if test="@height&gt;=0">
				<xsl:attribute name="height" select="@height" />
			</xsl:if>
			<LaunchFlashFile>
				<xsl:value-of select="@src" />
			</LaunchFlashFile>
		</Flash>
	</xsl:template>
	
	<xsl:template match="table">
		<xsl:variable name="default">
			<td colspan="1" />
		</xsl:variable>
		<Table align="left" border="{if (@border='0') then 'false' else 'true'}">
			<xsl:apply-templates select="@width|caption" />
			<TblGroup>
				<xsl:choose>
					<xsl:when test="col | colgroup">
						<xsl:apply-templates select="col | colgroup" />
					</xsl:when>
					<!-- no @width attribute in cells, make cols of equal width -->
					<xsl:when test="tr[not((td|th)[@width!=''])]">
						<xsl:variable name="column-count" select="sum(tr[1]/(td|th)/(if (@colspan!='') then @colspan else $default/td/@colspan))" />
						<xsl:for-each select="for $i in 1 to ($column-count cast as xs:integer) return $i">
							<TblCol width="{format-number(100 div $column-count, '#')}" />
						</xsl:for-each>
					</xsl:when>
					<!-- no @width attribute in cells, make cols of equal width -->
					<xsl:when test="tbody/tr[not((td|th)[@width!=''])]">
						<xsl:variable name="column-count" select="sum(tbody/tr[1]/(td|th)/(if (@colspan!='') then @colspan else $default/td/@colspan))" />
						<xsl:for-each select="for $i in 1 to ($column-count cast as xs:integer) return $i">
							<TblCol width="{format-number(100 div $column-count, '#')}" />
						</xsl:for-each>
					</xsl:when>
					<!-- otherwise it goes to pass2.xslt -->
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="thead">
						<xsl:apply-templates select="thead" />
					</xsl:when>
					<xsl:otherwise>
						<TblHeader />
					</xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="tbody">
						<xsl:apply-templates select="tbody" />
					</xsl:when>
					<xsl:otherwise>
						<TblBody>
							<xsl:apply-templates select="node() except (caption|col|colgroup)" />
						</TblBody>
					</xsl:otherwise>
				</xsl:choose>
				<TblFooter />
			</TblGroup>
		</Table>
	</xsl:template>

	<xsl:template match="table/caption">
		<TblTitle>
			<xsl:apply-templates select="node()" />
		</TblTitle>
	</xsl:template>

	<xsl:template match="colgroup">
		<xsl:apply-templates select="node()" />
	</xsl:template>

	<xsl:template match="col">
		<xsl:param name="index" select="1" />
		<xsl:variable name="width">
			<xsl:choose>
				<xsl:when test="@width">
					<xsl:value-of select="@width"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="index2" select="count(preceding-sibling::col)+$index"/>
					<xsl:value-of select="ancestor::table/(tr/(td|th)|tbody/tr/(td|th))[(
						count(preceding-sibling::*[self::td or self::th][not(@colspan) or @colspan&lt;2]) + sum(preceding-sibling::*[self::td or self::th][@colspan&gt;1]/@colspan)
						 + 1)=$index2]/@width"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<TblCol>
			<xsl:choose>
				<xsl:when test="ends-with($width, '%')">
					<xsl:attribute name="width" select="substring($width, 1, string-length($width)-1)" />
				</xsl:when>
				<xsl:when test="$width!=''">
					<xsl:variable name="table-width" select="if (ancestor::table/@width) then ancestor::table/@width/my:pictZ(.) else sum(../col/@width/my:pictZ(.))" />
					<xsl:if test="$table-width">
						<xsl:attribute name="width" select="format-number(my:pictZ($width) div $table-width * 100, '#')" />
					</xsl:if>
				</xsl:when>
			</xsl:choose>
		</TblCol>
		<xsl:if test="@span&gt;1 and $index&lt;@span">
			<xsl:apply-templates select=".">
				<xsl:with-param name="index" select="$index+1" />
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>

	<xsl:template match="thead">
		<TblHeader>
			<xsl:apply-templates select="node()" />
		</TblHeader>
	</xsl:template>

	<xsl:template match="tbody">
		<TblBody>
			<xsl:apply-templates select="node()" />
		</TblBody>
	</xsl:template>

	<xsl:template match="tr">
		<TableRow>
			<xsl:apply-templates select="node()" />
		</TableRow>
	</xsl:template>

	<xsl:template match="tr[not(node())]" priority="1.1" />

	<xsl:variable name="color_none" select="$config/color[@value='^$'][last()]/@name" />
	
	<xsl:template match="td | th" priority="2">
		<xsl:variable name="align" select="if (@align!='') then lower-case(@align) else if (p/@align!='') then lower-case(p/@align) else 'left'" />
		<Cell align="{if ($align=('justify', 'top')) then 'left' else if ($align='middle') then 'center' else $align}" colspan="{if (@colspan!='') then @colspan else '1'}" rowspan="{if (@rowspan!='') then @rowspan else '1'}"
			index="{count(preceding-sibling::*[self::td or self::th][not(@colspan) or @colspan&lt;2]) + sum(preceding-sibling::*[self::td or self::th][@colspan&gt;1]/@colspan)}"
			isInHeaderColumn="{if (self::th) then 1 else 0}" cell-bg="{$color_none}">
			<xsl:choose>
				<xsl:when test="@width='*'" />
				<xsl:when test="@width!=''">
					<xsl:attribute name="width" select="if (ends-with(@width, '%')) then substring-before(@width, '%') else @width" />
				</xsl:when>
			</xsl:choose>
			<xsl:next-match />
		</Cell>
	</xsl:template>

</xsl:stylesheet>