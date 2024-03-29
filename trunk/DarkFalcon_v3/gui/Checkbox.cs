using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DarkFalcon.gui
{
    /// <summary>
    /// A game component, inherits to Clickable.
    /// Has associated On and Off content.
    /// Has a state of IsChecked that is switched by click.
    /// Draws content according to state.
    /// </summary>
    public class _Checkbox : _Control
    {
        #region Fields
        string asset;
       // Texture2D textureOn;
        bool isChecked;

        #region Public accessors
        public bool IsChecked { get { return isChecked; } set { isChecked = value; } }
        public string Nome { get { return asset; } set { asset = value; } }
        #endregion
        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _Checkbox(hud game)
            : base(game)
        {
        }

        /// <summary>
        /// Load the texture
        /// </summary>
        #endregion

        #region Update and render and functions
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update()
        {
           
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        public override void Draw()
        {
        }

        #endregion
    }
}
