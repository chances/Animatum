using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animatum.SceneGraph;
using System.Drawing;
using System.Runtime.InteropServices;
using Animatum.SceneGraph.Serialization;
using Animatum.Animation;
using SharpGL.SceneGraph;
using System.Windows.Forms;

namespace Animatum.Controls
{
    [ComVisible(true)]
    public class TimelineScriptingObject
    {
        private bool timelineReady;

        public event EventHandler DOMReady;
        public event EventHandler ModelUpdated;
        public event EventHandler BeginPlayback;
        public event EventHandler PausePlayback;
        public event EventHandler StopPlayback;

        public TimelineScriptingObject(Model model)
        {
            this.model = model;

            timelineReady = false;
        }

        public Model model { get; set; }

        public bool isReady
        {
            get { return timelineReady; }
            set
            {
                if (value)
                {
                    timelineReady = true;

                    if (DOMReady != null)
                        DOMReady(this, new EventArgs());
                }
            }
        }

        public bool isModelLoaded { get { return model != null; } }

        public string getModel()
        {
            if (isModelLoaded)
            {
                return new JSONSerializer(model).Serialize();
            }
            else
                return null;
        }

        public void setModel(string model)
        {
            if (ModelUpdated != null)
                ModelUpdated(this, new EventArgs());
        }

        public float getCurrentTime()
        {
            return model.CurrentTime;
        }

        public Keyframe createKeyframe(string boneName, float time, int type, string vector)
        {
            KeyframeType typ = KeyframeType.Translation;
            if (type == 2) typ = KeyframeType.Rotation;
            string[] xyz = vector.Split(':');
            Vertex vertex = new Vertex(
                float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
            Keyframe keyframe = new Keyframe(time, typ, vertex);
            foreach (Bone bone in model.Bones)
            {
                if (bone.Name == boneName)
                {
                    bone.Animation.Add(keyframe);

                    if (ModelUpdated != null)
                        ModelUpdated(this, new EventArgs());

                    break;
                }
            }
            return keyframe;
        }

        public Keyframe getKeyframe(string boneName, int index)
        {
            foreach (Bone bone in model.Bones)
            {
                if (bone.Name == boneName)
                {
                    if (index > -1 && index < bone.Animation.Count)
                    {
                        return bone.Animation[index];
                    }
                }
            }
            return null;
        }

        public void setKeyframe(string boneName, int index, float time, int type, string transform)
        {
            KeyframeType typ = KeyframeType.Translation;
            if (type == 1) typ = KeyframeType.Rotation;
            string[] xyz = transform.Split(':');
            Vertex vertex;
            vertex = new Vertex(
                float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
            foreach (Bone bone in model.Bones)
            {
                if (bone.Name == boneName)
                {
                    if (index > -1 && index < bone.Animation.Count)
                    {
                        bone.Animation[index].Time = time;
                        bone.Animation[index].Type = typ;
                        bone.Animation[index].Transformation = vertex;
                        bone.Translation = new Vertex();
                        bone.Rotation = new Vertex();

                        if (ModelUpdated != null)
                            ModelUpdated(this, new EventArgs());
                    }
                }
            }
        }

        public void deleteKeyframe(string boneName, int index)
        {
            foreach (Bone bone in model.Bones)
            {
                if (bone.Name == boneName)
                {
                    if (index > -1 && index < bone.Animation.Count)
                    {
                        bone.Animation.RemoveAt(index);
                        bone.Translation = new Vertex();
                        bone.Rotation = new Vertex();

                        if (ModelUpdated != null)
                            ModelUpdated(this, new EventArgs());
                    }
                }
            }
        }

        public void setCurrentTime(float time)
        {
            model.CurrentTime = time;

            if (ModelUpdated != null)
                ModelUpdated(this, new EventArgs());
        }

        public bool getPlaybackEnabled()
        {
            foreach (Bone bone in model.Bones)
            {
                foreach (Keyframe keyframeI in bone.Animation)
                {
                    foreach (Keyframe keyframeV in bone.Animation)
                    {
                        if (keyframeI != keyframeV &&
                            keyframeI.Type == keyframeV.Type &&
                            keyframeI.Time != keyframeV.Time)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool getCanPlay()
        {
            if (getPlaybackEnabled())
            {
                int notEndedCount = 0;
                foreach (Bone bone in model.Bones)
                {
                    if (bone.Animation.Count > 0)
                    {
                        if (model.CurrentTime < bone.GetRightMostKeyframe().Time)
                            notEndedCount++;
                    }
                }
                if (notEndedCount == model.BonesWithKeyframesCount)
                    return true;
            }

            return false;
        }

        public void play()
        {
            if (BeginPlayback != null)
                BeginPlayback(this, new EventArgs());
        }

        public void pause()
        {
            if (PausePlayback != null)
                PausePlayback(this, new EventArgs());
        }

        public void stop()
        {
            if (StopPlayback != null)
                StopPlayback(this, new EventArgs());
        }

        public bool showConfirm(string title, string message, string def = "yes")
        {
            DialogResult result;
            if (def == "no")
                result = MessageBox.Show(message, title, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            else
                result = MessageBox.Show(message, title, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes) return true;
            return false;
        }

        public string getSystemColor(string name)
        {
            switch (name)
            {
                case "control":
                    return ColorTranslator.ToHtml(SystemColors.Control);
                case "controlDark":
                    return ColorTranslator.ToHtml(SystemColors.ControlDark);
                case "highlight":
                    return ColorTranslator.ToHtml(SystemColors.Highlight);
                case "highlightText":
                    return ColorTranslator.ToHtml(SystemColors.HighlightText);
            }

            return ColorTranslator.ToHtml(Color.Black);
        }
    }
}