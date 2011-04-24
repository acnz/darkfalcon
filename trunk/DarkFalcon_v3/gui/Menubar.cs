using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkFalcon.gui
{
    class _Menubar : _Control
    {
        public _Menubar(hud pai)
            : base(pai)
        {
        }
        public override void Update()
        {
        }
        public override void Draw()
        {
            Texture2D mb1 = Game.Content.Load<Texture2D>("Textures//Gui//mb1");
            Game.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //for (int i = 0; i < Rectangle.Width; i++)
            //{
            //    Game.spriteBatch.Draw(mb1,new Rectangle(i,0,1,30),Color.White);
            //    //Game.spriteBatch.DrawString(Game.font, "haha", new Vector2(i, 0), Color.White);
            //}
                Game.spriteBatch.End();
        }
    }
}
