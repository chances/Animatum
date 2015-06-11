using System;

namespace Animatum.SharpGL
{
	public class MultisampledNativeWindowRenderContextProvider : SharpGL.NativeWindowRenderContextProvider
	{
		private const int WGL_SAMPLE_BUFFERS_ARB = 0x2041;
		private const int WGL_SAMPLES_ARB = 0x2042;

		private bool arbMultisampleSupported = true;

		public MultisampledNativeWindowRenderContextProvider()
		{
			// We cannot layer GDI drawing on top of open gl drawing.
			GDIDrawingEnabled = false;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Animatum.SharpGL.MultisampledNativeWindowRenderContextProvider"/>
		/// supports multisampling.
		/// </summary>
		/// <value><c>true</c> if multisampling is supported; otherwise, <c>false</c>.</value>
		public bool MultisamplingSupported {
			get { return arbMultisampleSupported; }
		}

		/// <summary>
		/// Creates the render context provider. Must also create the OpenGL extensions.
		/// </summary>
		/// <param name="openGLVersion">The desired OpenGL version.</param>
		/// <param name="gl">The OpenGL context.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="bitDepth">The bit depth.</param>
		/// <param name="parameter">The parameter.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">A valid Window Handle must be provided for the NativeWindowRenderContextProvider</exception>
		public override bool Create(SharpGL.OpenGLVersion openGLVersion, SharpGL.OpenGL gl, int width, int height, int bitDepth, object parameter)
		{
			// Perform normal context creation
			base.Create(openGLVersion, gl, width, height, bitDepth, parameter);

			//	Setup a pixel format.
			Win32.PIXELFORMATDESCRIPTOR pfd = new Win32.PIXELFORMATDESCRIPTOR();
			pfd.Init();
			pfd.nVersion = 1;
			pfd.dwFlags = Win32.PFD_DRAW_TO_WINDOW | Win32.PFD_SUPPORT_OPENGL | Win32.PFD_DOUBLEBUFFER;
			pfd.iPixelType = Win32.PFD_TYPE_RGBA;
			pfd.cColorBits = (byte)bitDepth;
			pfd.cDepthBits = 16;
			pfd.cStencilBits = 8;
			pfd.iLayerType = Win32.PFD_MAIN_PLANE;

			int arbMultisampleFormat = 0;

			// Check for FSAA support

			if (!gl.IsExtensionFunctionSupported ("wglChoosePixelFormatARB")) {
				arbMultisampleSupported = false;
				return true;
			}

			// Test for FSAA 4x pixel format support
			int pixelFormat;
			uint numFormats;
			float[] fAttributes = {0, 0};

			// Test for 4x FSAA
			int[] iAttributes = {
				SharpGL.Win32.PFD_DRAW_TO_WINDOW, true,
				SharpGL.Win32.PFD_SUPPORT_OPENGL, true,
				SharpGL.Win32.PFD_DOUBLEBUFFER, true,
				WGL_SAMPLE_BUFFERS_ARB, true,
				WGL_SAMPLES_ARB, 4,
				0, 0
			};

			if (!ChoosePixelFormatARB(gl, deviceContextHandle, iAttributes, fAttributes, 1, pixelFormat, numFormats) || numFormats < 1) {
				arbMultisampleSupported = false;
				return true;
			}

			// Test for FSAA 2x pixel format support
			iAttributes[9] = 2;

			if (!ChoosePixelFormatARB(gl, deviceContextHandle, iAttributes, fAttributes, 1, pixelFormat, numFormats) || numFormats < 1) {
				arbMultisampleSupported = false;
				return true;
			}

			arbMultisampleFormat = pixelFormat;

			// Try to set the new pixel format
			if (Win32.SetPixelFormat (deviceContextHandle, arbMultisampleFormat, pfd) == 0) {
				arbMultisampleSupported = false;
				return true;
			}

			// FSAA is supported destroy old reder context and create new one
			if (SharpGL.Win32.wglDeleteContext (renderContextHandle) == 0) {
				return false;
			}
			renderContextHandle = SharpGL.Win32.wglCreateContext(deviceContextHandle);

			// Make the context current.
			MakeCurrent();

			// Update the render context if required.
			UpdateContextVersion(gl);

			return true;
		}

		/// <summary>
		/// Choose a pixel format from the list of supported pixel formats.
		/// </summary>
		/// <returns><c>true</c>, if a valid pexel format was chosen, <c>false</c> otherwise.</returns>
		/// <param name="hDC"></param>
		/// <param name="iAttributes">I attributes.</param>
		/// <param name="fAttributes">F attributes.</param>
		/// <param name="pixelFormat">Pixel format.</param>
		/// <param name="numFormats">Number formats.</param>
		public bool ChoosePixelFormatARB(SharpGL.OpenGL gl, IntPtr hDC, int[] iAttributes, float[] fAttributes, uint maxFormats, out int pixelFormat, out uint numFormats)
		{
			return (bool)gl.GetDelegateFor<wglChoosePixelFormatARB> () (hDC, iAttributes, fAttributes, pixelFormat, numFormats);
		}

		private delegate bool wglChoosePixelFormatARB(IntPtr hDC, int[] iAttributes, int[] fAttributes, uint maxFormats, out int pixelFormat, out uint numFormats);
	}
}

