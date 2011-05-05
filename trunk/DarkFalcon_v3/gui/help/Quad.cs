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
    public struct Quad
    {
        public Vector3 Origin;
        public Vector3 UpperLeft;
        public Vector3 LowerLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
        public Vector3 Normal;
        public Vector3 Up;
        public Vector3 Left;
        GraphicsDevice g;
        VertexDeclaration quadVertexDecl;
        public BasicEffect quadEffect;
        public VertexPositionNormalTexture[] Vertices;
        public int[] Indexes;

        public Quad( GraphicsDevice G, float width, float height )
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new int[6];
            Origin = Vector3.Zero;
            Normal = Vector3.Backward;
            Up = Vector3.Up;
            g = G;
            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            Vector3 uppercenter = (Up * height / 2) + Origin;
            UpperLeft = uppercenter + (Left * width / 2);
            UpperRight = uppercenter - (Left * width / 2);
            LowerLeft = UpperLeft - (Up * height);
            LowerRight = UpperRight - (Up * height );
            quadVertexDecl = new VertexDeclaration(g,
   VertexPositionNormalTexture.VertexElements);
            quadEffect = new BasicEffect(g, null);
            FillVertices();
        }
        
        private void FillVertices()
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2( 0.0f, 0.0f );
            Vector2 textureUpperRight = new Vector2( 1.0f, 0.0f );
            Vector2 textureLowerLeft = new Vector2( 0.0f, 1.0f );
            Vector2 textureLowerRight = new Vector2( 1.0f, 1.0f );

            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = LowerLeft;
            Vertices[0].TextureCoordinate = textureLowerLeft;
            Vertices[1].Position = UpperLeft;
            Vertices[1].TextureCoordinate = textureUpperLeft;
            Vertices[2].Position = LowerRight;
            Vertices[2].TextureCoordinate = textureLowerRight;
            Vertices[3].Position = UpperRight;
            Vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;
        }
        public void Draw(Texture2D texture,Matrix World)
        {
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            quadEffect.World = World* Matrix.CreateScale(2);
            quadEffect.View  = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up) ;
            quadEffect.Projection = Matrix.CreatePerspective(g.Viewport.Width, g.Viewport.Height, 1, 6);
            
            g.VertexDeclaration = quadVertexDecl;
            quadEffect.Begin();
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                g.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, Vertices, 0, 4, Indexes, 0, 2);

                pass.End();
            }
            quadEffect.End();
        }
    }
}
