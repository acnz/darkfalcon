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
using System.IO;
using DarkFalcon.df;
using DarkFalcon.gui.help;


namespace DarkFalcon.gui
{
    /// <summary>
    /// A game component, inherits to Clickable.
    /// Has associated On and Off content.
    /// Has a state of IsChecked that is switched by click.
    /// Draws content according to state.
    /// </summary>
    public class _Holder : _Control
    {
        #region Fields

        Vector2 origin = Vector2.Zero;


        bool bMouseOver = false;
        bool bMouseDown = false;
        bool zoom = false;

        Texture2D pixel, back, label;
        Texture2D[] arrows;
        Color[] arrowsc;
        Texture2D grid4, grid9,grid,dropbox;
        Texture2D[] tex;
        RenderTarget2D texBuffer;
        bool redrawG = false;

        dfCom selected;
        dfCom dropItem;

        int zselected, zselected2;
        bool zdefined,canglow = false;

        Rectangle[] dest;
        Rectangle[] zdest4;
        Rectangle[] zdest9;

        Rectangle zoomdest;
        Rectangle dropArea;

        _Panel Panel;
        _Listflow lf;
        _Listbox lout;
        dfPC pc;
        dfChecker ch;
        OuterGlow o;
        Texture2D outerglow;

        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _Holder(hud pai, string nome, _Panel pan, _Listbox listOut, _Listflow flow, dfPC PC)
            : base(pai, nome, pan.Position)
        {
            Name = nome;
            Position = pan.Position;
            Size = new Vector2(pan.Width, pan.Height);
            Panel = pan;
            lf = flow;
            lout = listOut;
            pc = PC;
        }


        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            area.X = (int)(Position.X);
            area.Y = (int)(Position.Y);
            area.Width = (int)Size.X;
            area.Height = (int)Size.Y;
            pixel = new Texture2D(graphics, 1, 1, 1, TextureUsage.None, graphics.PresentationParameters.BackBufferFormat);
            pixel.SetData<Color>(new Color[] { Color.White });

            arrows = new Texture2D[10];
            arrowsc = new Color[10];

            back = Texture2D.FromFile(graphics, @"gui\holder\background.png");
            label = Texture2D.FromFile(graphics, @"gui\holder\label.png");
            arrows[0] = Texture2D.FromFile(graphics, @"gui\holder\mobopro.png");
            arrows[1] = Texture2D.FromFile(graphics, @"gui\holder\mobofonte.png");
            arrows[2] = Texture2D.FromFile(graphics, @"gui\holder\mobogab.png");
            arrows[3] = Texture2D.FromFile(graphics, @"gui\holder\mobomouse.png");
            arrows[4] = Texture2D.FromFile(graphics, @"gui\holder\moboteclado.png");
            arrows[5] = Texture2D.FromFile(graphics, @"gui\holder\mobomemo.png");
            arrows[6] = Texture2D.FromFile(graphics, @"gui\holder\moboperi.png");
            arrows[7] = Texture2D.FromFile(graphics, @"gui\holder\mobopla.png");
            arrows[8] = Texture2D.FromFile(graphics, @"gui\holder\mobomon.png");
            arrows[9] = Texture2D.FromFile(graphics, @"gui\holder\plavmon.png");

            grid4 = Texture2D.FromFile(graphics, @"gui\holder\zoomgrid4.png");
            grid9 = Texture2D.FromFile(graphics, @"gui\holder\zoomgrid9.png");
            grid = Texture2D.FromFile(graphics, @"gui\holder\zoom.png");
            dropbox = Texture2D.FromFile(graphics, @"gui\holder\dropbox.png");

            texBuffer = new RenderTarget2D(Owner.gra, 300, 300, 0, SurfaceFormat.Color,RenderTargetUsage.PlatformContents);
            tex = new Texture2D[10];
            SetTex();

            dest = new Rectangle[10];
            dest[0] = new Rectangle((int)(Position.X + 13), (int)(Position.Y + 217), 98, 98);
            dest[1] = new Rectangle((int)(Position.X + 13), (int)(Position.Y + 13), 98, 98);
            dest[2] = new Rectangle((int)(Position.X + 205), (int)(Position.Y + 13), 98, 98);
            dest[3] = new Rectangle((int)(Position.X + 205), (int)(Position.Y + 164), 98, 98);
            dest[4] = new Rectangle((int)(Position.X + 328), (int)(Position.Y + 13), 98, 98);
            dest[5] = new Rectangle((int)(Position.X + 328), (int)(Position.Y + 165), 98, 98);
            dest[6] = new Rectangle((int)(Position.X + 443), (int)(Position.Y + 91), 98, 98);
            dest[7] = new Rectangle((int)(Position.X + 584), (int)(Position.Y + 14), 98, 98);
            dest[8] = new Rectangle((int)(Position.X + 584), (int)(Position.Y + 217), 98, 98);
            dest[9] = new Rectangle((int)(Position.X + 692), (int)(Position.Y + 114), 98, 98);

            zoomdest = new Rectangle((int)(18+Position.X), (int)(9+Position.Y), 300, 300);
            int pass = 0;
            zdest4 = new Rectangle[4];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    zdest4[pass] = new Rectangle(150 * j + zoomdest.X, 150 * i + zoomdest.Y, 150, 150);
                    pass++;
                }
            pass = 0;
            zdest9 = new Rectangle[9];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    zdest9[pass] = new Rectangle(100 * j + zoomdest.X, 100 * i + zoomdest.Y, 100, 100);
                    pass++;
                }
                    

                    lf.DragStop += new EventHandler(GetDrop);

            SetCompat();
            outerglow = Texture2D.FromFile(graphics, @"gui\listflow\outerglow.png");
            o = new OuterGlow(outerglow, spriteBatch);

            ch = new dfChecker();
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
        int Tstatic = 0;
        public override void Update()
        {
            base.Update();

            if (mNew.X != mOld.X || mNew.Y != mOld.Y)
                Tstatic = 0;
            else
                Tstatic++;

            UpdateEvents();
        }
        private void UpdateEvents()
        {
            if (Owner != null && a1 && a2)
            {
                if (!bMouseOver)
                {
                    bMouseOver = true;
                    if (OnMouseOver != null)
                        OnMouseOver(this, null);
                }

                if (!bMouseDown && wasPressed)
                {
                    bMouseDown = true;
                    if (OnPress != null)
                    {
                        OnPress(this, null);
                        Owner.focus = this;
                    }
                }
                else if (bMouseDown && wasReleased)
                {
                    bMouseDown = false;
                    if (OnRelease != null)
                        OnRelease(this, null);
                }
            }
            else if (bMouseOver)
            {
                bMouseOver = false;
                bMouseDown = false;
                if (OnMouseOut != null)
                    OnMouseOut(this, null);
            }
            if (!zoom)
            {
                for (int i = 0; i < 10; i++)
                    if (dest[i].Contains(mNew.X, mNew.Y))
                    {
                        if (wasPressed || (Tstatic == 30 && lf.isDraging))
                        {
                            Console.Out.WriteLine(i);

                            zselected = i;
                            zoom = true;
                        }
                    }
            }else
            {
                if (zselected < 6)
                {
                    zdefined = true;
                    dropArea = zoomdest;
                }
                if (zoomdest.Contains(mNew.X, mNew.Y))
                {
                    if (zselected >= 6)
                    {
                        int n = getItem2(zselected).GetAll().Count;
                        if (n == 1)
                        {
                            zdefined = true;
                            zselected2 = 0;
                            dropArea = zoomdest;
                        }
                        else if (n <= 4)
                        {
                            for (int i = 0; i < 4; i++)
                                if (zdest4[i].Contains(mNew.X, mNew.Y))
                                {
                                    zdefined = true;
                                    zselected2 = i;
                                    dropArea = zdest4[i];
                                }
                        }
                        else
                        {
                            for (int i = 0; i < 9; i++)
                                if (zdest9[i].Contains(mNew.X, mNew.Y))
                                {
                                    zdefined = true;
                                    zselected2 = i;
                                    dropArea = zdest9[i];
                                }
                        }
                    }

                }
                else
                    if (a2 && wasPressed) { zoom = false; zdefined = false; }         
            }
            if (zdefined)
            {
                
                if (dropArea.Contains(mNew.X, mNew.Y))
                {
                    if (zselected < 6)
                    {
                        if(wasPressed){
                        selected = getItem(zselected);
                        Owner.msgb.Show(new EventHandler(RemoveSelected), "Deseja realmente remover este componente?", _MsgBox.Type.YesNo);}
                        canglow = true;
                    }
                    else
                    {
                        List<dfCom> dl = getItem2(zselected).GetAll();
                        if (zselected2 < dl.Count)
                        {
                            if (wasPressed)
                            {
                                selected = getItem2(zselected).GetAll()[zselected2];
                                if(selected.ID !="x")
                                Owner.msgb.Show(new EventHandler(RemoveSelected), "Deseja realmente remover este componente?", _MsgBox.Type.YesNo);
                            }
                            canglow = true;
                        }
                    }
                }
            }else{
                canglow = false;
            }
        }

        private dfIObG getItem2(int i)
        {
            List<dfIObG> l = pc.getMCom();
            switch(i)
            {
                case 6:
                    return l[0];
                case 7:
                    return l[1];
                case 8:
                    return l[3];
                case 9:
                    return l[2];
                default:
                    return null;
            }
        }

        private dfCom getItem(int i)
        {
            switch (i)
            {
                case 0:
                    return pc.Motherboard;
                case 1:
                    return pc.Processador;
                case 2:
                    return pc.Fonte;
                case 3:
                    return pc.Gabinete;
                case 4:
                    return pc.Mouse;
                case 5:
                    return pc.Teclado;
                default:
                    return null;
            }
        }

        public void RemoveSelected(object sender, EventArgs e)
        {
            string r = (string)sender;

            if (r == "yes")
            {
                string output = "";
                switch (selected.Tipo)
                {
                    case "Motherboard":
                        output =pc.replace(new dfMobo(true), selected);
                        break;
                    case "Mouse":
                        output =pc.replace(new dfCom("Mouse", "$ps2",true), selected);
                        break;
                    case "Teclado": 
                        output =pc.replace(new dfCom("Teclado", "$ps2", true), selected);
                        break;
                    default:
                        if (selected.Tipo.Contains("Pla"))
                        {
                            output = pc.replace(new dfCom("Pla"), selected);
                        }else
                        output = pc.replace(new dfCom(selected.Tipo), selected);
                        break;
                        
                }
                if (output == "ok") output = selected.Tipo + " \"" + selected.Nome + "\" foi removido com sucesso!";
                Owner.info.Add(output);
                SetTex();
            }
            SetCompat(); 
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw()
        {

            spriteBatch.Draw(back, area, Color.White);



                spriteBatch.End();
                //spriteBatch.GraphicsDevice.ScissorRectangle = area;
            if(redrawG)
                drawG();

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            
            drawBoxes();
            drawArrows();

            spriteBatch.Draw(label, area, Color.White);

            if (zoom)
                zoomBox();
        }
        private void drawArrows()
        {
            for (int i = 0; i<10;i++)
                spriteBatch.Draw(arrows[i], area, arrowsc[i]);
        }
        private void drawG()
        {
            List<dfIObG> list2 = pc.getMCom();
            foreach (dfIObG g in list2)
            {
                List<dfCom> l = g.GetAll();

                if (l.Count == 1)
                {
                    try
                    {
                        tex[findType(g)] = Owner.con.Load<Texture2D>(l[0].LocalImagem2D);

                    }
                    catch { }
                }
                else if (l.Count <= 4)
                {
                    
                    Owner.gra.SetRenderTarget(0, texBuffer);
                    Owner.gra.Clear(Color.LightGray);
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    Texture2D temp;
                    int pass = 0;
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                        {
                            temp = pixel;
                            Color c;
                            try
                            {
                                temp = Owner.con.Load<Texture2D>(l[pass].LocalImagem2D);
                                c = Color.White;
                            }
                            catch { c = new Color(Color.White, 0f); }
                            
                            spriteBatch.Draw(temp, new Rectangle(150 * j, 150 * i, 150, 150), c);
                            pass++;
                        }
                    spriteBatch.Draw(grid4, new Rectangle(0,0, 300, 300), Color.White);
                    spriteBatch.End();
                    Owner.gra.SetRenderTarget(0, null);
                    try
                    {
                        texBuffer.GetTexture().Save(@"gui\holder\"+g.ToString()+".png",ImageFileFormat.Png);
                        tex[findType(g)] = Texture2D.FromFile(Owner.gra, @"gui\holder\" + g.ToString() + ".png");
                        File.Delete(@"gui\holder\" + g.ToString() + ".png");
                    }
                    catch { }
                
                }
                else
                {
                    Owner.gra.SetRenderTarget(0, texBuffer);
                    Owner.gra.Clear(Color.LightGray);
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    Texture2D temp;
                    int pass = 0;
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            Color c;
                            temp = pixel;
                            try
                            {
                                temp = Owner.con.Load<Texture2D>(l[pass].LocalImagem2D);
                                c = Color.White;
                            }
                            catch { c = new Color(Color.White, 0f); }
                            spriteBatch.Draw(temp, new Rectangle(100 * j , 100 * i, 100, 100), new Rectangle(0, 0, 300, 300), c);
                            pass++;
                        }
                    spriteBatch.Draw(grid9, new Rectangle(0, 0, 300, 300), Color.White);
                    spriteBatch.End();
                    Owner.gra.SetRenderTarget(0, null);
                    try
                    {
                        texBuffer.GetTexture().Save(@"gui\holder\" + g.ToString() + ".png", ImageFileFormat.Png);
                        tex[findType(g)] = Texture2D.FromFile(Owner.gra, @"gui\holder\" + g.ToString() + ".png");
                        File.Delete(@"gui\holder\" + g.ToString() + ".png");

                    }
                    catch { }

                }
            }
            redrawG = false;
        }
        private void drawBoxes()
        {
            for (int i = 0; i < 10;i++ )
                spriteBatch.Draw(tex[i], dest[i], Color.White);

            //spriteBatch.Draw(tex[6], Vector2.Zero, Color.White);
        }
        private void zoomBox()
        {

            spriteBatch.Draw(pixel, this.area, new Color(0, 0, 0, 0.8f));

            spriteBatch.Draw(tex[zselected], zoomdest, Color.White);

            spriteBatch.Draw(grid, area, Color.White);

            if (canglow) o.Draw(dropArea, Color.Red);

        }

        private void SetTex()
        {
            List<dfCom> list = pc.getSCom();
            for (int i = 0; i < 10; i++)
            {
                tex[i] = pixel;
            }
                foreach (dfCom d in list)
                {
                    try
                    {
                        tex[findType(d)] = Owner.con.Load<Texture2D>(d.LocalImagem2D);
                    }
                    catch { }

                }

                redrawG = true;
            
        }
        private int findType(dfCom d)
        {
            switch (d.Tipo)
            {
                case "Motherboard":
                    return 0;
                case "Processador":
                    return 1;
                case "Fonte":
                    return 2;
                case "Gabinete":
                    return 3;
                case "Mouse":
                    return 4;
                case "Teclado":
                    return 5;
                default:
                    return -1;

            }

        #endregion
        }
        private int findType(dfIObG g)
        {
            if (g.GetType() == typeof(dfMem))
                return 6;
            else if (g.GetType() == typeof(dfDados))
                return 7;
            else if (g.GetType() == typeof(dfPlaca))
                return 8;
            else if (g.GetType() == typeof(dfMon))
                return 9;
            else
                return -1;
        }

        private string ft(int i)
        {
            switch (i)
            {
                case 0:
                    return "Motherboard";
                case 1:
                    return "Processador";
                case 2:
                    return "Fonte";
                case 3:
                    return "Gabinete";
                case 4:
                    return "Mouse";
                case 5:
                    return "Teclado";
                case 6:
                    return "Memoria";
                case 7:
                    return "HD";
                case 8:
                    return "Placa";
                case 9:
                    return "Monitor";
                default:
                    return "nada";
            }
        }

        private void SetCompat()
        {
            Color t =  new Color(Color.DarkGray,0.5f);
            Color r =  Color.Red;
            Color g =  Color.Green;
            if (pc.Motherboard.Nome == "?")
            {

                for (int i = 0; i < 9; i++)
                    arrowsc[i] = t;
            }
            else
            {
                lout.Clear();
                string m = "";
                if (pc.Processador.Nome == "?") arrowsc[0] = t;
                else if ((m = ch.v(pc.Processador, pc.Motherboard)) == "ok") arrowsc[0] = g;
                else { lout.Add(m); arrowsc[0] = r; }
                m = "";
                if (pc.Fonte.Nome == "?") arrowsc[1] = t;
                else if ((m = ch.v(pc.Fonte, pc.Motherboard)) == "ok") arrowsc[1] = g; else { lout.Add(m); arrowsc[1] = r; }
                m = "";
                if (pc.Gabinete.Nome == "?") arrowsc[2] = t;
                else if ((m = ch.v(pc.Gabinete, pc.Motherboard)) == "ok") arrowsc[2] = g; else { lout.Add(m); arrowsc[2] = r; }
                m = "";
                if (pc.Mouse.Nome == "?") arrowsc[3] = t;
                else if ((m = ch.v(pc.Mouse, pc.Motherboard)) == "ok") arrowsc[3] = g; else { lout.Add(m); arrowsc[3] = r; }
                m = "";
                if (pc.Teclado.Nome == "?") arrowsc[4] = t;
                else if ((m = ch.v(pc.Teclado, pc.Motherboard)) == "ok") arrowsc[4] = g; else { lout.Add(m); arrowsc[4] = r; }
                m = "";
                bool red = false;
                bool color = false;
                foreach (dfCom d in pc.Memoria.GetAll())
                {
                    m = "";
                    if (d.Nome == "?") { if (!color) arrowsc[5] = t; }
                    else if ((m = ch.v(d, pc.Motherboard)) == "ok") { if (!red) { arrowsc[5] = g; color = true; } }
                    else { lout.Add(m); arrowsc[5] = r; red = true; color = true; }
                }
                red = false;
                color = false;
                foreach (dfCom d in pc.Dados.GetAll())
                {
                    m = "";
                    if (d.Nome == "?") { if (!color)  arrowsc[6] = t; }
                    else if ((m = ch.v(d, pc.Motherboard)) == "ok") { if (!red){ arrowsc[6] = g; color = true; }}
                    else { lout.Add(m); arrowsc[6] = r; red = true;color = true;  }
                }
                red = false;
                color = false;
                foreach (dfCom d in pc.Placas.GetAll())
                {
                    m = "";
                    if (d.Nome == "?") { if (!color)  arrowsc[7] = t; }
                    else if ((m = ch.v(d, pc.Motherboard)) == "ok") { if (!red){ arrowsc[7] = g; color = true; }}
                    else { lout.Add(m); arrowsc[7] = r; red = true; color = true; }
                }
                if (pc.numMon == 0)
                {
                    m = "";
                    if (pc.Mon.Nome == "?") arrowsc[8] = t;
                    else if ((m = ch.v(pc.Mon, pc.Motherboard)) == "ok") { if (!red) arrowsc[8] = g; }
                    else { lout.Add(m); arrowsc[8] = r; }
                    arrowsc[9] = t;

                }
                else {
                    red = false;
                    color = false;
                    foreach (dfCom d in pc.Monitor.GetAll())
                    {
                        m = "";
                        if (d.Nome == "?") { if (!color)  arrowsc[9] = t; }
                        else if ((m = ch.v(d, pc.PlaVideo)) == "ok") { if (!red) { arrowsc[9] = g; color = true; } }
                        else { lout.Add(m); arrowsc[9] = r; red = true; color = true; }
                    }
                    arrowsc[8] = t;
                }

            }

        }
        public void GetDrop(object sender, EventArgs e)
        {
            dfCom d = (dfCom)(((object[])sender)[0]);
            MouseState m = (MouseState)(((object[])sender)[1]);
            string r = "";
            if (!zoom)
            {
                for (int i = 0; i < 10; i++)
                    if (dest[i].Contains(m.X, m.Y))
                    {
                        string args = ft(i);
                        if (args == "Placa") args = "Pla";
                        if (d.Tipo.Contains(args))
                        {
                            r = pc.add(d);
                            if (r.Contains("Você já escolheu"))
                            {
                                zselected = i;
                                dropItem = d;
                                selected = getItem(zselected);
                                Owner.msgb.Show(new EventHandler(substSelected), "Deseja substituir este componente?", _MsgBox.Type.YesNo);
                            }
                        }
                        else
                            r = d.Nome + " não é um(a) " + ft(i);
                    }
                if (dest[7].Contains(m.X, m.Y))
                {
                    if (d.Tipo.Contains("Leitor"))
                        r = pc.add(d);
                    else
                        if (!d.Tipo.Contains("HD"))
                            r = d.Nome + " não é um Leitor";
                }
                SetTex();
                if (r == "ok") r = d.Tipo + " \"" + d.Nome + "\" foi adicionado com sucesso!";

                if(r != "")
                Owner.info.Add(r);
            }
            else
            {
                if (zdefined)
                {

                    if (dropArea.Contains(mNew.X, mNew.Y))
                    {
                        if (zselected < 6)
                        {
                            dropItem = d;
                                selected = getItem(zselected);
                                if (selected.ID != "x")
                                    Owner.msgb.Show(new EventHandler(substSelected), "Deseja realmente substituir este componente?", _MsgBox.Type.YesNo);
                                else
                                {

                                    string args = ft(zselected);
                                    if (args == "Placa") args = "Pla";
                                    if (d.Tipo.Contains(args))
                                    {
                                        r = pc.add(d);
                                        if (r == "ok") r = d.Tipo + " \"" + d.Nome + "\" foi adicionado com sucesso!";
                                        Owner.info.Add(r);
                                        
                                    }
                                    else
                                        r = d.Nome + " não é um(a) " + ft(zselected);
                                    if (zselected == 7)
                                    {
                                        if (d.Tipo.Contains("Leitor"))
                                            r = pc.add(d);
                                        else
                                            if (!d.Tipo.Contains("HD"))
                                                r = d.Nome + " não é um Leitor";
                                    }
                                    SetTex();

                                    Owner.info.Add(r);
                                }
                        }
                        else
                        {
                            List<dfCom> dl = getItem2(zselected).GetAll();
                            if (zselected2 < dl.Count)
                            {
                                dropItem = d;
                                    selected = getItem2(zselected).GetAll()[zselected2];
                                    if (selected.ID != "x")
                                        Owner.msgb.Show(new EventHandler(substSelected), "Deseja realmente substituir este componente?", _MsgBox.Type.YesNo);
                                    else
                                    {
                                        string args = ft(zselected);
                                        if (args == "Placa") args = "Pla";
                                        if (d.Tipo.Contains(args))
                                        {
                                            r = pc.add(d);
                                            if (r == "ok") r = d.Tipo + " \"" + d.Nome + "\" foi adicionado com sucesso!";
                                            Owner.info.Add(r);
                                            
                                        }
                                        else
                                            r = d.Nome + " não é um(a) " + ft(zselected);
                                        if (zselected == 7)
                                        {
                                            if (d.Tipo.Contains("Leitor"))
                                                r = pc.add(d);
                                            else
                                                if (!d.Tipo.Contains("HD"))
                                                    r = d.Nome + " não é um Leitor";
                                        }
                                        SetTex();
                                        if (r != "")
                                        Owner.info.Add(r);
                                    }
                            }
                        }
                    }
                }
            }
            SetCompat();
        }
        public void substSelected(object sender, EventArgs e)
        {
            string r = (string)sender;

            if (r == "yes")
            {
                string output = "";
              
                            output = pc.replace(dropItem, selected);
                       
                if (output == "ok") output = selected.Tipo + " \"" + selected.Nome + "\" foi substituido com sucesso!";
                Owner.info.Add(output);
                SetTex();
            }
            SetCompat();
        }
    }
}
