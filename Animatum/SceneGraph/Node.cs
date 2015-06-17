using System.Collections.Generic;
using SharpGL;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A node of the SceneGraph.
    /// </summary>
    [ComVisible(true)]
    public abstract class Node
    {
        protected Node parent;
        protected List<Node> children;

        /// <summary>
        /// Construct a new Node
        /// </summary>
        public Node()
        {
            Visible = true;
            parent = null;
            children = new List<Node>();
        }

        public string Type
        {
            get { return this.GetType().Name.ToLower(); }
        }

        /// <summary>
        /// Whether or not this Node is visible
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The parent of this Node
        /// </summary>
        [ScriptIgnore()]
        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Children belonging to this Node
        /// </summary>
        public List<Node> Children
        {
            get { return children; }
            set { children = value; }
        }

        public bool HasChildren
        {
            get { return children.Count > 0; }
        }

        /// <summary>
        /// Add a child to this Node.
        /// </summary>
        /// <param name="node">Node to add</param>
        public void Add(Node node)
        {
            children.Add(node);
            node.Parent = this;
        }

        /// <summary>
        /// Remove a child from this Node.
        /// </summary>
        /// <param name="node">Node to remove</param>
        /// <returns>Removed Node</returns>
        public Node Remove(Node node)
        {
            node.Parent = null;
            children.Remove(node);
            return node;
        }

        /// <summary>
        /// Get the root node of the scene graph containing this node
        /// </summary>
        /// <returns>The root node</returns>
        public Node GetRootNode()
        {
            Node node = this;
            while (node.Parent != null)
                node = node.Parent;
            return node;
        }

        /// <summary>
        /// Render the node to the scene
        /// </summary>
        /// <param name="gl">OpenGL object</param>
        public abstract void Render(OpenGL gl);

		/// <summary>
		/// Renders the node for hit testing.
		/// </summary>
		/// <param name="gl">OpenGL object</param>
		/// <param name="hitMap">Hit test result map</param>
		/// <param name="currentName">Current hit name</param>
		public abstract void RenderForHitTest(OpenGL gl, Dictionary<uint, Node> hitMap, ref uint currentName);
    }
}