using Core.Model;

namespace Infrastructure.ServiceModel
{
    /// <summary>
    /// Class <c>ImportData</c> describes the raw data for
    /// each imported source.
    /// </summary>
    public class ImportData
    {
        /// <summary>
        /// Gets or sets the <see cref="DataTypeEnum"/> for the data inported
        /// from the source
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// Gets or sets the actual data from the data source
        /// </summary>
        public string Data { get; set; }
    }
}
