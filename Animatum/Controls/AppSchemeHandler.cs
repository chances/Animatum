using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Controls
{
    class AppSchemeHandler : ISchemeHandler
    {
        private string dataPath = Application.StartupPath + "\\Timeline";

        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
        {
            if (request.Method == "GET")
            {
                Uri uri = new Uri(request.Url);
                string filePath = dataPath + uri.AbsolutePath.Replace("/", @"\");

                if (uri.Authority == "timeline" && File.Exists(filePath))
                {
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    response.ResponseStream = new MemoryStream(bytes);

                    switch (Path.GetExtension(filePath))
                    {
                        case ".html":
                            response.MimeType = "text/html";
                            break;
                        case ".css":
                            response.MimeType = "text/css";
                            break;
                        case ".js":
                            response.MimeType = "text/javascript";
                            break;
                        case ".png":
                            response.MimeType = "image/png";
                            break;
                        default:
                            response.MimeType = "application/octet-stream";
                            break;
                    }

                    requestCompletedCallback();
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }

    class AppSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new AppSchemeHandler();
        }

        public static string SchemeName { get { return "app"; } }
    }
}
