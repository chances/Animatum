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
            if (msg.Msg == 0x101    //WM_KEYUP 
                    && msg.WParam.ToInt32() > 0 &&
                    (ModifierKeys == Keys.Control ||
                      (ModifierKeys == Keys.Control && ModifierKeys == Keys.Shift)))
            {
                Debug.WriteLine("Key: " + msg.WParam.ToInt32());
                //this.Parent.
                return true;
            } 
            return base.PreProcessMessage(ref msg);
        }
    }
}