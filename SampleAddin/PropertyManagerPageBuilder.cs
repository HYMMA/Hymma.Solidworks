using Hymma.SolidTools.SolidAddins;

namespace SampleAddin
{
    public class PropertyManagerPageBuilder : PropertyManagerBuilderX64
    {
        public PropertyManagerPageBuilder(SampleAddin addin, PropertyManagerPageUI pmpUi) : base(addin, pmpUi)
        {

        }
        public override void Show()
        {
            base.Show();
        }
    }
}
