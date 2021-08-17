using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    public class PmpButtonBase<T> : PmpControl<T>
    {
        public PmpButtonBase(swPropertyManagerPageControlType_e type, string caption, string tip) : base(type, caption, tip)
        {

        }
        internal void Press()
        {
            OnPress?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// invoked when this button is clicked
        /// </summary>
        public event EventHandler OnPress;
    }
}
