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
    public class _Panel : _Control
    {
        #region Fields

        Vector2 origin = Vector2.Zero;
        new float alpha;


        bool bMouseOver = false;
        bool bMouseDown = false;

        public enum Anchor
        {
            C,B,D,E
        }

        Anchor[] anch;
        Texture2D pixel;
        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _Panel(hud pai,string nome,Vector2 position,float width,float height,Anchor[] anchor) : base(pai,nome,position) {
            Name = nome;
            Position = position;
            Size = new Vector2(width, height);
            anch = anchor;
            alpha = 0.7f;
            Rectangle dest = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y);
            if (anch.Contains(Anchor.C)) dest.Y = 5;
            if (anch.Contains(Anchor.E)) dest.X = 5;
            if (anch.Contains(Anchor.B)) dest.Height = Owner.gra.Viewport.Height - dest.Y - 5;
            if (anch.Contains(Anchor.D)) dest.Width = Owner.gra.Viewport.Width - dest.X - 5;

            Position = new Vector2(dest.X, dest.Y);
            Size = new Vector2(dest.Width, dest.Height);
        }
        public _Panel(hud pai, string nome, Vector2 position, float width, float height, Anchor[] anchor, float Alpha)
            : base(pai, nome, position)
        {
            Name = nome;
            Position = position;
            Size = new Vector2(width, height);
            anch = anchor;
            alpha = Alpha;
            Rectangle dest = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y);
            if (anch.Contains(Anchor.C)) dest.Y = 5;
            if (anch.Contains(Anchor.E)) dest.X = 5;
            if (anch.Contains(Anchor.B)) dest.Height = Owner.gra.Viewport.Height - dest.Y - 5;
            if (anch.Contains(Anchor.D)) dest.Width = Owner.gra.Viewport.Width - dest.X - 5;

            Position = new Vector2(dest.X, dest.Y);
            Size = new Vector2(dest.Width, dest.Height);
        }


        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            area.X = (int)(Position.X);
            area.Y = (int)(Position.Y);
            area.Width = (int)Size.X;
            area.Height = (int)Size.Y;
            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Update and render
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update()
        {
            base.Update();

           

            UpdateEvents();
        }
            private void UpdateEvents()
        {


            if (Owner != null && a1 && a2)
            {
                if (!bMouseOver)
                {
                    bMouseOver = true; 
                    if (OnMouseOver != null)
                        OnMouseOver(this, null);
                }

                if (!bMouseDown && wasPressed)
                {
                    bMouseDown = true;
                    if (OnPress != null)
                    {
                        OnPress(this, null);
                        Owner.focus = this;
                    }
                }
                else if (bMouseDown && wasReleased)
                {
                    bMouseDown = false;
                    if (OnRelease != null)
                        OnRelease(this, null);
                }
            }
            else if (bMouseOver)
            {
                bMouseOver = false;
                bMouseDown = false;
                if (OnMouseOut != null)
                    OnMouseOut(this, null);
            }
        }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
            public override void Draw()
            {

                Color c = new Color(Color.Black, alpha);
                spriteBatch.Draw(pixel, area, c);
            }
            
        #endregion
    }
}
