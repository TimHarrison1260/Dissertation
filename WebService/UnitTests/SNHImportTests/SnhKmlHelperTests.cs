using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Helpers;

namespace UnitTests.SNHImportTests
{
    [TestClass]
    public class SnhKmlHelperTests
    {
        //private IDataSource _datasource = null;     //  Class being tested.
        private string _kml = string.Empty;         //  Use for testing

        private string _kmlJune = string.Empty;     //  To load from file only once
        private string _kmlOctober = string.Empty;  //  To load from file only once
        private string _invalidKml = string.Empty;  //  An invalid kml file, has kml tag but no kmlns attribute
        private string _placemarkValid = string.Empty;
        private string _placemarkWithoutName = string.Empty;
        private string _placemarkwithoutStatus = string.Empty;
        private string _kmlJuneNoStyles = string.Empty;

        private XNamespace _kmlns = null;

        private DataHelper _testHelper = new DataHelper();


        public SnhKmlHelperTests()
        {
            //  Load the test data files
            _kmlJune = _testHelper.ReadJuneFile();
            _kmlOctober = _testHelper.ReadOctoberFile();
            _invalidKml = _testHelper.ReadInvalidTestFile();
            _placemarkValid = _testHelper.ReadValidPlacemark();
            _placemarkWithoutName = _testHelper.ReadPlacemarkWithoutName();
            _placemarkwithoutStatus = _testHelper.ReadPlacemarkWithoutStatus();
            _kmlJuneNoStyles = _testHelper.ReadNoStyles();
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetNamePlacemarksFromValidKmlFile_7Returned()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlJune, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlJune);
                
            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var names = helper.GetNamePlacemarks(kml);

            //  Assert
            Assert.AreEqual(7, names.Count(), "Expected 7 name placemarks to be returned.");
        }


        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetNamePlacemarks_NoPlacemarks_EmptyReturned()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlOctober, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlOctober);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var names = helper.GetNamePlacemarks(kml);

            //  Assert
            Assert.AreEqual(0, names.Count(), "Expected empty collection of name placemarks to be returned.");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetAreaPlacemarks_FromValidSource_10Returned()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlOctober, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlOctober);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var areas = helper.GetAreaPlacemarks(kml);

            //  Assert
            Assert.AreEqual(10, areas.Count(), "Expected 8 Area placemarks to be returned.");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void NameFolderExists_Yes()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlJune, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlJune);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var result = helper.NameFolderExists(kml);

            //  Assert
            Assert.IsTrue(result, "Expected TRUE to be returned, NAMES folder exists in June file.");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void NameFolderExists_No()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlOctober, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlOctober);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var result = helper.NameFolderExists(kml);

            //  Assert
            Assert.IsFalse(result, "Expected FALSE to be returned, no NAMES folder in October file.");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetStyles_Yes_9Returned()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlJune, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlJune);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var styles = helper.GetStyles(kml);

            //  Assert
            Assert.AreEqual(9 ,styles.Count, "Expected 9 Style elements to be returned");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetStyles_No_0Returned()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_kmlJuneNoStyles, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_kmlJuneNoStyles);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Act
            var styles = helper.GetStyles(kml);

            //  Assert
            Assert.AreEqual(0, styles.Count, "Expected an empty collection of Style elements to be returned as none in the file");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetPlacemarkName_Exists_ReturnsName()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkValid, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkValid);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);
            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark = placemarks[0];

            //  Act
            var name = helper.GetPlacemarkName(placemark);

            //  Assert
            Assert.AreEqual("Afton", name, "Expected name of 'Afton' to be returned");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetPlacemarkName_None_ReturneEmptyString()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkWithoutName, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkWithoutName);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);
            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark = placemarks[0];

            //  Act
            var name = helper.GetPlacemarkName(placemark);

            //  Assert
            Assert.AreEqual(string.Empty, name, "Expected no name to be returned, placemark has not Name tag.");
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetPlacemarkStatus_Exists_ReturnsStatus()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkValid, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkValid);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);
            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark = placemarks[0];

            //  Act
            var status = helper.GetPlacemarkStatus(placemark);

            //  Assert
            Assert.AreEqual("Application", status, "Expected status of 'Application' to be returned");            
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void GetPlacemarkStatus_None_ReturnsEmptyString()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkwithoutStatus, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkwithoutStatus);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);
            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark = placemarks[0];

            //  Act
            var status = helper.GetPlacemarkStatus(placemark);

            //  Assert
            Assert.AreEqual(String.Empty, status, "Expected no status to be returned, placemark has not Style tag");
        }


        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void CombineFootprintData_OK()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkValid);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkValid);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark1 = placemarks[0];
            var placemark2 = placemarks[0];

            var totalCoords = _testHelper.CountCoordinates(placemark1, _kmlns) + _testHelper.CountCoordinates(placemark2, _kmlns);

            //  Act
            //helper.CombineFootprintData(ref placemark1, placemark2);
            var newPlacemark = helper.CombineFootprintData(placemark1, placemark2);

            //  Assert
            //Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(placemark1, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
            Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(newPlacemark, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void CombineFoorprintData_NullInput_OK()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkValid);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkValid);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark1 = placemarks[0];
//            var placemark2 = placemarks[0];

            var totalCoords = _testHelper.CountCoordinates(placemark1, _kmlns);

            //  Act
            //helper.CombineFootprintData(ref placemark1, placemark2);
            var newPlacemark = helper.CombineFootprintData(null, placemark1);

            //  Assert
            //Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(placemark1, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
            Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(newPlacemark, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
        }

        [TestMethod]
        [TestCategory("SnhKmlHelper")]
        public void SamitisePlacemark_OK()
        {
            //  Arrange
            //  Get data from file
            var kml = XElement.Parse(_placemarkValid);
            //  Get namespace from input file.
            _kmlns = _testHelper.GetNameSpace(_placemarkValid);

            //  Instantiate helper class
            var helper = new Infrastructure.Helpers.SnhKmlHelper();
            helper.SetNameSpace(_kmlns);

            //  Extract the placemark from the kml file.
            var placemarks = kml.Descendants(_kmlns + "Placemark").ToArray();
            var placemark1 = placemarks[0];
            //            var placemark2 = placemarks[0];

            var totalCoords = _testHelper.CountCoordinates(placemark1, _kmlns);
            var coordElement = placemark1.Descendants(_kmlns + "coordinates").FirstOrDefault().Value;
            var expectedCoordLength = coordElement.Length - (14 + 12);

            //  Act
            //helper.CombineFootprintData(ref placemark1, placemark2);
            var newPlacemark = helper.SanitisePlacemarkCoordinates(placemark1);
            var newCoordElement = placemark1.Descendants(_kmlns + "coordinates").FirstOrDefault().Value;

            //  Assert
            //Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(placemark1, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
            Assert.AreEqual(totalCoords, _testHelper.CountCoordinates(newPlacemark, _kmlns), string.Format("Expected a total of '{0}' coordinates in combined placemark.", totalCoords));
            Assert.AreEqual(expectedCoordLength, newCoordElement.Length, "Expected the string to be shorter by 'NN' characters.");
        }
    }
}
