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
using gma.System.Windows;
using System.Windows.Forms;
using DarkFalcon.df;
using DarkFalcon.gui;
using System.IO;

namespace DarkFalcon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PcView : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        SpriteFont font;
        public Texture2D background;
        public bool noTabs;
        Texture2D white;
        private string Out;
        public bool focus;


        dfPC _pc;

        hud _hud1,_hud2,_hud3;
        public SpriteFont hudf;
        List<_Control> listaHUD = new List<_Control>();
        public List<_Control> listaMenu = new List<_Control>();
        public List<_Checkbox> groupM = new List<_Checkbox>();
        public bool nmv = true; 
        private IntPtr drawSurface;
        Frm3D fm;
        public EventHandler onMWchange = null;
        public MouseState prevMouse;

        int _step = 1, _tstep = 1;

         public PcView(IntPtr drawSurface, Frm3D frm)
        {
            graphics = new GraphicsDeviceManager(this);
            string dir = Properties.Settings.Default.CRoot + "Data";
            Content.RootDirectory = dir;
            fm = frm;
            this.drawSurface = drawSurface;
            graphics.PreferredBackBufferWidth = fm.PicPcView.Width;
            graphics.PreferredBackBufferHeight = fm.PicPcView.Height;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(PcView_VisibleChanged);
            frm.MouseWheel += new MouseEventHandler(MouseWheel_Value); 
            Mouse.WindowHandle = this.Window.Handle;
        }
         public void MouseWheel_Value(object sender, MouseEventArgs e)
         {
             if (onMWchange != null)
                 onMWchange(e.Delta, null);
         }
         public System.Drawing.Bitmap getBmp(string img)
         {

             Texture2D texture = Content.Load<Texture2D>(img);

             // This only works for 32bbpARGB for the bitmap and Color for the texture, since these formats match.
             // Because they match, we can simply have Marshal copy over the data, otherwise we'd need to go over
             // each pixel and do the conversion manually (or through some trick I'm unaware off).

             byte[] textureData = new byte[4 * texture.Width * texture.Height];
             texture.GetData<byte>(textureData);

             System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
                            texture.Width, texture.Height,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb
                          );

             System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(
                            new System.Drawing.Rectangle(0, 0, texture.Width, texture.Height),
                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb
                          );

             IntPtr safePtr = bmpData.Scan0;
             System.Runtime.InteropServices.Marshal.Copy(textureData, 0, safePtr, textureData.Length);
             bmp.UnlockBits(bmpData);

             // just some test output
             //bmp.Save(@"c:\workbench\smile.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
             return bmp;

         }
         void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
         {
             e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
             e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
         }

         public void resize(Frm3D frm)
         {
             graphics.PreferredBackBufferWidth = fm.PicPcView.Width;
             graphics.PreferredBackBufferHeight = fm.PicPcView.Height;
             graphics.ApplyChanges();
         }

         private void PcView_VisibleChanged(object sender, EventArgs e)
         {
             if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
               System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
         }


         public void FixPosition()
         {
             User32.SetWindowPos((uint)this.Window.Handle, 0, fm.MdiParent.Location.X + fm.Location.X + 10, fm.MdiParent.Location.Y + fm.Location.Y + 52,
 graphics.PreferredBackBufferWidth,
 graphics.PreferredBackBufferHeight, 0);
             

         }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            FixPosition();
            base.Initialize();

            _pc = new dfPC(true);

            _hud1 = new hud(this);
            _hud1.Initialize(Content, graphics.GraphicsDevice);
            _hud2 = new hud(this);
            _hud2.Initialize(Content, graphics.GraphicsDevice);
            _hud3 = new hud(this);
            _hud3.Initialize(Content, graphics.GraphicsDevice);
            _hud2.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width, 0);
            _hud3.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width, 0);

            createHUD1();
            createHUD2();
            createHUD3();



        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial");
            hudf = Content.Load<SpriteFont>("hudf");
            background = Texture2D.FromFile(graphics.GraphicsDevice,"guisrc//background.jpg");
            white = new Texture2D(graphics.GraphicsDevice, 1, 1, 1, TextureUsage.None, graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);
            white.SetData<Color>(new Color[] { Color.White });

           // cam.Update(Matrix.CreateTranslation(monitor.Position + new Vector3(5, 0, -10)), MouseWheel);
            
          
            // TODO: use this.Content to load your game content here
        }

        void createHUD1()
        {
            _Listflow lf1 = new _Listflow(_hud1, "lf1", new Vector2(5, 5), (4 * graphics.GraphicsDevice.Viewport.Width / 5)-10, new dfCom[] { });
            _Panel pPesquisa = new _Panel(_hud1, "pPesquisa", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, 100), 100, graphics.GraphicsDevice.Viewport.Height / 4 - 2.5f, new _Panel.Anchor[] { _Panel.Anchor.D, _Panel.Anchor.C });
            
            float px = pPesquisa.Position.X;
            float py = pPesquisa.Position.Y;
            float pw = pPesquisa.area.Width;
            float ph = pPesquisa.area.Height;

            _Panel pFiltros = new _Panel(_hud1, "pFiltros", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, py + ph + 2.5f), 100, 2*graphics.GraphicsDevice.Viewport.Height / 4 - 2.5f, new _Panel.Anchor[] { _Panel.Anchor.D });

            float fx = pFiltros.Position.X;
            float fy = pFiltros.Position.Y;
            float fw = pFiltros.area.Width;
            float fh = pFiltros.area.Height;

            _Panel pTags = new _Panel(_hud1, "pTags", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, fy + fh + 2.5f), 100, graphics.GraphicsDevice.Viewport.Height / 4 - 5, new _Panel.Anchor[] { _Panel.Anchor.B, _Panel.Anchor.D });
            float tx = pTags.Position.X;
            float ty = pTags.Position.Y;
            float tw = pTags.area.Width;
            float th = pTags.area.Height;

            
            _Textbox tbSearch = new _Textbox(_hud1, "tbSearch", new Vector2(px + 10, py + 70), "Digite sua Pesquisa...", (int)(pw-20));
            _Button butSearch = new _Button(_hud1, "butSearch", "Pesquisar", new Rectangle((int)(px + 80), (int)(py + 110), 40, 20), "default");

            
            _ComboBox cb1 = new _ComboBox(_hud1, "cb1", new Vector2(fx + 10, fy + 100), 150, new string[] { "Motherboard" , "Processador","Memoria","HD","Gabinete","Monitor","Leitor","Fonte","Placa de Video","Outros"});
            _Label labcb1 = new _Label(_hud1, "labcb1", new Vector2(cb1.Position.X, cb1.Position.Y - 20), "Tipo", _Label.Align.Left);
            _ComboBox cb2 = new _ComboBox(_hud1, "cb2", new Vector2(fx + 10, fy + 170), 150, new string[] { });
            _Label labcb2 = new _Label(_hud1, "labcb2", new Vector2(cb2.Position.X, cb2.Position.Y - 20), "Fabricante", _Label.Align.Left);
            _ComboBox cb3 = new _ComboBox(_hud1, "cb3", new Vector2(fx + 10, fy + 240), 150, new string[] { });
            _Label labcb3 = new _Label(_hud1, "labcb3", new Vector2(cb3.Position.X, cb3.Position.Y - 20), "Filtro", _Label.Align.Left);

            _tagcloud tc = new _tagcloud(_hud1, "tc", new Vector2(tx, ty), tw, th, _pc);

            
            _Listbox lbError = new _Listbox(_hud1, "lbError", new Vector2(5, 5 * graphics.GraphicsDevice.Viewport.Height / 6), (int)pTags.X - 10, graphics.GraphicsDevice.Viewport.Height / 6-5, new string[] { });
            _Panel pHolder = new _Panel(_hud1, "pHolder", new Vector2(5, lf1.Y + lf1.Height + 5), (int)pTags.X - 10, lbError.Y - 10 - (lf1.Y + lf1.Height), new _Panel.Anchor[] { }, 0.4f);
            _Holder Holder = new _Holder(_hud1, "Holder", pHolder, lbError, lf1, _pc);
            _Button bNstep = new _Button(_hud1, "bNstep", "", new Vector2(graphics.GraphicsDevice.Viewport.Width - 183, 10), "nextstep");
            

            
            _hud1.add(pPesquisa);
            _hud1.add(pFiltros);
            _hud1.add(pTags);
            _hud1.add(lbError);
            _hud1.add(pHolder);
            _hud1.add(Holder);

            _hud1.add(tbSearch);
            _hud1.add(butSearch);

            _hud1.add(cb1);
            _hud1.add(labcb1);
            _hud1.add(cb2);
            _hud1.add(labcb2);
            _hud1.add(cb3);
            _hud1.add(labcb3);

            _hud1.add(tc);

            _hud1.add(lf1);
            _hud1.add(bNstep);

            tbSearch.OnRelease = new EventHandler(tbSearch_release);
            butSearch.OnRelease = new EventHandler(butSearch_release);
            cb1.OnSelectionChanged += new EventHandler(cb1_change);
            cb2.OnSelectionChanged += new EventHandler(cb2_change);
            cb3.OnSelectionChanged += new EventHandler(cb3_change);
            tc.onSelect += new EventHandler(tc_click);
            bNstep.OnRelease = new EventHandler(Ver_step1);
        }
        void createHUD2()
        {
            _Panel pTeste = new _Panel(_hud2, "pTeste", new Vector2(100, 100), 100, 100, new _Panel.Anchor[] { _Panel.Anchor.D, _Panel.Anchor.E, _Panel.Anchor.C, _Panel.Anchor.B });
            _Button bNstep = new _Button(_hud2, "bNstep", "", new Vector2(graphics.GraphicsDevice.Viewport.Width - 183, 10), "nextstep");
            _Button bPstep = new _Button(_hud2, "bPstep", "", new Vector2(10, 10), "prevstep");

            _Listflow lf2 = new _Listflow(_hud2, "lf2", new Vector2(5, bPstep.Y + 43), graphics.GraphicsDevice.Viewport.Width - 10,graphics.GraphicsDevice.Viewport.Height/4,_Listflow.DragStyle.Rotate3D, new dfCom[] { new dfCom(true), new dfCom(true), new dfCom(true),new dfCom(true),new dfCom(true) });
            _Table3D table = new _Table3D(_hud2, "table", _pc,lf2);

            
            _hud2.add(bNstep);
           _hud2.add(bPstep);
            _hud2.add(lf2);
            _hud2.add(table);

            onMWchange += new EventHandler(table.Mwv);
            bNstep.OnRelease = new EventHandler(Ver_step2);
            bPstep.OnRelease = new EventHandler(bPstep_release);
        }

        void createHUD3()
        {
            _Panel pTeste = new _Panel(_hud3, "pTeste", new Vector2(100, 100), 100, 100, new _Panel.Anchor[] { _Panel.Anchor.D, _Panel.Anchor.E, _Panel.Anchor.C, _Panel.Anchor.B });
            _Button bRel = new _Button(_hud3, "bRel", "Gerara Relatório", 10, (int)(graphics.GraphicsDevice.Viewport.Height - 250), 200, 100, "default");
            _Button bPstep = new _Button(_hud3, "bPstep", "", new Vector2(10, 10), "prevstep");
            _Table3D table = new _Table3D(_hud3, "table", _pc, true);

            _hud3.add(pTeste);
            _hud3.add(bRel);
            _hud3.add(bPstep);
            _hud3.add(table);


            onMWchange += new EventHandler(table.Mwv);
            bRel.OnRelease = new EventHandler(bRel_release);
            bPstep.OnRelease = new EventHandler(bPstep_release);
        }

        #region events

        public void bRel_release(object sender, EventArgs e)
        {
            string arq = "Relatorio_do_PC("+DateTime.Now.ToBinary()+").txt";
            FileStream relater = File.Create(arq);
            List<dfCom> l = _pc.GetValCom();
            using (StreamWriter writer = new StreamWriter(relater))
            {
                writer.WriteLine("Modo: Relatório Provisório");
                writer.WriteLine("------------------------------------------------------------------------------------");
                writer.WriteLine(" ");

                foreach (dfCom d in l)
                {
                    writer.WriteLine(d.Tipo+" :");
                    writer.WriteLine("Nome :" + d.Nome);
                    writer.WriteLine("Preço :" + d.Preco);
                    writer.WriteLine("Informações: ");
                    writer.WriteLine("          ");
                    foreach (string inf in d.Tags.info)
                    {
                        writer.Write(inf+"   ");
                    }
                    writer.WriteLine("------------------------------------------------------------------------------------");
                    writer.WriteLine(" ");
                }

                writer.WriteLine("Gerado Automaticamente Por: DarkFalcon® Voando Mais Alto!");
            }
            _hud3.info.Add("Relatório Gerado (na pasta do executavel) Com o nome \""+arq+"\"");
        }
        public void tbSearch_release(object sender, EventArgs e)
        {
            if (_hud1["tbSearch"].Text == "Digite sua Pesquisa...")
            {
                ((_Textbox)_hud1["tbSearch"]).Text = "";
            }

        }
        public void butSearch_release(object sender, EventArgs e)
        {
            _hud1.lF.newSearch();
            if (_hud1["tbSearch"].Text != "Digite sua Pesquisa...")
                _hud1.lF.Items = fm.freeSearch(((_Textbox)_hud1["tbSearch"]).plainText);

        }

        private void cb1_change(object sender, EventArgs e)
        {
            _hud1.lF.newSearch();
            ((_ComboBox)_hud1["cb2"]).Clear();
            ((_ComboBox)_hud1["cb3"]).Clear();
            ((_ComboBox)_hud1["cb2"]).Items = fm.cb1Search2((string)sender);
            _hud1.lF.Items = fm.cbSearch((string)sender);
        }

        private void cb2_change(object sender, EventArgs e)
        {
            _hud1.lF.newSearch();
            string s1 = ((_ComboBox)_hud1["cb1"]).Text;
            ((_ComboBox)_hud1["cb3"]).Clear();
            ((_ComboBox)_hud1["cb3"]).Items = fm.cb2Search2(s1, (string)sender);
            _hud1.lF.Items = fm.cbSearch(s1,(string)sender);
        }

        private void cb3_change(object sender, EventArgs e)
        {
            _hud1.lF.newSearch();
            string s1 = ((_ComboBox)_hud1["cb1"]).Text;
            string s2 = ((_ComboBox)_hud1["cb2"]).Text;
            _hud1.lF.Items = fm.cbSearch(s1, s2, (string)sender);
        }

        private void tc_click(object sender, EventArgs e)
        {
            _hud1.lF.newSearch();
            string s = (string)sender;
            _hud1.lF.Items = fm.tagSearch(s);
        
        }
        public void bNstep_release(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(string))
            {
                if ((string)sender == "yes")
                {
                    if (_step < 3)
                        _tstep += 1;
                }
            }
            else
            {
                if (_step < 3)
                    _tstep += 1;
            }


        }
        public void bPstep_release(object sender, EventArgs e)
        {
            if (_step > 1)
                _tstep -= 1;

        }
        public void Ver_step1(object sender, EventArgs e)
        {
            _hud2.lF.Items = _pc.GetValCom();
            string v = _pc.canRun();
            if(((_Listbox)_hud1["lbError"]).Items.Count > 0)
                if (v == "ok") { v = "Atenção seu computador não ira ligar pois: \n - Possui algumas Incompatibilidades;"; } else { v += "- Possui algumas Incompatibilidades;"; }
            if (v == "ok")
            {
                bNstep_release("yes", null);
            }
            else
            {
                _hud1.info.Add(v);
                _hud1.msgb.Show(new EventHandler(bNstep_release), "Seu computador ainda não é Funcional! \n Deseja continuar mesmo assim?", _MsgBox.Type.YesNo);
            }
        }
        public void Ver_step2(object sender, EventArgs e)
        {
            validate_pc3();
            bNstep_release("yes", null);
        }
        public void validate_pc3()
        {
            string v = _pc.canRun();
            if (((_Listbox)_hud1["lbError"]).Items.Count > 0)
                if (v == "ok") { v = "Atenção seu computador não ira ligar pois: \n - Possui algumas Incompatibilidades;"; } else { v += "- Possui algumas Incompatibilidades;"; }
            if (v == "ok")
            {
                _hud3.info.Add("Parabéns! \n Seu computador está Perfeitamente Funcional!");
            }
            else
            {
                _hud3.info.Add(v);
            }
        }
        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        float x = 0; 
        protected override void Update(GameTime gameTime)
        {

            if (focus)
            {

                if (_hud1.focus != _hud1["tbSearch"] && _hud1["tbSearch"].Text == "")
                    _hud1["tbSearch"].Text = "Digite sua Pesquisa...";

                _hud1.Update();
                _hud2.Update();
                _hud3.Update();

                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && _hud1.focus == _hud1["tbSearch"])
                {
                    butSearch_release(null, null);
                    
                }
                
                if (_tstep != _step)
                {
                    if (_step == 1)
                    {
                        if (_tstep == 2)
                        {
                            if (_hud1.Position.X > -_hud1.area.Width) _hud1.Position -= new Vector2(30, 0); else _step = _tstep;
                            if (_hud2.Position.X - 30 > 0) { _hud2.Position -= new Vector2(30, 0); } else _hud2.Position = Vector2.Zero;
                        }
                    }
                    if (_step == 2)
                    {
                        if (_tstep == 1)
                        {
                            if (_hud1.Position.X + 30 < 0) _hud1.Position += new Vector2(30, 0); else { _hud1.Position = Vector2.Zero;}
                            if (_hud2.Position.X < _hud2.area.Width) { _hud2.Position += new Vector2(30, 0); } else _step = _tstep;
                        }
                        if (_tstep == 3)
                        {
                            if (_hud2.Position.X > -_hud2.area.Width) _hud2.Position -= new Vector2(30, 0); else _step = _tstep;
                            if (_hud3.Position.X - 30 > 0) { _hud3.Position -= new Vector2(30, 0); } else { _hud3.Position = Vector2.Zero; }
                        }
                    }
                    if (_step == 3)
                    {
                        if (_tstep == 2)
                        {
                            if (_hud2.Position.X + 30 < 0) _hud2.Position += new Vector2(30, 0); else { _hud2.Position = Vector2.Zero; }
                            if (_hud3.Position.X < _hud3.area.Width) { _hud3.Position += new Vector2(30, 0); } else _step = _tstep;
                        }
                    }
                    x++;
                }
                else x = 0;
                base.Update(gameTime);
                prevMouse = Mouse.GetState();

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        // Set the position of the camera in world space, for our view matrix.
        float alpha = 0;
        protected override void Draw(GameTime gameTime)
        {
            
            if (nmv)
            {
                if (focus)
                {
                    graphics.GraphicsDevice.Clear(Color.Black);
                    Viewport vp = GraphicsDevice.Viewport;
                    //  Note the order of the parameters! Projection first.
                    //m3d=GraphicsDevice.Viewport.Unproject(new Vector3(Mouse.GetState().X, Mouse.GetState().Y, -10 ), cam.projectionMatrix, cam.viewMatrix, Matrix.Identity);
                    if (Out == null)
                        Out += "null";

                    spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
                    spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), null, Color.White, 0, Vector2.Zero, 0, 1.0f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    spriteBatch.Draw(white, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Color(0, 0, 0, alpha));
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    _hud1.Draw();
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    _hud2.Draw();
                    spriteBatch.End(); 
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    _hud3.Draw();
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    //spriteBatch.DrawString(font, Out, new Vector2(0, graphics.PreferredBackBufferHeight - 40), Color.White);
                    spriteBatch.End();

                    if (noTabs)
                    {
                        Console.Out.WriteLine("no pc");
                        spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                        spriteBatch.Draw(white, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                        spriteBatch.DrawString(font, "Nenhum Computador esta aberto!", new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("Nenhum Computador esta aberto!").X / 2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("Nenhum Computador esta aberto!").Y / 2), Color.Black);
                        spriteBatch.End();
                    }

                    base.Draw(gameTime);
                }

            }
            nmv = true;
        }
    }
}
