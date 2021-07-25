using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a windows form host that solidworks uses to show win forms or wpf
    /// </summary>
    public class PmpWindowHandler : PmpControl<IPropertyManagerPageWindowFromHandle> , IEquatable<PmpWindowHandler>
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="ElementHost">solidworks uses this object to hook into a windows form</param>
        /// <param name="WinFormOrWpfControl">a windows form or wpf controller</param>
        public PmpWindowHandler(ElementHost ElementHost, UserControl WinFormOrWpfControl) : base(swPropertyManagerPageControlType_e.swControlType_WindowFromHandle)
        {
            this.ElementHost = ElementHost;
            this.WindowsControl = WinFormOrWpfControl;

            //although we set it up here we should call this every time a property manager page is displayed
            ElementHost.Child = WindowsControl;
        }

        /// <summary>
        /// A host for <see cref="WindowsControl"/>
        /// </summary>
        public ElementHost ElementHost { get; }
        
        /// <summary>
        /// a windows form or wpf controller
        /// </summary>
        public UserControl WindowsControl { get;  }
        
        /// <summary>
        /// a handle to connect <see cref="WindowsControl"/> to solidworks property manager page
        /// </summary>
        public IPropertyManagerPageWindowFromHandle ProperptyManagerPageHandle { get; internal set; }

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
