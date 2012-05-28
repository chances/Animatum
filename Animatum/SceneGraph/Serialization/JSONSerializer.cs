using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Animatum.Animation;

namespace Animatum.SceneGraph.Serialization
{
    class JSONSerializer : Serializer
    {
        private JavaScriptSerializer serializer;

        public JSONSerializer(Model model)
        {
            this.Model = model;
            serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            serializer.RecursionLimit = 35;
        }

        public override string Serialize()
        {
            return serializer.Serialize(Model);
        }

        public override Model Deserialize(string json)
        {
            return serializer.Deserialize<Model>(json);
        }
    }
}