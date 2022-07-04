using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// You should decorate your addin classes with this attribute <br/>
    /// defines informative properties about your addin
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AddinAttribute : Attribute
    {
        private string _icon;

        /// <summary>
        /// resource name of a bitmap icon in the <strong>.resx file</strong> <i>or</i> the filename (with extension) of an asset whose build property set to <strong>Embedded Resource</strong>.<br/> 
        /// The add-in icon displays next to the add-in name in the SOLIDWORKS Add-in Manager dialog<br/>
        /// </summary>
        /// <remarks>any space character will be replaced by '_'</remarks>
        public string AddinIcon
        {
            get => _icon.Replace(" ", "_");
            set => _icon = value;
        }

        /// <summary>
        /// Solidworks will load this addin at startup if set to True
        /// </summary>
        public bool LoadAtStartup { get; set; } = true;

        /// <summary>
        /// Description for this addin. will be used in addin-list
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The title for this addin in the addin-list
        /// </summary>
        public string Title { get;}

        /// <summary>
        /// default constructor
        /// </summary>
        public AddinAttribute(string title)
        {
            Title = title;
        }
    }
}
