/*I decided to define thic class as a test to see how it would affect the user experience with fluent design pattern. was not really needed */
using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// this class adds commands to a command group
    /// </summary>
    public class AddinCommands
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="group"></param>
        public AddinCommands(FluentCommandGroup group)
        {
            this.Group = group;
        }

        private FluentCommandGroup Group { get; }

        /// <summary>
        /// define commands to add to the group
        /// </summary>
        /// <param name="comandGenerator">a function that returns <see cref="IEnumerable{T}"/></param>
        /// <returns></returns>
        public IFluentCommandGroup Commands(Func<IEnumerable<AddinCommandBase>> comandGenerator)
        {
            var commands = comandGenerator.Invoke();
            return Commands(commands);
        }

        /// <summary>
        /// add a list of commands to this group
        /// </summary>
        /// <param name="commands"></param>
        public IFluentCommandGroup Commands(IEnumerable<AddinCommandBase> commands)
        {
            commands.ToList().ForEach(c =>
               Group.Commands.ToList().Add(c));
            return Group;
        }
    }
}

