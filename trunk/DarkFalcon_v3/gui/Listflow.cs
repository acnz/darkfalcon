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

        Rectangle selectionRect;
        int hoverIndex = -1;
        int selectedIndex = -1;
        dfCom selectedItem;

        Texture2D blackTex;
        Texture2D[] tex;
        Texture2D[] tex3;
        Rectangle dest;
        Rectangle src;
        Rectangle sbarRec;
        Effect reflex,skew;
        RenderTarget2D drawBuffer;
        RenderTarget2D texBuffer;
        VertexPositionTexture[] vertices = new VertexPositionTexture[4];
        Quad quad;
         short[] Indexes;
        BasicEffect basicEffect;

        public List<dfCom> Items
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
        bool bMouseOver = false;

        _Scrollbar scrollbar;
        int ScrollWidth = 0;

        public _Listflow(hud pai, string name, Vector2 position, int width, dfCom[] items)
            : base(pai,name,position)
        {
            // TODO: Construct any child components here
            this.Width = width;
            if (items != null)
                for (int i = 0; i < items.Length; i++)
                    this.items.Add(items[i]);
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
            visibleItems = new dfCom[11];
            tex = new Texture2D[11];
            tex3 = new Texture2D[11];
            reflex = content.Load<Effect>("Effects/reflect");

            blackTex = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            blackTex.SetData<Color>(new Color[] { Color.White });

            tex[6] = content.Load<Texture2D>("Textures/Motherboard/001");

            Height = Width / 2;
          
            for (int i = 0; i < Math.Min(items.Count,11); i++)
                visibleItems[i] = items[i];
            for (int i = 0; i < Math.Min(items.Count, 11); i++)
                tex[i] = content.Load<Texture2D>(items[i].LocalImagem2D);

            
            sbarRec = new Rectangle((int)(Position.X + 4), (int)(Position.Y + (4 * Height / 5)), (int)Width - 8, (int)Height / 5 -4);
            int square = (int)((area.Height - sbarRec.Height) * 0.9f);
            int x = (int)(area.Width / 2 - square / 2);
            int y = (int)((area.Height - sbarRec.Height) / 2 - square / 2);
            
           // dests[0] = new Rectangle((int)(x + Position.X), (int)(y + Position.Y), square, square);
            dest = new Rectangle(0, 0, square, square);
            src = new Rectangle((int)(graphics.Viewport.Width / 2 - 150), (int)(graphics.Viewport.Height / 2 - 150), 300, 300);
            area.Width = (int)Width;
            area.Height = (int)Height;
            PresentationParameters pp = Owner.gra.PresentationParameters;
           drawBuffer = new RenderTarget2D(Owner.gra, 
Owner.gra.Viewport.Width, Owner.gra.Viewport.Height, 1,
SurfaceFormat.Color);
           texBuffer = new RenderTarget2D(Owner.gra,
300, 300, 1,
SurfaceFormat.Color);

           scrollbar = new _Scrollbar(Owner, "scrollbar", new Vector2(sbarRec.X, sbarRec.Y+sbarRec.Height/2), _Scrollbar.Type.Horizontal, sbarRec.Width, this.area);

            quad = new Quad(Owner.gra, 300, 300);
            scrollbar.Initialize(content, graphics);


            
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
                selectedIndex = hoverIndex;
                if (OnSelect != null)
                    OnSelect(items[selectedIndex], null);
                if (selectedIndex != previousIndex && OnChangeSelection != null)
                    OnChangeSelection(items[selectedIndex], null);
            }
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
            
            if (wtf == 10)
            {
                x++;
                wtf = 0;
            }
            base.Update();
            wtf++;
        }
        public override void Draw()
        {
            // TODO: Add your drawing code here
            DrawBackground();
            spriteBatch.End();
            //spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.ScissorRectangle = area;
            
           
            
            RenderReflex();
            Render();

            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            DrawScrollbar();
            
             
           // RenderText();
        }
        int x=0;
        float angle = 0;
        private void Render()
        {

            Owner.gra.SetRenderTarget(0, drawBuffer);
            Owner.gra.Clear(Color.TransparentBlack);
            quad.Draw(tex[6], Matrix.CreateRotationY(MathHelper.ToRadians(0.01f * 2)) * Matrix.CreateScale(.5f, 0.68f, 1f));
  Owner.gra.SetRenderTarget(0, null);

            tex3[6]= drawBuffer.GetTexture();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            spriteBatch.Draw(tex3[6],dest,src, Color.White);
            spriteBatch.End();
        }
        public void RenderReflex()
        {
            Owner.gra.SetRenderTarget(0, texBuffer);
            Owner.gra.Clear(Color.Black);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            reflex.Begin();

            reflex.CurrentTechnique.Passes[0].Begin();
            Owner.gra.Textures[0] = tex[6];

            spriteBatch.Draw(tex[6], new Rectangle(0,0, 300, 300), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically,0);

            reflex.CurrentTechnique.Passes[0].End();

            reflex.End();

            spriteBatch.End();

            Owner.gra.SetRenderTarget(0, drawBuffer);
            Owner.gra.Clear(Color.TransparentBlack);
            Texture2D sprite = texBuffer.GetTexture();
            quad.Draw(sprite, Matrix.CreateRotationY(MathHelper.ToRadians(0.01f * -2)) * Matrix.CreateScale(.5f, 0.68f, 1f));
            Owner.gra.SetRenderTarget(0, null);
            tex3[6] = drawBuffer.GetTexture();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            spriteBatch.Draw(tex3[6], new Rectangle((int)(dest.X - dest.Width / 7), (int)(dest.Y + 2 + 5*dest.Height/7), dest.Width, dest.Height), src, Color.White);
            spriteBatch.End();

        }
        private void DrawBackground()
        {
            spriteBatch.Draw(blackTex, area, Color.Black);
        }
        private void RenderText()
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = true;
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)Position.X, (int)Position.Y + 3, (int)(Width), (int)(Height - 8));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            hoverIndex = -1;
            for (int i = scrollbar.Value; i < System.Math.Min(items.Count, scrollbar.Value + visibleItems.Count() + 1); i++)
            {
                //textOffset.X = 2f;
               // textOffset.Y = (int)((i - scrollbar.Value) * Font.LineSpacing);

               // RenderSelection(i);

               // spriteBatch.DrawString(Font, items[i], textOffset + Position, Color.Black);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.RenderState.ScissorTestEnable = false;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

        }
        private void DrawScrollbar()
        {
            //spriteBatch.Draw(blackTex, sbarRec, Color.White);
            scrollbar.Max = (int)System.Math.Max(0, items.Count - visibleItems.Count());
            if (scrollbar.Value > scrollbar.Max)
                scrollbar.Value = scrollbar.Max;

                scrollbar.Draw();

        }
    }
}