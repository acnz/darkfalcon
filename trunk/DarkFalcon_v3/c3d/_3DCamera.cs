using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkFalcon.c3d
{
    public class _3DCamera
    {

        public Vector3 position;
        private Vector3 desiredPosition;
        private Vector3 target;
        private Vector3 desiredTarget;
        public Vector3 offsetDistance;
        public float yaw, pitch, roll;
        GraphicsDevice g;
        private Matrix cameraRotation;
        public Matrix viewMatrix, projectionMatrix;
        private float POV = 30.0f;

       public _3DCamera(GraphicsDevice game)
        {
            g = game;
            ResetCamera();
        }

        public void ResetCamera()
        {
            position = new Vector3( 0,0,0);
            desiredPosition = position;
            target = new Vector3();
            desiredTarget = target;

            offsetDistance = new Vector3(0, 0, 150);

            roll = 0.0f;
            yaw = -1.2f;
            pitch = -0.4f;



            cameraRotation = Matrix.Identity;
            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(POV), g.Viewport.AspectRatio, .5f, 5000000f);
        }


        public void Update(Matrix chasedPoint)
        {
            UpdateViewMatrix(chasedPoint);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(POV), g.Viewport.AspectRatio, .5f, 5000000f);
        }

        private void UpdateViewMatrix(Matrix chasedObjectsWorld)
        {

            cameraRotation.Forward.Normalize();

            cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

            desiredPosition = Vector3.Transform(offsetDistance, cameraRotation);
            desiredPosition += chasedObjectsWorld.Translation;
            position = desiredPosition;

            target = chasedObjectsWorld.Translation;

            roll = MathHelper.SmoothStep(roll, 0f, .2f);
            //We'll always use this line of code to set up the View Matrix.
            viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
        }

        //This cycles through the different camera modes.

    }
}
