using UnityEngine;
using System.Collections;
using ConstructiveSolidGeometry;
public static class CSGOperations {
    public static CSG Subtract(GameObject a, GameObject b) {
        CSG A = CSG.fromMesh(a.GetComponent<MeshFilter>().mesh, a.transform);
        CSG B = CSG.fromMesh(b.GetComponent<MeshFilter>().mesh, b.transform);
        CSG result = A.subtract(B);
        return result;
    }
}