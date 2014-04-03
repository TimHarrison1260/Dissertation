using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HtmlAgilityPack;
using Infrastructure.Data;
using Infrastructure.Helpers;

namespace UnitTests.Helpers
{
    /// <summary>
    /// Class <c>DataHelpr</c> provides methods to assist in the
    /// unit tests of the <see cref="SnhDataSource"/> and 
    /// <see cref="SnhKmlHelper"/> classes.
    /// </summary>
    public class DataHelper
    {
        //  SNH Test Data Files
        private const string JuneKmlTestFile = @"..\..\..\UnitTests\Data\June2013.kml";
        private const string OctoberKmlTestFile = @"..\..\..\UnitTests\Data\October2013.kml";
        private const string InvalidKmlTestFile = @"..\..\..\UnitTests\Data\Invalid.kml";
        private const string ValidPlacemark = @"..\..\..\UnitTests\Data\ValidPlacemark.kml";
        private const string PlacemarkWithoutName = @"..\..\..\UnitTests\Data\PlacemarkWithoutName.kml";
        private const string PlacemarkWithoutStatus = @"..\..\..\UnitTests\Data\PlacemarkWithoutStatus.kml";
        private const string NamePlacemarks = @"..\..\..\UnitTests\Data\NamePlacemarks.kml";
        private const string AreaPlacemarks = @"..\..\..\UnitTests\Data\AreaPlacemarks.kml";
        private const string StyleTags = @"..\..\..\UnitTests\Data\OctoberStyles.kml";
        private const string NoStyleTags = @"..\..\..\UnitTests\Data\JuneNoStyles.kml";

        //  RenUK Test HTML files
        private const string HtmlFileName = @"..\..\..\UnitTests\Data\RenUkTest.html";
        private const string Html1FullPage = @"..\..\..\UnitTests\Data\RenUkFullPage1Test.html";
        private const string Html1PartialPage = @"..\..\..\UnitTests\Data\RenUkPage1Test.html";
        private const string Html404FileName = @"..\..\..\UnitTests\Data\RenUk404NotFound.html";
        private const string OneResultfileName = @"..\..\..\UnitTests\Data\RenUk1Result.html";
        private const string NoresultfileName = @"..\..\..\UnitTests\Data\RenUkNoresults.html";
        private const string LiFileName = @"..\..\..\UnitTests\Data\RenUkLiTagsAggregates.html";


        #region SNH Test Helper methods

        /// <summary>
        /// Load the June Kml file into memory
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadJuneFile()
        {
            return ReadFile(JuneKmlTestFile);
        }

        /// <summary>
        /// Load the October kml file into memory
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadOctoberFile()
        {
            return ReadFile(OctoberKmlTestFile);
        }

        /// <summary>
        /// Loads an invalid Kml file
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadInvalidTestFile()
        {
            return ReadFile(InvalidKmlTestFile);
        }

        /// <summary>
        /// Load a valid placemark tag
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadValidPlacemark()
        {
            return ReadFile(ValidPlacemark);
        }

        /// <summary>
        /// Load a placemark tag that doesn't have a Name tag
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadPlacemarkWithoutName()
        {
            return ReadFile(PlacemarkWithoutName);
        }

        /// <summary>
        /// Load a placemark tag that doesn't have a Status
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadPlacemarkWithoutStatus()
        {
            return ReadFile(PlacemarkWithoutStatus);
        }

        /// <summary>
        /// Loads a collection of placemarks from the NAME section
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadNamePlacemarks()
        {
            return ReadFile(NamePlacemarks);
        }

        /// <summary>
        /// Load a collection of placemarks from the STATUS section
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadAreaPlacemarks()
        {
            return ReadFile(AreaPlacemarks);
        }

        /// <summary>
        /// Load a collection of Style tags
        /// </summary>
        /// <returns>File Contents as string</returns>
        public string ReadStyles()
        {
            return ReadFile(StyleTags);
        }

        /// <summary>
        /// Load a kml file that is missing the Style tags
        /// </summary>
        /// <returns></returns>
        public string ReadNoStyles()
        {
            return ReadFile(NoStyleTags);
        }

        /// <summary>
        /// Counts the number of coordinates for a placemarks footpring
        /// </summary>
        /// <param name="kml">Placemark</param>
        /// <returns>Number of coordinates</returns>
        public int CountCoordinates(string kml)
        {
            XNamespace kmlns = GetNameSpace(kml);
            var xelement = XElement.Parse(kml);

            return CountCoordinates(xelement, kmlns);
        }

        /// <summary>
        /// Counts the number of coordinates for a placemarks footpring
        /// </summary>
        /// <param name="kml">the Placemark</param>
        /// <param name="kmlns">the Kml namespace</param>
        /// <returns>the number of coordinates</returns>
        public int CountCoordinates(XElement kml, XNamespace kmlns)
        {
            var coordElement = kml.Descendants(kmlns + "coordinates").FirstOrDefault();
            var coordinates = coordElement.Value.Trim('\t', ' ');  //.TrimEnd(' ');
            var coordArray = coordinates.Split(' ');
            var coordCount = coordArray.Count();

            return coordCount;
        }

        /// <summary>
        /// Gets the KML namespace from a kml file
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public XNamespace GetNameSpace(string xml)
        {
            var xelement = XElement.Parse(xml);
            XNamespace ns = null;

            var attrs = xelement.Attributes();

            foreach (XAttribute a in attrs)
            {
                if (a.Name.LocalName.Equals("xmlns"))
                    ns = a.Value;
            }
            return ns;
        }


        #endregion


        #region RenUk Test Helper methods

        /// <summary>
        /// Read the main Test HTML file
        /// </summary>
        /// <returns>Html Document</returns>
        public HtmlDocument ReadHtmlGood()
        {
            return ReadHtmlFile(HtmlFileName);
        }

        /// <summary>
        /// Read the main Test HTML page with 1 full page containing 50 wind farms
        /// </summary>
        /// <returns>Html Document</returns>
        public HtmlDocument ReadHtml1FullPage()
        {
            return ReadHtmlFile(Html1FullPage);
        }

        /// <summary>
        /// Read the main test HTML page, with 1 partialy completed page, 3 aggregates
        /// </summary>
        /// <returns></returns>
        public HtmlDocument ReadHtmlPartialPage()
        {
            return ReadHtmlFile(Html1PartialPage);
        }

        /// <summary>
        /// Read the Html from 404 Not found response
        /// </summary>
        /// <returns>Html Document</returns>
        public HtmlDocument ReadHtml404()
        {
            return ReadHtmlFile(Html404FileName);
        }

        /// <summary>
        /// Read the Html page that contains 1 single result (GET)
        /// </summary>
        /// <returns>Html Document</returns>
        public HtmlDocument ReadHtmlWithOneResult()
        {
            return ReadHtmlFile(OneResultfileName);
        }

        /// <summary>
        /// Read Html page containing no results (Not Found)
        /// </summary>
        /// <returns></returns>
        public HtmlDocument ReadHtmlWithNoResults()
        {
            return ReadHtmlFile(NoresultfileName);
        }

        /// <summary>
        /// Read the Html file, XElement containing the Li tags; Columns
        /// </summary>
        /// <returns></returns>
        public HtmlDocument ReadHtmlLiTags()
        {
            return ReadHtmlFile(LiFileName);
        }

        #endregion


        #region private methods

        /// <summary>
        /// Deserialise the kml from the specified input file, pass in the constructor, and 
        /// populate the corresponding Kml model class.
        /// </summary>
        /// <returns>Instance of the kml model.</returns>
        private string ReadFile(string fileName)
        {
            var kmlModel = new StringBuilder();
            //  Read input file, deserialise to kmlModel.
            using (var reader = new StreamReader(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    kmlModel.Append(line);
                }
            }
            return kmlModel.ToString();
        }

        /// <summary>
        /// Read Html from a text file and construct an HtmlDocument
        /// </summary>
        /// <param name="fileName">filename</param>
        /// <returns>Html Document</returns>
        private HtmlDocument ReadHtmlFile(string fileName)
        {
            var doc = new HtmlDocument();
            doc.Load(fileName);
            return doc;
        }

        #endregion

    }
}
