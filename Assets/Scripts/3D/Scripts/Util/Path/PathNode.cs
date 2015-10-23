using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util {
    public class PathNode<T> {
        T node;
        List<PathNode<T>> neighbors = new List<PathNode<T>>();
        public bool isDestination;

        public virtual T Node { get { return node; } }
        public virtual List<PathNode<T>> Neighbors { get { return neighbors; } }
		
		protected PathNode() {}
        public PathNode(T node) : this(node, false) { }
		public PathNode(T node, bool isDestination) {
            this.node = node;
            this.isDestination = isDestination;
        }
    }
}
