using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// provides useful arguments and parameters for <see cref="IPmpControl.OnDisplay"/>
    /// </summary>
    public class OnDisplay_EventArgs : EventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="activeDocument"></param>
        public OnDisplay_EventArgs(ModelDoc2 activeDocument)
        {
            ActiveDoc = activeDocument;
        }
        /// <summary>
        /// the active document in this 
        /// </summary>
        public ModelDoc2 ActiveDoc { get; }
    }

    /// <summary>
    /// event argument for <see cref="PmpSelectionBox.OnDisplay"/> event
    /// </summary>
    public class SelectionBox_OnDisplay_EventArgs : OnDisplay_EventArgs
    {

        /// <summary>
        /// public constructor
        /// </summary>
        /// <param name="pmpSelectionBox"></param>
        /// <param name="activeDocument"></param>
        /// <param name="filters"></param>
        /// <param name="height"></param>
        /// <param name="style"></param>
        internal SelectionBox_OnDisplay_EventArgs(PmpSelectionBox pmpSelectionBox, ModelDoc2 activeDocument, IEnumerable<swSelectType_e> filters, short height, int style) : base(activeDocument)
        {
            SolidworksObject = pmpSelectionBox.SolidworksObject;
            Filters = filters;
            Height = height;
            Style = style;
        }

        ///<summary>
        /// array of <see cref="swSelectType_e"/> to allow selection of specific types only.You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed. 
        /// </summary>
        /// <remarks>
        /// <list type="table"><strong><listheader>FILTER ---- RESULTS</listheader></strong>
        /// <item>swSelFACES,  swSelSOLIDBODIES<description> ----- Face<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSELCOMPSDONTOVERRIDE.</description></item>
        /// <item>swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// <item>swSelFACES, swSelSOLIDBODIES, swSelCOMPONENTS<description> ----- Component<br/>If you want a face to appear in the selection box, then use swSelCOMPSDONTOVERRIDE.<br/>If you want a body to appear in the selection box, then use swSelSOLIDBODIESFIRST.</description></item>
        /// </list>
        /// swSelSURFACEBODIES and swSelSURFBODIESFIRST behave simliar to swSelSOLIDBODIES and swSelSOLIDBODIESFIRST. swSelEDGES and swSelVERTICES behave similar to swSelFACES. If the Filters is set to swSelNOTHING or swSelUNSUPPORTED, this the call to this method fails.
        /// </remarks>
        public IEnumerable<swSelectType_e> Filters
        {
            set=> SolidworksObject.SetSelectionFilters(value.Cast<int>().ToArray());
        }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public short Height
        {
            get => SolidworksObject.Height;
            set => SolidworksObject.Height = value;
        }

        /// <summary>
        /// style of this selection box as defined by bitwise <see cref="SelectionBoxStyles"/>
        /// </summary>
        /// <remarks>this property must be set before property manager page is displayed. As a result setting this property will update the <see cref="PmpSelectionBox"/> on the next session of displaying the property manager page</remarks>
        public int Style
        {
            get => SolidworksObject.Style;
            set => SolidworksObject.Style = value;
        }

        private IPropertyManagerPageSelectionbox SolidworksObject;
    }
}
