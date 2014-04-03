using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Static class <c>ExtensionMethods</c> provides a series
    /// of extension methods.
    /// </summary>
    /// <remarks>
    /// Code courtesy of Eric White's blog
    /// http://blogs.msdn.com/b/ericwhite/archive/2008/12/22/convert-xelement-to-xmlnode-and-convert-xmlnode-to-xelement.aspx
    /// </remarks>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Provides an extension method that converts an
        /// XmlNode to a Linq XElement.
        /// </summary>
        /// <param name="node">the XmlNode</param>
        /// <returns>Linq XElement</returns>
        public static XElement GetXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using (var xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        /// <summary>
        /// Provides an extension method that converts a
        /// Linq XElement to an XmlNode
        /// </summary>
        /// <param name="element">The Linq XElement</param>
        /// <returns>XmlNode</returns>
        /// <remarks>
        /// This does not produce an identical XmlNode to the XElement
        /// but the OuterXml is exactly the same.  This is only needed
        /// for the serialisation of the XElement to a JSON object, and
        /// the Json Serialiser serialises the OuterXml correctly.
        /// </remarks>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        /// <summary>
        /// Provides an extension method that converts a 
        /// Linq XElement to a JSON Object using Newtonsoft.Json
        /// </summary>
        /// <param name="element">The Linq XElement</param>
        /// <returns>A STring containing the XElement serialised to JSON object</returns>
        /// <remarks>
        /// Newtownsoft.JSON.NET is used to serialise the XElement object
        /// </remarks>
        public static string ToJson(this XElement element)
        {
            //  Convert the linq.XElement to an XML element. (Extension methods)
            var xmlStyle = element.GetXmlNode();
            var json = JsonConvert.SerializeXmlNode(xmlStyle);
            return json;
        }

    }
}
