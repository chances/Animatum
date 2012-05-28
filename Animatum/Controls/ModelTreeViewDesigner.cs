using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace Animatum.Controls
{
    class ModelTreeViewDesigner : ControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
        }

        protected override void PostFilterProperties(System.Collections.IDictionary properties)
        {
            base.PostFilterProperties(properties);

            properties.Remove("ImageList");
            properties.Remove("Nodes");
        }
    }
}