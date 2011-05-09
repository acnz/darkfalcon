using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DarkFalcon.gui
{
    public class _InfoBox : _Control
    {
        int selectedIndex = -1;
        string selectedItem = string.Empty;
        List<string> items = new List<string>();
        Rectangle[] srcRect = new Rectangle[9];
        Rectangle[] destRect = new Rectangle[9];
        Texture2D texture,pixel,tail;

        _Label lab;
        _Button bnext, bprev,bdel;
        _Scrollbar scr;

        Rectangle clipArea;

        public List<string> Items
        {
            get { return items; }
            set
            {
                items = value;
                //InitScrollbars();
            }
        }
        public string this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
        public void Add(string item)
        {
            items.Add(item);
            selectedIndex = items.FindLastIndex(t => t == item);
        }
        public void Remove(string item)
        {
            if (!items.Contains(item))
                return;
            items.Remove(item);
        }
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
                return;
            items.RemoveAt(index);
        }
        public void Clear()
        {
            items.Clear();
        }

        public EventHandler OnIndexChange = null;
        bool bMouseOver = false;


         public _InfoBox(hud pai,string name, Vector2 position, int width, int height, string[] items)
            : base(pai,name, position)
        {
            this.Width = width;
            this.Height = height;
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    this.items.Add(items[i]);
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            // TODO: load your content here
            base.Initialize(content, graphics);
            texture = Texture2D.FromFile(graphics, @"gui\infobox\base.png");
            tail = Texture2D.FromFile(graphics, @"gui\infobox\tail.png");

            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });

            bprev = new _Button(Owner, "bprev", "", new Vector2(Position.X + 5, Position.Y + 5), "infoboxa");
            bprev.OnPress = bprev_OnPress;
            bprev.Initialize(content, graphics);
            bprev.Effect = SpriteEffects.FlipHorizontally;

            bnext = new _Button(Owner, "bnext", "", new Vector2(Position.X + 35, Position.Y + 5), "infoboxa");

            bnext.OnPress = bnext_OnPress;
            bnext.Initialize(Owner.con, Owner.gra);

            bdel = new _Button(Owner, "bdel", "", new Vector2(Position.X + 65, Position.Y + 5), "infoboxb");

            bdel.OnPress = bdel_OnPress;
            bdel.Initialize(Owner.con, Owner.gra);

            lab = new _Label(Owner, "lab", new Vector2(Position.X + 5, Position.Y + 30), "", (int)Width-10, _Label.Align.Left);

            lab.Initialize(Owner.con, Owner.gra);

            int vScrollbarHeight = 0;
                vScrollbarHeight = (int)Height - 35;

                scr = new _Scrollbar(Owner, "scr", Position + new Vector2(Width - 13, 30), _Scrollbar.Type.Vertical, vScrollbarHeight, this.lab.area);
            scr.Owner = this.Owner;
            scr.Initialize(content, graphics);

            clipArea = new Rectangle((int)(Position.X + 5), (int)(Position.Y + 30), (int)(Width - 10), (int)Height - 40);

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
            destRect[3] = new Rectangle(0, 0, srcRect[0].Width, destRect[4].Height-5);
            destRect[5] = new Rectangle(0, 0, srcRect[0].Width, destRect[4].Height);

            destRect[6] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            destRect[7] = new Rectangle(0, 0, destRect[1].Width, srcRect[0].Height);
            destRect[8] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            #endregion
        }
        private void bdel_OnPress(object obj, EventArgs e)
        {
            if (selectedIndex != -1)
            {
                RemoveAt(selectedIndex);
                if (selectedIndex > 0)
                    selectedIndex--;
                else if (items.Count > 1) selectedIndex = 0;
            }
            scr.Value = 0;
        }
        private void bnext_OnPress(object obj, EventArgs e)
        {
            if(selectedIndex != -1)
                if (items.Count > selectedIndex+1)
                {
                    selectedIndex++;
                    if (OnIndexChange != null)
                        OnIndexChange(selectedIndex, null);
                }
            scr.Value = 0;
        }
        private void bprev_OnPress(object obj, EventArgs e)
        {
            if (selectedIndex != -1)
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                    if (OnIndexChange != null)
                        OnIndexChange(selectedIndex, null);
                }
            scr.Value = 0;
        }
        public override void Dispose()
        {
            // TODO: dispose of your content here

            pixel.Dispose();
            texture.Dispose();
            spriteBatch.Dispose();

            base.Dispose();
        }

        public override void Update()
        {
            base.Update();
            if (items.Count > 0)
            {
            // TODO: Add your update logic here
            if (scr != null && scr.Visible)
                scr.Update();

            
                if (selectedIndex != -1)
                {
                    lab.Update();
                    if (selectedIndex != 0)
                        bprev.Update();
                    if (selectedIndex != items.Count - 1)
                        bnext.Update();
                }
                bdel.Update();   
            

            if (area.Contains(mNew.X, mNew.Y))
            {
                if (!bMouseOver)
                {
                    bMouseOver = true;
                    if (OnMouseOver != null)
                        OnMouseOver(this, null);
                }

                if (wasPressed && OnPress != null)
                {
                    OnPress(this, null);
                    Owner.focus = this;
                }
                else if (wasReleased && OnRelease != null)
                    OnRelease(this, null);
            }
            else if (bMouseOver)
            {
                bMouseOver = false;
                if (OnMouseOut != null)
                    OnMouseOut(this, null);
            }
            }
        }
        public override void Draw()
        {
            // TODO: Add your drawing code here
            if (items.Count > 0)
            {
                DrawBackground();

                Render();

                DrawScrollbar();
            }
        }
        private void DrawBackground()
        {
            Color c = new Color(Color.White, 0.5f);
            Rectangle tailbg = new Rectangle((int)Position.X - 14, (int)Position.Y, 20, 20);
            destRect[0].X = (int)Position.X;
            destRect[0].Y = (int)Position.Y;
            //spriteBatch.Draw(texture, destRect[0], srcRect[0], c);
            destRect[1].X = destRect[0].X + destRect[0].Width+1;
            destRect[1].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[1], srcRect[1], c);
            destRect[2].X = destRect[1].X + destRect[1].Width-1;
            destRect[2].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[2], srcRect[2], c);

            destRect[3].X = destRect[0].X;
            destRect[3].Y = destRect[0].Y + destRect[0].Height+5;
            spriteBatch.Draw(texture, destRect[3], srcRect[3], c);
            destRect[4].X = destRect[1].X-1;
            destRect[4].Y = destRect[0].Y + destRect[0].Height;
            //destRect[4].Height -= 5; 
            spriteBatch.Draw(texture, destRect[4], srcRect[4], c);
            destRect[5].X = destRect[2].X;
            destRect[5].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[5], srcRect[5], c);

            destRect[6].X = destRect[0].X;
            destRect[6].Y = destRect[3].Y + destRect[3].Height;
            spriteBatch.Draw(texture, destRect[6], srcRect[6], c);
            destRect[7].X = destRect[1].X-1;
            destRect[7].Y = destRect[4].Y + destRect[4].Height;
            spriteBatch.Draw(texture, destRect[7], srcRect[7], c);
            destRect[8].X = destRect[2].X;
            destRect[8].Y = destRect[5].Y + destRect[5].Height;
            spriteBatch.Draw(texture, destRect[8], srcRect[8], c);

            spriteBatch.Draw(tail, tailbg, c);
        }

        private void Render()
        {
            lab.Text = "";

                if (selectedIndex != -1)
                {
                    spriteBatch.End();
                    spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
                    spriteBatch.GraphicsDevice.ScissorRectangle = clipArea;
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    lab.Text = items[selectedIndex];
                    lab.Position = new Vector2(clipArea.X, clipArea.Y - scr.Value);
                    lab.Draw();
                    spriteBatch.End();
                    spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    if (selectedIndex != 0)
                        bprev.Draw();
                    if (selectedIndex != items.Count - 1)
                        bnext.Draw();
                }
                bdel.Draw();
            
            
        }
        private void DrawScrollbar()
        {
            scr.Max = (int)System.Math.Max(clipArea.Height, lab.Height-clipArea.Height);
            if (scr.Value > scr.Max)
                scr.Value = scr.Max;

            if (scr.Max > clipArea.Height)
            {
                scr.Draw();
            }
        }
    }
}
