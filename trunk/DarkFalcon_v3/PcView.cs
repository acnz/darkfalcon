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

namespace DarkFalcon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PcView : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;
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

        List<_3DClickable> listaHUD = new List<_3DClickable>();
        public List<_3DClickable> listaMenu = new List<_3DClickable>();
        public List<_3DCheckbox> groupM = new List<_3DCheckbox>();

        private IntPtr drawSurface;
        FrmTabs fm;
        private int MouseWheel;
        MouseState prevMouse;
        UserActivityHook actHook;

         public PcView(IntPtr drawSurface, FrmTabs frm)
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
         }

         public void resize(FrmTabs frm)
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
             User32.SetWindowPos((uint)this.Window.Handle, 0, fm.MdiParent.Location.X + fm.Location.X + 15, fm.MdiParent.Location.Y + fm.Location.Y + 105,
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
            background = Content.Load<Texture2D>("Textures//aurora");
            white = Content.Load<Texture2D>("Textures//white");
            this.monitor = new _3DObject("monitorc", cam, this.Content, Vector3.Zero, Vector3.Zero, 3f);
            this.gabinete = new _3DObject("Monitor/2/2", cam, this.Content, new Vector3(15, 0, 5), Vector3.Zero, 30f);
           // cam.Update(Matrix.CreateTranslation(monitor.Position + new Vector3(5, 0, -10)), MouseWheel);
            createHUD();

            // TODO: use this.Content to load your game content here
        }

        void createHUD()
        {
            _3DCheckbox cbMenu = new _3DCheckbox(this, "Menu", new Vector2(10, 0), false);


            _3DCheckbox cbMother = new _3DCheckbox(this, "Motherboard", new Vector2(30, 40), false);
            _3DCheckbox cbPro = new _3DCheckbox(this, "Processador", new Vector2(30, 80), false);
            _3DCheckbox cbMon = new _3DCheckbox(this, "Monitor", new Vector2(30, 120), false);
            _3DCheckbox cbGab = new _3DCheckbox(this, "Gabinete", new Vector2(30, 160), false);

            _3DButton butImg = new _3DButton(this, "Imagens", new Vector2(300, 40));
            _3DImageBox Img = new _3DImageBox(this, "check", new Vector2(400, 40));

            listaHUD.Add(cbMenu);
            groupM.Add(cbMother);
            groupM.Add(cbMon);
            groupM.Add(cbPro);
            groupM.Add(cbGab);
            listaMenu.Add(butImg);
            foreach (_3DCheckbox cb in groupM)
            {
                cb.modoRadio();
                listaHUD.Add(cb);
            }
            foreach (_3DClickable ob in listaMenu)
            {
                listaHUD.Add(ob);
            }
            listaHUD.Add(Img);

        }
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
       bool nenhum = true;
        protected override void Update(GameTime gameTime)
        {
           
            // Allows the game to exit
            //myObject.Rotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds/10 *
                // TODO: Add your update logic here
            prevMouse = Mouse.GetState();
            //Console.WriteLine(focus);
            if (!focus) MouseWheel =0;
            cam.Update(Matrix.CreateTranslation(monitor.Position+new Vector3(5,0,-10)), MouseWheel);
            MouseWheel = 0;
            

            foreach (_3DObject ob in lista3D)
            {
                ob.Update();
            }
            foreach (_3DClickable ob in listaHUD)
            {
                ob.Update();
            }

            if (((_3DCheckbox)this.listaHUD[0]).IsChecked)
            {
                if (alpha <= 0.7f)
                    alpha += 0.05f;
                if (alpha > 0.6f)
                {
                    nenhum = true;
                    foreach (_3DCheckbox cb in groupM)
                    {
                        cb.On = true;

                        if (cb.IsChecked)
                        {
                            switch (cb.Nome)
                            {
                                case "Motherboard":
                                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).Image = _pc.Motherboard.LocalImagem2D;
                                    break;
                                case "Processador":
                                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).Image = _pc.Processador.LocalImagem2D;
                                    break;
                                case "Gabinete":
                                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).Image = _pc.Gabinete.LocalImagem2D;
                                    break;

                            }
                            nenhum = false;
                            foreach (_3DClickable ob in listaMenu)
                            {

                                ob.On = true;
                            }
                        }
                        else
                        {
                            if (nenhum)
                                foreach (_3DClickable ob in listaMenu)
                                {
                                    ob.On = false;
                                }
                        }
                    }

                }

                if (((_3DButton)listaMenu[0]).Clicked)
                {
                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).On = true;
                }
                if (((_3DImageBox)listaHUD[listaHUD.Count-1]).Clicked)
                {
                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).On = false;
                    ((_3DImageBox)listaHUD[listaHUD.Count - 1]).Clicked = false;
                }
            }
            else
            {
                if (alpha >= 0.0f)
                    alpha -= 0.05f;

                foreach (_3DClickable ob in listaHUD)
                {
                    listaHUD[0].On = true;
                    ob.On = false;
                }

            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        // Set the position of the camera in world space, for our view matrix.
        float alpha = 0;
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            Viewport vp = GraphicsDevice.Viewport;
            //  Note the order of the parameters! Projection first.
            //m3d=GraphicsDevice.Viewport.Unproject(new Vector3(Mouse.GetState().X, Mouse.GetState().Y, -10 ), cam.projectionMatrix, cam.viewMatrix, Matrix.Identity);
            if(Out == null)
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

            foreach (_3DClickable ob in listaHUD)
            {
                ob.Draw();
            }

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //spriteBatch.DrawString(font, Out, new Vector2(0, graphics.PreferredBackBufferHeight - 40), Color.White);
            spriteBatch.End();

            if (noTabs)
            {
                Console.Out.WriteLine("no pc");
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                spriteBatch.Draw(white, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.DrawString(font, "Nenhum Computador esta aberto!", new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("Nenhum Computador esta aberto!").X/2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("Nenhum Computador esta aberto!").Y/2), Color.Black);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

    }
}
