using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DarkFalcon.gui
{
    public class _MsgBox : _Control
    {
        _Label lab;
        _Button byes, bno, bcancel;

        Texture2D texture,pixel;
        Rectangle[] srcRect = new Rectangle[9];
        Rectangle[] destRect = new Rectangle[9];

        public bool isShow = false; 

        private Type tipo;

        public EventHandler OnResponse = null;

        public enum Type
        {
            YesNo,
            YesNoCancel
        }

        public _MsgBox(hud pai,string name)
            : base(pai,name)
        {
            this.Width = 400;
            this.Height = 200;
            this.Position = new Vector2(Owner.gra.Viewport.Width / 2 - Width / 2, Owner.gra.Viewport.Height / 2 - Height / 2);
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            texture = Texture2D.FromFile(graphics, @"guisrc\infobox\base.png");
            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });

            byes = new _Button(Owner, "byes", "Yes",(int)Position.X + 5 ,(int)Position.Y+150,50,25,"default");
            byes.OnPress = byes_OnPress;
            byes.Initialize(content, graphics);

            bno = new _Button(Owner, "bno", "No",(int)Position.X + 175 ,(int)Position.Y+150,50,25,"default");
            bno.OnPress = bno_OnPress;
            bno.Initialize(content, graphics);
            

            bcancel = new _Button(Owner, "bcancel", "Cancel", (int)Position.X + 235, (int)Position.Y + 150, 50, 25, "default");
            bcancel.OnPress = bcancel_OnPress;


            lab = new _Label(Owner, "lab", new Vector2(Position.X + 5, Position.Y + 75), "", (int)Width - 10, _Label.Align.Center);
            lab.Initialize(Owner.con, Owner.gra);

            bcancel.Initialize(content, graphics);

            #region Create Background Rectangles
            srcRect = new Rectangle[9];
            srcRect[0] = new Rectangle(0, 0, texture.Width - 1, texture.Height / 2);
            srcRect[1] = new Rectangle(texture.Width - 1, 0, 1, texture.Height / 2);
            srcRect[2] = new Rectangle(texture.Width - 1, 0, -(texture.Width - 1), texture.Height / 2);

            srcRect[3] = new Rectangle(0, texture.Height / 2, texture.Width - 1, 1);
            srcRect[4] = new Rectangle(texture.Width - 1, texture.Height / 2, 1, 1);
            srcRect[5] = new Rectangle(texture.Width - 1, texture.Height / 2, -(texture.Width - 1), 1);

            srcRect[6] = new Rectangle(0, texture.Height / 2, texture.Width - 1, -(texture.Height / 2));
            srcRect[7] = new Rectangle(texture.Width - 1, texture.Height / 2, 1, -(texture.Height / 2));
            srcRect[8] = new Rectangle(texture.Width - 1, texture.Height / 2, -(texture.Width - 1), -(texture.Height / 2));

            destRect = new Rectangle[9];
            destRect[0] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            destRect[1] = new Rectangle(0, 0, (int)Width - srcRect[0].Width * 2, srcRect[0].Height);
            destRect[2] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);


            destRect[4] = new Rectangle(0, 0, destRect[1].Width, (int)Height - srcRect[0].Height * 2);
            destRect[3] = new Rectangle(0, 0, srcRect[0].Width, destRect[4].Height);
            destRect[5] = new Rectangle(0, 0, srcRect[0].Width, destRect[4].Height);

            destRect[6] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            destRect[7] = new Rectangle(0, 0, destRect[1].Width, srcRect[0].Height);
            destRect[8] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            #endregion

        }

        private void byes_OnPress(object obj, EventArgs e)
        {
            if (OnResponse != null)
                OnResponse("yes", null);

            isShow = false;
                OnResponse = null;
                Owner.focus = null;
        }
        private void bno_OnPress(object obj, EventArgs e)
        {
            if (OnResponse != null)
                OnResponse("no", null);
            isShow = false;
            OnResponse = null;
            Owner.focus = null;
        }
        private void bcancel_OnPress(object obj, EventArgs e)
        {
            if (OnResponse != null)
                OnResponse("cancel", null);
            isShow = false;
            OnResponse = null;
            Owner.focus = null;
        }

        public override void Update()
        {
            if (isShow)
            {
                base.Update();
                if (tipo == Type.YesNo)
                {
                    byes.Update();
                    bno.Update();
                }
                if (tipo == Type.YesNoCancel)
                {
                    byes.Update();
                    bno.Update();
                    bcancel.Update();
                }
                lab.Update();
            }
        }

        public override void Draw()
        {
            // TODO: Add your drawing code here
            base.Draw();
            if (isShow)
            {
                DrawBackground();

                Render();
                
            }
        }

        private void Render()
        {
            
            if (tipo == Type.YesNo)
            {
                byes.Position = new Vector2(Position.X + Width / 4 - bno.Width / 2, bno.Position.Y);
                bno.Position = new Vector2(Position.X + 3 * Width / 4 - bno.Width / 2, bno.Position.Y);
                byes.Draw();
                bno.Draw();
            }
            if (tipo == Type.YesNoCancel)
            {
                byes.Position = new Vector2(Position.X + 5, bno.Position.Y);
                bcancel.Position = new Vector2(Position.X + Width - bno.Width - 5, bno.Position.Y);
                bno.Position = new Vector2(Position.X + Width / 2 - bno.Width / 2, bno.Position.Y);
                byes.Draw();
                bno.Draw();
                bcancel.Draw();
            }
            lab.Draw();
        }
        private void DrawBackground()
        {

            Color c = new Color(Color.White, 0.5f);

            spriteBatch.Draw(pixel,new Rectangle(0,0,Owner.gra.Viewport.Width,Owner.gra.Viewport.Height),new Color(0,0,0,0.8f));
            destRect[0].X = (int)Position.X;
            destRect[0].Y = (int)Position.Y;
            spriteBatch.Draw(texture, destRect[0], srcRect[0], c);
            destRect[1].X = destRect[0].X + destRect[0].Width;
            destRect[1].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[1], srcRect[1], c);
            destRect[2].X = destRect[1].X + destRect[1].Width;
            destRect[2].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[2], srcRect[2], c);

            destRect[3].X = destRect[0].X;
            destRect[3].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[3], srcRect[3], c);
            destRect[4].X = destRect[1].X;
            destRect[4].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[4], srcRect[4], c);
            destRect[5].X = destRect[2].X;
            destRect[5].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[5], srcRect[5], c);

            destRect[6].X = destRect[0].X;
            destRect[6].Y = destRect[3].Y + destRect[3].Height;
            spriteBatch.Draw(texture, destRect[6], srcRect[6], c);
            destRect[7].X = destRect[1].X;
            destRect[7].Y = destRect[4].Y + destRect[4].Height;
            spriteBatch.Draw(texture, destRect[7], srcRect[7], c);
            destRect[8].X = destRect[2].X;
            destRect[8].Y = destRect[5].Y + destRect[5].Height;
            spriteBatch.Draw(texture, destRect[8], srcRect[8], c);
        }

        public void Show(EventHandler evento, string Message, Type t)
        {
            OnResponse += evento;
            lab.Text = Message;
            isShow = true;
            lab.Position = new Vector2(Position.X + Width / 2 - lab.Width / 2, Position.Y + Height / 2 - lab.Height / 2);
            this.tipo = t;
        }
    }
}
