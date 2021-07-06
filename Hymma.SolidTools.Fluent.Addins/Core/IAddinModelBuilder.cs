using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to create and register a property manager page
    /// </summary>
    public interface IAddinModelBuilder : IFluent
    {
        /// <summary>
        /// builds and adds a property manager page to solidworks
        /// </summary>
        /// <param name="pmp">use its .Show() method so users of your addin can actually see the property manger page once they clicked on a button</param>
        /// <returns></returns>
        IAddinModelBuilder BuildPropertyManagerPage(out PropertyManagerPageX64 pmp);
        
        /// <summary>
        /// define a new property manger page
        /// </summary>
        /// <returns></returns>
        IPmpUi AddPropertyManagerPage(string title, ISldWorks solidworks);
    }
}