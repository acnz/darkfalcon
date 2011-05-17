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
    public class _Button : _Control
    {
        #region Fields
        Texture2D texture;
        Texture2D[] textures;
        string texturePath = @"guisrc\button\";
        bool ct = false;


        Vector2 textPos;
        Vector2 textSize;

        string _text = String.Empty;

        Vector2 origin = Vector2.Zero;
        float scale = 1f;

        bool bMouseOver = false;
        bool bMouseDown = false;

        public SpriteEffects Effect;

        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _Button(hud pai) : base(pai) {
            this.texturePath += "default\\";
            Position = new Vector2(0, 0);
            ct = true;
        }
        public _Button(hud pai,string nome,string txt)
            : base(pai, nome)
        {
            this.Text = txt;
            this.texturePath += "default\\";
            Position = new Vector2(0, 0);
            ct = true;
        }
        public _Button(hud pai, string nome, string txt, Vector2 Location) :
        base(pai,nome,Location)
        {
            this.Text = txt;
            this.texturePath += "default\\";
            Position = Location;
            ct = true;
        }
        public _Button(hud pai, string nome, string txt, Vector2 Location, string TextureName) :
            base(pai, nome, Location)
        {
            this.Text = txt;
            this.texturePath += TextureName+"\\";
            Position = Location;
            ct = true;
        }
        public _Button(hud pai, string nome, string txt, Vector2 Location, string TextureName, float Scale) :
            base(pai, nome, Location)
        {
            this.Text = txt;
            this.texturePath += TextureName + "\\";
            Position = Location;
            scale = Scale;
            ct = true;
        }
        public _Button(hud pai, string nome, string txt, Rectangle Location, string TextureName) :
            base(pai, nome, new Vector2(Location.X, Location.Y))
        {
            this.Text = txt;
            this.texturePath += TextureName + "\\";
            Position = new Vector2(Location.X, Location.Y);
            Size = new Vector2(Location.Width, Location.Height);
            ct = false;
        }
        public _Button(hud pai, string nome, string txt, int x, int y, int w, int h, string TextureName) :
            base(pai, nome, new Vector2(x, y))
        {
            this.Text = txt;
            this.texturePath += TextureName + "\\";
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
            ct = false;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            if (ct)
            {
                texture = Texture2D.FromFile(graphics, texturePath + "base.png");
                Size = new Vector2(texture.Width * scale, texture.Height * scale);
            }
            else
            {
                textures = new Texture2D[9];
               

                textures[0] = Texture2D.FromFile(graphics, texturePath + "c1.png");
                textures[1] = Texture2D.FromFile(graphics, texturePath + "c2.png");
                textures[2] = Texture2D.FromFile(graphics, texturePath + "c3.png");
                textures[3] = Texture2D.FromFile(graphics, texturePath + "c4.png");

                textures[4] = Texture2D.FromFile(graphics, texturePath + "v1.png");
                textures[5] = Texture2D.FromFile(graphics, texturePath + "v2.png");

                textures[6] = Texture2D.FromFile(graphics, texturePath + "h1.png");
                textures[7] = Texture2D.FromFile(graphics, texturePath + "h2.png");

                textures[8] = Texture2D.FromFile(graphics, texturePath + "m.png");
                area.X = (int)(Position.X);
                area.Y = (int)(Position.Y);
                if (Size.X <= ((2*textures[0].Width)+1))
                    Size = new Vector2(((2 * textures[0].Width) + 1),Size.Y);
                if (Size.Y <= ((2 * textures[0].Height) + 1))
                    Size = new Vector2(Size.X,((2 * textures[0].Height) + 1));
               

            }
            

            if (Text != string.Empty)
                textSize = Font.MeasureString(Text);

            area.X = (int)(Position.X);
            area.Y = (int)(Position.Y);
            area.Width = (int)Size.X;
            area.Height = (int)Size.Y;
            Effect = SpriteEffects.None;
            if (scale == 0f) scale = 1f;
        }

        public override void Dispose()
        {
            if (texture != null)
                texture.Dispose();

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
                area.X = (int)(Position.X);
                area.Y = (int)(Position.Y);
           

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
                    Owner.focus = this;
                    bMouseDown = true;
                    if (OnPress != null)
                    {
                        OnPress(this, null);
                        
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
                if (Text != string.Empty)
                {
                    textPos.X = (int)(area.X + (Size.X - textSize.X) / 2f);
                    textPos.Y = (int)(area.Y + (area.Height - textSize.Y) / 2f);
                }

                if (ct)
                {
                    if (bMouseOver)
                    {
                        if (bMouseDown)
                        {
                            spriteBatch.Draw(texture, new Vector2(area.X, area.Y), new Rectangle(0, 0, area.Width, area.Height), Color.DimGray, 0f, Vector2.Zero, scale, Effect, 0f);
                            if (Text != string.Empty)
                                spriteBatch.DrawString(Font, Text, textPos, Color.DimGray, 0f, Vector2.Zero, scale, Effect, 0f);
                        }
                        else
                        {
                            spriteBatch.Draw(texture, new Vector2(area.X, area.Y), new Rectangle(0, 0, area.Width, area.Height), Color.White, 0f, Vector2.Zero, scale, Effect, 0f);
                            if (Text != string.Empty)
                                spriteBatch.DrawString(Font, Text, textPos, Color.White, 0f, Vector2.Zero, scale, Effect, 0f);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(texture, new Vector2(area.X,area.Y), new Rectangle(0,0,area.Width,area.Height), new Color(Color.White.ToVector3() * 0.85f), 0f, Vector2.Zero, scale, Effect, 0f);
                        if (Text != string.Empty)
                            spriteBatch.DrawString(Font, Text, textPos, Color.White, 0f, Vector2.Zero, scale, Effect, 0f);
                    }
                }
                else
                {
                    
                    if (bMouseOver)
                    {
                        if (bMouseDown)
                        {
                            drawTex(spriteBatch, Color.DimGray);
                            
                            if (Text != string.Empty)
                                spriteBatch.DrawString(Font, Text, textPos, Color.DimGray);
                        }
                        else
                        {
                            drawTex(spriteBatch, Color.White);
                            if (Text != string.Empty)
                                spriteBatch.DrawString(Font, Text, textPos, Color.White);
                        }
                    }
                    else
                    {
                        drawTex(spriteBatch, new Color(Color.White.ToVector4() * 0.85f));
                        if (Text != string.Empty)
                            spriteBatch.DrawString(Font, Text, textPos, Color.White);
                    }
                }

            }

            private void drawTex(SpriteBatch spriteBatch, Color color)
            {
                int x = area.X;
                int y = area.Y;
                int h=textures[0].Height;
                int w=textures[0].Width;
                int wl = area.Width - 2 * textures[0].Width;
                int hl = area.Height - 2 * textures[0].Height;
                int hp = area.Height - textures[0].Height;
                int wp = area.Width - textures[0].Width;
                Rectangle fillA = new Rectangle(textures[0].Width + x, textures[0].Height + y, wl, hl);

                spriteBatch.Draw(textures[0],new Rectangle(x,y,w,h),color);
                spriteBatch.Draw(textures[1], new Rectangle(wp + x, y, w, h), color);
                spriteBatch.Draw(textures[2], new Rectangle(x, hp + y, w, h), color);
                spriteBatch.Draw(textures[3], new Rectangle(wp + x, hp + y, w, h), color);

                for (int i = 0; i < wl; i++)
                {
                    spriteBatch.Draw(textures[4], new Rectangle(w + i + x, y, 1, h), color);
                    spriteBatch.Draw(textures[5], new Rectangle(w + i + x, hp + y, 1, h), color);
                }
                for (int i = 0; i < hl; i++)
                {
                    spriteBatch.Draw(textures[6], new Rectangle(x, h + i + y, w, 1), color);
                    spriteBatch.Draw(textures[7], new Rectangle(wp + x, h + i + y, w, 1), color);
                }
                spriteBatch.Draw(textures[8], fillA, color);      
             }
            
        #endregion
    }
}
