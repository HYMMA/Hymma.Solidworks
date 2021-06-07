using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PmpUiModel 
    {
        /// <summary>
        /// bitwise option as defined in <see cref="swPropertyManagerPageOptions_e"/> default is 35
        /// </summary>
        public int Options { get; set; } = 35;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public List<PmpGroup> PmpGroups { get; set; } = new List<PmpGroup>();

        /// <summary>
        /// a title for this property manager page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// return a specific control type based on its id
        /// </summary>
        /// <param name="id">id of control to return</param>
        /// <returns></returns>
        public IPmpControl GetControl(int id)
        {
            var control = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(ch => ch.Id == id).FirstOrDefault();
            return control;
        }

        /// <summary>
        /// get all controls of type T in this propery manger page
        /// </summary>
        /// <typeparam name="T">type of control to return</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetControls<T>() where T : IPmpControl 
        {
            var controls = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(c => c is T).Cast<T>();
            return controls;
        }

        /// <summary>   
        /// methode to invoke once user clicked on question mark button on property manager page
        /// </summary>
        public Func<bool> OnHelp { get; set; }
    }
}
