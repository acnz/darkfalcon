using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DarkFalcon.gui
{
    interface i_Control
    {
        /// <summary>
        /// Control name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Position of the control
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// Size of the control
        /// </summary>
        Vector2 Size { get; set; }
        /// <summary>
        /// Determines if the control is enabled
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Determines if the control is visible
        /// </summary>
        bool Visible { get; set; }
        /// <summary>
        /// Determines if the control has been disposed
        /// </summary>
        bool IsDisposed { get; set; }
    }
}
