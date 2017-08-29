using UnityEngine;
using System.Collections;
using Parabox.CSG;
public static class CSGUtil {


    public static void Subtract(GameObject a, GameObject b) {
        CSGObject obj = a.GetComponent<CSGObject>();
        GameObject[] slaves = new GameObject[2] {a, b};
        obj.PerformCSG(CsgOperation.ECsgOperation.CsgOper_Subtractive, slaves);
        a.name = "(" + a.name + ") - (" + b.name + ")";
    }

    public static void Union(GameObject a, GameObject b) {
        CSGObject obj = a.GetComponent<CSGObject>();
        GameObject[] slaves = new GameObject[2] {a ,b};
        obj.PerformCSG(CsgOperation.ECsgOperation.CsgOper_Additive, slaves);
        a.name = "(" + a.name + ") + (" + b.name + ")";
    }

    public static void Intersect(GameObject a, GameObject b) {
        CSGObject obj = a.GetComponent<CSGObject>();
        GameObject[] slaves = new GameObject[2] {a, b};
        obj.PerformCSG(CsgOperation.ECsgOperation.CsgOper_Intersect, slaves);
        a.name = "(" + a.name + ") x (" + b.name + ")"; 
    }

    /*public static GameObject Subtract(GameObject a, GameObject b, Material material = null) {
        Mesh m = CSG.Subtract(a,b);
		GameObject composite = new GameObject();
		composite.AddComponent<MeshFilter>().sharedMesh = m;
		composite.AddComponent<MeshRenderer>().sharedMaterial = material;
        composite.AddComponent<MeshCollider>().sharedMesh = m;
        composite.AddComponent<ObjectBehaviors>();
        return composite;
    }
    public static GameObject Intersect(GameObject a, GameObject b, Material material = null) {
        Mesh m = CSG.Intersect(a,b);
		GameObject composite = new GameObject();
		composite.AddComponent<MeshFilter>().sharedMesh = m;
		composite.AddComponent<MeshRenderer>().sharedMaterial = material;
        composite.AddComponent<MeshCollider>().sharedMesh = m;
        composite.AddComponent<ObjectBehaviors>();
        return composite;
    }
    public static GameObject Union(GameObject a, GameObject b, Material material = null) {
        Mesh m = CSG.Union(a,b);
		GameObject composite = new GameObject();
		composite.AddComponent<MeshFilter>().sharedMesh = m;
		composite.AddComponent<MeshRenderer>().sharedMaterial = material;
        composite.AddComponent<MeshCollider>().sharedMesh = m;
        composite.AddComponent<ObjectBehaviors>();
        return composite;
    }*/

    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
    public static float VolumeOfMesh(GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
}