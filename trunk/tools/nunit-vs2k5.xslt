<?xml version="1.0" encoding="UTF-8" ?>
<!-- VS2k5 IDE output transform for NUnit by Torbjrn Gyllebring (i.am@drunkencoder.com) -->
<!-- Minor ui-clarification: Kristoffer Roupe (kitofr@gmail.com) -->
<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" encoding="UTF-8"/>
<xsl:strip-space elements="*"/>

<xsl:variable name="linefeed" select="'&#xD;&#xA;'" />

<xsl:template match="test-case[@success='False']">
	<xsl:variable name="trace-data">
		<xsl:call-template name="last-line">
			<xsl:with-param name="source">
				<xsl:value-of select="failure/stack-trace"/>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:variable>

	<xsl:value-of select="normalize-space(substring-after(substring-before($trace-data, ':line'), ' in '))"/>
	<xsl:text>(</xsl:text>
		<xsl:value-of select="normalize-space(substring-after($trace-data, ':line '))"/>
	<xsl:text>,1): Test Failure error : --[</xsl:text>
	<xsl:value-of select="normalize-space(substring-after(substring-before($trace-data, ' in '), ' at '))"/>
	<xsl:text>]-- </xsl:text>
	<xsl:value-of select="normalize-space(translate(failure/message,'&#x9;&#xD;&#xA;',' '))" />
	<xsl:text disable-output-escaping="yes">&#xD;&#xA;</xsl:text>
</xsl:template>

<xsl:template name="last-line">
	<xsl:param name="source" />
	<xsl:variable name="tail">
		<xsl:value-of select="substring-after($source, $linefeed)"/>
	</xsl:variable>
	<xsl:choose>
		<xsl:when test="contains($source, $linefeed) and string-length($tail)!=0">
			<xsl:call-template name="last-line">
				<xsl:with-param name="source">
					<xsl:value-of select="$tail" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$source" />
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

</xsl:transform>