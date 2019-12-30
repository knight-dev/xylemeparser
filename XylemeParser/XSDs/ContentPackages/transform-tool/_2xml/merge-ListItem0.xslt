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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">

	<xsl:import href="identity.xslt" />

	<!-- fix ivx roman list markers that are actually alpha -->
	
	<xsl:template match="ListItem[@ListMarker='LowercaseRoman' and @StartAtNumber=('i','v','x') 
		and preceding-sibling::node()[1]/self::ListItem[@ListMarker='LowercaseAlpha']/@StartAtNumber=codepoints-to-string((string-to-codepoints(@StartAtNumber)[1]-1) cast as xs:integer)]/@ListMarker
			| ListItem[@ListMarker='(LowercaseRoman)' and @StartAtNumber=('i','v','x') 
		and preceding-sibling::node()[1]/self::ListItem[@ListMarker='(LowercaseAlpha)']/@StartAtNumber=codepoints-to-string((string-to-codepoints(@StartAtNumber)[1]-1) cast as xs:integer)]/@ListMarker
			| ListItem[@ListMarker='UppercaseRoman' and @StartAtNumber=('I','V','X') 
		and preceding-sibling::node()[1]/self::ListItem[@ListMarker='UppercaseAlpha']/@StartAtNumber=codepoints-to-string((string-to-codepoints(@StartAtNumber)[1]-1) cast as xs:integer)]/@ListMarker
			| ListItem[@ListMarker='(UppercaseRoman)' and @StartAtNumber=('I','V','X') 
		and preceding-sibling::node()[1]/self::ListItem[@ListMarker='(UppercaseAlpha)']/@StartAtNumber=codepoints-to-string((string-to-codepoints(@StartAtNumber)[1]-1) cast as xs:integer)]/@ListMarker">
		<xsl:attribute name="{name()}" select="../preceding-sibling::node()[1]/@ListMarker" />
	</xsl:template>
	
</xsl:stylesheet>