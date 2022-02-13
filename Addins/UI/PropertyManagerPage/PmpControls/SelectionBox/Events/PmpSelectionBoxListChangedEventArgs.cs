namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// provides event arguments for <see cref="PmpSelectionBox.ListChanged"/> event
    /// </summary>
    public class PmpSelectionBoxListChangedEventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="itemsCount"></param>
        public PmpSelectionBoxListChangedEventArgs(int itemsCount)
        {
            ItemsCount = itemsCount;
        }

        /// <summary>
        /// number of items in the selectionbox once the user interaction with the selection box is finished.
        /// </summary>
        public int ItemsCount { get; }
    }
}
