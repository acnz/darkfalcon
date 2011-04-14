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
    public class _3DImageBox : _3DClickable
    {
        #region Fields
        string asset;
        Vector2 Location;
        private bool _clicked = false;
        public bool Clicked
        {
            get { return (_clicked); }
            set { _clicked = value; }
        }

        public string Image
        {
            get { return (asset); }
            set { asset = value;
            Load();
            }
        }

        Texture2D textureOn;

        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        Rectangle rec;
        public _3DImageBox(PcView game, string textureName, Vector2 Locationa)
            : base(game)
        {
            asset = textureName;
            Location = Locationa;
            Load();
            Rectangle = rec;


        }

        private void Load()
        {
            textureOn = Game.Content.Load<Texture2D>("Textures//" + asset);
            rec = new Rectangle((int)Location.X,(int)Location.Y,(int)textureOn.Width,(int)textureOn.Height);
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
            if (wasClicking)
            {
                _clicked = true;
                wasClicking = false;
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
            Game.spriteBatch.Draw(textureOn, new Vector2(Rectangle.X, Rectangle.Y), null, new Color(Color.White, alpha));
            Game.spriteBatch.End();
        }
        #endregion
    }
}
