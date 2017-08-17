using UnityEngine;
using System.Collections;
using Parabox.CSG;
public static class CSGOperations {
    public static GameObject Subtract(GameObject a, GameObject b, Material material) {
        Mesh m = CSG.Subtract(a,b);
		GameObject composite = new GameObject();
		composite.AddComponent<MeshFilter>().mesh = m;
		composite.AddComponent<MeshRenderer>().material = material;
        return composite;
    }
}