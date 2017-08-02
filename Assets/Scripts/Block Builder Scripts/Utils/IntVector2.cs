using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IntVector2 {

	public int x, z;
	
	public IntVector2 (int x, int z) {
		this.x = x;
		this.z = z;
	}

	public static IntVector2 operator + (IntVector2 a, IntVector2 b) {
		a.x += b.x;
		a.z += b.z;
		return a;
	}

	public static bool operator == (IntVector2 a, IntVector2 b) {
		return a.x == b.x && a.z == b.z;
	}

	public static bool operator != (IntVector2 a, IntVector2 b) {
		return !(a==b);
	}

	public override string ToString() {
		return "(" + x + "," + z + ")";
	}

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ z.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Equals((IntVector2)obj);
    }

	public bool Equals(IntVector2 other)
    {
        return other.x.Equals(x) && other.z.Equals(z);
    }

}
