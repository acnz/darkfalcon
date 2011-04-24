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

        private IntPtr drawSurface;
        FrmTabs fm;
        private int MouseWheel;
        public MouseState prevMouse;
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
            hudf = Content.Load<SpriteFont>("hudf");
            background = Content.Load<Texture2D>("Textures//aurora");
            white = Content.Load<Texture2D>("Textures//white");
            this.monitor = new _3DObject("monitorc", cam, this.Content, Vector3.Zero, Vector3.Zero, 3f);
            this.gabinete = new _3DObject("Monitor/2/2", cam, this.Content, new Vector3(15, 0, 5), Vector3.Zero, 30f);
           // cam.Update(Matrix.CreateTranslation(monitor.Position + new Vector3(5, 0, -10)), MouseWheel);
            
          
            // TODO: use this.Content to load your game content here
        }

        void createHUD()
        {
            
            _Menubar mbMenu = new _Menubar(_hud);

            _Button but1 = new _Button(_hud, "Imagens","wow", new Vector2(300, 40));
            _Button but2 = new _Button(_hud, "Imagens","teste", new Rectangle(10, 40,20,200),"default");

            but1.OnMouseOut = new EventHandler(but1_out);
            but1.OnPress = new EventHandler(but1_press);
            but1.OnMouseOver = new EventHandler(but1_in);
            but1.OnRelease = new EventHandler(but1_release);
            but2.OnMouseOut = new EventHandler(but1_out);
            but2.OnPress = new EventHandler(but1_press);
            but2.OnMouseOver = new EventHandler(but1_in);
            but2.OnRelease = new EventHandler(but1_release);
            _hud.add(but1);
            _hud.add(but2);

        }

        #region events

        public void but1_out(object sender, EventArgs e)
        {
            Console.WriteLine(((_Button)sender).Name + " out");
        }
        public void but1_press(object sender, EventArgs e)
        {
            Console.WriteLine(((_Button)sender).Name + " pressed");
        }
        public void but1_in(object sender, EventArgs e)
        {
            Console.WriteLine(((_Button)sender).Name + " in");
        }
        public void but1_release(object sender, EventArgs e)
        {
            Console.WriteLine(((_Button)sender).Name + " released");
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
                spriteBatch.DrawString(font, "Nenhum Computador esta aberto!", new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("Nenhum Computador esta aberto!").X/2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("Nenhum Computador esta aberto!").Y/2), Color.Black);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

    }
}
