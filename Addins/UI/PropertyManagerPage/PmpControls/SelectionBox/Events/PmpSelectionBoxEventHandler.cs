// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license
namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an event handler for <see cref="PmpSelectionBox"/> where th event returns some event arguments
    /// </summary>
    /// <typeparam name="EventArgs"></typeparam>
    /// <param name="sender">the controller</param>
    /// <param name="eventArgs">event arguments provided to you by SOLIDWORKS when this event happens</param>
    /// <returns></returns>
    public delegate void PmpSelectionBoxEventHandler<EventArgs>(PmpSelectionBox sender, EventArgs eventArgs);

    /// <summary>
    /// handles events for a <see cref="PmpSelectionBox"/> event that does not have any event argument
    /// </summary>
    /// <param name="sender">is the selection box that fired the event</param>
    public delegate void PmpSelectionBoxEventHandler(PmpSelectionBox sender);
}
