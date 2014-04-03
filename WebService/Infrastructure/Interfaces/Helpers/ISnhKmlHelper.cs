using System.Collections.Generic;
using System.Xml.Linq;

namespace Infrastructure.Interfaces.Helpers
{
    /// <summary>
    /// Interface <c>ISnhKmoHelper</c> privdes the interface for the
    /// Kml helper used by the Snh Data source.
    /// </summary>
    public interface ISnhKmlHelper
    {
        /// <summary>
        /// Set the NameSpace from the KML input file.
        /// </summary>
        /// <param name="kmlns">The namespace of the file.</param>
        void SetNameSpace(XNamespace kmlns);
                
        /// <summary>
        /// Get the collection of (Placemark) tags for the Names Folder
        /// </summary>
        /// <returns>A collection of 'Placemark' XElements</returns>
        IEnumerable<XElement> GetNamePlacemarks(XElement xelement);

        /// <summary>
        /// Get the collection of (Placemark) tags for the STATUS Folder, which 
        /// define the footprints of the wind farm.
        /// </summary>
        /// <returns>A collection of 'Placemark' XElements</returns>
        /// <remarks>
        /// If no XElement is passed, it gets the placemarks for the STATUS folder
        /// otherwise it returns the placemarks within the XElement passed.
        /// </remarks>
        IEnumerable<XElement> GetAreaPlacemarks(XElement xelement);

        /// <summary>
        /// Determines if the (NAMES) folder exists within the KML.
        /// </summary>
        /// <returns>Returns TRUE if found otherwise FALSE</returns>
        bool NameFolderExists(XElement xelement);

        /// <summary>
        /// Determines if the (STATUS) folder exists within the KML.
        /// </summary>
        /// <returns>Returns TRUE if found otherwise FALSE</returns>
        bool StatusFolderExists(XElement xelement);

        /// <summary>
        /// Get the Name extracted from the KML Placemark tag
        /// </summary>
        /// <param name="placemark">the Placemeark</param>
        /// <returns>The name of the placemark</returns>
        string GetPlacemarkName(XContainer placemark);

        /// <summary>
        /// Get the status of the placemark tab
        /// </summary>
        /// <param name="placemark">the Placemark</param>
        /// <returns>String representing the status</returns>
        string GetPlacemarkStatus(XContainer placemark);

        /// <summary>
        /// Combines the footprint coordinates for two placemarks
        /// </summary>
        /// <param name="placemark">XElement being combined</param>
        /// <param name="combined">XElement being updated</param>
        /// <returns>Updated XElement</returns>
        /// <remarks>
        /// The placemark is returned if the cumulative xElement is empty.
        /// If the combined one has not coordinates the new coordinates are 
        /// added to the combined.  If the new XElement has no coordinates the 
        /// combined XElement is returned.
        /// </remarks>
        //void CombineFootprintData(ref XElement combined, XElement placemark);
        XElement CombineFootprintData(XElement combined, XElement placemark);

        /// <summary>
        /// Removes the special characters like tab from the coordinates
        /// </summary>
        /// <param name="placemark">Placemark</param>
        /// <returns>Cleaned Placemark</returns>
        XElement SanitisePlacemarkCoordinates(XElement placemark);

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
        IDictionary<string, XElement> GetStyles(XElement xelement);

    }
}
