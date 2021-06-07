using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a solidworks selection box 
    /// </summary>
    public class PmpSelectionBox : PmpControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public PmpSelectionBox(swSelectType_e[] Filter,  short Height=50) : base(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
        {
            this.Height = Height;
            this.Filter = Filter;
        }
        /// <summary>
        /// array of <see cref="swSelectType_e"/> to allow selection of specific types only
        /// </summary>
        public swSelectType_e[] Filter { get; set; }

        /// <summary>
        /// height of this selection box in proerty manager page
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// SOLIDWORKS will invoke this once focus is changed from this selection box
        /// </summary>
        public Action OnFocusChanged { get; set; }

        public Action<int> OnListChanged { get; set; }
    }
}


public void OnSelectionboxFocusChanged(int Id)
{
    throw new NotImplementedException();
}

public void OnSelectionboxListChanged(int Id, int Count)
{
    throw new NotImplementedException();
}

public void OnSelectionboxCalloutCreated(int Id)
{
    throw new NotImplementedException();
}

public void OnSelectionboxCalloutDestroyed(int Id)
{
    throw new NotImplementedException();
}