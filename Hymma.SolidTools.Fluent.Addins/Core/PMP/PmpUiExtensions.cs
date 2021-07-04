using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// extension methods to implemnte fluent into pmpUI
    /// </summary>
    public static class PmpUiExtensions
    {

        /// <summary>
        /// Add a group that hosts controls in a properyt manager page 
        /// </summary>
        /// <param name="pmpUi"></param>
        /// <param name="caption">caption of the group as appears in solidworks</param>
        /// <returns></returns>
        public static PmpGroup AddGroup(this PmpUi pmpUi, string caption)
        {
            var g = new PmpGroup(caption);

            //update the group propety
            g.PropertyManagerPageUIBase = pmpUi;

            //add group to the end of the list
            pmpUi.PmpGroups.Add(g);

            //return the boject in the list
            return pmpUi.PmpGroups[pmpUi.PmpGroups.Count - 1];
        }

        /// <summary>
        /// bitwise option as defined in <see cref="swPropertyManagerPageOptions_e"/> default is 35
        /// </summary>
        /// <param name="pmpUi"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static PmpUi WithOptions(this PmpUi pmpUi, swPropertyManagerPageOptions_e options)
        {
            pmpUi.Options = (int)options;
            return pmpUi;
        }

        /// <summary>
        /// define void() to invoke after the propety manager page is closed
        /// </summary>
        /// <param name="pmp"></param>
        /// <param name="doThis">void to invoke</param>
        /// <returns></returns>
        public static PmpUi AfterClose(this PmpUi pmp, Action doThis)
        {
            pmp.OnAfterClose = doThis;
            return pmp;
        }

        /// <summary>
        /// Action to invoke while the property manger page is closing
        /// </summary>
        /// <param name="pmp"></param>
        /// <param name="doThis"></param>
        /// <returns></returns>
        public static PmpUi WhileClosing(this PmpUi pmp, Action<PMPCloseReason> doThis)
        {
            pmp.OnClose = doThis;
            return pmp;
        }

        /// <summary>
        /// action to take after the property manager page is activated
        /// </summary>
        /// <param name="pmp"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static PmpUi AfterActivation(this PmpUi pmp, Action action)
        {
            pmp.OnAfterActivation = action;
            return pmp;
        }
    }
}
