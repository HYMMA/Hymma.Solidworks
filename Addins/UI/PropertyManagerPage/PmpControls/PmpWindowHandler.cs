using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a windows form host that solidworks uses to show win forms or wpf.
    /// </summary>
    /// <remarks>your addin must ad a reference to WindowsFormsIntegration</remarks>
    public class PmpWindowHandler : PmpControl<IPropertyManagerPageWindowFromHandle>, IEquatable<PmpWindowHandler>
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="ElementHost">solidworks uses <see cref="System.Windows.Forms.Integration.ElementHost"/> to hook into a windows form</param>
        /// <param name="WinFormOrWpfControl">a windows form or wpf controller</param>
        public PmpWindowHandler(ElementHost ElementHost, UserControl WinFormOrWpfControl) : base(swPropertyManagerPageControlType_e.swControlType_WindowFromHandle)
        {
            this.ElementHost = ElementHost;
            this.WindowsControl = WinFormOrWpfControl;
            OnDisplay += PmpWindowHandler_OnDisplay;
            OnRegister += PmpWindowHandler_OnRegister;
        }

        private void PmpWindowHandler_OnRegister()
        {
            if (ElementHost == null || !WindowsControl.HasContent)
                return;
            ElementHost.Child = WindowsControl;
            SolidworksObject?.SetWindowHandlex64(ElementHost.Handle.ToInt64());
        }

        private void PmpWindowHandler_OnDisplay()
        {
            //this should be callled everytime pmp is displayed and on the pmp registration
            PmpWindowHandler_OnRegister();
        }

        /// <summary>
        /// A host for <see cref="WindowsControl"/>
        /// </summary>
        public ElementHost ElementHost { get; }

        /// <summary>
        /// a windows form or wpf controller
        /// </summary>
        public UserControl WindowsControl { get; }

        /// <summary>
        /// makes sure each SwWindowHandler has its unique ElementHost
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PmpWindowHandler other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.ElementHost == other.ElementHost)
                return true;
            return false;
        }
    }
}
