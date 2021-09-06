using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// event arguments for a <see cref="PmpLabel"/>
    /// </summary>
    public class Label_OnDisplayEventArgs : EventArgs
    {
        #region fields
        private PropertyManagerPageLabel SolidworksObject;
        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="label"></param>
        public Label_OnDisplayEventArgs(PmpLabel label)
        {
            this.Lable = label;
            this.SolidworksObject = label.SolidworksObject;
        }
        #endregion

        #region property

        /// <summary>
        /// the label that the event arguments are related to
        /// </summary>
        public PmpLabel Lable { get; }
        #endregion

        #region methods
        /// <summary>
        /// get the bold status of specifid characteres 
        /// </summary>
        /// <param name="start">0-based start index</param>
        /// <param name="end">0-based end index</param>
        /// <returns></returns>
        public bool GetBold(short start, short end)
        {
            return SolidworksObject.Bold[start, end];
        }
        #endregion
    }
}
