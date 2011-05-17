#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using DarkFalcon.df;
#endregion

namespace DarkFalcon.c3d
{
    public class _3DObject
    {
        #region Fields
        // The XNA framework Model object that we are going to display.
        internal string model;
        Vector3 _position = Vector3.Zero;
        internal float _scale = 1.0f;
        Vector3 _rotation = Vector3.Zero;
        internal _3DCamera cam;
        internal ContentManager content;
        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        internal Matrix[] boneTransforms;

        internal Matrix rotation;
        internal Model _model;
        internal bool[] lights;
        bool isTextureEnabled;
        bool isPerPixelLightingEnabled;
        internal BoundingSphere boundingSphere;

        #endregion

        #region Initialization
        /// <summary>
        /// Loads the model.
        /// </summary>
        public _3DObject()
        {
        }
        public _3DObject(_3DCamera camera, ContentManager Content)
        {
            cam = camera;
            content = Content;
        }
        public _3DObject(string Model_name,_3DCamera camera,ContentManager Content)
        {
            model = Model_name;
            cam = camera;
            content = Content;
            this.Load();
        }

        /// <summary>
        /// Loads the model with all informations.
        /// </summary>
        public _3DObject(string Model_name, _3DCamera camera,ContentManager Content, Vector3 _Position, Vector3 _Rotation, float _Scale)
        {
            model = Model_name;
            cam = camera;
            content = Content;
            _position = _Position;
            _scale = _Scale;
            _rotation = _Rotation;
            this.Load();
        }
        public void Load()
        {
            _model = content.Load<Model>("Models/" + model);
            boundingSphere = GetBoundingSphereFromModel(this);
            // Allocate the transform matrix array.
            boneTransforms = new Matrix[_model.Bones.Count];
            bool[] a = { true,true, true };
            IsTextureEnabled = true;
            Lights = a;
            IsPerPixelLightingEnabled = true;
        }
        #endregion

        #region Public accessors
        /// <summary>
        /// Gets or sets the projection matrix value.
        /// </summary>
        public _3DCamera Camera
        {
            get { return cam; }
            set { cam = value; }
        }

        /// <summary>
        /// Gets or sets the model value.
        /// </summary>
        public string ModelName
        {
            get { return model; }
            set { model = value; }
        }

        public Model Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }

        /// <summary>
        /// Gets or sets the rotation matrix value.
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Gets or sets the rotation matrix value.
        /// </summary>
        public bool IsTextureEnabled
        {
            get { return isTextureEnabled; }
            set { isTextureEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the view matrix value.
        /// </summary>

        /// <summary>
        /// Gets or sets the lights states.
        /// </summary>
        public bool[] Lights
        {
            get { return lights; }
            set { lights = value; }
        }

        /// <summary>
        /// Gets or sets the per pixel lighting preferences
        /// </summary>
        public bool IsPerPixelLightingEnabled
        {
            get { return isPerPixelLightingEnabled; }
            set { isPerPixelLightingEnabled = value; }
        }
        #endregion

        #region Functions

        private Matrix GetRotationMatrix(Vector3 rot)
        {

            Matrix matrix = Matrix.CreateRotationX(MathHelper.ToRadians(rot.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(rot.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rot.Z));
            return matrix;
        }

        private void SetEffectPerPixelLightingEnabled(BasicEffect effect)
        {
            effect.PreferPerPixelLighting = isPerPixelLightingEnabled;
        }

        /// <summary>
        /// Sets effects lighting properties
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="lights"></param>
        private void SetEffectLights(BasicEffect effect, bool[] lights)
        {
            effect.Alpha = 1.0f;
            effect.DiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 5.0f;
            effect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            effect.DirectionalLight0.Enabled = lights[0];
            effect.DirectionalLight0.DiffuseColor = Vector3.One;
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, 0));
            effect.DirectionalLight0.SpecularColor = Vector3.One;

            effect.DirectionalLight1.Enabled = lights[1];
            effect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1, -1, 0));
            effect.DirectionalLight1.SpecularColor = new Vector3(1f, 1f, 1f);

            effect.DirectionalLight2.Enabled = lights[2];
            effect.DirectionalLight2.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f);
            effect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3(-1, -1, -1));
            effect.DirectionalLight2.SpecularColor = new Vector3(0.3f, 0.3f, 0.3f);

            effect.LightingEnabled = true;
        }
        public BoundingSphere GetBoundingSphereFromModel(_3DObject Ob)
        {
            BoundingSphere finalSphere;
            finalSphere = new BoundingSphere(_position, 1f);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                BoundingSphere meshSphere = _model.Meshes[0].BoundingSphere;

                meshSphere.Radius *= _scale;
                meshSphere.Center = _position;
                //Console.Out.WriteLine(Ob);
                finalSphere = BoundingSphere.CreateMerged(finalSphere, meshSphere);
            }


            return finalSphere;
        } 
        #endregion

        #region Update

        public void Update()
        {
            boundingSphere = GetBoundingSphereFromModel(this);
           // Console.Out.WriteLine(boundingSphere);
        }
        
        #endregion

        #region Draw
        public void Draw()
        {
            Matrix[] modelTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            rotation = GetRotationMatrix(_rotation);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect ef in mesh.Effects)
                {
                    if(ef.Texture != null)
                        ef.TextureEnabled = true;
                    ef.EnableDefaultLighting();
                    ef.World = rotation * Matrix.CreateScale(_scale) * modelTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(_position);
                    ef.Projection = cam.projectionMatrix;
                    ef.View = cam.viewMatrix;
                    //ef.Alpha = 0.5f;

                }
                mesh.Draw();
            }
        }
        
        #endregion

    }
}
