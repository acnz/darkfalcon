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
        Rectangle backsrc1, backsrc2;


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
        Rectangle cursorArea,masterArea;
        Vector2 cursorPos, cursorOffset;
        _Control Master;

        public EventHandler OnChangeValue = null;

        new public float Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        public _HScrollbar(hud pai, Vector2 position, float width, _Control master)
            : base(pai)
        {
            this.Position = position;
            this.Width = width;
            this.Height = 12;
            this.masterArea = master.area;
            Master = master;
        }

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);

            btLeft = new _Button(Owner, "btLeft", "", new Vector2(Position.X, (int)(Position.Y - (masterArea.Height / 2)-16)), "hscrollbar");
            btLeft.OnPress = btUp_OnPress;
            btLeft.Initialize(content, graphics);
            btLeft.Effect = SpriteEffects.FlipHorizontally;

            btRight = new _Button(Owner, "btRight", "", new Vector2(Position.X + Width - 32, Position.Y - (masterArea.Height / 2) - 16), "hscrollbar");

            btRight.OnPress = btDown_OnPress;
            btRight.Initialize(Owner.con, Owner.gra);

            background = Texture2D.FromFile(graphics, @"guisrc\scrollbar\hscrollbar_back.png");
            backsrc1 = new Rectangle(0,0,5,14);
            backsrc2 = new Rectangle(6, 0, 4, 14);
            cursorTex = Texture2D.FromFile(graphics, @"guisrc\scrollbar\hscrollbar_cursor.png");
            backArea.X = (int)Position.X + 4;
            backArea.Y = (int)Position.Y + 2;
            backArea.Width = (int)Width - 8;
            backArea.Height = 14;   
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
                    if (cursorArea.Contains(mNew.X, mNew.Y))
                    {
                        isScrolling = true;
                        cursorOffset = new Vector2(mNew.X, mNew.Y) - new Vector2(cursorArea.X, cursorArea.Y);
                    }
                    else if (btLeft.a2 || btRight.a2)
                    {

                    }else
                    {
                        isAutoScrolling = true;
                    }
                    Owner.focus = Master;
                }
            }

            if (isAutoScrolling)
            {
                cursorOffset = new Vector2(0, 10);
                UpdateScrolling();
                isAutoScrolling = false;

            }
            else if (isScrolling)
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

            if (cursorPos.X < Position.X )
                cursorPos.X = Position.X ;
            else if (cursorPos.X > Position.X + Width - cursorArea.Width - 8)
                cursorPos.X = Position.X + Width - cursorArea.Width - 8;

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
            spriteBatch.Draw(background, new Rectangle(backArea.X,backArea.Y,6,backArea.Height),backsrc1, Color.White);
            spriteBatch.Draw(background, new Rectangle(backArea.X+6, backArea.Y, backArea.Width-12, backArea.Height),backsrc2, Color.White);
            spriteBatch.Draw(background, new Rectangle(backArea.X+backArea.Width-6, backArea.Y, 6, backArea.Height), backsrc1, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }

        private void DrawCursor()
        {
            cursorArea.Width = 51;
            cursorArea.Height = 18;

            cursorPos.Y = Position.Y;
            if (!isScrolling)
            {
                if (!inverted)
                    cursorPos.X = backArea.X + (Width - 8 - cursorArea.Width) * ((float)value / (float)max);
                else
                    cursorPos.X = backArea.X + (Width - 8 - cursorArea.Width) * ((float)(max - value) / (float)max);
            }

            cursorArea.X = (int)(cursorPos.X);
            cursorArea.Y = (int)(cursorPos.Y);

            spriteBatch.Draw(cursorTex, cursorPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
