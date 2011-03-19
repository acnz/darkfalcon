using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkFalcon_v3
{
    class CameraTP
    {

        private Vector3 position;
        private Vector3 desiredPosition;
        private Vector3 target;
        private Vector3 desiredTarget;
        private Vector3 offsetDistance;
        private float yaw, pitch, roll;
        GraphicsDevice g;
        List<_3DObject> list;
        private Matrix cameraRotation;
        public Matrix viewMatrix, projectionMatrix;
        private float POV = 30.0f;
       MouseState prevMouse;

       public CameraTP(GraphicsDevice game, List<_3DObject> Objects)
        {
            g = game;
            list = Objects;
            ResetCamera();
        }

        public void ResetCamera()
        {
            position = new Vector3(0, 0, 0);
            desiredPosition = position;
            target = new Vector3();
            desiredTarget = target;


            offsetDistance = new Vector3(0, 0, 56);

            // yaw = 0.8f;
            //pitch = -0.6f;
            roll = 0.0f;
            yaw = 0.6f;
            pitch = -0.4f;



            cameraRotation = Matrix.Identity;
            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(POV), g.Viewport.AspectRatio, .5f, 5000000f);
        }


        public void Update(Matrix chasedObjectsWorld, int mW)
        {
            HandleInput(mW);
            UpdateViewMatrix(chasedObjectsWorld);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(POV), g.Viewport.AspectRatio, .5f, 5000000f);
        }


        private void HandleInput(int mW)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState m = Mouse.GetState();
            float dMouseX = (m.X - prevMouse.X) / 5;
            float dMouseY = (m.Y - prevMouse.Y) / 5;
            float dMouseW = (mW) / 30;

            if (m.RightButton == ButtonState.Pressed)
            {
                if (dMouseX > 0)
                {
                    yaw += -0.2f;
                }
                else if (dMouseX < 0)
                {
                    yaw += 0.2f;
                }
                if (dMouseY > 0)
                {
                    pitch += -0.2f;
                }
                else if (dMouseY < 0)
                {
                    pitch += 0.2f;
                }
                if (dMouseW > 0)
                {
                    roll += -0.2f;
                    POV += 1;
                }
                else if (dMouseW < 0) 
                {
                    roll += 0.2f;
                    POV += -1;
                }
                if (m.LeftButton == ButtonState.Pressed)
                    ResetCamera();
            }
            else
            {
                offsetDistance += new Vector3(0, 0, dMouseW);
                
            }
            prevMouse = m;
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
