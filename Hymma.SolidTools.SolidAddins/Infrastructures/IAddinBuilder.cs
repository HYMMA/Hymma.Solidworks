using Hymma.SolidTools.SolidAddins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    public interface IAddinBuilder : IFluentInterface
    {
        /// <summary>
        /// set unique identifier for addin
        /// </summary>
        /// <param name="id">an iteger used by <see cref="SldWorks"/> object</param>
        /// <returns></returns>
        IAddinBuilder WithId(int id);

        /// <summary>
        /// adds this add-in to types defined in this method
        /// </summary>
        /// <param name="docTypes">an <see cref="IEnumerable{T}"/> identifying document types</param>
        /// <returns></returns>
        IAddinBuilder ForTypes(IEnumerable<swDocumentTypes_e> docTypes);


        IAddinBuilder WithCommands(ICommandBuilder commandBuilder);

        /// <summary>
        /// defines title of this addin
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IAddinBuilder Title(string title);

        /// <summary>
        /// the description of the addin
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IAddinBuilder Description(string description);

        /// <summary>
        /// hooks up this add-in to solidworks
        /// </summary>
        /// <returns></returns>
        SldWorks Build();

    }
}
