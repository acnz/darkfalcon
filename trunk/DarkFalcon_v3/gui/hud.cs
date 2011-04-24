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
        ContentManager con;
        GraphicsDevice gra;
        public Rectangle area;

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
        }
        public void Update()
        {
            foreach (_Control c in _controls)
                c.Update();
        }
        public void Draw()
        {
            foreach (_Control c in _controls)
                c.Draw();
        }

    }
}
