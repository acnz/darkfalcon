using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkFalcon.gui
{
    public class hud
    {
        PcView _game;
        public PcView Game { get { return _game; } }
        List<_Control> _controls;
        public ContentManager con;
        public GraphicsDevice gra;
        public Rectangle area;
        public _Control focus;
        public _InfoBox info;
        public _MsgBox msgb;
        Vector2 _pos = Vector2.Zero;
        bool on = true;
        bool off = false;
        RenderTarget2D rt;
        Texture2D st;
        public Vector2 Position
        {
            get { return _pos; }
            set
            {
                _pos = value;
                if (_pos == Vector2.Zero) on = true; else on = false;
                if (_pos.X <= -area.Width || _pos.X >= gra.Viewport.Width) off = true; else off = false;
            }
        }

        public _Listflow lF
        {
            get { 
            _Control i = _controls.Find(item => item.GetType() == typeof(_Listflow)); 
            return (_Listflow)i; 
        }
            set {
                _Control i = _controls.Find(item => item.GetType() == typeof(_Listflow));
                i = value;
            }
        }

        public _Control this[string index]
        {
            get {
                _Control i = _controls.Find(item => item.Name == index);
                return i; 
            }
            set {
                _Control i = _controls.Find(item => item.Name == index);
                i = value; }
        }

        public hud(PcView g)
        {
            _controls = new List<_Control>();
            _game = g;
            area = new Rectangle(0, 0, g.Window.ClientBounds.Width, g.Window.ClientBounds.Height);
           
        }

        public void add(_Control c)
        {
            _controls.Add(c);
            if(c.IsDisposed  == true)
            c.Initialize(con, gra);
        }
        public void remove(_Control c)
        {
            _controls.Remove(c);
            c.Dispose();
        }
        public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            con = content;
            gra = graphics;
            info = new _InfoBox(this, "msg", new Vector2(100, 100), 200, 300, new string[] { });
            info.Initialize(content, graphics);
            msgb = new _MsgBox(this, "msgb");
            msgb.Initialize(content, graphics);
            rt = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height, 0, SurfaceFormat.Color, RenderTargetUsage.PlatformContents);
        }
        public void Update()
        {
            if (on)
            {
                if (!msgb.isShow)
                {
                    foreach (_Control c in _controls)
                        c.Update();

                    info.Update();
                }
                else
                {
                    msgb.Update();
                }
            }
        }
        public void Draw()
        {
            if (!off)
            {
                if (on)
                {
                    foreach (_Control c in _controls)
                        if (c != focus)
                            c.Draw();
                    if (focus != null)
                        focus.Draw();
                    info.Draw();
                    if (msgb.isShow)
                        msgb.Draw();
                }
                else
                {
                    Game.spriteBatch.End();
                    gra.SetRenderTarget(0, rt);
                    gra.Clear(Color.TransparentBlack);
                    Game.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

                    foreach (_Control c in _controls)
                        if (c != focus)
                            c.Draw();
                    if (focus != null)
                        focus.Draw();
                    info.Draw();
                    if (msgb.isShow)
                        msgb.Draw();
                    Game.spriteBatch.End();
                    gra.SetRenderTarget(0, null);
                    st = rt.GetTexture();
                    Game.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
                    Game.spriteBatch.Draw(st, _pos, Color.White);
                }
            }
        }

    }
}
