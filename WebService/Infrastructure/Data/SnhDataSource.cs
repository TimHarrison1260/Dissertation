using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Core.Model;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Helpers;
using Infrastructure.ServiceModel;

namespace Infrastructure.Data
{
    /// <summary>
    /// Class <c>SnhDataSource</c> represents the source data
    /// for the SNH dataset import.  It accepts the data as 
    /// a string, that would have been uploaded through the 
    /// hosting web service.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This data source is currently not directly accessible 
    /// through a URL from another web service.  This is due 
    /// sometime in the future, but currently will be invoked 
    /// when the source file has been uploaded through the 
    /// web service.
    /// </para>
    /// <para>
    /// The private methods are kept within this module as they 
    /// are specific to this data source and should not be of
    /// any use outside this data source, there is no need for
    /// a helper class yet.  Refactor this to create a KML object
    /// when another KML dataset is being imported.
    /// </para>
    /// </remarks>
    public class SnhDataSource : ISnhDataSource // IDataSource
    {
        private XElement _kml = null; //  The contents of the KML source.
        private XNamespace _kmlns = null; //  The namespace contained in the KML source.
        private ISnhKmlHelper _helper = null; //  Contains KML Helper methods.

        /// <summary>
        /// Ctor:
        /// </summary>
        /// <param name="helper">The instance of the SnhKmlHelper class</param>
        public SnhDataSource(ISnhKmlHelper helper)
        {
            if (helper == null)
                throw new ArgumentNullException("helper", "No valid Kml Helper class available for Snh Data source");
            _helper = helper;
        }


        #region IDataSource Interface

        /// <summary>
        /// Loads the source data supplied
        /// </summary>
        /// <param name="source">The source kml.</param>
        /// <returns>True if the source loads successfully, otherwise False.  False also represents invalid KML file.</returns>
        ///// <exception cref="ArgumentException">
        ///// Thrown if the source does not contain either KML or specifically SNH kml.
        ///// </exception>
        public bool LoadSource(string source)
        {
            XElement kml = null;
            XNamespace kmlns = null;
            if (!LoadKml(out kml, out kmlns, source))
                //  If we get here, there is no corresponding kml namespace, it's not a kml file, return FALSE
                return false;

            //  Data obtained ok, load the internal store.
            if (kml != null)
                SetSource(kml, kmlns);

//            _helper = new SnhKmlHelper(_kmlns); //  Helper class, only referenced here so OK to NEW the instance
            _helper.SetNameSpace(kmlns);

            return true;
        }

        /// <summary>
        /// Gets all unique names from the SNH dataset
        /// </summary>
        /// <returns>Collection of names</returns>
        public IEnumerable<string> GetAllNames()
        {
            if (_helper.NameFolderExists(_kml))
                return _helper.GetNamePlacemarks(_kml).Select(p => p.Element(_kmlns + "name").Value)
                    .OrderBy(s => s)
                    .Distinct();
            else
                return _helper.GetAreaPlacemarks(_kml).Select(p => p.Element(_kmlns + "name").Value)
                    .OrderBy(s => s)
                    .Distinct();
        }

        /// <summary>
        /// Gets all data available for all wind farms defined in SNH dataset
        /// </summary>
        /// <returns>Collection of ImportAggregate models containing the windfarm dtata</returns>
        /// <remarks>
        /// SNH can contain multiple records for some windfarms, they must be rolled up into 1.  The 
        /// reason stated by SNH is that technical limitations in their own systems mean the total 
        /// number of coordinates needed to map the wind farm footprint cannot be held on a single
        /// record.  They therefore use multiple records.  The only difference between them is 
        /// the coordinates for the footprint, all other information is the same. So this method
        /// rolls up the coordinates for any such wind farm.
        /// </remarks>
        public IEnumerable<ImportAggregate> GetAll()
        {
            var results = new List<ImportAggregate>();

            //  Get all the styles so they can be looked up easily.
            var styles = _helper.GetStyles(_kml);

            var previousName = string.Empty;
                //  SNH can contain multiple records for some windfarms They must be rolled up into 1
            XElement previousPlacemark = null; //  

            //  Loop through the folder with name (STATUS), and process each each placemark (Windfarm)
            foreach (var placemark in _helper.GetAreaPlacemarks(_kml))
            {
                var name = _helper.GetPlacemarkName(placemark);

                //  output previous placemark as it's finished with
                if (name != previousName && !string.IsNullOrEmpty(previousName))
                {
                    var cleanedPlacemark = _helper.SanitisePlacemarkCoordinates(previousPlacemark);
                    results.Add(CreateAggregate(previousName, cleanedPlacemark, styles));
                    //  Initialise with current placemark.
                    previousPlacemark = placemark;
                    previousName = name;
                }
                else
                {
                    //  Combine the footprint data.
                    //_helper.CombineFootprintData(ref previousPlacemark, placemark);
                    var updatedPlacemark = _helper.CombineFootprintData(previousPlacemark, placemark);
                    previousPlacemark = updatedPlacemark;
                    previousName = name;
                }
            }

            //  Output the last placemark, if it exists
            if (!string.IsNullOrEmpty(previousName) && previousPlacemark != null)
            {
                //var cleanedLastPlacemark = _helper.SanitisePlacemarkCoordinates(previousPlacemark);
                var cleanedLastPlacemark = _helper.SanitisePlacemarkCoordinates(previousPlacemark);
                results.Add(CreateAggregate(previousName, cleanedLastPlacemark, styles));
            }

            //  Sort the results; because having them in name order will help in the matching process
            //  when multiple instances occur. The chances are that when matching the import against
            //  the aggregates is more likely to match the original and extensions if they are sorted
            //  by name, which will have a better chance of ordering them according to original and 
            //  extensions following.
            //  It should use the comparer defined for the ImportAggregate class.
            results.Sort();

            return results;
        }

        /// <summary>
        /// Gets the actual data from the external data source, by making 
        /// an HTTP request using the specified url.  This SNH data source is 
        /// stored internally so this method throws a "NotImplementedException".
        /// </summary>
        /// <param name="url">The url to the specific external data source</param>
        /// <returns>The external data</returns>
        public ImportAggregate Get(string url)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAltIndices(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the source data for the SNH data source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="nameSpace"></param>
        public void SetSource(XElement source, XNamespace nameSpace)
        {
            _kml = source;
            _kmlns = nameSpace;
        }


        #endregion



        #region private methods

        /// <summary>
        /// Load the KML file into memory
        /// </summary>
        /// <remarks>
        /// This method kept within this class as it needs to extract the namespace 
        /// before it can instantiate the helper class which requires the namespace
        /// in its constructor.  The namespace cannot be injected as it is obtained
        /// from the kml (file) itself.
        /// </remarks>
        private bool LoadKml(out XElement kml, out XNamespace kmlns, string source)
        {
            //  Load the kml file into the XElement.
            var inKml = XElement.Parse(source, LoadOptions.None | LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            //  Get the kml namespace encoded in the file.
            //  It is the xmlns on the <kml> tag.
            var attr = inKml.Attributes();

            foreach (XAttribute a in attr)
            {
                //  TODO: Refactor, this should look for a KML attribute first failing that the XMLNS attribute.
                if (a.Name.LocalName.Equals("xmlns") || a.Name.LocalName.Equals("kml"))
                {
                    //_kml = inKml;       //  set the kml
                    //_kmlns = a.Value;   //  set the namespace
                    kml = inKml;       //  set the kml
                    kmlns = a.Value;   //  set the namespace
                    return true;        //  success
                }
            }

            kml = null;
            kmlns = null;
            return false;               //  failure, not a valid kml file.
        }

        /// <summary>
        /// Create the Import model for the source, which contains all the
        /// data extracted from the source.
        /// </summary>
        /// <param name="name">Name of the aggregate</param>
        /// <param name="placemark">The placemark (data) for the aggregate</param>
        /// <param name="styles">The style, representing the status of the aggregat</param>
        /// <returns>Fully constructed instance of the aggregate containing all data extracted from the source</returns>
        private ImportAggregate CreateAggregate(string name, XElement placemark, IDictionary<string, XElement> styles)
        {
            var statusid = _helper.GetPlacemarkStatus(placemark);

            //  Get the style (Status)
            XElement styleKml = null;
            styles.TryGetValue(statusid, out styleKml);

            //  TODO: get the LOCATION information (from the NAMES folder) by name
            //  TODO: Refactor creation of ImportData and ImportAggregate classes using factory 

            //  Format output object and add to results.
            //var strFootprintData = placemark.ToJson();
            var footprintData = new ImportData()
            {
                DataType = DataTypeEnum.FootPrint,
                Data = placemark.ToJson()
            };
            var statusData = new ImportData()
            {
                DataType = DataTypeEnum.Status,
                Data = (styleKml != null) ? styleKml.ToJson() : string.Empty
            };
            var aggregate = new ImportAggregate()
            {
                Identifier = name,
                SourceId = 1,
                Data = new Collection<ImportData>()
                                {
                                    statusData,
                                    footprintData
                                }
            };
            return aggregate;
        }


        #endregion

    }
}
