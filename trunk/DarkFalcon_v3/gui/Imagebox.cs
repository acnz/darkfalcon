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
    public class _Imagebox : _Control
    {
        #region Fields
        string asset;
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
        public _Imagebox(hud game)
            : base(game)
        {

        }

        private void Load()
        {
            textureOn = Game.Content.Load<Texture2D>("Textures//" + asset);
            //rec = new Rectangle((int)Location.X,(int)Location.Y,(int)textureOn.Width,(int)textureOn.Height);
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
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw()
        {
        }
        #endregion
    }
}
