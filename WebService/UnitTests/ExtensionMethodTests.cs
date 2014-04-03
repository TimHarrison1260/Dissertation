using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Helpers;
using Newtonsoft.Json;
using UnitTests.Helpers;

namespace UnitTests
{
    [TestClass]
    public class ExtensionMethodTests
    {
        private string _kmlJune = string.Empty;     //  To load from file only once
        private string _areaPlacemarks = string.Empty;          //  A list of valid Area placemarks
        private XNamespace _kmlns = null;

        private DataHelper _testHelper = new DataHelper();      //  Functions to support the unit tests.

        /// <summary>
        /// Ctor:
        /// </summary>
        public ExtensionMethodTests()
        {
            //  Load the external files only once for a test run.
            _kmlJune = _testHelper.ReadJuneFile();
            _areaPlacemarks = _testHelper.ReadAreaPlacemarks();

            _kmlns = _testHelper.GetNameSpace(_kmlJune);        //  Set the namespace used by these files.
        }


        [TestMethod]
        [TestCategory("ExtensionMethods")]
        public void XElement_GetXmlNode_Successful()
        {
            //  Arrange:
            var areaKml = XElement.Parse(_areaPlacemarks);
            var xElementPlacemarks = areaKml.Descendants(_kmlns + "Placemark").ToArray();

            var areaDoc = new System.Xml.XmlDocument();
            areaDoc.LoadXml(_areaPlacemarks);
            var xmlNodePlacemarks = areaDoc.GetElementsByTagName("Placemark");

            //  Act
            var resultNode = xElementPlacemarks[0].GetXmlNode();

            //  Assert
            var resultText = resultNode.OuterXml;
            var xmlNodeText = xmlNodePlacemarks[0].OuterXml;

            Assert.AreEqual(xmlNodeText, resultText, string.Format("Expected the same OuterXml '{0}' to be returned.", xmlNodeText));
        }

        [TestMethod]
        [TestCategory("ExtensionMethods")]
        public void XmlNode_GetXElement_Successful()
        {
            //  Arrange:
            var areaKml = XElement.Parse(_areaPlacemarks);
            var xElementPlacemarks = areaKml.Descendants(_kmlns + "Placemark").ToArray();

            var areaDoc = new System.Xml.XmlDocument();
            areaDoc.LoadXml(_areaPlacemarks);
            var xmlNodePlacemarks = areaDoc.GetElementsByTagName("Placemark");

            //  Act
            var resultXElement = xmlNodePlacemarks[0].GetXElement();

            //  Assert
            var xElementNamespace = xElementPlacemarks[0].GetDefaultNamespace();
            var resultNamespace = resultXElement.GetDefaultNamespace();

            var xElementValue = xElementPlacemarks[0].Value;
            var resultValue = resultXElement.Value;

            var xElementToString = xElementPlacemarks[0].ToString();
            var resultToSTring = resultXElement.ToString();

            Assert.AreEqual(xElementNamespace, resultNamespace, string.Format("Expected the namespace '{0}' to be returned.", xElementNamespace));
            Assert.AreEqual(xElementValue, resultValue, string.Format("Expected the value '{0}' to be returned.", xElementValue));
            Assert.AreEqual(xElementToString, resultToSTring, string.Format("Expected the same contents '{0}' to be returned.", xElementToString));

        }

        [TestMethod]
        [TestCategory("ExtensionMethods")]
        public void XElement_GetXmlNode_GetXElement_Result_SameXelementReturned()
        {
            //  Arrange:
            var areaKml = XElement.Parse(_areaPlacemarks);
            var xElementPlacemarks = areaKml.Descendants(_kmlns + "Placemark").ToArray();

            //  Act
            var resultXmlNode = xElementPlacemarks[0].GetXmlNode();
            var resultXElement = resultXmlNode.GetXElement();

            //  Assert
            var inNamespace = xElementPlacemarks[0].GetDefaultNamespace();
            var resultNamespace = resultXElement.GetDefaultNamespace();

            var inValue = xElementPlacemarks[0].Value;
            var resultValue = resultXElement.Value;

            var inToString = xElementPlacemarks[0].ToString();
            var resultToSTring = resultXElement.ToString();

            Assert.AreEqual(inNamespace, resultNamespace, string.Format("Expected the namespace '{0}' to be returned.", inNamespace));
            Assert.AreEqual(inValue, resultValue, string.Format("Expected the value '{0}' to be returned.", inValue));
            Assert.AreEqual(inToString, resultToSTring, string.Format("Expected the same contents '{0}' to be returned.", inToString));
        }

        [TestMethod]
        [TestCategory("ExtensionMethods")]
        public void XmlNode_GetXElement_GeetXmlNode_Result_SameXmlNode()
        {
            //  Arrange:
            var areaDoc = new System.Xml.XmlDocument();
            areaDoc.LoadXml(_areaPlacemarks);
            var xmlNodePlacemarks = areaDoc.GetElementsByTagName("Placemark");

            //  Act
            var resultXElement = xmlNodePlacemarks[0].GetXElement();
            var resultXmlNode = resultXElement.GetXmlNode();

            //  Assert
            var resultText = resultXmlNode.OuterXml;
            var xmlNodeText = xmlNodePlacemarks[0].OuterXml;

            Assert.AreEqual(xmlNodeText, resultText, string.Format("Expected the same OuterXml '{0}' to be returned.", xmlNodeText));

        }


        [TestMethod]
        [TestCategory("ExtensionMethods")]
        public void XElement_ToJson_ResultSuccessfulJsonObject()
        {
            //  Arrange
            var areaKml = XElement.Parse(_areaPlacemarks);
            var xElementPlacemarks = areaKml.Descendants(_kmlns + "Placemark").ToArray();

            var expectedJsonObject = "{\"Placemark\":{\"name\":\"10 x 50kW Windbank turbines at Tambowie Farm, Milngavie\",\"Snippet\":{\"@maxLines\":\"0\"},\"styleUrl\":\"#Application\",\"Polygon\":{\"outerBoundaryIs\":{\"LinearRing\":{\"coordinates\":\"\\t\\t\\t\\t\\t\\t\\t\\t-4.36424819758965,55.947886106607,0 -4.36571058449389,55.94893346356281,0 -4.36855929496669,55.9476980433338,0 -4.36737375344283,55.94686475898669,0 -4.36424819758965,55.947886106607,0 \\t\\t\\t\\t\\t\\t\\t\"}}}}}";

            //  Act
            var result = xElementPlacemarks[0].ToJson();

            //  Assert
            Assert.AreEqual(expectedJsonObject, result, string.Format("Expected the Json Object '{0}' to be returned.", expectedJsonObject));

        }



    }
}
