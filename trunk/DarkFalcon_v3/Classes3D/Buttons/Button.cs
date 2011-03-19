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


namespace DarkFalcon_v3
{
    /// <summary>
    /// A game component, inherits to Clickable.
    /// Has associated On and Off content.
    /// Has a state of IsChecked that is switched by click.
    /// Draws content according to state.
    /// </summary>
    public class _3DButton : _3DClickable
    {
        #region Fields
        readonly string asset;
        private bool _clicked = false;
        public bool Clicked
        {
            get { return (_clicked); }
        }

        //Texture2D textureOn;

        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _3DButton(PcView game, string textureName, Vector2 Location)
            : base(game)
        {
            Rectangle rec = new Rectangle((int)Location.X, (int)Location.Y, (int)Game.font.MeasureString(textureName).X, (int)Game.font.MeasureString(textureName).Y);
            Rectangle = rec;
            asset = textureName;
        }

        /// <summary>
        /// Load the texture
        /// </summary>
        #endregion

        #region Update and render
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update()
        {
            if (On)
            HandleInput();
            if (IsClicked)
            {
                _clicked = true;
            }else{
                _clicked = false;
            }
            //Console.Out.WriteLine(_clicked);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw()
        {
            Game.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            Game.spriteBatch.DrawString(Game.font, asset , new Vector2(Rectangle.X, Rectangle.Y),
                IsClicking ? new Color(Color.Red, alpha) : new Color(Color.White, alpha));
            Game.spriteBatch.End();
        }
        #endregion
    }
}
