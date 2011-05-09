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
        CameraTP cam;
        public bool focus;

        List<_3DObject> lista3D = new List<_3DObject>();
        _3DObject monitor;
        _3DObject gabinete;

        dfPC _pc;

        hud _hud;
        public SpriteFont hudf;
        List<_Control> listaHUD = new List<_Control>();
        public List<_Control> listaMenu = new List<_Control>();
        public List<_Checkbox> groupM = new List<_Checkbox>();
        public bool active = true; 
        private IntPtr drawSurface;
        Frm3D fm;
        private int MouseWheel;
        public MouseState prevMouse;
        UserActivityHook actHook;

         public PcView(IntPtr drawSurface, Frm3D frm)
        {
            graphics = new GraphicsDeviceManager(this);
            actHook = new UserActivityHook();
            actHook.Start();
            string dir = Properties.Settings.Default.CRoot + "Data";
            Content.RootDirectory = dir;
            fm = frm;
            this.drawSurface = drawSurface;
            graphics.PreferredBackBufferWidth = fm.PicPcView.Width;
            graphics.PreferredBackBufferHeight = fm.PicPcView.Height;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(PcView_VisibleChanged);
            actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            Mouse.WindowHandle = this.Window.Handle;
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
         public void MouseMoved(object sender, MouseEventArgs e)
         {
             MouseWheel = e.Delta;
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

            lista3D.Add(monitor);
            lista3D.Add(gabinete);

            _pc = new dfPC(true);
            cam.ResetCamera();

            _hud = new hud(this);
            _hud.Initialize(Content, graphics.GraphicsDevice);

            createHUD();


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.cam = new CameraTP(GraphicsDevice, lista3D);
            font = Content.Load<SpriteFont>("Arial");
            hudf = Content.Load<SpriteFont>("hudfont");
            background = Content.Load<Texture2D>("Textures//aurora");
            white = Content.Load<Texture2D>("Textures//white");
            this.monitor = new _3DObject("monitorc", cam, this.Content, Vector3.Zero, Vector3.Zero, 3f);
            this.gabinete = new _3DObject("Monitor/2/2", cam, this.Content, new Vector3(15, 0, 5), Vector3.Zero, 30f);
           // cam.Update(Matrix.CreateTranslation(monitor.Position + new Vector3(5, 0, -10)), MouseWheel);
            
          
            // TODO: use this.Content to load your game content here
        }

        void createHUD()
        {
            _Listflow lf1 = new _Listflow(_hud, "lf1", new Vector2(5, 5), (4 * graphics.GraphicsDevice.Viewport.Width / 5)-10, new dfCom[] { });
            _Panel pPesquisa = new _Panel(_hud, "pPesquisa", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, 100), 100, graphics.GraphicsDevice.Viewport.Height / 4 - 2.5f, new _Panel.Anchor[] { _Panel.Anchor.D, _Panel.Anchor.C });
            
            float px = pPesquisa.Position.X;
            float py = pPesquisa.Position.Y;
            float pw = pPesquisa.area.Width;
            float ph = pPesquisa.area.Height;

            _Panel pFiltros = new _Panel(_hud, "pFiltros", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, py + ph + 2.5f), 100, 2*graphics.GraphicsDevice.Viewport.Height / 4 - 2.5f, new _Panel.Anchor[] { _Panel.Anchor.D });

            float fx = pFiltros.Position.X;
            float fy = pFiltros.Position.Y;
            float fw = pFiltros.area.Width;
            float fh = pFiltros.area.Height;

            _Panel pTags = new _Panel(_hud, "pTags", new Vector2(4 * graphics.GraphicsDevice.Viewport.Width / 5, fy + fh + 2.5f), 100, graphics.GraphicsDevice.Viewport.Height / 4 - 5, new _Panel.Anchor[] { _Panel.Anchor.B, _Panel.Anchor.D });
            float tx = pTags.Position.X;
            float ty = pTags.Position.Y;
            float tw = pTags.area.Width;
            float th = pTags.area.Height;

            _Textbox tbSearch = new _Textbox(_hud, "tbSearch", new Vector2(px + 10, py + 70), "Digite sua Pesquisa...", (int)(pw-20));
            _Button butSearch = new _Button(_hud, "butSearch", "Pesquisar", new Rectangle((int)(px + 80), (int)(py + 110), 40, 20), "default");

            
            _ComboBox cb1 = new _ComboBox(_hud, "cb1", new Vector2(fx + 10, fy + 100), 150, new string[] { });
            _Label labcb1 = new _Label(_hud, "labcb1", new Vector2(cb1.Position.X, cb1.Position.Y - 20), "Tipo", _Label.Align.Left);
            _ComboBox cb2 = new _ComboBox(_hud, "cb2", new Vector2(fx + 10, fy + 170), 150, new string[] { });
            _Label labcb2 = new _Label(_hud, "labcb2", new Vector2(cb2.Position.X, cb2.Position.Y - 20), "Fabricante", _Label.Align.Left);
            _ComboBox cb3 = new _ComboBox(_hud, "cb3", new Vector2(fx + 10, fy + 240), 150, new string[] { });
            _Label labcb3 = new _Label(_hud, "labcb3", new Vector2(cb3.Position.X, cb3.Position.Y - 20), "Filtro", _Label.Align.Left);


            _Listbox lbError = new _Listbox(_hud, "lbError", new Vector2(5, 5 * graphics.GraphicsDevice.Viewport.Height / 6), (int)pTags.X - 10, graphics.GraphicsDevice.Viewport.Height / 6-5, new string[] { });
            _Panel pHolder = new _Panel(_hud, "pHolder", new Vector2(5, lf1.Y + lf1.Height + 5), (int)pTags.X - 10, lbError.Y - 10 - (lf1.Y + lf1.Height), new _Panel.Anchor[] { }, 0.4f);
            _Holder Holder = new _Holder(_hud, "Holder", pHolder, lbError, lf1, _pc);

            

            _hud.add(lf1);
            _hud.add(pPesquisa);
            _hud.add(pFiltros);
            _hud.add(pTags);
            _hud.add(lbError);
            _hud.add(pHolder);
            _hud.add(Holder);

            _hud.add(tbSearch);
            _hud.add(butSearch);

            _hud.add(cb1);
            _hud.add(labcb1);
            _hud.add(cb2);
            _hud.add(labcb2);
            _hud.add(cb3);
            _hud.add(labcb3);

            tbSearch.OnRelease = new EventHandler(tbSearch_release);
            butSearch.OnRelease = new EventHandler(butSearch_release);

        }

        #region events

        public void tbSearch_release(object sender, EventArgs e)
        {
            Console.WriteLine(((_Control)sender).Name + " out");
        }
        public void butSearch_release(object sender, EventArgs e)
        {
            Console.WriteLine(((_Control)sender).Name + " pressed");
            if (_hud["tbSearch"].Text != "Digite sua Pesquisa...")
                _hud.lF.Items = fm.freeSearch(((_Textbox)_hud["tbSearch"]).plainText);

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
        protected override void Update(GameTime gameTime)
        {
            if (!focus) MouseWheel =0;
            cam.Update(Matrix.CreateTranslation(monitor.Position+new Vector3(5,0,-10)), MouseWheel);
            MouseWheel = 0;

            _hud.Update();
            


            foreach (_3DObject ob in lista3D)
            {
                ob.Update();
            }
          

            base.Update(gameTime);
            prevMouse = Mouse.GetState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        // Set the position of the camera in world space, for our view matrix.
        float alpha = 0;
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            if (active)
            {
                Viewport vp = GraphicsDevice.Viewport;
                //  Note the order of the parameters! Projection first.
                //m3d=GraphicsDevice.Viewport.Unproject(new Vector3(Mouse.GetState().X, Mouse.GetState().Y, -10 ), cam.projectionMatrix, cam.viewMatrix, Matrix.Identity);
                if (Out == null)
                    Out += "null";

                spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
                spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), null, Color.White, 0, Vector2.Zero, 0, 1.0f);
                spriteBatch.End();


                foreach (_3DObject ob in lista3D)
                {
                    ob.Draw();
                }


                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                spriteBatch.Draw(white, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Color(0, 0, 0, alpha));
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                _hud.Draw();
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
            active = true;
        }
    }
}
