namespace Hymma.Solidworks.Fluent.Addins
{
    /// <summary>
    /// Creates Multiple object required to make a solidworks addin using fluent pattern
    /// </summary>
    public class AddinFactory : IFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <remarks> a factory to make addin ui
        /// </remarks>
        public AddinFactory()
        {
            addinModelBuilder = new AddinModelBuilder();
        }

        private AddinModelBuilder addinModelBuilder;
        
        /// <summary>
        /// Access main object to make a an addin ui 
        /// </summary>
        /// <returns></returns>
        public AddinModelBuilder GetUiBuilder()
        {
            if (addinModelBuilder == null)
                return new AddinModelBuilder();
            return addinModelBuilder;
        }
    }
}
