/*
xWinForms © 2007-2009
Eric Grossinger - ericgrossinger@gmail.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkFalcon.gui
{
    class _HScrollbar : _Control
    {
        _Button btLeft, btRight;

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
        bool inverted = false;
        public bool Inverted { get { return inverted; } set { inverted = value; } }

        Texture2D cursorTex;
        Rectangle cursorArea, cursorLeft, cursorRight, cursorMiddle, cursorMidDest;
        Vector2 cursorPos, cursorOffset;

        public EventHandler OnChangeValue = null;

        new public float Width
        {
            get { return base.Width; }
            set { base.Width = value; if (!IsDisposed) Redraw(); }
        }

        public _HScrollbar(hud pai,Vector2 position, float width):base(pai)
        {
            this.Position = position;
            this.Width = width;
            this.Height = 12;
        }

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphics)
        {
            btLeft = new _Button(Owner,"btLeft","", Position,"hscrollbar_button");
            btLeft.OnPress = btUp_OnPress;
            btLeft.Initialize(content, graphics);

            background = Texture2D.FromFile(graphics, @"scrollbar\hscrollbar_back.png");

            cursorTex = Texture2D.FromFile(graphics, @"scrollbar\hscrollbar_cursor.png");
            cursorLeft = new Rectangle(0, 0, 3, cursorTex.Height);
            cursorMiddle = new Rectangle(3, 0, 1, cursorTex.Height);
            cursorRight = new Rectangle(cursorTex.Width - 3, 0, 3, cursorTex.Height);
            cursorMidDest = new Rectangle(0, 0, 1, cursorTex.Height);

            Redraw();

            base.Initialize(content, graphics);
        }

        private void Redraw()
        {
            backArea.X = (int)Position.X + 12;
            backArea.Y = (int)Position.Y;
            backArea.Width = (int)Width - 24;
            backArea.Height = 12;

            btRight = new _Button(Owner,"btRight","", new Vector2(Position.X + Width - 12f, Position.Y), "hscrollbar_button");
            btRight.Effect = SpriteEffects.FlipHorizontally;
            btRight.OnPress = btDown_OnPress;
            btRight.Initialize(Owner.con, Owner.gra);
        }

        private void btUp_OnPress(object obj, EventArgs e)
        {
            if (!inverted)
            {
                if (Value > 0)
                {
                    Value -= step;
                    if (OnChangeValue != null)
                        OnChangeValue(Value, null);
                }
            }
            else
            {
                if (Value < max)
                {
                    Value += step;
                    if (OnChangeValue != null)
                        OnChangeValue(Value, null);
                }
            }
        }
        private void btDown_OnPress(object obj, EventArgs e)
        {
            if (!inverted)
            {
                if (Value < max)
                {
                    Value += step;
                    if (OnChangeValue != null)
                        OnChangeValue(Value, null);
                }
            }
            else
            {
                if (Value > 0)
                {
                    Value -= step;
                    if (OnChangeValue != null)
                        OnChangeValue(Value, null);
                }
            }
        }

        public override void Dispose()
        {
            cursorTex.Dispose();
            background.Dispose();
            btLeft.Dispose();
            btRight.Dispose();

            base.Dispose();
        }

        public override void Update()
        {
            base.Update();

            if (a1 && a2)
            {
                if (wasPressed)
                {
                    isScrolling = true;
                    cursorOffset = new Vector2(mNew.X, mNew.Y) - new Vector2(cursorArea.X, cursorArea.Y);
                }
            }

            if (isScrolling)
                if (mNew.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    UpdateScrolling();
                else if (mNew.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    isScrolling = false;

            if (max > 0)
            {
                btLeft.Update();
                btRight.Update();
            }
        }

        private void UpdateScrolling()
        {
            cursorPos.X = mNew.X - cursorOffset.X;

            if (cursorPos.X < Position.X + 12)
                cursorPos.X = Position.X + 12;
            else if (cursorPos.X > Position.X + Width - cursorArea.Width - 9)
                cursorPos.X = Position.X + Width - cursorArea.Width - 9;

            float x = cursorPos.X - backArea.X;

            int value = 0;

            if (!inverted)
                value = (int)System.Math.Round(x / (backArea.Width - cursorArea.Width) * max);
            else
                value = max - (int)System.Math.Round(x / (backArea.Width - cursorArea.Width) * max);

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
            if (max > 0)
                DrawCursor();
            btLeft.Draw();
            btRight.Draw();
        }

        private void DrawBackground()
        {            
            spriteBatch.Draw(background, backArea, Color.White);
        }

        private void DrawCursor()
        {
            cursorArea.Width = System.Math.Max(20, backArea.Width - System.Math.Max(20, max / 4));
            cursorArea.Height = 12;

            cursorPos.Y = Position.Y;
            if (!isScrolling)
            {
                if (!inverted)
                    cursorPos.X = backArea.X + (Width - 21 - cursorArea.Width) * ((float)value / (float)max);
                else
                    cursorPos.X = backArea.X + (Width - 21 - cursorArea.Width) * ((float)(max - value) / (float)max);
            }

            cursorArea.X = (int)(cursorPos.X);
            cursorArea.Y = (int)(cursorPos.Y);

            spriteBatch.Draw(cursorTex, cursorPos, cursorLeft, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            cursorMidDest.X = (int)cursorPos.X + 3;
            cursorMidDest.Y = (int)cursorPos.Y;
            cursorMidDest.Width = cursorArea.Width - 6;
            spriteBatch.Draw(cursorTex, cursorMidDest, cursorMiddle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.Draw(cursorTex, cursorPos + new Vector2(cursorMidDest.Width, 0f), cursorRight, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
