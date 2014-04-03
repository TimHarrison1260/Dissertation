using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Helpers;
using Newtonsoft.Json;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Class <c>KmlHelper</c> provides methods to manipulate
    /// the KML file (SNH)
    /// </summary>
    public class SnhKmlHelper : ISnhKmlHelper
    {
        private XNamespace _kmlns;     //  Namespace for the KML file

        /// <summary>
        /// Set the NameSpace from the KML input file.
        /// </summary>
        /// <param name="kmlns">The namespace of the file.</param>
        public void SetNameSpace(XNamespace kmlns)
        {
            if (kmlns != null)
                _kmlns = kmlns;
        }

        /// <summary>
        /// Get the collection of (Placemark) tags for the Names Folder
        /// </summary>
        /// <returns>A collection of 'Placemark' XElements</returns>
        public IEnumerable<XElement> GetNamePlacemarks(XElement xelement)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return new Collection<XElement>();

            var result = GetFolders(xelement).Where(IsNameFolder).Select(GetPlacemarks).FirstOrDefault();
            return result != null ? result : new Collection<XElement>();
        }


        /// <summary>
        /// Get the collection of (Placemark) tags for the STATUS Folder, which 
        /// define the footprints of the wind farm.
        /// </summary>
        /// <returns>A collection of 'Placemark' XElements</returns>
        /// <remarks>
        /// If no XElement is passed, it gets the placemarks for the STATUS folder
        /// otherwise it returns the placemarks within the XElement passed.
        /// </remarks>
        public IEnumerable<XElement> GetAreaPlacemarks(XElement xelement)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return new Collection<XElement>();

            //return (xelement == null) ? GetFolders().Where(IsStatusFolder).Select(GetPlacemarks).FirstOrDefault() : GetPlacemarks(xelement);
            var results = GetFolders(xelement).Where(IsStatusFolder).Select(GetPlacemarks).FirstOrDefault();
            return results;
        }



        /// <summary>
        /// Determines if the (NAMES) folder exists within the KML.
        /// </summary>
        /// <returns>Returns TRUE if found otherwise FALSE</returns>
        public bool NameFolderExists(XElement xelement)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return false;

            //  Get the <Folder> tags and look for the one named (NAME)
            var result = GetFolders(xelement).Any(xElement => IsNameFolder(xElement));
            //  Not found the Names folder tag.
            return result;
        }

        /// <summary>
        /// Determines if the (STATUS) folder exists within the KML.
        /// </summary>
        /// <returns>Returns TRUE if found otherwise FALSE</returns>
        public bool StatusFolderExists(XElement xelement)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return false;

            var result = GetFolders(xelement).Any(IsStatusFolder);
            return result;
        }


        /// <summary>
        /// Get the Name extracted from the KML Placemark tag
        /// </summary>
        /// <param name="placemark">the Placemeark</param>
        /// <returns>The name of the placemark</returns>
        public string GetPlacemarkName(XContainer placemark)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return string.Empty;

            if (placemark == null)
                return string.Empty;    //  reject NULL placemeark

            //  Set name
            var nameXElement = placemark.Element(_kmlns + "name");
            return (nameXElement != null) ? nameXElement.Value : string.Empty;
        }

        /// <summary>
        /// Get the status of the placemark tab
        /// </summary>
        /// <param name="placemark">the Placemark</param>
        /// <returns>String representing the status</returns>
        public string GetPlacemarkStatus(XContainer placemark)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return string.Empty;

            if (placemark == null)
                return string.Empty;    //  reject NULL placemeark

            //  Set Status
            var statusXElement = placemark.Element(_kmlns + "styleUrl");
            return (statusXElement != null) ? statusXElement.Value.Trim('#') : string.Empty;
        }

        /// <summary>
        /// Combines the footprint coordinates for two placemarks
        /// </summary>
        /// <param name="placemark">XElement being combined</param>
        /// <param name="combined">XElement being updated</param>
        /// <returns>Updated XElement</returns>
        /// <remarks>
        /// The placemark is returned if the cumulative xElement is empty.
        /// If the combined one has no coordinates the new coordinates are 
        /// added to the combined.  If the new XElement has no coordinates the 
        /// combined XElement is returned.
        /// </remarks>
        //public void CombineFootprintData(ref XElement combined, XElement placemark)
        public XElement CombineFootprintData(XElement combined, XElement placemark)
        {
            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return null;
                //return;

            //if (combined == null) return placemark;
            if (combined == null)
            {
                var coords = GetPlacemarkCoordinates(placemark);
                UpdatePlacemarkCoordinate(ref placemark, coords);
                return placemark;
                //combined = placemark;
                //return;
            }

            //  Set the area coordinates
            var strPlacemarkCoords = GetPlacemarkCoordinates(placemark);
            var strCombinedCoords = GetPlacemarkCoordinates(combined);

            var combinedCoordinates = !string.IsNullOrEmpty(strCombinedCoords)
                                            ? strCombinedCoords + " " + strPlacemarkCoords
                                            : strPlacemarkCoords;

            //  update the coordinates in the combined.
            UpdatePlacemarkCoordinate(ref combined, combinedCoordinates);

            //return combined;
            return combined;
        }

        /// <summary>
        /// Removes the special characters like tab from the coordinates
        /// </summary>
        /// <param name="placemark">Placemark</param>
        /// <returns>Cleaned Placemark</returns>
        public XElement SanitisePlacemarkCoordinates(XElement placemark)
        {
            var outPlacemark = placemark;
            var coords = GetPlacemarkCoordinates(placemark);
            UpdatePlacemarkCoordinate(ref outPlacemark, coords);
            return outPlacemark;
        }

        /// <summary>
        /// Get the Styles xElements in the document, they represent
        /// the various status' of the wind farms.
        /// </summary>
        /// <returns>A Dictionary containing the styles</returns>
        /// <remarks>
        /// IT returns a dictionary
        /// with the key set the the style Id and the value set to the KML 
        /// snippet. It is for use as a lookup using the name.
        /// </remarks>
        public IDictionary<string, XElement> GetStyles(XElement xelement)
        {
            var results = new Dictionary<string, XElement>();

            //  Exit if the namespace isn't defined.
            if (_kmlns == null)
                return results;

            var styles = xelement.Descendants(_kmlns + "Style");
            foreach (var style in styles)
            {
                var attr = style.Attribute("id");
                if (attr != null)
                    results.Add(attr.Value, style);
            }
            return results;
        }


        /*
         *  Private methods
         * 
         * */

        /// <summary>
        /// Gets the collection of all "Folder" tags in the KML.
        /// </summary>
        /// <param name="xelement">The XElement containing Folder tags</param>
        /// <returns>Collection of Folder XElements</returns>
        private IEnumerable<XElement> GetFolders(XElement xelement)
        {
            var results = xelement.Descendants(_kmlns + "Folder");
            return results;
        }

        /// <summary>
        /// Gets the collection of "PlaceMark" tags in the xElement
        /// </summary>
        /// <param name="xElement">The xElement</param>
        /// <returns>Collection of PlaceMark elements</returns>
        private IEnumerable<XElement> GetPlacemarks(XContainer xElement)
        {
            return xElement.Descendants(_kmlns + "Placemark");
        }

        /// <summary>
        /// Gets the coordinates as a string for the placemark
        /// </summary>
        /// <param name="placemark">The placemark tag</param>
        /// <returns>String of cooredinates</returns>
        /// <remarks>
        /// The output would have to be parsed to obtain the individual coordinates
        /// </remarks>
        private string GetPlacemarkCoordinates(XContainer placemark)
        {
            var xElement = placemark.Descendants(_kmlns + "coordinates").FirstOrDefault();
            return (xElement != null) ? xElement.Value.Trim(' ', '\t') : string.Empty;

        }

        /// <summary>
        /// Updates the coordinates of the placemark with the specified coordinates
        /// </summary>
        /// <param name="placemark">Placemark to be updated (ref)</param>
        /// <param name="coordinates">Coordinates to update with</param>
        private void UpdatePlacemarkCoordinate(ref XElement placemark, string coordinates)
        {
            //  update the coordinates in the combined.
            var poly = placemark.Descendants(_kmlns + "Polygon").First();
            var boundary = poly.Descendants(_kmlns + "outerBoundaryIs").First();
            var ring = boundary.Descendants(_kmlns + "LinearRing").First();
            var coords = ring.Descendants(_kmlns + "coordinates").First();
            coords.SetValue(coordinates);
        }

        /// <summary>
        /// Checks if the xElement is the (NAME) folder
        /// </summary>
        /// <param name="xElement">the xElement being tested</param>
        /// <returns>True if the xElement has children, with the name '(NAME)'</returns>
        private bool IsNameFolder(XContainer xElement)
        {
            var folderNameElement = xElement.Element(_kmlns + "name");
            return (folderNameElement != null && !string.IsNullOrEmpty(folderNameElement.Value) &&
                    folderNameElement.Value.EndsWith("(NAME)"));
        }

        /// <summary>
        /// Checks if the xElement is the (STATUS) folder
        /// </summary>
        /// <param name="xElement">the xElement being tested</param>
        /// <returns>True if the xElement has children, with the name '(STATUS)'</returns>
        private bool IsStatusFolder(XContainer xElement)
        {
            var folderNameElement = xElement.Element(_kmlns + "name");
            return (folderNameElement != null && !string.IsNullOrEmpty(folderNameElement.Value) &&
                    folderNameElement.Value.EndsWith("(STATUS)"));
        }

    

    }
}
