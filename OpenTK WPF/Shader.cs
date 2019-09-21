using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_WPF
{
    public class Shader
    {
        #region Field

        readonly int Handle;
        readonly int _vertexShader;
        readonly int _fragmentShader;
        private readonly Dictionary<string, int> _uniformLocations;
        #endregion


        //public Shader(string vertexPath, string fragmentPath)
        //{
        //    ////
        //    //string VertexShaderSource;

        //    //using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
        //    //{
        //    //    VertexShaderSource = reader.ReadToEnd();
        //    //}

        //    //string FragmentShaderSource;

        //    //using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
        //    //{
        //    //    FragmentShaderSource = reader.ReadToEnd();
        //    //}

        //    ////
        //    //_vertexShader = GL.CreateShader(ShaderType.VertexShader);
        //    //GL.ShaderSource(_vertexShader, VertexShaderSource);

        //    //_fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        //    //GL.ShaderSource(_fragmentShader, FragmentShaderSource);

        //    ////
        //    //GL.CompileShader(_vertexShader);

        //    //string infoLogVert = GL.GetShaderInfoLog(_vertexShader);
        //    //if (infoLogVert != System.String.Empty)
        //    //    System.Console.WriteLine(infoLogVert);

        //    //GL.CompileShader(_fragmentShader);

        //    //string infoLogFrag = GL.GetShaderInfoLog(_fragmentShader);

        //    //if (infoLogFrag != System.String.Empty)
        //    //    System.Console.WriteLine(infoLogFrag);

        //    ////
        //    //Handle = GL.CreateProgram();

        //    //GL.AttachShader(Handle, _vertexShader);
        //    //GL.AttachShader(Handle, _fragmentShader);

        //    //GL.LinkProgram(Handle);

        //    ////
        //    //GL.DetachShader(Handle, _vertexShader);
        //    //GL.DetachShader(Handle, _fragmentShader);
        //    //GL.DeleteShader(_fragmentShader);
        //    //GL.DeleteShader(_vertexShader);
        //}


        public Shader(string vertexPath, string fragPath)
        {
            //Load vertex shader and compile
            var shaderSource = LoadSource(vertexPath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, shaderSource);

            CompileShader(vertexShader);

            //Load fragment shader and compile
            shaderSource = LoadSource(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            //
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            //
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            //
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            //
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        private void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        private void CompileShader(int Shader)
        {
            GL.CompileShader(Shader);

            // Check for compilation errors
            GL.GetShader(Shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                throw new Exception($"Error occurred whilst compiling Shader({Shader})");
            }
        }

        //
        private string LoadSource(string vertexPath)
        {
            using (var sr = new StreamReader(vertexPath, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        //
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        //
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        //~Shader()
        //{
        //    GL.DeleteProgram(Handle);
        //}


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
