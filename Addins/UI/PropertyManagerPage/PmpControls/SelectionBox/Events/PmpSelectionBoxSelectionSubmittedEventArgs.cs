using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event arguemtns for <see cref="PmpSelectionBox.SelectionSubmitted"/>
    /// </summary>
    public class PmpSelectionBoxSelectionSubmittedEventArgs : EventArgs
    {
        /// <summary> default constructor
        /// </summary>
        /// <param name="selection">Object being selected</param>
        /// <param name="selectType">Entity type of the selection as defined in<see cref="swSelectType_e"/> </param>
        /// <param name="tag">ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection.</param>
        public PmpSelectionBoxSelectionSubmittedEventArgs(object selection, int selectType, string tag)
        {
            this.Selection = selection;
            this.SelectType = selectType;
            this.Tag = tag;
        }

        /// <summary>
        /// object being selected
        /// </summary>
        public object Selection { get; }

        /// <summary>
        /// Entity type of the selection as defined in <see cref="swSelectType_e"/> 
        /// </summary>
        public int SelectType { get; }

        /// <summary>
        /// ItemText is returned to SOLIDWORKS and stored on the selected object and can be used by your PropertyManager page selection list boxes for the life of that selection
        /// </summary>
        public string Tag { get; }
    }
}
