using System;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// event args for <see cref="CalloutEventHandler"/>
    /// </summary>
    public class OnRowValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// construtor
        /// </summary>
        /// <param name="row"></param>
        /// <param name="text"></param>
        public OnRowValueChangedEventArgs(CalloutRow row, string text)
        {
            Row = row;
            Value = text;
        }
        /// <summary>
        /// the row whose value was changed
        /// </summary>
        public CalloutRow Row { get; set; }

        /// <summary>
        /// the new value
        /// </summary>
        public string Value { get; set; }
    }
}
