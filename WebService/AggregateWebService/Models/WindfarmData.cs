
namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>WindfarmData</c> represents the actual
    /// data contained within the aggregate
    /// </summary>
    public class WindfarmData
    {
        /// <summary>
        /// Gets or sets the Type of data 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the actaul data
        /// </summary>
        public string Data { get; set; }
    }
}