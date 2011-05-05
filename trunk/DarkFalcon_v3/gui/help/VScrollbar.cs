/*
xWinForms © 2007-2009
Eric Grossinger - ericgrossinger@gmail.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNATweener;

namespace DarkFalcon.gui
{
    class _VScrollbar: _Control
    {

        Texture2D background;
        Rectangle backArea;

        int value = 0;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (this.value < 0)
                    this.value = 0;
                else if (this.value > max)
                    this.value = max;
            }
        }
        int max = 0;
        public int Max
        {
            get { return max; }
            set
            {
                max = value;
                if (max < 0)
                    max = 0;
                if (this.value > max)
                    this.value = max;
            }
        }

        int step = 1;
        public int Step { get { return step; } set { step = value; } }

        bool isScrolling = false;
        bool isAutoScrolling = false;
        
        bool inverted = false;
        public bool Inverted { get { return inverted; } set { inverted = value; } }
        
        Texture2D cursorTex;
        Rectangle cursorArea, cursorTop, cursorBottom, cursorMiddle, cursorMidDest,masterArea;
        Vector2 cursorPos, cursorOffset;

        public EventHandler OnChangeValue = null;

        new public float Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        public _VScrollbar(hud pai,Vector2 position, float height,Rectangle masterarea):base(pai)
        {
            this.Position = position;
            this.Height = height;
            masterArea = masterarea;
        }

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            background = Texture2D.FromFile(graphics, @"gui\scrollbar\vscrollbar_back.png");

            Size = new Vector2(12, Height);

            cursorTex = Texture2D.FromFile(graphics, @"gui\scrollbar\vscrollbar_cursor.png");
            cursorTop = new Rectangle(0, 0, cursorTex.Width, 3);
            cursorMiddle = new Rectangle(0, 3, cursorTex.Width, 1);
            cursorBottom = new Rectangle(0, cursorTex.Height - 3, cursorTex.Width, 3);
            cursorMidDest = new Rectangle(0, 0, cursorTex.Width, 1);
            backArea.X = (int)Position.X + 4;
            backArea.Y = (int)Position.Y + 2;
            backArea.Width = 4;
            backArea.Height = (int)Height - 8;
        }


        public override void Dispose()
        {
            cursorTex.Dispose();
            background.Dispose();

            base.Dispose();
        }

        public override void Update()
        {
            base.Update();

            if (a1 && a2)
            {
                if (wasPressed)
                {
                    if (cursorArea.Contains(mNew.X, mNew.Y))
                    {
                        isScrolling = true;
                        cursorOffset = new Vector2(mNew.X, mNew.Y) - new Vector2(cursorArea.X, cursorArea.Y);
                    }
                    else
                    {
                        isAutoScrolling = true;
                    }
                }
            }


            if (isAutoScrolling)
            {
                cursorOffset = new Vector2(0, 10);
                UpdateScrolling();
                isAutoScrolling = false;
                
            }else if (isScrolling)
                if (mNew.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    UpdateScrolling();
                else if (mNew.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    isScrolling = false;

        }

        private void UpdateScrolling()
        {
            cursorPos.Y = mNew.Y - cursorOffset.Y;

            if (cursorPos.Y < Position.Y+2)
                cursorPos.Y = Position.Y+2;
            else if (cursorPos.Y > Position.Y + Height - cursorArea.Height-2)
                cursorPos.Y = Position.Y + Height - cursorArea.Height-2;

            float y = cursorPos.Y - backArea.Y;

            int value = 0;

            if (!inverted)
                value = (int)System.Math.Round(y / (backArea.Height - cursorArea.Height) * max);
            else
                value = max - (int)System.Math.Round(y / (backArea.Height - cursorArea.Height) * max);

            if (value < 0)
                value = 0;
            else if (value > max)
                value = max;

            if (this.value != value)
            {
                this.value = value;
                if (OnChangeValue != null)
                    OnChangeValue(value, null);
            }
        }

        public override void Draw()
        {
            DrawBackground();
            
            DrawCursor();
        }

        private void DrawBackground()
        {
           
            spriteBatch.Draw(background, backArea, Color.White);
        }

        private void DrawCursor()
        {
            cursorArea.Height = 20;
            cursorArea.Width = 8;

            cursorPos.X = Position.X;
            if (!isScrolling)
            {
                if (!inverted)
                    cursorPos.Y = backArea.Y + (Height - 4 - cursorArea.Height) * ((float)value / (float)max);
                else
                    cursorPos.Y = backArea.Y + (Height - 4 - cursorArea.Height) * ((float)(max - value) / (float)max);
            }

            cursorArea.X = (int)(cursorPos.X);
            cursorArea.Y = (int)(cursorPos.Y);

            spriteBatch.Draw(cursorTex, cursorPos, cursorTop, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);            
            
            cursorMidDest.X = (int)cursorPos.X;
            cursorMidDest.Y = (int)cursorPos.Y + 3;
            cursorMidDest.Height = cursorArea.Height - 6;
            spriteBatch.Draw(cursorTex, cursorMidDest, cursorMiddle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.Draw(cursorTex, cursorPos + new Vector2(0f, cursorMidDest.Height), cursorBottom, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
