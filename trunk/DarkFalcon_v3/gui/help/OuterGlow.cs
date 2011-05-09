#region File Description
//-----------------------------------------------------------------------------
// Quad.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace DarkFalcon.gui.help
{
    public struct OuterGlow
    {

        Texture2D tex;
        SpriteBatch spriteBatch;


        public OuterGlow(Texture2D Tex,SpriteBatch S)
        {
            spriteBatch = S;
            tex = Tex;
            // Calculate the quad corners
        }
        
        public void Draw(Rectangle quad,Color c)
        {
            int x = (int)(quad.X - (quad.Width*0.3f)/2)+2;
            int y = (int)(quad.Y - (quad.Height*0.3f)/2)+2;
            int w = (int)(quad.Width + (quad.Width*0.3f))-2;
            int h = (int)(quad.Height + (quad.Height*0.3f))-2;
            Rectangle outRect = new Rectangle(x, y, w, h);

            spriteBatch.Draw(tex, outRect, c);
        }
    }
}
