using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class PriorityQueue<P, V>
    {
        private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
        
		 public bool IsEmpty {
	        get { 
				return list.Count == 0; 
			}
        }
		
		public int Count {
			get{
				int count = 0;
				foreach(var item in list.Values){
					count += item.Count;
				}
				return count;
			}
		}
		
		/*public void Remove(V value){
			foreach(var key in list.Keys){
				if(list[key].Contains(value)){
					var queue = new Queue<V>();
					//foreach(
					//list[key] = 
				}
			}
		}*/
		
        public void Enqueue(P priority, V value) {
            Queue<V> q;
            if (!list.TryGetValue(priority, out q)) {
                q = new Queue<V>();
                list.Add(priority, q);
            }
            q.Enqueue(value);
        }
        
        public V Dequeue() {
            // will throw if there isn’t any first element!
            var enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            KeyValuePair<P, Queue<V>> pair =  enumerator.Current;//new KeyValuePair<P, Queue<V>>(list.Keys);
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                list.Remove(pair.Key);
            return v;
        }
		
		public Queue<V> DequeueAll() {
            // will throw if there isn’t any first element!
            var enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            KeyValuePair<P, Queue<V>> pair = enumerator.Current;//new KeyValuePair<P, Queue<V>>(list.Keys);
            list.Remove(pair.Key);
            return pair.Value;
        }

        public P Peek() {
            var enumerator = list.GetEnumerator();
            if (!enumerator.MoveNext()) {
				return default(P);
			}
            return enumerator.Current.Key;
        }
		
		public override string ToString ()
		{
			var s = "";
			foreach(var key in list.Keys){
				foreach(var val in list[key]){
					s += key + ": " +  val.ToString() + "; ";
				}
				s += "\n";
			}
			return s;
		}
    }
}
