using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a base class for text based controls in a property manager page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PmpTextBase<T> : PmpControl<T>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type"></param>
        public PmpTextBase(swPropertyManagerPageControlType_e type,string caption="", string tip="") : base(type,caption,tip)
        {

        }

        private Color bgColor;
        private Color txtColor;

        /// <summary>
        ///  Gets or sets the background color of an edit box or label on the PropertyManager page. 
        /// </summary>
        /// <value><see cref="Color"/> value for the color of an edit box, a list box, or a label on the PropertyManager page</value>
        public Color BackGroundColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                if (SolidworksObject != null)
                {
                    var control = SolidworksObject as PropertyManagerPageControl;
                    try
                    {
                        //convert color to int 
                        control.BackgroundColor = ColorTranslator.ToWin32(value);
                    }
                    catch (System.Exception)
                    {
#if DEBUG
                        throw;
#endif
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets color of the text of a label on a PropertyManager page. 
        /// </summary>
        /// <value><see cref="Color"/> color value for the text in a PropertyManager page</value>
        public Color TextColor
        {
            get { return txtColor; }
            set
            {
                txtColor = value;
                var control = SolidworksObject as PropertyManagerPageControl;
                try
                {
                    //convert color to int 
                    control.TextColor = ColorTranslator.ToWin32(value);
                }
                catch (System.Exception)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }

    }
}
