using UnityEngine;
using System.Collections;

public struct IntVector3 {
	public int x, z, y;
	
	public IntVector3 (int x, int z, int y) {
		this.x = x;
		this.z = z;
		this.y = y;
	}

	public IntVector3 (IntVector2 v, int y) {
		this.x = v.x;
		this.z = v.z;
		this.y = y;
	}

	public static IntVector3 operator + (IntVector3 a, IntVector3 b) {
		a.x += b.x;
		a.z += b.z;
		a.y += b.y;
		return a;
	}


	public static bool operator == (IntVector3 a, IntVector3 b) {
		return a.x == b.x && a.z == b.z && a.y == b.y;
	}

	public static bool operator != (IntVector3 a, IntVector3 b) {
		return !(a==b);
	}

	public override string ToString() {
		return "(" + x + "," + z + "," + y + ")";
	}

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ z.GetHashCode() ^ y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Equals((IntVector3)obj);
    }

	public bool Equals(IntVector3 other)
    {
        return other.x.Equals(x) && other.z.Equals(z) && other.y.Equals(y);
    }

}
