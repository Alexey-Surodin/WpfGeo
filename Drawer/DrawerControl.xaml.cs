using ClassLibrary.PointsModel;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.WPF;
using System.Collections.Generic;

namespace Drawer
{
    public partial class DrawerControl
    {
        public List<List<List<GeoPoint>>> Model { set; private get; }
        public List<List<List<GeoPoint>>> VzModel { set; private get; }

        public bool FillTriangles { get; set; } = true;

        public DrawerControl()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = (sender as OpenGLControl)?.OpenGL;
            gl?.Enable(OpenGL.GL_BLEND);
            gl?.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha);
            gl?.ClearColor(0, 0, 0, 0);

        }
        private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            ChangeCameraPosition((sender as OpenGLControl)?.OpenGL);
        }
        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = (sender as OpenGLControl)?.OpenGL;
            gl?.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            Draw3DModel(Model, gl);
            Draw2DModel(VzModel, gl);
            DrawColorScale(gl);
            DrawAxis(gl);
        }

        private void Draw3DModel(List<List<List<GeoPoint>>> model, OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            foreach (var slice in model)
            {
                foreach (var layer in slice)
                {
                    for (var i = 0; i < layer.Count - 2; i += 3)
                    {
                        var p1 = layer[i];
                        var p2 = layer[i + 1];
                        var p3 = layer[i + 2];
                        var defColor = System.Drawing.Color.Black;


                        if (p1.Visible && p2.Visible && p3.Visible)
                        {
                            System.Drawing.Color c;
                            if (FillTriangles)
                            {
                                gl.Begin(OpenGL.GL_TRIANGLES);
                                c = p1.Color;
                                gl.Color(c.R, c.G, c.B, c.A);
                                gl.Vertex(Right * p1.X, p1.Y, p1.Z);

                                c = p2.Color;
                                gl.Color(c.R, c.G, c.B, c.A);
                                gl.Vertex(Right * p2.X, p2.Y, p2.Z);

                                c = p3.Color;
                                gl.Color(c.R, c.G, c.B, c.A);
                                gl.Vertex(Right * p3.X, p3.Y, p3.Z);
                                gl.End();
                            }

                            gl.LineWidth(1.4f);
                            gl.Begin(OpenGL.GL_LINES);
                            c = FillTriangles ? defColor : p1.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p1.X, p1.Y, p1.Z);

                            c = FillTriangles ? defColor : p2.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p2.X, p2.Y, p2.Z);

                            c = FillTriangles ? defColor : p2.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p2.X, p2.Y, p2.Z);

                            c = FillTriangles ? defColor : p3.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p3.X, p3.Y, p3.Z);

                            c = FillTriangles ? defColor : p3.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p3.X, p3.Y, p3.Z);

                            c = FillTriangles ? defColor : p1.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p1.X, p1.Y, p1.Z);

                            gl.End();
                            gl.LineWidth(1.0f);
                        }


                        gl.PointSize(4.0f);
                        gl.Begin(OpenGL.GL_POINTS);
                        if (p1.Selected && p1.Visible)
                        {
                            gl.Color(255f, 255f, 255f, 255f);
                            gl.Vertex(Right * p1.X, p1.Y, p1.Z);
                        }
                        if (p2.Selected && p2.Visible)
                        {
                            gl.Color(255f, 255f, 255f, 255f);
                            gl.Vertex(Right * p2.X, p2.Y, p2.Z);
                        }
                        if (p3.Selected && p3.Visible)
                        {
                            gl.Color(255f, 255f, 255f, 255f);
                            gl.Vertex(Right * p3.X, p3.Y, p3.Z);
                        }
                        gl.End();
                        gl.PointSize(1.0f);

                    }
                }
            }
            gl.Flush();
        }

        private void Draw2DModel(List<List<List<GeoPoint>>> model, OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            foreach (var slice in model)
            {
                foreach (var layer in slice)
                {
                    for (var i = 0; i < layer.Count - 1; i += 1)
                    {
                        var p1 = layer[i];
                        var p2 = layer[i + 1];

                        if (p1.Visible && p2.Visible)
                        {
                            gl.LineWidth(1.6f);
                            gl.Begin(OpenGL.GL_LINES);

                            var c = p1.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p1.X, p1.Y, p1.Z);

                            c = p2.Color;
                            gl.Color(c.R, c.G, c.B, c.A);
                            gl.Vertex(Right * p2.X, p2.Y, p2.Z);

                            gl.End();
                            gl.LineWidth(1.0f);
                        }

                        gl.PointSize(4.0f);
                        gl.Begin(OpenGL.GL_POINTS);
                        if (p1.Selected && p1.Visible)
                        {
                            gl.Color(255f, 255f, 255f, 255f);
                            gl.Vertex(Right * p1.X, p1.Y, p1.Z);
                        }
                        if (p2.Selected && p2.Visible)
                        {
                            gl.Color(255f, 255f, 255f, 255f);
                            gl.Vertex(Right * p2.X, p2.Y, p2.Z);
                        }
                        gl.End();
                        gl.PointSize(1.0f);

                    }
                }
            }
            gl.Flush();
        }
    }
}
