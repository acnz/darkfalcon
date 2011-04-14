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


namespace DarkFalcon
{
    /// <summary>
    /// A game component, inherits to Clickable.
    /// Has associated On and Off content.
    /// Has a state of IsChecked that is switched by click.
    /// Draws content according to state.
    /// </summary>
    public class _3DCheckbox : _3DClickable
    {
        #region Fields
        string asset;
       // Texture2D textureOn;
        bool isChecked;
        bool radioMode = false;

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
        public _3DCheckbox(PcView game, string textureName, Vector2 Location, bool isChecked)
            : base(game)
        {
            Rectangle rec = new Rectangle((int)Location.X, (int)Location.Y, (int)Game.font.MeasureString(textureName).X, (int)Game.font.MeasureString(textureName).Y);
            Rectangle = rec;
            asset = textureName;
            this.isChecked = isChecked;
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
            if(On)
            HandleInput();

            isChecked = IsClicked ? !isChecked : isChecked;
            if (radioMode)
            {
                if (isChecked)
                {
                    foreach (_3DCheckbox ob in Game.groupM)
                        ob.IsChecked = false;
                    this.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        public override void Draw()
        {
            Game.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            Game.spriteBatch.DrawString(Game.font, asset , new Vector2(Rectangle.X, Rectangle.Y),
                IsChecked ? new Color(Color.Red, alpha) : new Color(Color.White, alpha));
            Game.spriteBatch.End();
        }

        public void modoRadio(){
            radioMode = true;
        }
        #endregion
    }
}
