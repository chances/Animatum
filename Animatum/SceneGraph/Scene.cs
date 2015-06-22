using Animatum.SceneGraph.Primitives;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A 3D scene, the root of a scene graph.
    /// </summary>
    public class Scene : Node, IDisposable
    {
        private MovingLookAtCamera camera;
        private Size viewportSize;
        private Grid grid;
        private Axies axies;
        private OpenGLAttributesEffect attrs;

        private bool postprocessingEnabled;
        private uint[] glFrameBufferTexArray = new uint[1] { 0 },
            glRenderBufferArray = new uint[1] { 0 },
            glFrameBufferArray = new uint[1] { 0 };
        private uint fbo, fboTexture, rboDepth;

        private Shader postShader = null;

        private Model model;

        private OpenGL gl;

        /// <summary>
        /// Construct a new scene, optionally enableing post-processing, if available.
        /// </summary>
        /// <param name="postprocessingEnabled">Whether or not to enable post-processing; defaults to false</param>
        public Scene(OpenGL gl, Size viewportSize)
        {
            this.gl = gl;
            this.viewportSize = viewportSize;

            model = null;

            postprocessingEnabled = false;

            ClearColor = Color.Black;
            camera = new MovingLookAtCamera()
            {
                Position = new Vertex(10f, 0f, 0f),
                Target = new Vertex(0f, 0f, 0f),
                UpVector = new Vertex(0f, 0f, 1f),
                Near = 0,
                Far = 250,
                HorizontalTheta = 0.785f,
                VerticalTheta = -0.785f
            };
            RenderGrid = true;
            RenderAxies = true;
            grid = new Grid();
            grid.Size = 150;
            axies = new Axies();
            axies.Size = 150;
            //Set attributes
            attrs = new OpenGLAttributesEffect();
            attrs.EnableAttributes.EnableDepthTest = true;
            attrs.EnableAttributes.EnableNormalize = true;
            attrs.EnableAttributes.EnableLighting = true;
            attrs.EnableAttributes.EnableTexture2D = true;
            attrs.LightingAttributes.TwoSided = false;

            Color col = Color.FromArgb(40, 40, 40);

            // Nice soft-ish lighting
            Light light = new Light(OpenGL.GL_LIGHT0)
            {
                Position = new Vertex(-9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT1)
            {
                Position = new Vertex(9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT2)
            {
                Position = new Vertex(0, 15, 15),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
        }

        public Size ViewportSize
        {
            get { return viewportSize; }
            set
            {
                ResizeViewport(value);
            }
        }

        public Color ClearColor { get; set; }

        public MovingLookAtCamera Camera
        {
            get { return camera; }
        }

        public bool RenderGrid { get; set; }

        public bool RenderAxies { get; set; }

        public Model Model
        {
            get { return model; }
        }

        public bool PostProcessingEnabled {
            get { return postprocessingEnabled; }
            set
            {
                UpdatePostProcessing(value);
            }
        }

        public void Dispose()
        {
            if (postprocessingEnabled)
            {
                gl.DeleteRenderbuffersEXT(1, glRenderBufferArray);
                gl.DeleteTextures(1, glFrameBufferTexArray);
                gl.DeleteFramebuffersEXT(1, glFrameBufferArray);

                postShader.Dispose();
                postShader = null;
            }
        }

        /// <summary>
        /// Add a child to this Scene.
        /// </summary>
        /// <param name="node">Node to add</param>
        public new void Add(Node node)
        {
            if (node is Model)
            {
                if (model == null)
                {
                    model = node as Model;
                    model.Parent = this;
                }
            }
            else
            {
                base.Add(node);
            }
        }

        public void ResizeViewport(Size newSize)
        {
            viewportSize = newSize;

            if (camera != null)
            {
                camera.AspectRatio = (float)viewportSize.Width / (float)viewportSize.Height;
                camera.Project(gl);
            }

            if (postprocessingEnabled)
            {
                // Resize frame buffer
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, fboTexture);
                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA,
                    viewportSize.Width, viewportSize.Height, 0,
                    OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, null);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);

                // Resize render buffer
                gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER, rboDepth);
                gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER, OpenGL.GL_DEPTH_COMPONENT16,
                    viewportSize.Width, viewportSize.Height);
                gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER, 0);
            }
        }

        public override void Render(OpenGL gl)
        {
            Render();
        }

        public void Render()
        {
            if (postprocessingEnabled && postShader != null && postShader.Compiled)
            {
                gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, fbo);
            }

            float[] clear = Convert.ColorToGLColor(ClearColor);
            gl.ClearColor(clear[0], clear[1], clear[2], clear[3]);

            camera.Project(gl);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT |
                OpenGL.GL_STENCIL_BUFFER_BIT);

            attrs.Push(gl, null);

            if (RenderGrid)
                grid.Render(gl);
            if (RenderAxies)
                axies.Render(gl);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_ONE, OpenGL.GL_ONE);

            foreach (Node node in children)
            {
                if (node is Light)
                    node.Render(gl);
            }

            if (Model != null)
                Model.Render(gl);

            foreach (Node node in children)
            {
                if (node is Light)
                    ((Light)node).Pop(gl);
            }

            gl.Disable(OpenGL.GL_BLEND);

            attrs.Pop(gl, null);

            gl.Flush();

            if (postprocessingEnabled && postShader != null && postShader.Compiled)
            {
                gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, 0);

                gl.ClearColor(0, 0, 0, 1);
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.LoadIdentity();
                gl.Ortho(0, viewportSize.Width, viewportSize.Height, 0, -1, 1);
                gl.MatrixMode(OpenGL.GL_MODELVIEW);
                gl.LoadIdentity();

                postShader.Bind();
                postShader.SetUniform1("resolution_x", viewportSize.Width);
                postShader.SetUniform1("resolution_y", viewportSize.Height);
                //gl.Uniform2(postShader.GetUniformLocation("resolution"),
                //    viewportSize.Width, viewportSize.Height);

                gl.ActiveTexture(OpenGL.GL_TEXTURE0);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, fboTexture);
                postShader.SetUniform1("texFramebuffer", 0);

                gl.Begin(OpenGL.GL_QUADS);
                gl.TexCoord(0, 1); gl.Vertex(0, 0);
                gl.TexCoord(0, 0); gl.Vertex(0, (float)viewportSize.Height);
                gl.TexCoord(1, 0); gl.Vertex((float)viewportSize.Width, (float)viewportSize.Height);
                gl.TexCoord(1, 1); gl.Vertex((float)viewportSize.Width, 0);
                gl.End();

                postShader.Unbind();
            }
        }

        public override void RenderForHitTest(OpenGL gl, Dictionary<uint, Node> hitMap, ref uint currentName)
        {
            RenderForHitTest(hitMap, ref currentName);
        }

        public void RenderForHitTest(Dictionary<uint, Node> hitMap, ref uint currentName)
        {
            model.RenderForHitTest(gl, hitMap, ref currentName);
        }

        /// <summary>
        /// Performs a hit test on the Model.
        /// </summary>
        /// <returns>The list of <see cref="Animatum.SharpGL.SceneGraph.Node"/>'s hit at the specified point</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        private IEnumerable<Node> HitTest(int x, int y)
        {
            if (model == null || (model.Meshes.Count == 0 && model.Bones.Count == 0))
                return null;

            List<Node> resultSet = new List<Node>();
            Dictionary<uint, Node> hitMap = new Dictionary<uint, Node>();

            int[] viewport = new int[4];

            // Get the viewport, then convert the mouse point to an opengl point.
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            y = viewport[3] - y;

            uint[] selectBuffer = new uint[512];
            gl.SelectBuffer(512, selectBuffer);

            gl.RenderMode(OpenGL.GL_SELECT);

            // Initialise the names, and add the first name.
            gl.InitNames();
            gl.PushName(0);

            // Push picking projection  matrix
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.PickMatrix(x, y, 1, 1, viewport);
            camera.TransformProjectionMatrix(gl);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            uint currentName = 1;

            this.RenderForHitTest(gl, hitMap, ref currentName);

            // Pop matrix and flush commands.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Flush();

            // End selection.
            int hits = gl.RenderMode(OpenGL.GL_RENDER);

            // Get hit Nodes
            uint posinarray = 0;
            for (int hit = 0; hit < hits; hits++)
            {
                uint nameCount = selectBuffer[posinarray++];
                uint z1 = selectBuffer[posinarray++];
                uint z2 = selectBuffer[posinarray++];

                if (nameCount == 0)
                    continue;

                // Add each hit element to the result set
                for (int name = 0; name < nameCount; name++)
                {
                    uint hitName = selectBuffer[posinarray++];
                    resultSet.Add(hitMap[hitName]);
                }
            }

            return resultSet;
        }

        private void UpdatePostProcessing(bool enabled)
        {
            if (!postprocessingEnabled && enabled)
            {
                // Setup back buffer
                gl.ActiveTexture(OpenGL.GL_TEXTURE0);
                gl.GenTextures(1, glFrameBufferTexArray);
                fboTexture = glFrameBufferTexArray[0];
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, fboTexture);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_CLAMP_TO_EDGE);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_CLAMP_TO_EDGE);
                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA,
                    viewportSize.Width, viewportSize.Height, 0,
                    OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, null);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);

                // Setup depth buffer
                gl.GenRenderbuffersEXT(1, glRenderBufferArray);
                rboDepth = glRenderBufferArray[0];
                gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER, rboDepth);
                gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER, OpenGL.GL_DEPTH_COMPONENT16,
                    viewportSize.Width, viewportSize.Height);
                gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER, 0);

                // Setup framebuffer
                gl.GenFramebuffersEXT(1, glFrameBufferArray);
                fbo = glFrameBufferArray[0];
                gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, fbo);
                gl.FramebufferTexture2DEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_COLOR_ATTACHMENT0_EXT,
                    OpenGL.GL_TEXTURE_2D, fboTexture, 0);
                gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_DEPTH_ATTACHMENT_EXT,
                    OpenGL.GL_RENDERBUFFER, rboDepth);

                uint fboStatus = gl.CheckFramebufferStatusEXT(OpenGL.GL_FRAMEBUFFER_EXT);

                if (gl.CheckFramebufferStatusEXT(OpenGL.GL_FRAMEBUFFER_EXT) != OpenGL.GL_FRAMEBUFFER_COMPLETE_EXT)
                {
                    postprocessingEnabled = false;

                    gl.DeleteRenderbuffersEXT(1, glRenderBufferArray);
                    gl.DeleteTextures(1, glFrameBufferTexArray);
                    gl.DeleteFramebuffersEXT(1, glFrameBufferArray);

                    return;
                }

                gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, 0);

                // Setup post-processing shader
                postShader = new Shader(gl, ManifestResourceLoader.LoadTextFile(@"Resources\Shaders\screen.vert"),
                    ManifestResourceLoader.LoadTextFile(@"Resources\Shaders\screen.frag"), null);

                if (!postShader.Compiled)
                {
                    throw new Exception(postShader.CompilerOutput);
                }

                postprocessingEnabled = true;
            }
            else if (postprocessingEnabled && !enabled)
            {
                gl.DeleteRenderbuffersEXT(1, glRenderBufferArray);
                gl.DeleteTextures(1, glFrameBufferTexArray);
                gl.DeleteFramebuffersEXT(1, glFrameBufferArray);

                postShader.Dispose();
                postShader = null;

                postprocessingEnabled = false;
            }
        }
    }
}
