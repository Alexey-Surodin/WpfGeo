using SharpGL;
using SharpGL.WPF;
using System;
using System.Windows;
using System.Windows.Input;
namespace Drawer
{
    public partial class DrawerControl
    {
        private int Right => RightHandCoordinateSystem ? 1 : -1;
        public bool RightHandCoordinateSystem { get; set; }
        private Camera camera = new Camera
        {
            Position = new Vector3F(0, 0, -10),
            Direction = new Vector3F(0, 0, 0),
            Top = new Vector3F(0, 1, 0)
        };
        private void ChangeCameraPosition(OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            
            gl.LoadIdentity();
            
            gl.Perspective(80.0f, ActualWidth / ActualHeight, 0.01, 500.0);
            
            gl.LookAt(camera.Position.X, camera.Position.Y, camera.Position.Z, 
                        camera.Direction.X, camera.Direction.Y, camera.Direction.Z, 
                        camera.Top.X, camera.Top.Y, camera.Top.Z);    
            
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }

        #region EventHandlers
        private void OpenGLControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var step = Vector3F.Normalize((camera.Direction - camera.Position) * Math.Sign(e.Delta));
            if (camera.Position + step != camera.Direction)
                camera.Position += step;
            ChangeCameraPosition((sender as OpenGLControl)?.OpenGL);
        }
        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            var dx = e.GetPosition(sender as IInputElement).X - camera.mouseX;
            var dy = e.GetPosition(sender as IInputElement).Y - camera.mouseY;
            camera.mouseX = e.GetPosition(sender as IInputElement).X;
            camera.mouseY = e.GetPosition(sender as IInputElement).Y;
            var front = camera.Direction - camera.Position;
            var speed = front.Norma;
            if (camera.isDragging)
            {
                var dvy = Vector3F.Normalize(camera.Top) * dy;
                var dvx = Vector3F.Normalize(Vector3F.Cross(front, camera.Top)) * dx;
                var sum = Vector3F.Normalize(dvy - dvx) * speed / 100;
                camera.Position += sum;
                camera.Direction += sum;
                ChangeCameraPosition((sender as OpenGLControl)?.OpenGL);
            }
            if (camera.isRotating)
            {
                var radius = front.Norma;
                var right = Vector3F.Normalize(Vector3F.Cross(camera.Top, front));
                camera.Position += right * radius * dx / 100;
                front = camera.Direction - camera.Position;
                camera.Position += Vector3F.Normalize(front) * (front.Norma - radius);
                right = Vector3F.Normalize(Vector3F.Cross(front, camera.Top));
                camera.Position += camera.Top * radius * dy / 100;
                front = camera.Direction - camera.Position;
                camera.Position += Vector3F.Normalize(front) * (front.Norma - radius);
                camera.Top = Vector3F.Normalize(Vector3F.Cross(right, front));
                ChangeCameraPosition((sender as OpenGLControl)?.OpenGL);
            }
        }
        private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsFocused) Focus();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                camera.isDragging = true;
                camera.mouseX = e.GetPosition(sender as IInputElement).X;
                camera.mouseY = e.GetPosition(sender as IInputElement).Y;
                Mouse.OverrideCursor = Cursors.SizeAll;
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                camera.isRotating = true;
                camera.mouseX = e.GetPosition(sender as IInputElement).X;
                camera.mouseY = e.GetPosition(sender as IInputElement).Y;
            }
        }
        private void OpenGLControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && camera.isDragging)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                camera.isDragging = false;
            }
            if (e.MiddleButton == MouseButtonState.Released && camera.isRotating)
            {
                camera.isRotating = false;
            }
        }
        
        //
        private void OpenGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            var front = camera.Direction - camera.Position;
            var radius = front.Norma;
            var right = Vector3F.Normalize(Vector3F.Cross(camera.Top, front));
            int dx = 0, dy = 0;
            switch (e.Key)
            {
                case Key.Right:
                    dx = 1;
                    break;
                case Key.Left:
                    dx = -1;
                    break;
                case Key.Up:
                    dy = 1;
                    break;
                case Key.Down:
                    dy = -1;
                    break;
                default:
                    return;
            }
            camera.Position += right * radius * dx;
            front = camera.Direction - camera.Position;
            camera.Position += Vector3F.Normalize(front) * (front.Norma - radius);
            right = Vector3F.Normalize(Vector3F.Cross(front, camera.Top));
            camera.Position += camera.Top * radius * dy;
            front = camera.Direction - camera.Position;
            camera.Position += Vector3F.Normalize(front) * (front.Norma - radius);
            camera.Top = Vector3F.Normalize(Vector3F.Cross(right, front));
            ChangeCameraPosition((sender as OpenGLControl)?.OpenGL);
        }
        #endregion
    }
}
