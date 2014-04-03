
namespace Core
{
    /// <summary>
    /// <para>
    /// The <c>Infrastructure</c> namespace contains the implementations of the various
    /// Repositories, Services and physical Data contexts required by the application.
    ///</para>
    /// <para>
    /// <list type="table">
    /// <listheader>
    /// <term>Folder</term>
    /// <description>General description of components</description>
    /// </listheader>
    /// <item>
    /// <term>Algorithms</term>
    /// <description>The overall matching process and the specific string similarity algorithm implementations.</description>
    /// </item>
    /// <item>
    /// <term>Data</term>
    /// <description>The Data context of the Domain model.  Also contains the implementations of the Import Data sources.</description>
    /// </item>
    /// <item>
    /// <term>Helpers</term>
    /// <description>General helper components, KML helper and a custom IComparer(string).</description>
    /// </item>
    /// <item>
    /// <term>Interfaces</term>
    /// <description>Interfaces for components that are only required within the Infrastructure project</description>
    /// </item>
    /// <item>
    /// <term>Mappers</term>
    /// <description>Components for mapping the import classes to the domain classes</description>
    /// </item>
    /// <item>
    /// <term>Migrations</term>
    /// <description>Database migrations and configuration for the Entity Framework Code First model</description>
    /// </item>
    /// <item>
    /// <term>Repositories</term>
    /// <description>Components providing access to the data context.</description>
    /// </item>
    /// <item>
    /// <term>ServiceModel</term>
    /// <description>Classes describing the intermediate model for the data import services</description>
    /// </item>
    /// <item>
    /// <term>Services</term>
    /// <description>Implementation of the various services.</description>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// It contains all elements of the
    /// Application design model, referred to as the 'Onion Model', considered
    /// to be part of the application dealing with the application data model
    /// and logic.
    /// see <a href="http://jeffreypalermo.com/blog/the-onion-architecture-part-1/.">jeffreypalermo.com.</a>
    /// </para>
    /// <para>
    /// It is a separate project within the Application, which therefore holds
    /// all objects in a separate assembly, thus improving the Separation of 
    /// Concerns required for reusable and unit testable code.
    /// </para>
    /// </remarks>
    [System.Runtime.CompilerServices.CompilerGenerated()]
    class NamespaceDoc
    {
    }
}
