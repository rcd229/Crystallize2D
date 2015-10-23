using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Util {
	public enum Cardinal{
		North, 
		South, 
		East, 
		West
	}
	
	[Serializable]
    public struct Vector3int {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        private static readonly Vector3int zero;
        private static readonly Vector3int up;
        private static readonly Vector3int down;
        private static readonly Vector3int left;
        private static readonly Vector3int right;
        private static readonly Vector3int forward;
        private static readonly Vector3int back;
		private static readonly Vector3int one;
		private static readonly Dictionary<Cardinal, Vector3int> direction;
		private static readonly Dictionary<Vector3int, Cardinal> compass;

        public static Vector3int Zero { get { return zero; } }
        public static Vector3int Up { get { return up; } }
        public static Vector3int Down { get { return down; } }
        public static Vector3int Left { get { return left; } }
        public static Vector3int Right { get { return right; } }
        public static Vector3int Forward { get { return forward; } }
        public static Vector3int Back { get { return back; } }
		public static Vector3int One { get { return one; } }
		
		public int Max{
			get{
				return Mathf.Max(x, y, z);
			}
		}
		
		public static Dictionary<Cardinal, Vector3int> Direction{get{return direction;}}
		public static Dictionary<Vector3int, Cardinal> Compass{get{return compass;}}
		
		public static Vector3int[] Planar8 {
			get{
				return new Vector3int[]{
					Forward, Forward + Left, Left, Left + Back,
					Back, Back + Right, Right, Right + Forward};
			}
		}
		
		public static Vector3int[] Planar4 {
			get{
				return new Vector3int[]{
					Forward, Left, Back, Right};
			}
		}

        //public Vector3int[] Neighbours { get { return VoxelMap.GetPathableNeighbors(this); } }
        static Vector3int() {
            zero = new Vector3int(0, 0, 0);
            up = new Vector3int(0, 1, 0);
            down = new Vector3int(0, -1, 0);
            left = new Vector3int(-1, 0, 0);
            right = new Vector3int(1, 0, 0);
            forward = new Vector3int(0, 0, 1);
            back = new Vector3int(0, 0, -1);
			one = new Vector3int(1, 1, 1);
			
			direction = new Dictionary<Cardinal, Vector3int>();
			compass = new Dictionary<Vector3int, Cardinal>();
			direction[Cardinal.North] = forward;
			direction[Cardinal.South] = back;
			direction[Cardinal.East] = left;
			direction[Cardinal.West] = right;
			foreach(var pair in direction) compass[pair.Value] = pair.Key;
        }
		
		//public Vector3int(){}

        public Vector3int(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
		public Vector3int(Vector3 vec):this((int)vec.x, (int)vec.y, (int)vec.z){}
		
        public override int GetHashCode() {
			int hash = 17;
			// Maybe nullity checks, if these are objects not primitives!
			hash = hash * 23 + z.GetHashCode();
			hash = hash * 23 + x.GetHashCode();
			hash = hash * 23 + y.GetHashCode();
			return hash;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;

			if (!(obj is Vector3int)) {
				return false;
			}

            Vector3int p = (Vector3int)obj;
            if ((System.Object)p == null) return false;

            if (x != p.x) return false;
            if (y != p.y) return false;
            if (z != p.z) return false;
            return true;
        }
		
		public static int SquareDistance(Vector3int a, Vector3int b){
			return (a.x - b.x).Squared() + (a.y - b.y).Squared() + (a.z - b.z).Squared();
		}
		
        public static int BlockDistance(Vector3int a, Vector3int b) {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
        }

        public static Vector3int operator +(Vector3int left, Vector3int right) {
            return new Vector3int(left.x + right.x, left.y + right.y, left.z + right.z);
        }
		
		public static Vector3int operator -(Vector3int left, Vector3int right) {
            return new Vector3int(left.x - right.x, left.y - right.y, left.z - right.z);
        }
		
		public static Vector3int operator *(Vector3int left, int right) {
            return new Vector3int(left.x * right, left.y * right, left.z * right);
        }
		public static Vector3int operator *(int left, Vector3int right) {
            return right * left;
        }

        public static bool operator ==(Vector3int a, Vector3int b) {
            if (System.Object.ReferenceEquals(a, b)) return true;

            if (((object)a == null) || ((object)b == null)) return false;

            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3int a, Vector3int b) {
            return !(a == b);
        }
		
		public override string ToString ()
		{
			return string.Format ("({0}, {1}, {2})", x, y, z);
		}
		
		public Vector3 ToVector3(){
			return new Vector3((float)x, (float)y, (float)z);
		}
		
		public Vector3int Flatten(){
			return new Vector3int(x, 0, z);
		}
		
		public static Vector3int FromVector3(Vector3 vec){
			return new Vector3int((int)vec.x, (int)vec.y,(int)vec.z);
		}
		
		public Vector3int[] SurroundingPlanar4(){
			var surr = new Vector3int[4];
			surr[0] = this + left;
			surr[1] = this + right;
			surr[2] = this + forward;
			surr[3] = this + back;
			return surr;
		}
		
		public Vector3int[] SurroundingPlanar4Diagonal(){
			var surr = new Vector3int[4];
			surr[0] = this + left + forward;
			surr[1] = this + left + back;
			surr[2] = this + right + forward;
			surr[3] = this + right + back;
			return surr;
		}
		
		public static Vector3int[] QuadBetween(Vector3int first, Vector3int second){
			var array = new Vector3int[2];
			var offset = second - first;
			array[0] = first + new Vector3int(offset.x, 0, 0);
			array[1] = first + new Vector3int(0, 0, offset.z);
			return array;
		}
		
		public Vector3int[] SurroundingPlanar8(){
			var surr = new Vector3int[8];
			surr[0] = this + left;
			surr[1] = this + right;
			surr[2] = this + forward;
			surr[3] = this + back;
			surr[4] = this + left + forward;
			surr[5] = this + left + back;
			surr[6] = this + right + forward;
			surr[7] = this + right + back;
			return surr;
		}

		public Vector3int[] Surrounding6(){
			return new Vector3int[]{
				this + left,
				this + right,
				this + forward,
				this + back,
				this + up,
				this + down
			};
		}
		
		public static Vector3int Cross(Vector3int v1, Vector3int v2){
			return new Vector3int(
				v1.y * v2.z - v1.z * v2.y,
				v1.z * v2.x - v1.x * v2.z,
				v1.x * v2.y - v1.y * v2.x);
		}
		
		public static Vector3int Snap(Vector3 v3){
			return new Vector3int(Mathf.RoundToInt(v3.x),
				Mathf.RoundToInt(v3.y),
				Mathf.RoundToInt(v3.z));
		}
		
    }
}
