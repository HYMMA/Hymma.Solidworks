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
        /// file name of bitmap icon in Resources.resx without extension. <br/>
        /// The add-in icon displays next to the add-in name in the SOLIDWORKS Add-in Manager dialog<br/>
        /// </summary>
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
        public string Title { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public AddinAttribute()
        {
        }
    }
}
