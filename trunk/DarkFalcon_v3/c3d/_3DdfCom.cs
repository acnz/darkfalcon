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
using DarkFalcon.df;


namespace DarkFalcon.c3d
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class _3DdfCom : _3DObject
    {
        public dfCom c;

        public _3DdfCom()
            : base()
        {
            // TODO: Construct any child components here
        }
        public _3DdfCom(dfCom ob, _3DCamera camera, ContentManager Content)
            : base(camera,Content)
        {
            try
            {
                _model = content.Load<Model>(ob.LocalImagem3D);
            }catch(Exception)
            {

                try { _model = content.Load<Model>(ob.Default3D); }
                catch (Exception) { _model = content.Load<Model>("Models//x//x"); }
            }
            c = ob;
            boundingSphere = GetBoundingSphereFromModel(this);
            // Allocate the transform matrix array.
            boneTransforms = new Matrix[_model.Bones.Count];
            bool[] a = { true, true, true };
            IsTextureEnabled = true;
            Lights = a;
            IsPerPixelLightingEnabled = true;
        }
    }
}