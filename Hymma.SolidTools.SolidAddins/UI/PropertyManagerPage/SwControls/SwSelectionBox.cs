using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class SwSelectionBox : SwPMPConcreteControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SwSelectionBox() : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
        {

        }
        /// <summary>
        /// array of <see cref="swSelectType_e"/> to allow selection of specific types only
        /// </summary>
        public swSelectType_e[] Filter { get; set; }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        public short Height { get; set; }
    }
}
