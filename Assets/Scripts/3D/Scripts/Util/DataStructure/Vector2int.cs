using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Util {

    [Serializable]
    public sealed class Vector2int {
        public readonly int x;
        public readonly int y;

        private static readonly Vector2int zero;
        private static readonly Vector2int up;
        private static readonly Vector2int down;
        private static readonly Vector2int left;
        private static readonly Vector2int right;
        private static readonly Vector2int one;

        public static Vector2int Zero { get { return zero; } }
        public static Vector2int Up { get { return up; } }
        public static Vector2int Down { get { return down; } }
        public static Vector2int Left { get { return left; } }
        public static Vector2int Right { get { return right; } }
        public static Vector2int One { get { return one; } }

        public int Max {
            get {
                return Mathf.Max(x, y);
            }
        }

        public static Vector2int[] Planar8 {
            get {
                return new Vector2int[]{
					Up, Up + Left, Left, Left + Down,
					Down, Down + Right, Right, Right + Up};
            }
        }

        public static Vector2int[] Planar4 {
            get {
                return new Vector2int[]{
					Up, Left, Down, Right};
            }
        }

        //public Vector3int[] Neighbours { get { return VoxelMap.GetPathableNeighbors(this); } }
        static Vector2int() {
            zero = new Vector2int(0, 0);
            up = new Vector2int(0, 1);
            down = new Vector2int(0, -1);
            left = new Vector2int(-1, 0);
            right = new Vector2int(1, 0);
            one = new Vector2int(1, 1);
        }

        public Vector2int() { }

        public Vector2int(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public Vector2int(Vector2 vec) : this((int)vec.x, (int)vec.y) { }

        public override int GetHashCode() {
            return x.GetHashCode() * 17 + y.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;

            Vector2int p = obj as Vector2int;
            if ((System.Object)p == null) return false;

            if (x != p.x) return false;
            if (y != p.y) return false;
            return true;
        }

        public static int SquareDistance(Vector2int a, Vector2int b) {
            return (a.x - b.x).Squared() + (a.y - b.y).Squared();
        }

        public static int BlockDistance(Vector2int a, Vector2int b) {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static Vector2int operator +(Vector2int left, Vector2int right) {
            return new Vector2int(left.x + right.x, left.y + right.y);
        }

        public static Vector2int operator -(Vector2int left, Vector2int right) {
            return new Vector2int(left.x - right.x, left.y - right.y);
        }

        public static Vector2int operator *(Vector2int left, int right) {
            return new Vector2int(left.x * right, left.y * right);
        }

        public static Vector2int operator *(int left, Vector2int right) {
            return right * left;
        }

        public static Vector2 operator *(float left, Vector2int right) {
            return new Vector2(left * right.x, left * right.y);
        }

        public static bool operator ==(Vector2int a, Vector2int b) {
            if (System.Object.ReferenceEquals(a, b)) return true;

            if (((object)a == null) || ((object)b == null)) return false;

            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2int a, Vector2int b) {
            return !(a == b);
        }

        public override string ToString() {
            return string.Format("({0}, {1})", x, y);
        }

        public Vector2 ToVector2() {
            return new Vector3((float)x, (float)y);
        }

        public static Vector2int FromVector2(Vector2 vec) {
            return new Vector2int((int)vec.x, (int)vec.y);
        }

        public Vector2int[] SurroundingPlanar4() {
            var surr = new Vector2int[4];
            surr[0] = this + left;
            surr[1] = this + right;
            surr[2] = this + up;
            surr[3] = this + down;
            return surr;
        }

        public Vector2int[] SurroundingPlanar4Diagonal() {
            var surr = new Vector2int[4];
            surr[0] = this + left + up;
            surr[1] = this + left + down;
            surr[2] = this + right + up;
            surr[3] = this + right + down;
            return surr;
        }

        public Vector2int[] SurroundingPlanar8() {
            var surr = new Vector2int[8];
            surr[0] = this + left;
            surr[1] = this + right;
            surr[2] = this + up;
            surr[3] = this + down;
            surr[4] = this + left + up;
            surr[5] = this + left + down;
            surr[6] = this + right + up;
            surr[7] = this + right + down;
            return surr;
        }

        public static Vector2int Cross(Vector2int v1, Vector2int v2) {
            return new Vector2int(
                v1.y * v2.x - v1.x * v2.y,
                v1.x * v2.y - v1.y * v2.x);
        }

        public static Vector2int Snap(Vector2 v2) {
            return new Vector2int(Mathf.RoundToInt(v2.x),
                Mathf.RoundToInt(v2.y));
        }

        public static Vector2int Snap(float x, float y) {
            return new Vector2int(Mathf.RoundToInt(x),
                                  Mathf.RoundToInt(y));
        }

        public static Vector2int FromVector3intXZ(Vector3int v3) {
            return new Vector2int(v3.x, v3.z);
        }

    }
}
