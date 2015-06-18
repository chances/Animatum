using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;

namespace Animatum.SceneGraph
{
    class Shader : IDisposable
    {
        private OpenGL gl;
        private SharpGL.Shaders.ShaderProgram shader;

        public Shader(OpenGL gl, string vertexShaderSource, string fragmentShaderSource,
            Dictionary<uint, string> attributeLocations)
        {
            this.gl = gl;

            Compiled = false;
            CompilerOutput = null;

            shader = new SharpGL.Shaders.ShaderProgram();
            try
            {
                shader.Create(gl, vertexShaderSource, fragmentShaderSource, attributeLocations);
            }
            catch (SharpGL.Shaders.ShaderCompilationException ex)
            {
                CompilerOutput = ex.CompilerOutput;
                return;
            }

            try
            {
                shader.AssertValid(gl);
            }
            catch (Exception ex)
            {
                CompilerOutput = ex.Message;
                return;
            }

            Compiled = true;
        }

        public bool Compiled { get; private set; }

        public string CompilerOutput { get; private set; }

        public void Dispose()
        {
            shader.Delete(gl);
        }

        public int GetAttributeLocation(string attributeName)
        {
            return shader.GetAttributeLocation(gl, attributeName);
        }

        public void BindAttributeLocation(uint location, string attribute)
        {
            shader.BindAttributeLocation(gl, location, attribute);
        }

        public void Bind()
        {
            shader.Bind(gl);
        }

        public void Unbind()
        {
            shader.Unbind(gl);
        }

        public void SetUniform1(string uniformName, int v1)
        {
            gl.Uniform1(GetUniformLocation(uniformName), v1);
        }

        public void SetUniform1(string uniformName, float v1)
        {
            shader.SetUniform1(gl, uniformName, v1);
        }

        public void SetUniform3(string uniformName, float v1, float v2, float v3)
        {
            shader.SetUniform3(gl, uniformName, v1, v2, v3);
        }

        public void SetUniformMatrix3(string uniformName, float[] m)
        {
            shader.SetUniformMatrix3(gl, uniformName, m);
        }

        public void SetUniformMatrix4(string uniformName, float[] m)
        {
            shader.SetUniformMatrix4(gl, uniformName, m);
        }

        public int GetUniformLocation(string uniformName)
        {
            return shader.GetAttributeLocation(gl, uniformName);
        }
    }
}
