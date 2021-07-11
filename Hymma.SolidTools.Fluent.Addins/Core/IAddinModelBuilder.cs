using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to create and register a property manager page
    /// </summary>
    public interface IAddinModelBuilder : IFluent
    {
        /// <summary>
        /// define a new property manger page. property manager pages open on the left hand side of the window (By default) when you run a command. 
        /// </summary>
        /// <returns></returns>
        IPmpUi AddPropertyManagerPage(string title, ISldWorks solidworks);

        /// <summary>
        /// Add a command tab to the solidworks ui for example 'Features' and 'Sketch' are command tabs
        /// </summary>
        /// <returns></returns>
        IFluentCommandTab AddCommandTab();
    }
}