using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace Animatum.Controls
{
    class DockToolPanelDesigner : ControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (this.Control is DockToolPanel)
                EnableDesignMode(((DockToolPanel)this.Control).Content, "Content");
        }
    }
}