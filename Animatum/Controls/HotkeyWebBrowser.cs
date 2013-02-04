using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace Animatum.Controls
{
    public class HotkeyWebBrowser : WebBrowser
    {
        /// <summary>
        /// Occurs when the user performed a keyboard shortcut.
        /// </summary>
        [Category("Key")]
        [Description("Occurs when the user performed a keyboard shortcut.")]
        public event KeyCommandHandler KeyCommand;

        public delegate bool KeyCommandHandler(object sender, int key);

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == 0x100    //WM_KEYDOWN 
                    && msg.WParam.ToInt32() > 0)
            {
                bool block = false;
                int key = msg.WParam.ToInt32();
                    if (KeyCommand != null)
                        block = KeyCommand(this, key);

                if (block)
                {
                    return true;
                }
            }
            return base.PreProcessMessage(ref msg);
        }
    }
}