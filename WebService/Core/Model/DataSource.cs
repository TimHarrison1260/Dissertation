using System;
using System.Collections.Generic;

namespace Core.Model
{
    /// <summary>
    /// Class <c>DataSource</c> defines the information that
    /// describes the data source the service accesses.
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Gets or sets the unique Id of the Data source
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Title of the Data source
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the data source
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the copyright associated with the data source
        /// </summary>
        public string CopyRight { get; set; }

        /// <summary>
        /// Gets or sets the Url that points to the data source
        /// </summary>
        public string AccessPath { get; set; }

        /// <summary>
        /// Gets or sets the Type of the Data source
        /// </summary>
        public SourceTypeEnum SourceType { get; set; }

        /// <summary>
        /// Gets or sets the date/time the data source was last imported
        /// </summary>
        public DateTime LastImported { get; set; }

        /// <summary>
        /// Navigation property to the data imported for this data source
        /// </summary>
        public virtual ICollection<AggregateData> SourceData { get; set; }

        /// <summary>
        /// Determines if the data for this Data Source holds a reference to 
        /// external data or holds the actual data.
        /// </summary>
        /// <returns>Returns TRUE if a reference to external data is held otherwise FALSE</returns>
        public bool IsDataExternal()
        {
            return (this.SourceType == SourceTypeEnum.Scraper || this.SourceType == SourceTypeEnum.WebService);
        }

        /// <summary>
        /// Determines if the datasource requires an import to be run
        /// to update the data within the service.
        /// </summary>
        /// <returns>True if the Import is required, otherwise False</returns>
        public bool RequiresImport()
        {
            return (this.SourceType == SourceTypeEnum.Dataset || this.SourceType == SourceTypeEnum.Scraper ||
                    this.SourceType == SourceTypeEnum.WebService);
        }

        /// <summary>
        /// Determines if the import for this data source requires the 
        /// source data to be passed into the Import service
        /// </summary>
        /// <returns>Returns TRUE if source data required otherwise false</returns>
        public bool ImportRequiresSourceData()
        {
            return (this.SourceType == SourceTypeEnum.Dataset)
            ;
        }
    }
}
