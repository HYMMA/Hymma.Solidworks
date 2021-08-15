using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// event argument for <see cref="PmpSelectionBox.OnDisplay"/> event
    /// </summary>
    public class SelectionBox_OnDisplay_EventArgs : OnDisplay_EventArgs
    {
        #region Constructor
        /// <summary>
        /// default ocnstructor
        /// </summary>
        /// <param name="pmpSelectionBox"></param>
        /// <param name="activeDocument"></param>
        /// <param name="filters"></param>
        /// <param name="style"></param>
        /// <param name="allowMultipleSelectOfSameEntity"></param>
        /// <param name="singleItemOnly"></param>
        /// <param name="height"></param>
        internal SelectionBox_OnDisplay_EventArgs(PmpSelectionBox pmpSelectionBox, ModelDoc2 activeDocument, IEnumerable<swSelectType_e> filters, int style, bool allowMultipleSelectOfSameEntity, bool singleItemOnly, short height) : base(activeDocument)
        {
            SolidworksObject = pmpSelectionBox.SolidworksObject;
            _filters = filters;
            _style = style;
            _allowMultipleSelectOfSameEntity = allowMultipleSelectOfSameEntity;
            _singleItemOnly = singleItemOnly;
            _height = height;

        }
        #endregion

        #region public properties

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
            get => _filters;
            set
            {
                SolidworksObject.SetSelectionFilters(value.Cast<int>().ToArray());
                _filters = value;
            }
        }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public short Height
        {
            get => _height;
            set
            {
                SolidworksObject.Height = value;
                _height = value;
            }
        }

        /// <summary>
        /// style of this selection box as defined by bitwise <see cref="SelectionBoxStyles"/>
        /// </summary>
        /// <remarks>this property must be set before property manager page is displayed. As a result setting this property will update the <see cref="PmpSelectionBox"/> on the next session of displaying the property manager page</remarks>
        public int Style
        {
            get => _style;
            set
            {
                SolidworksObject.Style = value;
                _style = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the same entity can be selected multiple times in this selection box
        /// </summary>
        /// <value>True if the same entity can be selected multiple times in this selection box, false if not</value>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public bool AllowMultipleSelectOfSameEntity
        {
            get => _allowMultipleSelectOfSameEntity;
            set
            {
                SolidworksObject.AllowMultipleSelectOfSameEntity = value;
                _allowMultipleSelectOfSameEntity = value;
            }
        }

        /// <summary>
        ///  Gets or sets whether this selection box is for single or multiple items. 
        /// </summary>
        /// <remarks>You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public bool SingleItemOnly
        {
            get => _singleItemOnly;
            set
            {
                SolidworksObject.SingleEntityOnly = value;
                _singleItemOnly = value;
            }
        }
        #endregion
        
        #region private fields
        private IPropertyManagerPageSelectionbox SolidworksObject;
        private IEnumerable<swSelectType_e> _filters;
        private int _style;
        private bool _allowMultipleSelectOfSameEntity;
        private bool _singleItemOnly;
        private short _height;
        #endregion
    }
}
