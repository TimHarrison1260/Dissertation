
namespace AggregateWebService.Models
{
    /// <summary>
    /// class <c>Link</c> is intended to provide the
    /// link that is posted back along with the HTTP
    /// response, as the Hypermedia element of the
    /// response.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Describes the relationship the link has with the parent
        /// </summary>
        public string Rel { get; set; }
        /// <summary>
        /// Contains the link address to the resource
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// Specifies a tag or title for the link
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Specifies the MIME / Hypermedia type of the resource
        /// </summary>
        public string Type { get; set; }
    }
}