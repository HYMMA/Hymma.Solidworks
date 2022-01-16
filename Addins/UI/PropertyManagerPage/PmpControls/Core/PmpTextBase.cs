using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a base class for text based controls in a property manager page
    /// </summary>
    public class PmpTextBase : PmpControl
    {
        #region fields

        private Color bgColor;
        private Color txtColor;
        #endregion

        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caption"></param>
        /// <param name="tip"></param>
        public PmpTextBase(swPropertyManagerPageControlType_e type, string caption = "", string tip = "") : base(type, caption, tip)
        {
     
        }
        #endregion

        #region properties
       
        /// <summary>
        ///  Gets or sets the background color of an edit box or label on the PropertyManager page. 
        /// </summary>
        /// <value><see cref="Color"/> value for the color of an edit box, a list box, or a label on the PropertyManager page</value>
        public Color BackGroundColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                if (Control != null)
                {
                    try
                    {
                        //convert color to int 
                        Control.BackgroundColor = ColorTranslator.ToWin32(value);
                    }
                    catch (System.Exception)
                    {
                    }
                }
                else
                {
                    OnRegister += () =>
                    {
                        //convert color to int 
                        Control.BackgroundColor = ColorTranslator.ToWin32(value);
                    };
                }
            }
        }


        /// <summary>
        /// Gets or sets color of the text of a label on a PropertyManager page. 
        /// </summary>
        /// <value><see cref="Color"/> color value for the text in a PropertyManager page</value>
        public Color TextColor
        {
            get => txtColor;
            set
            {
                txtColor = value;
                if (Control != null)
                {
                    try
                    {
                        //convert color to int 
                        Control.TextColor = ColorTranslator.ToWin32(value);
                    }
                    catch (System.Exception)
                    {
                    }
                }
                else
                {
                    OnRegister += () =>
                    {
                        //convert color to int 
                        Control.TextColor = ColorTranslator.ToWin32(value);
                    };
                }
            }
        }
        #endregion
    }
}
