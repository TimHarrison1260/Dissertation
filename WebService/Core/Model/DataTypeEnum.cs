
namespace Core.Model
{
    /// <summary>
    /// Enumeration <c>DataTypeEnum</c> describes the nature of the 
    /// data source. It might be used to allow data from a particular
    /// source to be extracted idividually.  It should be possible for
    /// multiple sources to have the same DataType: ie SNH and some
    /// other source can be the source of location information.
    /// </summary>
    public enum DataTypeEnum
    {
        Status,         //  The status of the wind farm
        Location,       //  Post code location of wind farm centre
        FootPrint,      //  Snh footprint locaiont
        Statistics,     //  Ren Uk Technical data
        Turbine,        //  Turbine coordinates
        Style           //  Used as a style for representing the status on Google Earth
    }
}
