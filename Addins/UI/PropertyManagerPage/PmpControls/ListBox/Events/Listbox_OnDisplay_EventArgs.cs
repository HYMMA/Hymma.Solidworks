using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// on display event arguments for <see cref="PmpListBox"/>
    /// </summary>
    public class Listbox_OnDisplay_EventArgs : OnDisplay_EventArgs
    {
        private short _height;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="listBox">the controller that has fired the event</param>
        /// <param name="height">height of the control in dialogue units</param>
        public Listbox_OnDisplay_EventArgs(PmpListBox listBox, short height) 
            : base((IPropertyManagerPageControl )listBox.SolidworksObject)
        {
            this._height = height;
            this.Height = height;
            this.SolidworksObject = listBox.SolidworksObject;
        }


        /// <summary>
        /// gets and sets the attached drop down list in this list box
        /// </summary>
        /// <value>
        /// 0 	Default height with no scrolling<br/>
        ///1 &lt; 30 	Specified height and no scrolling<br/>
        ///&gt;30  	    Specified height and scrolling, but no auto sizing<br/>
        ///</value>
        ///<remarks>The height is in dialog-box units. You can convert these values to screen units (pixels) by using the Windows MapDialogRect function.<br/>
        ///You can only use this method to set properties on the PropertyManager page before it is displayed or while it is closed</remarks>
        public short Height
        {
            get => _height;
            set => _height = SolidworksObject.Height = value;
        }

        private PropertyManagerPageListbox SolidworksObject { get; }
    }
}
