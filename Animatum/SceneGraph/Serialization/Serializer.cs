using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animatum.SceneGraph.Serialization
{
    abstract class Serializer
    {
        public Model Model { get; set; }

        public abstract string Serialize();
        public abstract Model Deserialize(string model);
    }
}