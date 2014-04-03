
namespace Core.Model
{
    /// <summary>
    /// Enumeration <c>SourceTypeEnum</c> defines the type
    /// of source the DataSource is.  it can be a Dataset Import
    /// as SNH, or a Screen Scrape as RenUK, or a call to a 
    /// web service.
    /// It should be used to determine exactly how to access 
    /// the information held.  
    /// </summary>
    public enum SourceTypeEnum
    {
        Local,      //  Within local datastore
        Dataset,    //  A dataset imported
        Scraper,    //  A HTML Screen Scrape, imported
        WebService, //  A Web Service, accessed dynamically
        Manual      //  Manually entered through application
    }
}
