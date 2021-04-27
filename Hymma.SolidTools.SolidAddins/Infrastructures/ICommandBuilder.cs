using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins
{
    public interface ICommandBuilder : IFluentInterface
    {
        /// <summary>
        /// build a <see cref="CommandGroup"/> boject
        /// </summary>
        /// <returns></returns>
        CommandGroup Build();

        /// <summary>
        /// <param name="iconAddress">full file name of the icon for this command</param>
        /// <returns></returns>
        /// </summary>
        ICommandBuilder IconIs(string iconAddress);

        /// <summary>
        /// name as appears in solidworks
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICommandBuilder Name(string name);

        /// <summary>
        /// hint for this command, when user hovers mouse over the button
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        ICommandBuilder Hint(string hint);

        /// <summary>
        /// tooltip for this command
        /// </summary>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        ICommandBuilder Tooltip(string tooltip);

        /// <summary>
        /// name of function that this button will fire
        /// </summary>
        /// <param name="callBackFunction"></param>
        /// <returns></returns>
        ICommandBuilder CallBackFunction(string callBackFunction);

    }
}
