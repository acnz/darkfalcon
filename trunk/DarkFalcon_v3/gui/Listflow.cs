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
using DarkFalcon.df;
using DarkFalcon.gui.help;


namespace DarkFalcon.gui
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class _Listflow : _Control

    {
        List<dfCom> items = new List<dfCom>();
        bool sorted = false;
        bool isSorted = false;
        dfCom[] visibleItems;

        int hoverIndex = -1;
        int selectedIndex = -1;

        Texture2D blackTex;
        Texture2D[] tex;

        public enum DragStyle 
        { 
        Normal,Rotate3D
        }

        DragStyle ds;

        _3DObject mod;

        Rectangle[] dest;
        int space;
        int _vision = 4;
        OuterGlow outer;
        Texture2D outerglow;
        Rectangle drgRec;
        Texture2D drgTex;
        bool drawDrag=false;

        Rectangle sbarRec;
        Effect reflex;
        RenderTarget2D drawBuffer;

        public List<dfCom> Items
        {
            get { return items; }
            set
            {
                items = value;
                updateVision(null,null);
                //InitScrollbars();
            }
        }
                public bool Sorted { get { return sorted; } set { sorted = value; } }
        public int SelectedIndex { get { return selectedIndex; } set { selectedIndex = value; } }
        public dfCom SelectedItem
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
        public EventHandler DragStart = null;
        public EventHandler DragMove = null;
        public EventHandler DragStop = null;
        bool bMouseOver = false;
        bool bMouseDown = false;
        public bool isDraging = false;

        _Scrollbar scrollbar;

        public _Listflow(hud pai, string name, Vector2 position, int width, dfCom[] items)
            : base(pai,name,position)
        {
            // TODO: Construct any child components here
            this.Width = width;
            this.Height = Width / 4;
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    this.items.Add(items[i]);
            ds = DragStyle.Normal;
        }
        public _Listflow(hud pai, string name, Vector2 position, int width,int height,DragStyle DragS, dfCom[] items)
            : base(pai, name, position)
        {
            // TODO: Construct any child components here
            this.Width = width;
            this.Height = height;
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    this.items.Add(items[i]);

            ds = DragS;
        }
        public dfCom this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
        public void Add(dfCom item)
        {
            items.Add(item);
        }
        public void Remove(dfCom item)
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

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            // TODO: Add your initialization code here
            base.Initialize(content,graphics);
            visibleItems = new dfCom[_vision];
            tex = new Texture2D[_vision];
            reflex = content.Load<Effect>("Effects/reflect");

            

            blackTex = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            blackTex.SetData<Color>(new Color[] { Color.White });

            //SetTex();


            for (int i = 0; i < Math.Min(items.Count, _vision); i++)
                visibleItems[i] = items[i];
            for (int i = 0; i < Math.Min(items.Count, _vision); i++)
                tex[i] = content.Load<Texture2D>(items[i].LocalImagem2D);

            
            sbarRec = new Rectangle((int)(Position.X + 4), (int)(Position.Y + (4 * Height / 5)), (int)Width - 8, (int)Height / 5 -4);
            int square = (int)((area.Height - sbarRec.Height) * 0.7f);
            int y = (int)((area.Height - sbarRec.Height) / 2 - square / 2);
            space = square + (((area.Width) - (square * visibleItems.Count())) / _vision);
            Rectangle d = new Rectangle((int)Position.X + (space - square) / 2, (int)(y + Position.Y), square, square);
            dest = new Rectangle[_vision];
            for (int i = 0; i < visibleItems.Count(); i++)
            {

                dest[i]=new Rectangle(d.X + space * i, d.Y, d.Width, d.Height);
            }
            //dest = new Rectangle(0, 0, square, square);
            area.Width = (int)Width;
            area.Height = (int)Height;
            PresentationParameters pp = Owner.gra.PresentationParameters;
           drawBuffer = new RenderTarget2D(Owner.gra,Owner.gra.Viewport.Width, Owner.gra.Viewport.Height, 1,SurfaceFormat.Color);


           scrollbar = new _Scrollbar(Owner, "scrollbar", new Vector2(sbarRec.X, sbarRec.Y+sbarRec.Height/2), _Scrollbar.Type.Horizontal, sbarRec.Width, this);
           
            scrollbar.Initialize(content, graphics);
            scrollbar.OnChangeValue += new EventHandler(updateVision);
            updateVision(null,null);
            outerglow = Texture2D.FromFile(graphics,@"gui\listflow\outerglow.png");
            outer = new OuterGlow(outerglow,spriteBatch);

            this.DragStart += new EventHandler(drgS);
            this.DragMove += new EventHandler(drgM);
            this.DragStop += new EventHandler(drgT);
        }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        Vector3 rotation = new Vector3(0, 0, 0);
        Vector3 t = new Vector3(0, 0, 0);
        int wtf = 0;
        public override void Update()
        {
            // TODO: Add your update code here
                scrollbar.Update();

            if (!isSorted && sorted)
                items.Sort();


            if (hoverIndex != -1 && wasPressed)
            {
                int previousIndex = selectedIndex;
                selectedIndex = scrollbar.Value + hoverIndex;
                if (OnSelect != null)
                    OnSelect(items[selectedIndex], null);
                if (selectedIndex != previousIndex && OnChangeSelection != null)
                    OnChangeSelection(items[selectedIndex], null);
            }
            if (bMouseDown && wasReleased)
            {
                bMouseDown = false;
            }
            if (area.Contains(mNew.X, mNew.Y))
            {
                if (!bMouseOver)
                {
                    bMouseOver = true;
                    if (OnMouseOver != null)
                        OnMouseOver(this, null);
                }

                if (!bMouseDown && wasPressed )
                {
                    bMouseDown = true;
                    if(OnPress != null)
                    OnPress(this, null);
                    Owner.focus = this;
                }
                else if (wasReleased)
                {
                    if (OnRelease != null)
                        OnRelease(this, null);
                }
            }
            else if (bMouseOver)
            {
                bMouseOver = false;
                if (OnMouseOut != null)
                    OnMouseOut(this, null);
            }

            for (int i = 0; i < visibleItems.Count(); i++)
            {
                if (isDraging && wasReleased)
                {
                    isDraging = false;

                        if (DragStop != null)
                            DragStop(new object[] { items[selectedIndex], mNew }, null);
                    
                }
                if (!wasPressed && bMouseDown && (mNew.X != mOld.X || mNew.Y != mOld.Y) && isDraging)
                {
                    if (DragMove != null)
                        DragMove(new object[] { items[selectedIndex], mNew }, null);
                }
                if (dest[i].Contains(mNew.X, mNew.Y))
                {
                    if (!wasPressed && bMouseDown && (mNew.X != mOld.X || mNew.Y != mOld.Y))
                    {
                        if (!isDraging)
                        {
                            if (selectedIndex != -1 && hoverIndex == selectedIndex - scrollbar.Value)
                            {
                                isDraging = true;
                                if (DragStart != null)
                                    DragStart(new object[] { items[selectedIndex], mNew }, null);
                            }
                        }
                        
                    }
                }

            }

                if (wtf == 10)
                {
                    
                    wtf = 0;
                }
            base.Update();
            wtf++;

            if (drgalp < 0.3f)
                flash = true;
            else
                if(!flash)
                drgalp -= 0.02f;

            if (flash)
            {
                drgalp += 0.02f;
                if (drgalp == 1) flash = false;
            }

        }
        public override void Draw()
        {
            // TODO: Add your drawing code here
            DrawBackground();
            spriteBatch.End();

            hoverIndex = -1;
            spriteBatch.GraphicsDevice.ScissorRectangle = area;

            RenderReflex();
            Render();
            
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            RenderOuterGlow();
            DrawScrollbar();
             
           RenderText();
           if (drawDrag)
               DrawDrag();
        }
        float drgalp = 1;
        bool flash=false;
        private void DrawDrag()
        {
            if (ds == DragStyle.Normal)
            {
                Color c = new Color(Color.White, drgalp);

                spriteBatch.Draw(drgTex, drgRec, c);
            }
            else
            {
                spriteBatch.End();

                Owner.gra.SetRenderTarget(0, drawBuffer);


                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            }
           
        }
        
        private void Render()
        {
            for (int i = 0; i < visibleItems.Count();i++ )
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                spriteBatch.Draw(tex[i], dest[i], Color.White);
                spriteBatch.End();
                if (dest[i].Contains(mNew.X, mNew.Y)) hoverIndex = i;
            }
        }
        public void RenderReflex()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            reflex.Begin();

            reflex.CurrentTechnique.Passes[0].Begin();
            for (int i = 0; i < visibleItems.Count(); i++)
            {
                spriteBatch.Draw(tex[i], new Rectangle(dest[i].X, dest[i].Y + dest[i].Height, dest[i].Width, dest[i].Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            }

            reflex.CurrentTechnique.Passes[0].End();

            reflex.End();

            spriteBatch.End();

        }
        public void RenderOuterGlow()
        {
            int i = selectedIndex - scrollbar.Value;
            if (i > -1 && i < visibleItems.Count())
                outer.Draw(dest[selectedIndex-scrollbar.Value],Color.Red);
            if (hoverIndex != -1)
                outer.Draw(dest[hoverIndex], Color.White);
        }

        private void DrawBackground()
        {
            Color c = new Color(Color.Black, 0.7f);
            spriteBatch.Draw(blackTex, area, c);
        }
        private void RenderText()
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)Position.X, (int)Position.Y + 3, (int)(Width), (int)(Height - 8));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            if (hoverIndex != -1)
            {
                Text = items[scrollbar.Value + hoverIndex].Nome;
            }
            else
            {
                if(selectedIndex != -1)
                Text = items[selectedIndex].Nome;
            }
            Vector2 m =  Font.MeasureString(Text);

            Vector2 Tpos = new Vector2((int)(Position.X + Width / 2 - m.X / 2), (int)sbarRec.Y);

            spriteBatch.DrawString(Font, Text, Tpos, Color.White);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            Text = "";

        }
        private void DrawScrollbar()
        {
            scrollbar.Max = (int)System.Math.Max(0, items.Count - visibleItems.Count());
            if (scrollbar.Value > scrollbar.Max)
                scrollbar.Value = scrollbar.Max;

                scrollbar.Draw();

        }

        public void updateVision(object sender, EventArgs e)
        {
            selectedIndex = -1; hoverIndex = -1;
            visibleItems = new dfCom[Math.Min(items.Count, _vision)];

            for (int i = 0; i < visibleItems.Count(); i++)
            {
                visibleItems[i] = items[i + scrollbar.Value];
                tex[i] = Owner.con.Load<Texture2D>(visibleItems[i].LocalImagem2D);
            }
        }

        public void newSearch(object sender, EventArgs e)
        {
            scrollbar.Value = 0;
        }
        public void drgS(object sender, EventArgs e)
        {
            drawDrag=true;
            drgRec = new Rectangle(dest[hoverIndex].X, dest[hoverIndex].Y, dest[hoverIndex].Width, dest[hoverIndex].Height);
            if(ds == DragStyle.Normal)
            drgTex = tex[hoverIndex];
        }
        public void drgM(object sender, EventArgs e)
        {
            drawDrag = true;
            drgRec.X = ((MouseState)((object[])sender)[1]).X;
            drgRec.Y = ((MouseState)((object[])sender)[1]).Y;
        }
        public void drgT(object sender, EventArgs e)
        {
            drawDrag = false;
        }
    }
}