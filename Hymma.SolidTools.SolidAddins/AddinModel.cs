using System.Collections.Generic;


namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper class for a typical addin for solidworks
    /// </summary>
    public class AddinModel
    {
        /// <summary>
        /// unique identifier for this addin, gets assigned to by solidworks 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// defines whether this add-in should load at stratup or not
        /// </summary>
        public bool LoadAtStartup { get; set; }

        /// <summary>
        /// description for this add-in
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// title of the addin
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// list of command groups in this addin
        /// </summary>
        public IEnumerable<AddinCommandGroup> CommandGroups { get; set; } 

        /// <summary>
        /// list of command tabs that this addin will add to solidworks
        /// </summary>
        public IEnumerable<AddinCommandTab> CommandTabs { get; set; }

        /// <summary>
        /// list of all call back functions in this add-in
        /// </summary>
        public IEnumerable<string> CallBackFunctions
        {
            get
            {
                var list = new List<string>();
                foreach (var group in CommandGroups)
                {
                    foreach (var command in group.Commands)
                    {
                        list.Add(command.CallBackFunction);
                    }
                }
                return list;
            }
        }

    }
}
