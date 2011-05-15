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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using  DarkFalcon.df;


namespace DarkFalcon.gui
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class _tagcloud : _Control
    {
       #region Fields

        Vector2 origin = Vector2.Zero;

        Texture2D pixel;
        Rectangle clipArea;

        List<_Label> labs;
        _Panel pan;
       // _Scrollbar scr;
        dfPC pc;

        public EventHandler onSelect = null;
        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _tagcloud(hud pai,string nome,Vector2 position,float width,float height,dfPC PC) : base(pai,nome,position) {
            Name = nome;
            Position = position;
            Size = new Vector2(width, height);
            pc = PC;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            pan = new _Panel(Owner, "pan", Position+new Vector2(5,5), Width-23, Height-10, new _Panel.Anchor[] { }, 0f);
            pan.Initialize(content, graphics);
            area.X = (int)(Position.X);
            area.Y = (int)(Position.Y);
            area.Width = (int)Size.X;
            area.Height = (int)Size.Y;
            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });
            labs = new List<_Label>();
            //scr = new _Scrollbar(Owner, "scr", new Vector2(Position.X + Width-13, Position.Y-13), _Scrollbar.Type.Vertical, (int)Height - 10, this.pan);
            //scr.Owner = this.Owner;
            //scr.Initialize(content, graphics);

            clipArea = new Rectangle((int)(Position.X + 5), (int)(Position.Y + 5), (int)(Width - 10), (int)Height - 10);
            pc.onChange += new EventHandler(updateLabs);
           
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
            //base.Update();
            //    scr.Update();
             foreach (_Label lab in labs)
             {
                 lab.Update();
             }
        }
        private void UpdateTags()
        {
            int x = (int)pan.Position.X;
            int y = (int)pan.Position.Y;
            pan.Height = 0;
            foreach (_Label lab in labs)
            {

                if(lab.Width > (pan.Position.X + pan.Width)-x){
                    x=(int)pan.Position.X;
                    y += (int)lab.Height+2;
                    pan.Height += (int)lab.Height + 2;
                }

                lab.X = x;
                lab.Y = y;
                x += (int)lab.Width + 10;
            }

        }
        public void updateLabs(object sender, EventArgs e)
        {
            labs.Clear();
            List<dfCom> c = pc.GetAllCom();
            Random r = new Random();
            foreach (dfCom d in c)
            {
                if (d.Nome != "?")
                {
                    foreach (string t in d.Tags.compat)
                    {
                        bool ok  = true;
                        foreach (_Label lab in labs)
                            if (lab.Text == t) ok = false;

                        if (ok) {
                          
                            Color cl = new Color(r.Next(10, 100) / 100f, r.Next(10, 100) / 100f, r.Next(10, 100) / 100f, 1.0f);
                            _Label nl = new _Label(Owner, labs.Count.ToString(), pan.Position, t, _Label.Align.Left, cl);
                            nl.Initialize(Owner.con,Owner.gra);
                            labs.Add(nl);
                            nl.OnRelease = new EventHandler(lab_click);
                        }

                    }
                }
            }
            UpdateTags();
        }
        private void lab_click(object sender, EventArgs e)
        {
            if (onSelect != null)
                onSelect(((_Label)sender).Text, null);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
            public override void Draw()
            {
                    spriteBatch.End();
                spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
                 spriteBatch.GraphicsDevice.ScissorRectangle = clipArea;
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                foreach (_Label lab in labs)
                {
                    lab.Draw();
                }
                        
                              spriteBatch.End();
                spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

                //scr.Max = (int)System.Math.Max(clipArea.Height, 10000);
                //if (scr.Value > scr.Max)
                //    scr.Value = scr.Max;

                //if (scr.Max > clipArea.Height)
                //{
                //    scr.Draw();
                //}
                   
            }
            
        #endregion
    }
}