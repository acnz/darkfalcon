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
using DarkFalcon.df;
using DarkFalcon.c3d;


namespace DarkFalcon.gui
{
    /// <summary>
    /// A game component, inherits to Clickable.
    /// Has associated On and Off content.
    /// Has a state of IsChecked that is switched by click.
    /// Draws content according to state.
    /// </summary>
    public class _Table3D : _Control
    {
        #region Fields

        Vector2 origin = Vector2.Zero;
        _Listflow lf;
        int mWheelV = 0;

        enum CamState:int
        {
            Default=0,Motherboard=1,Processador=2,Memoria=3,PlaVideo=4
        }


        List<_3DObject> lista3D = new List<_3DObject>();
        _3DObject gbase;
        _3DCamera cam;
        CamState cs;
        Rectangle[] boxes;
        Vector3[] camC;
        Vector3[] camYPO;
        Vector3[] camT;
        _3DObject[] camP;

        _Button back;

        int target,current;

        bool bMouseOver = false;
        bool bMouseDown = false;

        bool freem=false;
        Texture2D pixel;
        #endregion

        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The Game object</param>
        /// <param name="textureName">Texture name</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        /// <param name="isChecked">Initial state of the checkbox</param>
        public _Table3D(hud pai,string nome, dfPC PC,_Listflow LISTF) : base(pai,nome,Vector2.Zero) {
            Name = nome;
            Position = Vector2.Zero;
            Size = new Vector2(pai.area.Width,pai.area.Height);
            this.cam = new _3DCamera(Owner.gra);
           lf = LISTF;

        }
        public _Table3D(hud pai, string nome, dfPC PC, bool free)
            : base(pai, nome, Vector2.Zero)
        {
            Name = nome;
            Position = Vector2.Zero;
            Size = new Vector2(pai.area.Width, pai.area.Height);
            this.cam = new _3DCamera(Owner.gra);
            freem =free;
            lf = new _Listflow(Owner, "lfx",new Vector2(-200,-200), 100, new dfCom[] { });
            lf.Initialize(Owner.con, Owner.gra);
            lf.Visible = false;

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
            this.gbase = new _3DObject("gabpronto/gabpronto", cam, content, new Vector3(0, 0, 0), Vector3.Zero, 2f);
            lista3D.Add(gbase);
            cam.ResetCamera();

            back = new _Button(Owner, "back", "Voltar", (int)(graphics.Viewport.Width - 154), (int)(graphics.Viewport.Height - 154), 100, 100, "default");
            back.Initialize(content, graphics);

            back.OnRelease = new EventHandler(click_back);

            BoundingSphereRenderer.InitializeGraphics(graphics, 8);
            camC = new Vector3[5];

            camC[0] = new Vector3(7.5f, 28f, -24.5f);
            camC[1] = new Vector3(-32.9f, 18.5f, -38f);
            camC[2] = new Vector3(24.9f, 14.2f, -34.1f);
            camC[3] = new Vector3(10.7f, 31.3f, -40.3f);
            camC[4] = new Vector3(7.2f, 7.2f, -29.6f);
            

            camYPO = new Vector3[5];

            camYPO[0] = new Vector3(-1.2f, -0.4f, 150f);
            camYPO[1] = new Vector3(-1.6f, 0f, 6f);
            camYPO[2] = new Vector3(-2f, -0.2f, 30f);
            camYPO[3] = new Vector3(-1.6f, 1.6f, 22f);
            camYPO[4] = new Vector3(-0.2f, 0.2f, 14f);


            camT = new Vector3[5];

            camT[0] = new Vector3(-121.2f, 86.4f, 25.56f);
            camT[1] = new Vector3(-37.0f, 18.5f, -40.0f);
            camT[2] = new Vector3(-1.8f,20.16f,-46.3f);
            camT[3] = new Vector3(3.13f,9.5f,-40.5f);
            camT[4] = new Vector3(4.5f,4.4f,-16.1f);

            camP = new _3DObject[5];

            for(int i = 0;i<camP.Count();i++)
                camP[i] = new _3DObject("cPoint", cam, content, camT[i], Vector3.Zero, 2f);

            boxes = new Rectangle[5];
            //lf.DragMove += new EventHandler(checkBoxes);

            lf.DragStop += new EventHandler(checkBoxes);
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
        /// 

        public void click_back(object sender, EventArgs e)
        {
            target = 0;
            Owner.focus = null;
        }
        public void checkBoxes(object sender, EventArgs e)
        {
            dfCom d = (dfCom)((object[])sender)[0];
            MouseState m = (MouseState)((object[])sender)[1];
            string mode = cs.ToString();
            if (d.Tipo == mode)
            {
                Owner.info.Add("Animações não Disponiveis");
                vidType = d.Tipo;
                Owner.msgb.Show(new EventHandler(show_videos), "Deseja ver videos de Montagem de " + d.Tipo + " ?", _MsgBox.Type.YesNo);

            }
            else
            {
                if (cs == CamState.Default) Owner.info.Add("Escolha Alguma camera (pontos vermelhos)!");else
                Owner.info.Add("Você esta na Camera errada! \n"+d.Nome +" nao é um(a) "+mode);
            }
        }
        string vidType = "";
        public void show_videos(object sender, EventArgs e)
        {
            if ((string)sender == "yes")
            {
                FormWeb video = new FormWeb();
                video.Show();
                video.TopMost = true;
                video.PlayVideo(vidType);
            }
        }
        public override void Update()
        {
            base.Update();
            UpdateEvents();
            if (cs == CamState.Default)
            {
                HandleInput();
            }
            if (target != current)
            {
                cam.yaw = camYPO[target].X;
                cam.pitch = camYPO[target].Y;
                cam.offsetDistance.Z = camYPO[target].Z;

                current = target;
                cs = (CamState)target;
            }
            cam.Update(Matrix.CreateTranslation(camC[target]));

            foreach (_3DObject ob in lista3D)
            {
                ob.Update();

            }
            for (int i = 0; i < camP.Count(); i++)
                camP[i].Update();

            if (cs != CamState.Default)
                back.Update(); 
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
                    }
                    
                    DetectRay();
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

            Owner.focus = lf;
        }

            private void DetectRay()
            {
                Vector3 nearsource = new Vector3((float)mNew.X, (float)mNew.Y, 0f);
                Vector3 farsource = new Vector3((float)mNew.X, (float)mNew.Y, 1f);

                Matrix world = Matrix.Identity;

                Vector3 nearPoint = Owner.gra.Viewport.Unproject(nearsource,
                    cam.projectionMatrix, cam.viewMatrix, world);

                Vector3 farPoint = Owner.gra.Viewport.Unproject(farsource,
                    cam.projectionMatrix, cam.viewMatrix, world);

                Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                Ray pickRay = new Ray(nearPoint, direction);

                for (int i = 0; i < camP.Count();i++ )
                {
                    _3DObject ob = camP[i];
                    float? dist = pickRay.Intersects(ob.BoundingSphere);
                    if (dist != null)
                    {
                        //Console.Out.WriteLine(i + " hited " + dist);
                        target = i;
                    }
                }
            }

            float a = 7.5f;
            float b = 28.0f;
            float ci = -24.5f;
            private void HandleInput()
            {
                KeyboardState keyboardState = Keyboard.GetState();
                float dMouseX = (mNew.X - mOld.X) / 5;
                float dMouseY = (mNew.Y - mOld.Y) / 5;

                float dMouseW = mWheelV / 30;
                if (mNew.RightButton == ButtonState.Pressed)
                {
                    if (dMouseX > 0)
                    {
                        cam.yaw += -0.2f;
                    }
                    else if (dMouseX < 0)
                    {
                        cam.yaw += 0.2f;
                    }
                    if (freem)
                    {
                        if (dMouseY > 0)
                        {
                            cam.pitch += -0.2f;
                        }
                        else if (dMouseY < 0)
                        {
                            cam.pitch += 0.2f;
                        }

                        if (dMouseW > 0)
                        {
                            cam.roll += -0.2f;

                        }
                        else if (dMouseW < 0)
                        {
                            cam.roll += 0.2f;

                        }
                    }
                    if (mNew.LeftButton == ButtonState.Pressed)
                        target = 0;
                }
                else
                {
                    cam.offsetDistance += new Vector3(0, 0, dMouseW);

                }
                mWheelV = 0;
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    Console.Out.WriteLine("(" + a + " ; " + b + " ; " + ci + " ) ; " + "(" + cam.yaw + " ; " + cam.pitch + " ; " + cam.offsetDistance.Z +" ); " + cam.position);
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    a += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    a -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
                {
                    b += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    b -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    ci += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    ci -= 0.1f;
                }
            }
            public void Mwv(object sender, EventArgs e)
            {
                mWheelV = (int)sender;
            }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
            public override void Draw()
            {
                spriteBatch.End();
                foreach (_3DObject ob in lista3D)
                {
                    ob.Draw();
                   // BoundingSphereRenderer.Render(ob.BoundingSphere, Owner.gra, cam.viewMatrix, cam.projectionMatrix, Color.Blue);
                }
                for (int i = 1; i < camP.Count(); i++)
                {
                    if (!freem)
                    if (cs == CamState.Default)
                    {
                        camP[i].Draw();
                        BoundingSphereRenderer.Render(camP[i].BoundingSphere, Owner.gra, cam.viewMatrix, cam.projectionMatrix, Color.Red);
                    }
                }
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);


                if (cs != CamState.Default) back.Draw();
            }
            
        #endregion
    }
}
