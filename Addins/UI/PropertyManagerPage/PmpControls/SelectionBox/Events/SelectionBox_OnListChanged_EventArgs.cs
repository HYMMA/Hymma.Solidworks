using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// provides event arguments for <see cref="PmpSelectionBox.OnListChanged"/> event
    /// </summary>
    public class SelectionBox_OnListChanged_EventArgs : EventArgs
    {
        /// <summary>
        /// number of items in the selectionbox once the user interaction with the selection box is finished.
        /// </summary>
        public int ItemsCount { get; internal set; }
    }
}
