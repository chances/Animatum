using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Animatum.Controls
{
    class HotkeyWebBrowser : WebBrowser
    {
        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == 0x100    //WM_KEYDOWN 
                    && msg.WParam.ToInt32() > 0)
            {
                bool block = false;
                int key = msg.WParam.ToInt32();
                const int KEY_O = 79;
                if (ModifierKeys == Keys.Control && key == KEY_O)
                    block = true;

                //Debug.WriteLine("Key: " + msg.WParam.ToInt32());

                if (block)
                {
                    //msg.HWnd = this.Parent.Parent.Handle;
                    //WndProc(ref msg);
                    return true;
                }
            }
            return base.PreProcessMessage(ref msg);
        }
    }
}