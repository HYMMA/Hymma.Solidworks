using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// provides event arguments for <see cref="PmpSelectionBox.OnListChanged"/> event
    /// </summary>
    public class SelectionBox_OnListChanged_EventArgs : EventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="itemsCount"></param>
        public SelectionBox_OnListChanged_EventArgs(int itemsCount)
        {
            ItemsCount = itemsCount;
        }

        /// <summary>
        /// number of items in the selectionbox once the user interaction with the selection box is finished.
        /// </summary>
        public int ItemsCount { get; }
    }
}
