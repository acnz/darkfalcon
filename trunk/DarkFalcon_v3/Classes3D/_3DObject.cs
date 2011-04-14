#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace DarkFalcon
{
    class _3DObject
    {
        #region Fields
        // The XNA framework Model object that we are going to display.
        private string model;
        Vector3 _position = Vector3.Zero;
        private float _scale = 1.0f;
        Vector3 _rotation = Vector3.Zero;
        private CameraTP cam;
        private ContentManager content;
        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        Matrix[] boneTransforms;

        private Matrix rotation;
        private Model _model;
        private bool[] lights;
        bool isTextureEnabled;
        bool isPerPixelLightingEnabled;
        private BoundingSphere boundingSphere;
        #endregion

        #region Initialization
        /// <summary>
        /// Loads the model.
        /// </summary>

        public _3DObject(String Model_name,CameraTP camera,ContentManager Content)
        {
            model = Model_name;
            cam = camera;
            content = Content;
        }

        /// <summary>
        /// Loads the model with all informations.
        /// </summary>
        public _3DObject(String Model_name, CameraTP camera,ContentManager Content, Vector3 _Position, Vector3 _Rotation, float _Scale)
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
            _model = content.Load<Model>("Models/"+model);
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
        public CameraTP Camera
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
            //effect.DiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);
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
            BoundingSphere modelBox;
            modelBox = new BoundingSphere();

                BoundingSphere meshSphere = Ob._model.Meshes[0].BoundingSphere;
                meshSphere.Center = Ob._position; 
                meshSphere.Radius *= _scale;
                //Console.Out.WriteLine(Ob);
                modelBox = BoundingSphere.CreateMerged(modelBox, meshSphere);
            

            return modelBox;
        } 
        #endregion

        #region Update

        public void Update()
        {
            boundingSphere = GetBoundingSphereFromModel(this);
            //Console.Out.WriteLine(boundingBox);
        }
        
        #endregion

        #region Draw
        /// <summary>
        /// Draws the spaceship model, using the current drawing parameters.
        /// </summary>
        public void Draw()
        {
            Matrix[] modelTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            rotation = GetRotationMatrix(_rotation);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect ef in mesh.Effects)
                {
                    ef.EnableDefaultLighting();
                    ef.PreferPerPixelLighting = true;
                    ef.World = rotation * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_position)*modelTransforms[mesh.ParentBone.Index] ;
                    ef.Projection = cam.projectionMatrix;
                    ef.View = cam.viewMatrix;

                    SetEffectLights(ef, Lights);
                    SetEffectPerPixelLightingEnabled(ef);
                }
                mesh.Draw();
            }
        }
        
        #endregion

    }
}
