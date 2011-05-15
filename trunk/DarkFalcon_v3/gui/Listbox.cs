using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DarkFalcon.gui
{
    public class _Listbox : _Control
    {
        List<string> items = new List<string>();
        bool sorted = false;
        bool isSorted = false;
        int visibleItems;

        Texture2D texture, pixel;
        Vector2 textOffset;
        Rectangle[] srcRect = new Rectangle[9];
        Rectangle[] destRect = new Rectangle[9];

        Rectangle selectionRect;
        int hoverIndex = -1;
        int selectedIndex = -1;
        string selectedItem = string.Empty;

        _Scrollbar vscrollbar, hscrollbar;
        bool horizontalScrollbar = false;
        int hScrollWidth = 0;
        public bool HorizontalScrollbar
        {
            get { return horizontalScrollbar; }
            set
            {
                horizontalScrollbar = value;
                //InitScrollbars();
            }
        }
        public bool VScrollArea
        {
            get { return vsa ; }
        }
        bool vsa = false;
        public List<string> Items
        {
            get { return items; }
            set
            {
                items = value;
                //InitScrollbars();
            }
        }
        public bool Sorted { get { return sorted; } set { sorted = value; } }
        public int SelectedIndex { get { return selectedIndex; } set { selectedIndex = value; } }
        public string SelectedItem
        {
            get { return items[selectedIndex]; }
            set
            {
                for (int i = 0; i < items.Count; i++)
                    if (items[i] == value)
                    {
                        selectedIndex = i;
                        break;
                    }
            }
        }

        public EventHandler OnSelect = null;
        public EventHandler OnChangeSelection = null;
        bool bMouseOver = false;

        public _Listbox(hud pai,string name, Vector2 position, int width, int height, string[] items)
            : base(pai,name, position)
        {
            this.Width = width;
            this.Height = height;
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    this.items.Add(items[i]);
        }

        public string this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
        public void Add(string item)
        {
            items.Add(item);
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
            SelectedIndex = -1;
            hoverIndex = -1;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            // TODO: load your content here
            base.Initialize(content, graphics);
            texture = Texture2D.FromFile(graphics, @"gui\textbox\base.png");

            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });

            textOffset = new Vector2(5f, (int)((texture.Height - Font.LineSpacing) / 2f));



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

            destRect[3] = new Rectangle(0, 0, srcRect[0].Width, (int)Height - srcRect[0].Height * 2);
            destRect[4] = new Rectangle(0, 0, destRect[1].Width, destRect[3].Height);
            destRect[5] = new Rectangle(0, 0, srcRect[0].Width, destRect[3].Height);

            destRect[6] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            destRect[7] = new Rectangle(0, 0, destRect[1].Width, srcRect[0].Height);
            destRect[8] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            #endregion

           

            visibleItems = (int)System.Math.Ceiling(Height / Font.LineSpacing);
            //Height = visibleItems * Font.LineSpacing - 8;

            area.Width = (int)Width;
            area.Height = (int)Height;

            selectionRect = new Rectangle(0, 0, (int)Width - 8, Font.LineSpacing - 2);

            //UpdateHScrollbar();
            InitScrollbars(content, graphics);

            if (sorted)
            {
                items.Sort();
                isSorted = true;
            }

        }

        private void InitScrollbars(ContentManager content, GraphicsDevice graphics)
        {
            if (vscrollbar != null && !vscrollbar.IsDisposed)
                vscrollbar.Dispose();
            if (hscrollbar != null && !hscrollbar.IsDisposed)
                hscrollbar.Dispose();

            #region initialize Horizontal scrollbar
            if (!HorizontalScrollbar)
            {
                if (items.Count > visibleItems)
                    hscrollbar = new _Scrollbar(Owner, "hscrollbar", Position + new Vector2(1, Height - 13), _Scrollbar.Type.Horizontal, (int)Width - 14, this);
                else
                    hscrollbar = new _Scrollbar(Owner, "hscrollbar", Position + new Vector2(1, Height - 13), _Scrollbar.Type.Horizontal, (int)Width - 2, this);

                hscrollbar.Owner = this.Owner;
                hscrollbar.Initialize(content, graphics);
            }
            #endregion

            int vScrollbarHeight = 0;
            if (HorizontalScrollbar && hscrollbar.Visible && hscrollbar.Max > 0)
                vScrollbarHeight = (int)Height - 12;
            else
                vScrollbarHeight = (int)Height - 2;

            vscrollbar = new _Scrollbar(Owner, "vscrollbar", Position + new Vector2(Width - 13, 1), _Scrollbar.Type.Vertical, vScrollbarHeight, this);
            vscrollbar.Owner = this.Owner;
            vscrollbar.Initialize(content, graphics);
            //vscrollbar.Visible = false;
        }

        public override void Dispose()
        {
            // TODO: dispose of your content here
            vscrollbar.Dispose();
            if (hscrollbar != null)
                hscrollbar.Dispose();

            pixel.Dispose();
            texture.Dispose();
            spriteBatch.Dispose();

            base.Dispose();
        }

        public override void Update()
        {
            base.Update();

            // TODO: Add your update logic here
            if (vscrollbar != null && vscrollbar.Visible)
                vscrollbar.Update();
            if (horizontalScrollbar && hscrollbar != null && hscrollbar.Visible)
                hscrollbar.Update();

            if (!isSorted && sorted)
                items.Sort();


                if (hoverIndex != -1 && wasPressed)
                {
                    int previousIndex = selectedIndex;
                    selectedIndex = hoverIndex;
                    if (OnSelect != null)
                        OnSelect(items[selectedIndex], null);
                    if (selectedIndex != previousIndex && OnChangeSelection != null)
                        OnChangeSelection(items[selectedIndex], null);
                }
                if (area.Contains(mNew.X,mNew.Y))
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
                if (Owner.focus == vscrollbar)
                    Owner.focus = this;
        }

        private void UpdateHScrollbar()
        {
            float ew = 0;
            for (int i = 0; i < items.Count; i++)
            {
                float w = 0f;

                if (vscrollbar == null || vscrollbar.Max == 0)
                    w = (Font.MeasureString(items[i]).X - Width + 12f);
                else
                    w = (Font.MeasureString(items[i]).X - Width + 30f);

                if (w > ew)
                    ew = w;
            }

            hScrollWidth = (int)ew;
            if (hscrollbar != null)
                hscrollbar.Max = hScrollWidth;
        }

        private void Render()
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)Position.X, (int)Position.Y + 3, (int)(Width), (int)(Height - 8));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            hoverIndex = -1;
            for (int i = vscrollbar.Value; i < System.Math.Min(items.Count, vscrollbar.Value + visibleItems+1); i++)
            {
                if (hscrollbar != null)
                    textOffset.X = -hscrollbar.Value + 2f;
                else
                    textOffset.X = 2f;
                textOffset.Y = (int)((i - vscrollbar.Value) * Font.LineSpacing);

                RenderSelectionBox(i);

                spriteBatch.DrawString(Font, items[i], textOffset+Position, Color.Black);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

        }

        private void RenderSelectionBox( int index)
        {
            //selectionRect.X = (int)textOffset.X - 3;
            selectionRect.X = (int)Position.X + 2;
            selectionRect.Y = (int)(textOffset.Y + Position.Y)+3;
            selectionRect.Height = Font.LineSpacing-4;

            if (vscrollbar.Max > 0)
                selectionRect.Width = (int)Width - 15;

            else
                selectionRect.Width = (int)Width - 3;

            if (index == selectedIndex)
                spriteBatch.Draw(pixel, selectionRect, Color.Silver);
            else if (Owner.area.Contains(mNew.X, mNew.Y) &&
                selectionRect.Contains(mNew.X , mNew.Y))
            {
                hoverIndex = index;
                spriteBatch.Draw(pixel, selectionRect, Color.LightGray);
            }
        }

        public override void Draw()
        {
            // TODO: Add your drawing code here
            

            DrawBackground();
            //spriteBatch.Draw(capturedTexture, Position + Vector2.One, Color.White);

            UpdateHScrollbar();
            DrawScrollbar();
            Render();
        }

        private void DrawBackground()
        {
            destRect[0].X = (int)Position.X;
            destRect[0].Y = (int)Position.Y;
            spriteBatch.Draw(texture, destRect[0], srcRect[0], Color.White);
            destRect[1].X = destRect[0].X + destRect[0].Width;
            destRect[1].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[1], srcRect[1], Color.White);
            destRect[2].X = destRect[1].X + destRect[1].Width;
            destRect[2].Y = destRect[0].Y;
            spriteBatch.Draw(texture, destRect[2], srcRect[2], Color.White);

            destRect[3].X = destRect[0].X;
            destRect[3].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[3], srcRect[3], Color.White);
            destRect[4].X = destRect[1].X;
            destRect[4].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[4], srcRect[4], Color.White);
            destRect[5].X = destRect[2].X;
            destRect[5].Y = destRect[0].Y + destRect[0].Height;
            spriteBatch.Draw(texture, destRect[5], srcRect[5], Color.White);

            destRect[6].X = destRect[0].X;
            destRect[6].Y = destRect[3].Y + destRect[3].Height;
            spriteBatch.Draw(texture, destRect[6], srcRect[6], Color.White);
            destRect[7].X = destRect[1].X;
            destRect[7].Y = destRect[4].Y + destRect[4].Height;
            spriteBatch.Draw(texture, destRect[7], srcRect[7], Color.White);
            destRect[8].X = destRect[2].X;
            destRect[8].Y = destRect[5].Y + destRect[5].Height;
            spriteBatch.Draw(texture, destRect[8], srcRect[8], Color.White);
        }

        private void DrawScrollbar()
        {
            vscrollbar.Max = (int)System.Math.Max(0, items.Count - visibleItems);
            if (vscrollbar.Value > vscrollbar.Max)
                vscrollbar.Value = vscrollbar.Max;

            if (vscrollbar.Max > 0)
            {
                vscrollbar.Draw();
                if (hscrollbar != null)
                {
                    hscrollbar.Width = Width - 14;
                    UpdateHScrollbar();
                }
            }
            else if (hscrollbar != null)
            {
                hscrollbar.Width = Width - 2;
                UpdateHScrollbar();
            }

            if (horizontalScrollbar && hscrollbar != null && hscrollbar.Max > 0)
            {
                vscrollbar.Height = Height - 12;
                hscrollbar.Max = hScrollWidth;
                hscrollbar.Draw();
                visibleItems = (int)System.Math.Ceiling(Height / Font.LineSpacing) - 1;
            }
            else if ((hscrollbar == null || hscrollbar.Max == 0) && vscrollbar.Height != Height - 2)
            {
                vscrollbar.Height = Height - 2;
                visibleItems -= 1;
            }
        }
    }
}