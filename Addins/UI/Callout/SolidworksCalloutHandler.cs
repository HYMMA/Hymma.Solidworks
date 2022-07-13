// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swpublished;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
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
        public SolidworksCalloutHandler(CalloutModel callout)
        {
            this._callout = callout;
        }

        private CalloutModel _callout;

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
            var row = _callout.GetRows().FirstOrDefault(r => r.Id == RowID);
            row.Value=Text;
            return true;
        }
        #endregion
    }
}
