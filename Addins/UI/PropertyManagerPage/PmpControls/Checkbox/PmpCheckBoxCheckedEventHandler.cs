namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpCheckBox.OnChecked"/>
    /// </summary>
    /// <param name="pmpCheckBox">the controller</param>
    /// <param name="isChecked">the state of the checkbox passed in by SOLIDWORKS</param>
    public delegate void PmpCheckBoxCheckedEventHandler(PmpCheckBox pmpCheckBox, bool isChecked);
}
