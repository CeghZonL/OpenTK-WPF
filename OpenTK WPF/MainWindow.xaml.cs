using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace OpenTK_WPF
{
    

    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow.xaml 的交互逻辑
        /// </summary>

        #region Field

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;


        #endregion

        #region Property

        float[] vertices = {
    -0.5f, -0.5f, 0.0f,
     0.5f, -0.5f, 0.0f,
     -0.5f,  0.5f, 0.0f,
     0.5f, 0.5f, 0.0f

};
        #endregion
        public MainWindow()
        {
            InitializeComponent();

        }

        private void GLControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //
            _shader.Use();

            //
            GL.BindVertexArray(_vertexArrayObject);

            //
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            //
            _glControl.SwapBuffers();
        }

        private void GLControl_Load(object sender, EventArgs e)
        {            
            GL.ClearColor(Color4.Red);
            //
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //
            _shader = new Shader("Shaders/VertexShader.GLSL", "Shaders/FragmentShader.GLSL");

            //
            _shader.Use();

            //
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            //
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        }

        //
        protected void OnUnload(EventArgs e)
        {
            //
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            //
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
        }

        private void _glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _glControl.Width, _glControl.Height);
        }
    }
}
