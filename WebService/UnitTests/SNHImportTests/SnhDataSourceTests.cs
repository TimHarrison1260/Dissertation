using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Infrastructure.Data;
using Infrastructure.Interfaces.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.SNHImportTests
{
    [TestClass]
    public class SnhDataSourceTests
    {
        private SnhDataSource _datasource = null;     //  Class being tested.
        private string _kml = string.Empty;         //  Use for testing
        private XNamespace _kmlns = null;
        private Mock<ISnhKmlHelper> _mockSnhHelper = null;

        private string _kmlJune = string.Empty;     //  To load from file only once
        private string _kmlOctober = string.Empty;  //  To load from file only once
        private string _invalidKml = string.Empty;  //  An invalid kml file, has kml tag but no kmlns attribute
        private string _namePlacemarks = string.Empty;  //  A list of valid Name placemarks
        private string _areaPlacemarks = string.Empty;  //  A list of valid Area placemarks
        private string _styles = string.Empty;          // A list of styles from the october file.

        private DataHelper _testHelper = new DataHelper();      //  Functions to support the unit tests.

        public SnhDataSourceTests()
        {
            _kmlJune = _testHelper.ReadJuneFile();
            _kmlOctober = _testHelper.ReadOctoberFile();
            _invalidKml = _testHelper.ReadInvalidTestFile();
            _namePlacemarks = _testHelper.ReadNamePlacemarks();
            _areaPlacemarks = _testHelper.ReadAreaPlacemarks();
            _styles = _testHelper.ReadStyles();

            _kmlns = _testHelper.GetNameSpace(_kmlJune);        //  Set the namespace used by these files.
        }

        [TestInitialize]
        public void Initialise()
        {
            _datasource = null;
            _kml = string.Empty;
            _mockSnhHelper = new Mock<ISnhKmlHelper>();
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void LoadValidKml_Successfully_TrueTeturned()
        {
            //  Arrange
            //  Make sure we're getting a new instance initialised properly
            //  so that the ones loaded from files do not get changed.
            var bldr = new StringBuilder(_kmlJune);
            _kml = bldr.ToString();

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.SetNameSpace(It.IsAny<string>()));

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);

            //  Act
            var success = _datasource.LoadSource(_kml);

            //  Assert
            Assert.IsTrue(success, "Expected the load to succeed and return TRUE");
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void LoadInvalidKml_FalseReturned()
        {
            //  Arrange
            var bldr = new StringBuilder(_invalidKml);
            _kml = bldr.ToString();

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.SetNameSpace(It.IsAny<string>()));

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);

            //  Act
            var success = _datasource.LoadSource(_kml);

            //  Assert
            Assert.IsFalse(success, "Expected the load to fail and return FALSE");
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void GetAllNames_NamesSectionExists_Successful_7_NamesReturned()
        {
            //  Arrange
            var nameKml = XElement.Parse(_namePlacemarks);
            var namePlacemarks = nameKml.Descendants(_kmlns + "Placemark");

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.NameFolderExists(It.IsAny<XElement>())).Returns(true);
            _mockSnhHelper.Setup(n => n.GetNamePlacemarks(It.IsAny<XElement>())).Returns(namePlacemarks);

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);
            _datasource.SetSource(nameKml, _kmlns);     // Set the Kmlns and data in the data source model.

            //  Act
            var results = _datasource.GetAllNames().ToArray();

            //  Assert
            Assert.AreEqual(7,results.Count(),"Expected 7 names to be returned");
            Assert.AreEqual("10 x 50kW Windbank turbines at Tambowie Farm, Milngavie",results[0], "expected 'Tambowie Farm' as 1st wind farm");
            Assert.AreEqual("Achany",results[1], "expected 'Achany' as 2nd wind farm");
            Assert.AreEqual("Achlachan",results[2], "expected 'Achlachan' as 3rd wind farm");
            Assert.AreEqual("Achnaba",results[3], "expected 'Achnaba' as 4th wind farm");
            Assert.AreEqual("A'Chruach", results[4], "expected 'A Cruach' as 5th wind farm");
            Assert.AreEqual("Allt Dearg 2", results[5], "expected 'Allt Dearg 2' as 6th wind farm");
            Assert.AreEqual("Whitelee Wind Farm", results[6], "expected 'Whilelee Wind Farm' as 7th wind farm");
        }
        
        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void GetAllNames_NoNameSection_Successful_8_NamesReturned()
        {
            //  Arrange
            var areaKml = XElement.Parse(_areaPlacemarks);
            var areaPlacemarks = areaKml.Descendants(_kmlns + "Placemark");

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.NameFolderExists(It.IsAny<XElement>())).Returns(false);
            _mockSnhHelper.Setup(n => n.GetAreaPlacemarks(It.IsAny<XElement>())).Returns(areaPlacemarks);

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);
            _datasource.SetSource(areaKml, _kmlns);     // Set the Kmlns and data in the data source model.

            //  Act
            var results = _datasource.GetAllNames().ToArray();

            //  Assert
            Assert.AreEqual(8,results.Count(),"Expected 8 names to be returned");
            Assert.AreEqual("10 x 50kW Windbank turbines at Tambowie Farm, Milngavie", results[0], "expected 'Tambowie Farm' as 1st wind farm");
            Assert.AreEqual("Achany", results[1], "expected 'Achany' as 2nd wind farm");
            Assert.AreEqual("Achlachan", results[2], "expected 'Achlachan' as 3rd wind farm");
            Assert.AreEqual("A'Chruach", results[3], "expected 'A Cruach' as 4th wind farm");
            Assert.AreEqual("A'Chruach Extension", results[4], "expected 'A Cruach Extension' as 5th wind farm");
            Assert.AreEqual("Afton", results[5], "expected 'Afton' as 6th wind farm");
            Assert.AreEqual("Aikengall", results[6], "expected 'Aikengall' as 7th wind farm");
            Assert.AreEqual("Allt Dearg 2", results[7], "expected 'Allt Dearg 2' as 8th wind farm");
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void GetAll_Successful_8_AggregatesReturned()
        {
            //  Arrange
            var styleKml = XElement.Parse(_styles);
            var styles = styleKml.Descendants(_kmlns + "Style");
            var styleDictionary = styles.ToDictionary(style => style.Attribute("id").Value);

            var areaKml = XElement.Parse(_areaPlacemarks);
            var areaPlacemarks = areaKml.Descendants(_kmlns + "Placemark");
            var refAreaPlacemarks = areaPlacemarks.ToArray();
            //XElement nullAreaPlacemark = null;

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.NameFolderExists(It.IsAny<XElement>()))
                .Returns(false);
            _mockSnhHelper.Setup(s => s.GetStyles(It.IsAny<XElement>()))
                .Returns(styleDictionary);
            _mockSnhHelper.Setup(p => p.GetAreaPlacemarks(It.IsAny<XElement>()))
                .Returns(areaPlacemarks);
            _mockSnhHelper.Setup(n => n.GetPlacemarkName(It.IsAny<XElement>()))
                .Returns((XElement x) => x.Element(_kmlns + "name").Value);
            for (int i = 0; i < refAreaPlacemarks.Count(); i++)
            {
                _mockSnhHelper.Setup(f => f.CombineFootprintData(It.IsAny<XElement>(), refAreaPlacemarks[i]))
                    .Returns(refAreaPlacemarks[i]);        // Should return the value passed in.
            }
            _mockSnhHelper.Setup(s => s.SanitisePlacemarkCoordinates(It.IsAny<XElement>()))
                .Returns((XElement x) => x);        //  Just return the XElement passed in.
            _mockSnhHelper.Setup(s => s.GetPlacemarkStatus(It.IsAny<XElement>()))
                .Returns((XElement x) => x.Element(_kmlns + "styleUrl").Value.Trim('#'));
                
            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);
            _datasource.SetSource(areaKml, _kmlns);     // Set the Kmlns and data in the data source model.

            //  Act
            var results = _datasource.GetAll().ToArray();

            //  Assert
            Assert.AreEqual(8, results.Count(), "Expected 8 results to be returned");
            Assert.AreEqual("A'Chruach", results[1].Identifier, "Expected the 2nd aggregate to be 'A'Cruach'");
            Assert.AreEqual("A'Chruach Extension", results[2].Identifier, "Expected the 3rd aggregate to be 'A'Cruach Extension'");
            Assert.AreEqual("Aikengall", results[6].Identifier, "Expected the 7th aggregate to be 'Aikengall'");
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void GetAll_NoStyles_Successful_8_AggregatesReturned()
        {
            //  Arrange
            var styleKml = XElement.Parse(_styles);
            var styles = styleKml.Descendants(_kmlns + "Style");
            var styleDictionary = styles.ToDictionary(style => style.Attribute("id").Value);

            var areaKml = XElement.Parse(_areaPlacemarks);
            var areaPlacemarks = areaKml.Descendants(_kmlns + "Placemark");
            var refAreaPlacemarks = areaPlacemarks.ToArray();
            //XElement nullAreaPlacemark = null;

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.NameFolderExists(It.IsAny<XElement>()))
                .Returns(false);
            _mockSnhHelper.Setup(s => s.GetStyles(It.IsAny<XElement>()))
                .Returns(new Dictionary<string, XElement>());       // empty styles collection
            _mockSnhHelper.Setup(p => p.GetAreaPlacemarks(It.IsAny<XElement>()))
                .Returns(areaPlacemarks);
            _mockSnhHelper.Setup(n => n.GetPlacemarkName(It.IsAny<XElement>()))
                .Returns((XElement x) => x.Element(_kmlns + "name").Value);
            for (int i = 0; i < refAreaPlacemarks.Count(); i++)
            {
                _mockSnhHelper.Setup(f => f.CombineFootprintData(It.IsAny<XElement>(), refAreaPlacemarks[i]))
                    .Returns(refAreaPlacemarks[i]);        // Should return the value passed in.
            }
            _mockSnhHelper.Setup(s => s.SanitisePlacemarkCoordinates(It.IsAny<XElement>()))
                .Returns((XElement x) => x);        //  Just return the XElement passed in.
            var returnsResult = _mockSnhHelper.Setup(s => s.GetPlacemarkStatus(It.IsAny<XElement>()))
                .Returns(string.Empty);

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);
            _datasource.SetSource(areaKml, _kmlns);     // Set the Kmlns and data in the data source model.

            //  Act
            var results = _datasource.GetAll().ToArray();

            //  Assert
            Assert.AreEqual(8, results.Count(), "Expected 8 results to be returned");
            Assert.AreEqual("A'Chruach", results[1].Identifier, "Expected the 2nd aggregate to be 'A'Cruach'");
            Assert.AreEqual("A'Chruach Extension", results[2].Identifier, "Expected the 3rd aggregate to be 'A'Cruach Extension'");
            Assert.AreEqual("Aikengall", results[6].Identifier, "Expected the 7th aggregate to be 'Aikengall'");
        }

        [TestMethod]
        [TestCategory("SnhDataSource")]
        public void GetAll_NoAreaPlacemarks_Unsuccessful_0_AggregatesReturned()
        {
            //  Arrange
            var styleKml = XElement.Parse(_styles);
            var styles = styleKml.Descendants(_kmlns + "Style");
            var styleDictionary = styles.ToDictionary(style => style.Attribute("id").Value);

            var areaKml = XElement.Parse(_areaPlacemarks);
            var areaPlacemarks = areaKml.Descendants(_kmlns + "Placemark");
            //var refAreaPlacemarks = areaPlacemarks.ToArray();
            //XElement nullAreaPlacemark = null;

            //  Set up the method responses from the Mock helper class
            _mockSnhHelper.Setup(h => h.NameFolderExists(It.IsAny<XElement>()))
                .Returns(false);
            _mockSnhHelper.Setup(s => s.GetStyles(It.IsAny<XElement>()))
                .Returns(styleDictionary);
            _mockSnhHelper.Setup(p => p.GetAreaPlacemarks(It.IsAny<XElement>()))
                .Returns(new Collection<XElement>());               // No area placemarks found in file (unlike situation)
            _mockSnhHelper.Setup(n => n.GetPlacemarkName(It.IsAny<XElement>()))
                .Returns(string.Empty);
            //for (int i = 0; i < refAreaPlacemarks.Count(); i++)
            //{
            //    _mockSnhHelper.Setup(f => f.CombineFootprintData(It.IsAny<XElement>(), refAreaPlacemarks[i]))
            //        .Returns(refAreaPlacemarks[i]);        // Should return the value passed in.
            //}
            _mockSnhHelper.Setup(s => s.GetPlacemarkStatus(It.IsAny<XElement>()))
                .Returns(string.Empty);

            //  Instantiate the SnhDataSource class
            _datasource = new SnhDataSource(_mockSnhHelper.Object);
            _datasource.SetSource(areaKml, _kmlns);     // Set the Kmlns and data in the data source model.

            //  Act
            var results = _datasource.GetAll().ToArray();

            //  Assert
            Assert.AreEqual(0, results.Count(), "Expected zero results to be returned");
            //Assert.AreEqual("A'Chruach", results[1].Identifier, "Expected the 2nd aggregate to be 'A'Cruach'");
            //Assert.AreEqual("A'Chruach Extension", results[2].Identifier, "Expected the 3rd aggregate to be 'A'Cruach Extension'");
            //Assert.AreEqual("Aikengall", results[6].Identifier, "Expected the 7th aggregate to be 'Aikengall'");
        }



        //[TestMethod]
        //[TestCategory("SnhDataSource")]
        //public void GetAll_NoNameSection_Successful_7_AggregatesReturned()
        //{
        //    //  Arrange
        //    var bldr = new StringBuilder(_kmlOctober);
        //    _kml = bldr.ToString();
        //    _datasource.LoadSource(_kml);
           
        //    //  Act
        //    var results = _datasource.GetAll().ToArray();

        //    //  Analyse
        //    //  Tambowie should have 10 coordinates for the footprint: combined
        //    var tambowie = 
        //        results.FirstOrDefault(n => n.Identifier.Contains("Tambowie Farm"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Afton should have 32 coordinates for the footprint: combined
        //    var afton =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Afton"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data)
        //               .FirstOrDefault();

        //    //  A'Chruach Extension should have 10 coordinates for the footpring: single
        //    var aChruachExtension =
        //        results.FirstOrDefault(n => n.Identifier.Contains("A'Chruach Extension"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data)
        //               .FirstOrDefault();

        //    //  Achlachan should have 30 coordinates for the footprint: single
        //    var achlachan =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Achlachan"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data)
        //               .FirstOrDefault();


        //    //  Assert
        //    Assert.AreEqual(8,results.Count(),"Expected 8 names to be returned");
        //    Assert.AreEqual(10, _testHelper.CountCoordinates(tambowie), "Expected 10 coordinates for Tambowie footprint");
        //    Assert.AreEqual(32, _testHelper.CountCoordinates(afton), "Expected 32 coordinates for Afton footprint");
        //    Assert.AreEqual(10, _testHelper.CountCoordinates(aChruachExtension), "Expected 10 coordinates for A'Chruach Extension footprint");
        //    Assert.AreEqual(30, _testHelper.CountCoordinates(achlachan), "Expected 30 coordinates for Achlachan footprint");
        //}

        //[TestMethod]
        //[TestCategory("SnhDataSource")]
        //public void GetAllAggregateDataFromJunefile()
        //{
        //    //  Arrange
        //    var bldr = new StringBuilder(_kmlJune);
        //    _kml = bldr.ToString();
        //    _datasource.LoadSource(_kml);

        //    //  Act
        //    var results = _datasource.GetAll().ToArray();

        //    //  Analyse
        //    //  Tambowie should have 5 coordinates for the footprint: combined
        //    var tambowie =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Tambowie Farm"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  A'Chruach should have 145 coordinates for the footprint: combined
        //    var aChruach =
        //        results.FirstOrDefault(n => n.Identifier.Contains("A'Chruach"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Achany should have 84 coordinates for the footprint: combined
        //    var achany =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Achany"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Allt Daerg 2 should have 361 coordinates for the footprint: combined
        //    var alltDearg2 =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Allt Dearg 2"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Achlachan should have 30 coordinates for the footprint: combined
        //    var achlachan =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Achlachan"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Achnaba should have 457 coordinates for the footprint: combined
        //    var achnaba =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Achnaba"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();
        //    //  Whitelee should have 1396 coordinates for the footprint: combined
        //    var whitelee =
        //        results.FirstOrDefault(n => n.Identifier.Contains("Whitelee Wind Farm"))
        //               .Data.Where(d => d.DataType == DataTypeEnum.FootPrint)
        //               .Select(d => d.Data).FirstOrDefault();

        //    //  Assert
        //    Assert.AreEqual(7, results.Count(), "Expected 7 names to be returned");
        //    Assert.AreEqual(5, _testHelper.CountCoordinates(tambowie), "Expected 5 coordinates for Tambowie footprint");
        //    Assert.AreEqual(145, _testHelper.CountCoordinates(aChruach), "Expected 146 coordinates for A'Chruach footprint");
        //    Assert.AreEqual(84, _testHelper.CountCoordinates(achany), "Expected 84 coordinates for Achany footprint");
        //    Assert.AreEqual(361, _testHelper.CountCoordinates(alltDearg2), "Expected 361 coordinates for Allt Daerg 2 footprint");
        //    Assert.AreEqual(30, _testHelper.CountCoordinates(achlachan), "Expected 30 coordinates for Achlachan footprint");
        //    Assert.AreEqual(457, _testHelper.CountCoordinates(achnaba), "Expected 457 coordinates for Achnaba footprint");
        //    Assert.AreEqual(1396, _testHelper.CountCoordinates(whitelee), "Expected 1396 coordinates for Whitelee footprint");
        //}

        [TestCleanup]
        public void CleanUp()
        {
            _datasource = null;
            _kml = string.Empty;
            _mockSnhHelper = null;
        }
    }
}
