using SolidWorks.Interop.swpublished;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Allows access to and modifcation of text in a specific row in a callout
    /// </summary>
    [ComVisible(true)]
    public class SolidworksCalloutHandler : SwCalloutHandler
    {
        /// <summary>
        /// solidworks uses this to handle changes in the callout
        /// </summary>
        /// <param name="callout"></param>
        public SolidworksCalloutHandler(SwCallout callout)
        {
            this.Callout = callout;
        }

        public SwCallout Callout { get; }

        #region ISwCalloutHandler Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pManipulator">ICallout object whose text was edited</param>
        /// <param name="RowID">Row in which the text was edi</param>
        /// <param name="Text">New text of RowID</param>
        /// <returns>True to use updated text in RowID, false to use original text in RowID</returns>
        bool ISwCalloutHandler.OnStringValueChanged(object pManipulator, int RowID, string Text)
        {
            //default is to update the callout in each change
            if (Callout.OnValueChanged == null) return true;

            //implemet user instruction otherwise
            return Callout.OnValueChanged.Invoke(RowID, Text);
        }
        #endregion
    }
}
