using SharpGL;
using SharpGL.Enumerations;
using System.Collections.Generic;
using System.Drawing;
namespace Drawer
{
    public partial class DrawerControl
    {
        public bool ShowAxis { get; set; } = true;
        public bool ShowColorScale { get; set; } = true;
        public List<KeyValuePair<float, Color>> rhoColors;
        private void DrawAxis(OpenGL gl)
        {
            if (!ShowAxis) return;
            var front = Vector3F.Normalize(camera.Direction - camera.Position);
            var right = Vector3F.Normalize(Vector3F.Cross(front, camera.Top));
            var axisPosition = new Vector3F
            {
                X = camera.Position.X + front.X,
                Y = camera.Position.Y + front.Y,
                Z = camera.Position.Z + front.Z
            };
            axisPosition -= camera.Top * 0.7 + right * 1.2;
            gl.MatrixMode(MatrixMode.Modelview);
            gl.LoadIdentity();
            gl.Translate(axisPosition.X, axisPosition.Y, axisPosition.Z);

            gl.Begin(BeginMode.Lines);
            gl.Color(1.0, 0, 0);
            gl.Vertex(0, 0, 0);
            gl.Vertex(Right * 0.1, 0, 0);
            gl.Color(0, 1.0, 0);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0.1, 0);
            gl.Color(0, 0, 1.0);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, 0.1);
            gl.End();
        }
        private void DrawColorScale(OpenGL gl)
        {
            if (!ShowColorScale) return;
            var w = ActualWidth;
            var h = ActualHeight;
            gl.MatrixMode(MatrixMode.Projection);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Ortho(0.0, w, 0.0, h, 0, 10);
            gl.MatrixMode(MatrixMode.Modelview);
            gl.LoadIdentity();
            gl.Translate(w - 30, 40, 0);
            Drq(gl, 20, h - 80, w - 60, 40);

            gl.MatrixMode(MatrixMode.Projection);
            gl.PopMatrix();
            gl.MatrixMode(MatrixMode.Modelview);
            gl.LoadIdentity();
        }
        private void Drq(OpenGL gl, double width, double height, double tx, double ty)
        {
            if (rhoColors == null || rhoColors.Count == 0) return;
            var n = rhoColors.Count - 1;
            var dy = height / n;
            var dx = width;
            var y = 0.0;
            var fontSize = 10 + dy / 12;
            if (fontSize > 20) fontSize = 20;
            for (var i = 0; i < n; i++)
            {
                gl.Begin(BeginMode.Quads);
                var c = rhoColors[i].Value;
                gl.Color(c.R, c.G, c.B);
                gl.Vertex(0, y);
                gl.Vertex(dx, y);
                gl.Vertex(dx, y + dy);
                gl.Vertex(0, y + dy);
                gl.End();
                y += dy;
                gl.DrawText((int)tx, (int)ty, 255, 255, 255, "Arial", (float)fontSize, ((int)rhoColors[i].Key).ToString());
                ty += dy;
            }
            gl.DrawText((int)tx, (int)ty, 255, 255, 255, "Arial", (float)fontSize, ((int)rhoColors[n].Key).ToString());
        }
    }
}
