using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animatum.Controls.ChromiumBrowserHandlers
{
    public class MenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {
            if (parameters.IsEditable)
                return true;

            return false;
        }
    }

    public class KeyboardHandler : IKeyboardHandler
    {
        /// <summary>
        /// Occurs when the user performed a keyboard shortcut.
        /// </summary>
        public event KeyCommandHandler KeyCommand;

        public delegate bool KeyCommandHandler(object sender, int key);

        public bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, bool isKeyboardShortcut)
        {
            bool block = false;
            if (KeyCommand != null)
                block = KeyCommand(this, windowsKeyCode);

            return block;
        }

        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey)
        {
            return false;
        }
    }

    public class JsDialogHandler : IJsDialogHandler
    {
        public bool OnJSAlert(IWebBrowser browser, string url, string message)
        {
            throw new NotImplementedException();
        }

        public bool OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload)
        {
            throw new NotImplementedException();
        }

        public bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval)
        {
            throw new NotImplementedException();
        }

        public bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result)
        {
            throw new NotImplementedException();
        }
    }
}
