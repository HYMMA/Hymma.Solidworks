<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:wix="http://wixtoolset.org/schemas/v4/wxs"
	xmlns="http://schemas.microsoft.com/wix/2006/wi"
	version="1.0"
	exclude-result-prefixes="xsl wix">

	<xsl:output method="xml"
				indent="yes"
				omit-xml-declaration="yes" />

	<xsl:strip-space elements="*" />


	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

<xsl:template match="wix:RegistryValue[@Root='HKCU']" />
</xsl:stylesheet>