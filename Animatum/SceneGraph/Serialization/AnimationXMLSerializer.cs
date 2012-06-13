using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Animatum.Animation;
using SharpGL.SceneGraph;

namespace Animatum.SceneGraph.Serialization
{
    class AnimationXMLSerializer : Serializer
    {
        private XmlDocument xmlDoc;
        private Settings.Settings settings;
        private string handedness, up;
        private string animationName;

        public AnimationXMLSerializer(Model model, String animationName)
        {
            this.Model = model;
            this.animationName = animationName;
            settings = new Settings.Settings();
            //Load handedness and up settings
            handedness = settings.GetSetting("xml/handedness", "right");
            up = settings.GetSetting("xml/up", "z");
            //Create document
            xmlDoc = new XmlDocument();
        }

        public override string Serialize()
        {
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><model/>");
            XmlNode model = xmlDoc.ChildNodes[1];
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns");
            xmlns.Value = "http://rct3.sourceforge.net/rct3xml/model";
            model.Attributes.Append(xmlns);
            //System handedness and up
            XmlElement system = xmlDoc.CreateElement("system");
            XmlAttribute handedness = xmlDoc.CreateAttribute("handedness");
            handedness.Value = this.handedness;
            XmlAttribute up = xmlDoc.CreateAttribute("up");
            up.Value = this.up;
            system.Attributes.Append(handedness);
            system.Attributes.Append(up);
            model.AppendChild(system);
            //Animation node
            XmlElement animation = xmlDoc.CreateElement("animation");
            XmlAttribute aniName = xmlDoc.CreateAttribute("name");
            aniName.Value = animationName;
            animation.Attributes.Append(aniName);
            //Bone nodes
            foreach (Bone bone in Model.Bones)
            {
                XmlElement boneElem = xmlDoc.CreateElement("bone");
                XmlAttribute boneName = xmlDoc.CreateAttribute("name");
                boneName.Value = bone.Name;
                boneElem.Attributes.Append(boneName);
                //If it has a parent bone assign it
                /* Importer doesn't like this :/
                if (!(bone.Parent is Model))
                {
                    if (bone.Parent is Bone)
                    {
                        XmlAttribute parent = xmlDoc.CreateAttribute("parent");
                        parent.Value = ((Bone)bone.Parent).Name;
                        boneElem.Attributes.Append(parent);
                    }
                }*/
                //Get all keyframes sorted by time
                List<Keyframe> keyframes = bone.Animation.OrderBy(o => o.Time).ToList();
                //Translations should only occur before rotations
                //Add translate keyframes
                foreach (Keyframe keyframe in keyframes)
                    if (keyframe.Type == KeyframeType.Translation)
                        boneElem.AppendChild(createKeyframe(keyframe));
                //Add rotate keyframes
                foreach (Keyframe keyframe in keyframes)
                    if (keyframe.Type == KeyframeType.Rotation)
                        boneElem.AppendChild(createKeyframe(keyframe));
                //Add bone to animation, if there's animation for the bone
                if (bone.Animation.Count > 0)
                    animation.AppendChild(boneElem);
            }
            model.AppendChild(animation);

            return xmlDoc.OuterXml;
        }

        public override Model Deserialize(string document)
        {
            xmlDoc.LoadXml(document);
            try
            {
                XmlNode model = xmlDoc.GetElementsByTagName("model")[0];
                //Look for an animation
                foreach (XmlNode child in model.ChildNodes)
                {
                    if (child.Name == "animation")
                    {
                        //Get bones
                        XmlNodeList bones = child.ChildNodes;
                        foreach (XmlNode bone in bones)
                        {
                            if (bone.Name == "bone")
                            {
                                //Get bone name
                                string name = bone.Attributes["name"].Value;
                                string parent = null;
                                if (bone.Attributes["parent"] != null)
                                    parent = bone.Attributes["parent"].Value;
                                //Get the model's counterpart
                                Bone modelBone = null;
                                foreach (Bone mBone in Model.Bones)
                                {
                                    if (mBone.Name == name)
                                    {
                                        modelBone = mBone;
                                        break;
                                    }
                                }
                                if (modelBone != null)
                                {
                                    //Try to assign a parent
                                    if (parent != null)
                                    {
                                        foreach (Bone mBone in Model.Bones)
                                        {
                                            if (modelBone.Name == parent)
                                            {
                                                modelBone.Parent = mBone;
                                            }
                                        }
                                    }
                                    modelBone.Animation.Clear();
                                    //Assign keyframes
                                    foreach (XmlNode node in bone.ChildNodes)
                                    {
                                        modelBone.Animation.Add(parseKeyframe(node));
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch
            {
                return null;
            }

            return Model;
        }

        public void Save(string filename)
        {
            this.Serialize();
            xmlDoc.Save(filename);
        }

        private XmlElement createKeyframe(Keyframe keyframe)
        {
            string type = "translate";
            if (keyframe.Type == KeyframeType.Rotation)
                type = "rotate";
            XmlElement keyframeElem = xmlDoc.CreateElement(type);
            XmlAttribute timeAttr = xmlDoc.CreateAttribute("time");
            timeAttr.Value = keyframe.Time.ToString();
            //Get transformation values
            float x = keyframe.Transformation.X;
            float y = keyframe.Transformation.Y;
            float z = keyframe.Transformation.Z;
            //If rotation, convert to radians
            if (keyframe.Type == KeyframeType.Rotation)
            {
                x = Convert.degreesToRadians(x);
                y = Convert.degreesToRadians(y);
                z = Convert.degreesToRadians(z);
            }
            XmlAttribute xAttr = xmlDoc.CreateAttribute("x");
            xAttr.Value = x.ToString();
            XmlAttribute yAttr = xmlDoc.CreateAttribute("y");
            yAttr.Value = y.ToString();
            XmlAttribute zAttr = xmlDoc.CreateAttribute("z");
            zAttr.Value = z.ToString();
            keyframeElem.Attributes.Append(timeAttr);
            keyframeElem.Attributes.Append(xAttr);
            keyframeElem.Attributes.Append(yAttr);
            keyframeElem.Attributes.Append(zAttr);

            return keyframeElem;
        }

        private Keyframe parseKeyframe(XmlNode node)
        {
            //Get type
            KeyframeType type;
            if (node.Name == "translate")
                type = KeyframeType.Translation;
            else if (node.Name == "rotate")
                type = KeyframeType.Rotation;
            else
                return null;
            //Get time
            float time = float.Parse(node.Attributes["time"].Value);
            //Get transform
            float x = float.Parse(node.Attributes["x"].Value);
            float y = float.Parse(node.Attributes["y"].Value);
            float z = float.Parse(node.Attributes["z"].Value);
            //If it's a rotation, convert to degrees
            if (type == KeyframeType.Rotation)
            {
                x = (float)Math.Round(Convert.radiansToDegrees(x), 2);
                y = (float)Math.Round(Convert.radiansToDegrees(y), 2);
                z = (float)Math.Round(Convert.radiansToDegrees(z), 2);
            }

            //Return created keyframe
            return new Keyframe(time, type, new Vertex(x, y, z));
        }
    }
}