﻿/*
xWinForms © 2007-2009
Eric Grossinger - ericgrossinger@gmail.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using DarkFalcon.gui.help;

namespace DarkFalcon.gui
{
    public class _Textbox : _Control
    {
        Texture2D texture;
        Rectangle[] srcRect;
        Rectangle[] destRect;

        Vector2 textOffset;

        KeyboardHelper keyboard = new KeyboardHelper();
        
        SpriteFont cursorFont;

        Point cursorLocation = Point.Zero;
        Point previousLocation = Point.Zero;

        Vector2 cursorOffset = Vector2.Zero;
        Vector2 scrollOffset = Vector2.Zero;

        bool hasFocus = false;
        public bool HasFocus { get { return hasFocus; } set { hasFocus = value; } }
        bool multiline = false;
        List<string> line = new List<string>();
        Vector2 lineOffset = Vector2.Zero;
        int visibleLines = 0;

        bool acento = false;
        string ps ="";
        string[,] acen;

        bool locked = false;
        public bool Locked { get { return locked; } set { locked = value; } }

        Rectangle sRect, dRect;

        _Scrollbar vscrollbar, hscrollbar;
        
        Scrollbars scrollbar = Scrollbars.None;
        public Scrollbars Scrollbar { get { return scrollbar; } set { scrollbar = value; } }
        public enum Scrollbars
        {
            None,
            Horizontal,
            Vertical,
            Both
        }

        bool bCursorVisible = true;
        System.Timers.Timer timer = new System.Timers.Timer(500);
        bool bMouseOver = false;

        public _Textbox(hud pai,string name, Vector2 position, int width)
            : base(pai,name, position)
        {
            this.Width = width;
        }
        public _Textbox(hud pai, string name, Vector2 position, string text,int width)
            : base(pai, name, position)
        {
            this.Width = width;
            Add(text);
            cursorLocation.X = this.Text.Length;
        }
        //public _Textbox(hud pai, string name, Vector2 position, int width, int height, string text)
        //    : base(pai, name, position)
        //{
        //    this.Width = width;
        //    this.Height = height;
        //    this.multiline = true;
        //    Add(text);
        //    cursorLocation.Y = line.Count - 1;
        //    cursorLocation.X = line[line.Count - 1].Length;
        //}

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            // TODO: load your content here
            if (texture == null)
                texture = Texture2D.FromFile(graphics, @"guisrc\textbox\base.png");
            if (cursorFont == null)
                cursorFont = Font;

            textOffset = new Vector2(3f, (int)((texture.Height - Font.LineSpacing) / 2f));

            if (!multiline)
            {
                srcRect = new Rectangle[3];
                srcRect[0] = new Rectangle(0, 0, texture.Width - 1, texture.Height);
                srcRect[1] = new Rectangle(texture.Width - 1, 0, 1, texture.Height);
                srcRect[2] = new Rectangle(texture.Width - 1, 0, -(texture.Width - 1), texture.Height);
                destRect = new Rectangle[3];
                destRect[0] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
                destRect[1] = new Rectangle(0, 0, (int)Width - srcRect[0].Width * 2, srcRect[1].Height);
                destRect[2] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
                Height = texture.Height;
            }

            acen= new string[17,2];
            acen[0, 0] = "á"; acen[0, 1] = "a";
            acen[1, 0] = "à"; acen[1, 1] = "a";
            acen[2, 0] = "ã"; acen[2, 1] = "a";
            acen[3, 0] = "â"; acen[3, 1] = "a";
            acen[4, 0] = "é"; acen[4, 1] = "e";
            acen[5, 0] = "è"; acen[5, 1] = "e";
            acen[6, 0] = "ê"; acen[6, 1] = "e";
            acen[7, 0] = "í"; acen[7, 1] = "i";
            acen[8, 0] = "ì"; acen[8, 1] = "i";
            acen[9, 0] = "î"; acen[9, 1] = "i";
            acen[10, 0] = "ó"; acen[10, 1] = "o";
            acen[11, 0] = "ò"; acen[11, 1] = "o";
            acen[12, 0] = "ô"; acen[12, 1] = "o";
            acen[13, 0] = "õ"; acen[13, 1] = "o";
            acen[14, 0] = "ù"; acen[14, 1] = "u";
            acen[15, 0] = "ú"; acen[15, 1] = "u";
            acen[16, 0] = "û"; acen[16, 1] = "u";
           
            //else
            //{
            //    visibleLines = (int)System.Math.Ceiling(Height / Font.LineSpacing);
            //    Height = visibleLines * Font.LineSpacing + 2;

            //    //if (IsDisposed)
            //    //{
            //        srcRect = new Rectangle[9];
            //        srcRect[0] = new Rectangle(0, 0, texture.Width - 1, texture.Height / 2);
            //        srcRect[1] = new Rectangle(texture.Width - 1, 0, 1, texture.Height / 2);
            //        srcRect[2] = new Rectangle(texture.Width - 1, 0, -(texture.Width - 1), texture.Height / 2);

            //        srcRect[3] = new Rectangle(0, texture.Height / 2, texture.Width - 1, 1);
            //        srcRect[4] = new Rectangle(texture.Width - 1, texture.Height / 2, 1, 1);
            //        srcRect[5] = new Rectangle(texture.Width - 1, texture.Height / 2, -(texture.Width - 1), 1);

            //        srcRect[6] = new Rectangle(0, texture.Height / 2, texture.Width - 1, -(texture.Height / 2));
            //        srcRect[7] = new Rectangle(texture.Width - 1, texture.Height / 2, 1, -(texture.Height / 2));
            //        srcRect[8] = new Rectangle(texture.Width - 1, texture.Height / 2, -(texture.Width - 1), -(texture.Height / 2));

            //        destRect = new Rectangle[9];
            //        destRect[0] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            //        destRect[1] = new Rectangle(0, 0, (int)Width - srcRect[0].Width * 2, srcRect[0].Height);
            //        destRect[2] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);

            //        destRect[3] = new Rectangle(0, 0, srcRect[0].Width, (int)Height - srcRect[0].Height * 2);
            //        destRect[4] = new Rectangle(0, 0, destRect[1].Width, destRect[3].Height);
            //        destRect[5] = new Rectangle(0, 0, srcRect[0].Width, destRect[3].Height);

            //        destRect[6] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            //        destRect[7] = new Rectangle(0, 0, destRect[1].Width, srcRect[0].Height);
            //        destRect[8] = new Rectangle(0, 0, srcRect[0].Width, srcRect[0].Height);
            //    //}
            //}
            area.Width = (int)Width;
            area.Height = (int)Height;

            sRect = new Rectangle(0, 0, (int)Width, (int)Height);
            dRect = new Rectangle(0, 0, (int)Width, (int)Height);

            if (keyboard.OnKeyPress == null)
                keyboard.OnKeyPress += Keyboard_OnPress;
            if (keyboard.OnPaste == null)
                keyboard.OnPaste += Keyboard_OnPaste;

            //if (multiline && scrollbar != _Textbox.Scrollbars.None)
            //    InitScrollbars(content, graphics);

            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private void InitScrollbars(ContentManager content, GraphicsDevice graphics)
        {
            if (scrollbar == Scrollbars.Vertical)
                vscrollbar = new _Scrollbar(Owner,"vscrollbar", Position + new Vector2(Width - 2, 1), _Scrollbar.Type.Vertical, (int)Height - 2,this);
            else if(scrollbar == Scrollbars.Horizontal)
                hscrollbar = new _Scrollbar(Owner, "hscrollbar", Position + new Vector2(1, Height - 2), _Scrollbar.Type.Horizontal, (int)Width - 2, this);
            else if (scrollbar == Scrollbars.Both)
            {
                vscrollbar = new _Scrollbar(Owner, "vscrollbar", Position + new Vector2(Width - 13, 1), _Scrollbar.Type.Vertical, (int)Height - 14, this);
                hscrollbar = new _Scrollbar(Owner, "hscrollbar", Position + new Vector2(1, Height - 13), _Scrollbar.Type.Horizontal, (int)Width - 14, this);
            }

            if (vscrollbar != null)
            {
                vscrollbar.OnChangeValue = vScrollbar_OnChangeValue;
                vscrollbar.Initialize(content, graphics);
            }
            if (hscrollbar != null)
            {
                hscrollbar.OnChangeValue = hScrollbar_OnChangeValue;
                hscrollbar.Initialize(content, graphics);
            }
        }

        public override void Dispose()
        {

            if (multiline)
            {
                if (vscrollbar != null)
                    vscrollbar.Dispose();
                if (hscrollbar != null)
                    hscrollbar.Dispose();
            }

            texture.Dispose();
            spriteBatch.Dispose();

            base.Dispose();
        }
        private bool volatiled = false;
        public string plainText
        {
            get
            {
                string txt = (string)base.Text.ToLower().Clone();
                try
                {
                    for (int i = 0; i < 17; i++)
                    {
                        string a = acen[i, 0];
                        string b = acen[i, 1];
                        txt = txt.Replace(a,b);
                    }
                }
                catch { }

                return txt;
            }
        }
        public new string Text
        {
            get {
                return base.Text;
            }
            set
            {
               
                base.Text = "";
                line.Clear();
                if (volatiled)
                    base.Text = value;
                else
                {
                    cursorLocation.X = 0;
                    Add(value);
                }
            }
        }

        /* Need to be optimized.. */
        private void Add(string text)
        {
            acento = false;
            if (!multiline)
            {
                base.Text = base.Text.Insert(cursorLocation.X, text);
                //cursorLocation.X += text.Length;
            }
            //else
            //{
            //    if (text.Contains("\n"))
            //    {
            //        string[] lines = text.Split(new char[] { '\n' });

            //        for (int i = 0; i < lines.Length; i++)
            //        {
            //            char tab = '\u0009';
            //            lines[i] = lines[i].Replace(tab.ToString(), "    ");
            //            if (i == 0)
            //            {
            //                if (line.Count == 0)
            //                    line.Add("");
            //                if (cursorLocation.X > line[cursorLocation.Y].Length)
            //                    cursorLocation.X = line[cursorLocation.Y].Length;
            //                line[cursorLocation.Y] = line[cursorLocation.Y].Insert(cursorLocation.X, lines[i]);
            //            }
            //            else
            //                line.Insert(cursorLocation.Y + i, lines[i]);
            //        }
            //    }
            //    else
            //    {
            //        if (line.Count == 0)
            //            line.Add("");

            //        if (cursorLocation.X > line[cursorLocation.Y].Length)
            //            cursorLocation.X = line[cursorLocation.Y].Length;

            //        line[cursorLocation.Y] = line[cursorLocation.Y].Insert(cursorLocation.X, text);
            //    }

            //    //base.Text = "";
            //    for (int i = 0; i < line.Count; i++)
            //        base.Text += line[i];
            //}

            //UpdateScrolling();
        }

        private void Keyboard_OnPress(object obj, EventArgs e)
        {
            if (obj == null)
                return;

            Keys key = (Keys)obj;
            KeyboardEventsArgs args = (KeyboardEventsArgs)e;
            #region keys
            switch (key)
            {
                case Keys.Left:
                    #region Move Cursor Left
                    if (args.ControlDown)
                    {
                        bool foundSpace = false;
                        if (line.Count > 0)
                        {
                            for (int i = cursorLocation.X - 2; i > 0; i--)
                                if (line[cursorLocation.Y].Substring(i, 1) == " ")
                                {
                                    cursorLocation.X = i + 1;
                                    foundSpace = true;
                                    break;
                                }
                        }

                        if (!foundSpace)
                            if (cursorLocation.X != 0)
                                cursorLocation.X = 0;
                            else if (cursorLocation.Y > 0)
                            {
                                cursorLocation.Y -= 1;
                                cursorLocation.X = line[cursorLocation.Y].Length;
                            }
                    }
                    else
                    {
                        if (!multiline && cursorLocation.X > 0)
                            cursorLocation.X -= 1;
                        else if (multiline)
                        {
                            if (cursorLocation.X > 0)
                                cursorLocation.X -= 1;
                            else if (cursorLocation.Y > 0)
                            {
                                cursorLocation.X = line[cursorLocation.Y - 1].Length;
                                cursorLocation.Y -= 1;
                            }
                        }
                    }
                    #endregion
                    break;
                case Keys.Right:
                    #region Move Cursor Right
                    if (args.ControlDown)
                    {
                        bool foundSpace = false;
                        if (line.Count > 0)
                        {
                            for (int i = cursorLocation.X; i < line[cursorLocation.Y].Length; i++)
                                if (line[cursorLocation.Y].Substring(i, 1) == " ")
                                {
                                    cursorLocation.X = i + 1;
                                    foundSpace = true;
                                    break;
                                }

                            if (!foundSpace)
                                if (cursorLocation.X != line[cursorLocation.Y].Length)
                                    cursorLocation.X = line[cursorLocation.Y].Length;
                                else if (cursorLocation.Y < line.Count - 1)
                                {
                                    cursorLocation.X = 0;
                                    cursorLocation.Y += 1;
                                }
                        }
                    }
                    else
                    {
                        if (!multiline && cursorLocation.X < Text.Length)
                            cursorLocation.X += 1;
                        else if (multiline)
                        {
                            if (cursorLocation.X < line[cursorLocation.Y].Length)
                                cursorLocation.X += 1;
                            else if (cursorLocation.Y < line.Count - 1)
                            {
                                cursorLocation.X = 0;
                                cursorLocation.Y += 1;
                            }
                        }
                    }
                    #endregion
                    break;
                case Keys.Up:
                    if (cursorLocation.Y > 0)
                        cursorLocation.Y -= 1;
                    break;
                case Keys.Down:
                    if (cursorLocation.Y < line.Count - 1)
                        cursorLocation.Y += 1;
                    break;
                case Keys.Back:
                    #region Backspace
                    if (!multiline && cursorLocation.X > 0)
                    {
                        if (cursorLocation.X > 1 && Text.Substring(cursorLocation.X - 2, 2) == "\n")
                        {
                            cursorLocation.X -= 2;
                            volatiled = true;
                            Text = Text.Remove(cursorLocation.X - 2, 2);
                            volatiled = false;
                        }
                        else
                        {
                            cursorLocation.X -= 1;
                            volatiled = true;
                            Text = Text.Remove(cursorLocation.X, 1);
                            volatiled = false;
                            
                        }
                    }
                    else if (multiline)
                    {
                        if (cursorLocation.X > 0)
                        {
                            line[cursorLocation.Y] = line[cursorLocation.Y].Remove(cursorLocation.X - 1, 1);
                            cursorLocation.X -= 1;
                        }
                        else
                        {
                            if (cursorLocation.Y > 0)
                            {
                                cursorLocation.X = line[cursorLocation.Y - 1].Length;
                                line[cursorLocation.Y - 1] += line[cursorLocation.Y];
                                line.RemoveAt(cursorLocation.Y);
                                cursorLocation.Y -= 1;
                            }
                        }
                    }
                    #endregion
                    break;
                case Keys.Delete:
                    #region Delete
                    if (!multiline && cursorLocation.X < Text.Length)
                    {
                        if (cursorLocation.X < Text.Length - 1 && Text.Substring(cursorLocation.X, 2) == "\n"){
                           volatiled = true;
                            Text = Text.Remove(cursorLocation.X, 2);
                            volatiled = false;
                            
                    }else{
                        volatiled = true;
                            Text = Text.Remove(cursorLocation.X, 1);
                        volatiled = false;}
                    }
                    else if (multiline)
                    {
                        if (cursorLocation.X < line[cursorLocation.Y].Length)
                            line[cursorLocation.Y] = line[cursorLocation.Y].Remove(cursorLocation.X, 1);
                        else if (cursorLocation.X == line[cursorLocation.Y].Length)
                            if (cursorLocation.Y < line.Count - 1)
                            {
                                if (line[cursorLocation.Y + 1].Length > 0)
                                    line[cursorLocation.Y] += line[cursorLocation.Y + 1];
                                line.RemoveAt(cursorLocation.Y + 1);
                            }
                    }
                    #endregion
                    break;
                case Keys.Home:
                    cursorLocation.X = 0;
                    if (multiline && args.ControlDown)
                    {
                        cursorLocation.Y = 0;
                        scrollOffset.X = 0;
                    }
                    break;
                case Keys.End:
                    if (!multiline)
                        cursorLocation.X = Text.Length;
                    else
                    {
                        if (args.ControlDown)
                        {
                            cursorLocation.Y = line.Count - 1;
                            cursorLocation.X = line[cursorLocation.Y].Length;                            
                        }
                        else
                            cursorLocation.X = line[cursorLocation.Y].Length;
                    }
                    break;
                case Keys.Enter:
                    if (multiline)
                    {
                        string lineEnd = "";

                        if (line[cursorLocation.Y].Length > cursorLocation.X)
                        {
                            lineEnd = line[cursorLocation.Y].Substring(cursorLocation.X, line[cursorLocation.Y].Length - cursorLocation.X);
                            line[cursorLocation.Y] = line[cursorLocation.Y].Substring(0, line[cursorLocation.Y].Length - lineEnd.Length);
                        }

                        line.Insert(cursorLocation.Y + 1, lineEnd);
                        cursorLocation.X = 0;
                        cursorLocation.Y += 1;
                    }
                    break;
                case Keys.Space:
                    Add(" ");
                    cursorLocation.X += 1;
                    break;
                case Keys.Decimal:
                    Add(".");
                    cursorLocation.X += 1;
                    break;
                case Keys.Divide:
                    Add("/");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemBackslash:
                    if (args.ShiftDown) Add("|"); else Add("\\");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemCloseBrackets:
                    if (args.ShiftDown) Add("{"); else Add("[");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemComma:
                    if (args.ShiftDown) Add("<"); else Add(",");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemMinus:
                    if (args.ShiftDown) Add("_"); else Add("-");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemOpenBrackets:
                    if (args.ShiftDown)
                    {
                        if (!acento) { ps = "`"; acento = true; }
                        else
                        {
                            Add("`"); cursorLocation.X += 1;
                            
                        }
                    }
                    else
                    {
                        if (!acento) { ps = "´"; acento = true; }
                        else
                        {
                            Add("´"); cursorLocation.X += 1;
                            
                        }
                    }
                    break;
                case Keys.OemPeriod:
                    if (args.ShiftDown) Add(">"); else Add(".");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemPipe:
                    if (args.ShiftDown) Add("}"); else Add("]");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemPlus:
                    if (args.ShiftDown) Add("+"); else Add("=");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemQuestion:
                    if (args.ShiftDown) Add(":"); else Add(";");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemQuotes:
                    if (args.ShiftDown){
                        if (!acento) { ps = "^"; acento = true; }
                        else
                        {
                            Add("^"); cursorLocation.X += 1;
                            
                        }
                    } else {
                        if (!acento) { ps = "~"; acento = true; }
                        else
                        {
                            Add("~"); cursorLocation.X += 1;
                            
                        }
                    }
                    break;
                case Keys.OemSemicolon:
                    Add("ç");
                    cursorLocation.X += 1;
                    break;
                case Keys.OemTilde:
                    if (args.ShiftDown) Add("\""); else Add("'");
                    cursorLocation.X += 1;
                    break;
                case Keys.Oem8:
                    if (args.ShiftDown) Add("?"); else Add("/");
                    cursorLocation.X += 1;
                    break;
                case Keys.Subtract:
                    Add("-");
                    cursorLocation.X += 1;
                    break;
                case Keys.Multiply:
                    Add("*");
                    cursorLocation.X += 1;
                    break;
                case Keys.Tab:
                    Add("      ");
                    cursorLocation.X += 6;
                    break;
                case Keys.Add:
                    Add("+");
                    cursorLocation.X += 1;
                    break;
                default:
                    string k = key.ToString();
                    if (k.Length == 1)
                    {
                        if (args.CapsLock || args.ShiftDown)
                        {
                            if (ps == "`")
                            {
                                if (k == "A") { Add("À"); ps = ""; }
                                else
                                    if (k == "E") { Add("È"); ps = ""; }
                                    else
                                        if (k == "I") { Add("Ì"); ps = ""; }
                                        else
                                            if (k == "O") { Add("Ò"); ps = ""; }
                                            else
                                                if (k == "U") { Add("Ù"); ps = ""; } else Add(k.ToUpper());

                               
                            }
                            else
                                if (ps == "´")
                                {
                                    if (k == "A") { Add("Á"); ps = ""; }else
                                    if (k == "E") { Add("É"); ps = ""; }else
                                    if (k == "I") { Add("Í"); ps = ""; }else
                                    if (k == "O") { Add("Ó"); ps = ""; }else
                                    if (k == "U") { Add("Ú"); ps = ""; }else
                                         Add(k.ToUpper());

                                    
                                }
                                else
                                    if (ps == "^")
                                    {
                                        if (k == "A") { Add("Â"); ps = ""; }
                                        else
                                            if (k == "E") { Add("Ê"); ps = ""; }
                                            else
                                                if (k == "I") { Add("Î"); ps = ""; }
                                                else
                                                    if (k == "O") { Add("Ô"); ps = ""; }
                                                    else
                                                        if (k == "U") { Add("Û"); ps = ""; }
                                                        else
                                                            Add(k.ToUpper());
                                        
                                    }
                                    else
                                        if (ps == "~")
                                        {
                                            if (k == "A") { Add("Ã"); ps = ""; }
                                            else
                                                if (k == "O") { Add("Õ"); ps = ""; }
                                                else
                                                    Add(k.ToUpper());
                                           
                                        }
                                        else
                                            Add(k.ToUpper());
                        }

                        else
                        {
                            if (ps == "`")
                            {
                                if (k == "A") { Add("à"); ps = ""; }
                                else
                                    if (k == "E") { Add("è"); ps = ""; }
                                    else
                                        if (k == "I") { Add("ì"); ps = ""; }
                                        else
                                            if (k == "O") { Add("ò"); ps = ""; }
                                            else
                                                if (k == "U") { Add("ù"); ps = ""; }
                                                else
                                                    Add(k.ToLower());

                                
                            }
                            else
                                if (ps == "´")
                                {
                                    if (k == "A") { Add("á"); ps = ""; }
                                    else
                                        if (k == "E") { Add("é"); ps = ""; }
                                        else
                                            if (k == "I") { Add("í"); ps = ""; }
                                            else
                                                if (k == "O") { Add("ó"); ps = ""; }
                                                else
                                                    if (k == "U") { Add("ú"); ps = ""; }
                                                    else
                                                        Add(k.ToLower());

                                   
                                }
                                else
                                    if (ps == "^")
                                    {
                                        if (k == "A") { Add("â"); ps = ""; }
                                        else
                                            if (k == "E") { Add("ê"); ps = ""; }
                                            else
                                                if (k == "I") { Add("î"); ps = ""; }
                                                else
                                                    if (k == "O") { Add("ô"); ps = ""; }
                                                    else
                                                        if (k == "U") { Add("û"); ps = ""; } else Add(k.ToLower());

                                    }
                                    else
                                        if (ps == "~")
                                        {
                                            if (k == "A") {Add("ã");ps = "";}else
                                                if (k == "O") { Add("õ"); ps = ""; }
                                                else
                                                Add(k.ToLower());
                                            
                                        }
                                        else
                                            Add(k.ToLower());
                        }
                        cursorLocation.X += 1;
                      
                    }
                    else if (k.Length == 2 && k.StartsWith("D"))
                    {
                        if (args.ShiftDown)
                        {
                            switch (int.Parse(k.Substring(1, 1)))
                            {
                                case 0:
                                    Add(")");
                                    break;
                                case 1:
                                    Add("!");
                                    break;
                                case 2:
                                    Add("@");
                                    break;
                                case 3:
                                    Add("#");
                                    break;
                                case 4:
                                    Add("$");
                                    break;
                                case 5:
                                    Add("%");
                                    break;
                                case 6:
                                    Add("^");
                                    break;
                                case 7:
                                    Add("&");
                                    break;
                                case 8:
                                    Add("*");
                                    break;
                                case 9:
                                    Add("(");
                                    break;
                            }
                        }
                        else
                        {
                            string letra = k.Substring(1, 1);

                            Add(letra);

                        }
                        cursorLocation.X += 1;

                    }
                    break;
            }
            #endregion

            if (!acento)
            {
                if(ps != ""){
                Add(ps);
                cursorLocation.X += 1;
                ps = "";}
            }
            UpdateScrolling();

            bCursorVisible = true;
            timer.Start();
        }

        private void Keyboard_OnPaste(object obj, EventArgs e)
        {
            if (obj != null)
            {
                System.Windows.Forms.IDataObject dataObj = (System.Windows.Forms.IDataObject)obj;
                if (dataObj.GetDataPresent(System.Windows.Forms.DataFormats.Text))
                {
                    string clipboardText = (string)dataObj.GetData(System.Windows.Forms.DataFormats.Text);
                    clipboardText = clipboardText.Replace("\r", "");
                    if (multiline)
                        clipboardText = clipboardText.Replace("\t", "    ");

                    Add(clipboardText);

                    Vector2 textSize = Font.MeasureString(clipboardText);
                    cursorLocation.Y += (int)(textSize.Y / Font.LineSpacing) - 1;

                    string[] lines = clipboardText.Split(new char[] { '\n' });
                    cursorLocation.X += lines[lines.Length - 1].Length;

                    UpdateScrolling();
                }
            }
        }

        private void vScrollbar_OnChangeValue(object obj, EventArgs e)
        {
            scrollOffset.Y = vscrollbar.Value * Font.LineSpacing;
            hasFocus = true;
        }

        private void hScrollbar_OnChangeValue(object obj, EventArgs e)
        {
            if (multiline)
            {
                scrollOffset.X = hscrollbar.Value;
                hasFocus = true;
            }
        }

        private void UpdateScrollPosition(object obj, EventArgs e)
        {
            scrollOffset.Y = vscrollbar.Value * Font.LineSpacing;
        }

        public override void Update()
        {
            base.Update();
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
            // TODO: Add your update logic here
            CheckFocus();

            if (hasFocus && !locked)
            {
                keyboard.Update();
                
                if (multiline && scrollbar == Scrollbars.Vertical && cursorLocation.X > line[cursorLocation.Y].Length)
                    cursorLocation.X = line[cursorLocation.Y].Length;
            }

            if (multiline && scrollbar != Scrollbars.None)
            {
                //UpdateScrolling();
                UpdateScrollbars();
            }

            if (Owner.focus == vscrollbar)
                Owner.focus = this;
        }

        private void UpdateScrollbars()
        {
            if (vscrollbar != null)
            {
                if (hscrollbar != null && hscrollbar.Max > 0)
                    vscrollbar.Max = System.Math.Max(0, line.Count - (visibleLines - 1));
                else
                    vscrollbar.Max = System.Math.Max(0, line.Count - visibleLines);
            }


            if (vscrollbar != null && vscrollbar.Max > 0)
            {
                if (hscrollbar != null && hscrollbar.Max > 0)
                    vscrollbar.Height = Height - 13;
                else
                    vscrollbar.Height = Height - 2;

                vscrollbar.Update();
            }

            if (hscrollbar != null && hscrollbar.Max > 0)
            {
                if (vscrollbar != null && vscrollbar.Max > 0)
                    hscrollbar.Width = Width - 13;
                else
                    hscrollbar.Width = Width - 2;

                hscrollbar.Update();
            }
        }

        Rectangle focusArea = Rectangle.Empty;
        private void CheckFocus()
        {

            if (wasPressed && Owner.area.Contains(mNew.X, mNew.Y))
            {
                if (a2)
                    Owner.focus=this;
                
            }
            if (Owner.focus == this)
                    hasFocus = true;
                else
                    hasFocus = false;
            
        }

        private void timer_Elapsed(object obj, EventArgs e)
        {
            bCursorVisible = !bCursorVisible;
        }

        private void UpdateScrolling()
        {
            if (cursorOffset.X > scrollOffset.X + (sRect.Width - 20))
                scrollOffset.X = cursorOffset.X - (sRect.Width - 20);
            else if (cursorOffset.X - 20 < scrollOffset.X)
                scrollOffset.X = System.Math.Max(0, cursorOffset.X - 20);

            if (scrollOffset.X < 0f)
                scrollOffset.X = 0f;
            if (cursorLocation.X == 0)
                scrollOffset.X = 0;

            if(multiline)
            {
                #region horizontal

                //if (hscrollbar != null && scrollOffset.X > hscrollbar.Max)
                //    scrollOffset.X = hscrollbar.Max;

                if (hscrollbar != null)
                    hscrollbar.Value = (int)scrollOffset.X;

                #endregion

                #region vertical

                if (Font != null)
                {
                    int offsetY = (int)(scrollOffset.Y / Font.LineSpacing);

                    if (hscrollbar != null && hscrollbar.Max > 0)
                    {
                        if (cursorLocation.Y > offsetY + (visibleLines - 2))
                            scrollOffset.Y = (cursorLocation.Y - (visibleLines - 2)) * Font.LineSpacing;
                        else if (cursorLocation.Y < offsetY)
                            scrollOffset.Y = cursorLocation.Y * Font.LineSpacing;
                    }
                    else
                    {
                        if (cursorLocation.Y > offsetY + (visibleLines - 1))
                            scrollOffset.Y = (cursorLocation.Y - (visibleLines - 1)) * Font.LineSpacing;
                        else if (cursorLocation.Y < offsetY)
                            scrollOffset.Y = cursorLocation.Y * Font.LineSpacing;
                    }

                    if (vscrollbar != null)
                        vscrollbar.Value = (int)(scrollOffset.Y / Font.LineSpacing);
                }

                #endregion
            }
        }

        //private void Render(GraphicsDevice graphics)
        //{
        //    graphics.SetRenderTarget(0, renderTarget);

        //    // TODO: Add your code to draw to the render target here.
        //    RenderText(graphics);

        //    graphics.SetRenderTarget(0, null);
        //    capturedTexture = renderTarget.GetTexture();
        //    //graphics.Clear(Color.TransparentBlack);
        //}

        private void RenderText()
        {
            //graphics.Clear(Color.TransparentBlack);
            spriteBatch.End();
            //SpriteBatch spb = new SpriteBatch(graphics);
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width), (int)(Height));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            
            if (!multiline)
            {
                cursorOffset.X = Font.MeasureString(Text.Substring(0, cursorLocation.X)).X + 2;
                cursorOffset.Y = Height / 2 - Font.MeasureString(" ").Y/2-1;
                spriteBatch.DrawString(Font, Text, textOffset - scrollOffset + Position, Color.Black);
                if (hasFocus && !locked && bCursorVisible)
                    spriteBatch.DrawString(cursorFont, "|", cursorOffset - scrollOffset + Position, Color.Black);
            
   
            }
            
            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
        }

        public override void Draw()
        {
            // TODO: Add your drawing code here

            
            if (!multiline)
                DrawMonoline();
            //DrawText();
                //DrawMultiline();
            RenderText();

            //Adjust render source rectangle depending on scrollbars visibility
            if (vscrollbar != null && vscrollbar.Max > 0)
                sRect.Width = (int)Width - 14;
            else
                sRect.Width = (int)Width - 2;

            if (hscrollbar != null && hscrollbar.Max > 0)
                sRect.Height = (int)Height - 14;
            else
                sRect.Height = (int)Height - 2;

            if (vscrollbar != null && vscrollbar.Max > 0)
                vscrollbar.Draw();
            if (hscrollbar != null && hscrollbar.Max > 0)
                hscrollbar.Draw();
        }

        private void DrawMonoline()
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
        }
        //private void DrawMultiline()
        //{
        //    destRect[0].X = (int)Position.X;
        //    destRect[0].Y = (int)Position.Y;
        //    spriteBatch.Draw(texture, destRect[0], srcRect[0], Color.White);
        //    destRect[1].X = destRect[0].X + destRect[0].Width;
        //    destRect[1].Y = destRect[0].Y;
        //    spriteBatch.Draw(texture, destRect[1], srcRect[1], Color.White);
        //    destRect[2].X = destRect[1].X + destRect[1].Width;
        //    destRect[2].Y = destRect[0].Y;
        //    spriteBatch.Draw(texture, destRect[2], srcRect[2], Color.White);

        //    destRect[3].X = destRect[0].X;
        //    destRect[3].Y = destRect[0].Y + destRect[0].Height;
        //    spriteBatch.Draw(texture, destRect[3], srcRect[3], Color.White);
        //    destRect[4].X = destRect[1].X;
        //    destRect[4].Y = destRect[0].Y + destRect[0].Height;
        //    spriteBatch.Draw(texture, destRect[4], srcRect[4], Color.White);
        //    destRect[5].X = destRect[2].X;
        //    destRect[5].Y = destRect[0].Y + destRect[0].Height;
        //    spriteBatch.Draw(texture, destRect[5], srcRect[5], Color.White);

        //    destRect[6].X = destRect[0].X;
        //    destRect[6].Y = destRect[3].Y + destRect[3].Height;
        //    spriteBatch.Draw(texture, destRect[6], srcRect[6], Color.White);
        //    destRect[7].X = destRect[1].X;
        //    destRect[7].Y = destRect[4].Y + destRect[4].Height;
        //    spriteBatch.Draw(texture, destRect[7], srcRect[7], Color.White);
        //    destRect[8].X = destRect[2].X;
        //    destRect[8].Y = destRect[5].Y + destRect[5].Height;
        //    spriteBatch.Draw(texture, destRect[8], srcRect[8], Color.White);
        //}
    }
}
