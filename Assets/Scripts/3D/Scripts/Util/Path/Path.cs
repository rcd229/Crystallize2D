using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Util
{	
	
    public class Path<T> : IEnumerable<T>
    {
        public PathNode<T> LastStep { get; private set; }
        public Path<T> PreviousSteps { get; private set; }
        public float TotalCost { get; private set; }

        private Path(PathNode<T> lastStep, Path<T> previousSteps, float totalCost) {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        public Path(PathNode<T> start) : this(start, null, 0) { }

        public Path<T> AddStep(PathNode<T> step, float stepCost) {
            return new Path<T>(step, this, TotalCost + stepCost);
        }

        #region IEnumerable<T> Members

        IEnumerator IEnumerable.GetEnumerator() {
            for (Path<T> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            for (Path<T> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep.Node;
        }

        #endregion

        public static Path<T> FindPath(PathNode<T> origin, Func<T, T, float> costF, Func<T, float> estimateF) {
            var queue = new PriorityQueue<float, Path<T>>();
            var closed = new HashSet<PathNode<T>>();

            queue.Enqueue(0, new Path<T>(origin));

            while (!queue.IsEmpty) {
                Path<T> path = queue.Dequeue();
                if (closed.Contains(path.LastStep)) {
					continue;
				}
                if (path.LastStep.isDestination) {
					return path;
				}
                closed.Add(path.LastStep);
                foreach (var n in path.LastStep.Neighbors) {
                    float cost = costF(path.LastStep.Node, n.Node); 
                    var newPath = path.AddStep(n, cost);
                    queue.Enqueue(newPath.TotalCost + estimateF(n.Node), newPath);
                }
            }

            return null;
        }
		
		public override string ToString ()
		{
			return string.Format ("[Path: LastStep={0}, TotalCost={1}]", LastStep.Node, TotalCost);
		}
    }
}
