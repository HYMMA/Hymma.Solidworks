using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PropertyManagerPageUIBase:IWrapSolidworksObject<IPropertyManagerPage2>
    {
        /// <summary>
        /// base class for making proeprty manager page UI models and controls
        /// </summary>
        /// <param name="solidworks"></param>
        public PropertyManagerPageUIBase(ISldWorks solidworks)
        {
            this.Solidworks = solidworks;
        }

        /// <summary>
        /// bitwise option as defined in <see cref="PmpOptions"/> default is 32807
        /// </summary>
        public int Options { get; set; } = 32807;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public List<PmpGroup> PmpGroups { get; internal set; } = new List<PmpGroup>();
        
        
        /// <summary>
        /// solidworks property managager tabs that in turn can contain other group boxes and controls
        /// </summary>
        public List<PmpTab> PmpTabs { get; set; } 


        /// <summary>
        /// a title for this property manager page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// return a specific control type based on its id
        /// </summary>
        /// <param name="id">id of control to return</param>
        /// <returns></returns>
        public IPmpControl GetControl(int id)
        {
            var control = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(ch => ch.Id == id).FirstOrDefault();
            return control;
        }

        /// <summary>
        /// get all controls of type T in this propery manger page
        /// </summary>
        /// <typeparam name="T">type of control to return</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetControls<T>() where T : IPmpControl
        {
            var controls = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(c => c is T).Cast<T>();
            return controls;
        }

        /// <summary>   
        /// methode to invoke once user clicked on question mark button on property manager page
        /// </summary>
        public Func<bool> OnHelp { get; set; }

        /// <summary>
        /// methode to invoke after the propety manager page is actaved
        /// </summary>
        public Action OnAfterActivation { get; set; }

        /// <summary>
        /// methode to invoke after the propety manager page is closed
        /// </summary>
        public Action OnAfterClose { get; set; }

        /// <summary>
        /// methode to invoke while the property manager page is closing
        /// </summary>
        public Action<PmpCloseReason> OnClose { get; set; }

        /// <summary>
        /// methode to invoke when user goes to the previous page of a property manager page
        /// </summary>
        public Func<bool> OnPreviousPage { get; set; }


        /// <summary>
        /// methode to invoke when user goes to next page of a property manager page
        /// </summary>
        public Func<bool> OnNextPage { get; set; }

        /// <summary>
        /// methode to invoke when user previews the results 
        /// </summary>
        public Func<bool> OnPreview { get; set; }

        /// <summary>
        /// methode to invoke when user selects on Wahts new button
        /// </summary>
        public Action OnWhatsNew { get; set; }

        /// <summary>
        /// methode to invoke when user calls undo (ctrl+z)
        /// </summary>
        public Action OnUndo { get; set; }
        /// <summary>
        /// methode to invoke when user Re-do something (ctrl+y)
        /// </summary>
        public Action OnRedo { get; set; }

        /// <summary>
        /// method to invoke when user clickes on a tab
        /// </summary>
        public Func<int, bool> OnTabClicked { get; set; }

        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }
        
        public IPropertyManagerPage2 SolidworksObject { get; set; }

        internal void Register(ISldWorks solidworks, PropertyManagerPage2Handler9 eventHandler)
        {
            int errors = 0;
            Solidworks = solidworks;
            SolidworksObject = Solidworks.CreatePropertyManagerPage(Title, Options, eventHandler, ref errors) as IPropertyManagerPage2;
            if (SolidworksObject!=null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //add controls
                try
                {
                    PmpGroups.ForEach(group => group.Register(SolidworksObject));
                    PmpTabs.ForEach(tab => tab.Register(SolidworksObject));
                }
                catch (Exception e)
                {
                    Solidworks.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }
    }
}