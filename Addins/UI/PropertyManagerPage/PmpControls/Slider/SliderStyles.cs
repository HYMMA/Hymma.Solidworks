// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Bitwise styles for <see cref="PmpSlider"/>
    /// </summary>
    [Flags]
    public enum SliderStyles
    {
        /// <summary>
        ///Slider is horizontal.
        ///Slider has not ticks.
        ///Your application is only notified when the user is done dragging the slider to move it.
        /// </summary>
        NoTicks = 0,

        /// <summary>
        /// If set, then the slider is oriented vertically; if not set, then the slider is oriented horizontally
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// If set, then tick marks are created based on BottomLeftTicks and TopRightTicks
        /// </summary>
        AutoTicks = 2,

        /// <summary>
        /// If set, then tick marks appear at the bottom(horizontal) or left(vertical) of the slider
        /// </summary>
        BottomLeftTicks = 4,

        /// <summary>
        /// If set, then tick marks appear at the top (horizontal) or right (vertical) of the slider
        /// </summary>
        TopRightTicks = 8,

        /// <summary>
        /// If set, then your application is notified when the user is dragging the slider, each time the value changes; if not set, then your application is not notified when the user is dragging the slider, only when the user is done dragging the slider; setting this bit allows your application to react immediately to changes, but it does generate many more callbacks, so it is less efficient
        /// </summary>
        NotifyWhileTracking = 16,

    }
}
