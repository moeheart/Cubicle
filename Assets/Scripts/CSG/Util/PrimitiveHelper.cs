 using System.Collections.Generic;
 using UnityEngine;
 
 public static class PrimitiveHelper
 {
     private static Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();
 
     public static GameObject CreatePrimitive(PrimitiveType type, Material material)
     {
 
         GameObject gameObject = new GameObject(type.ToString());
         MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
         meshFilter.sharedMesh = PrimitiveHelper.GetPrimitiveMesh(type);
         gameObject.AddComponent<MeshRenderer>().material = material;
		 gameObject.AddComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
 
         return gameObject;
     }
 
     public static Mesh GetPrimitiveMesh(PrimitiveType type)
     {
         if (!PrimitiveHelper.primitiveMeshes.ContainsKey(type))
         {
             PrimitiveHelper.CreatePrimitiveMesh(type);
         }
 
         return PrimitiveHelper.primitiveMeshes[type];
     }
 
     private static Mesh CreatePrimitiveMesh(PrimitiveType type)
     {
         GameObject gameObject = GameObject.CreatePrimitive(type);
         Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
         GameObject.Destroy(gameObject);
 
         PrimitiveHelper.primitiveMeshes[type] = mesh;
         return mesh;
     }

	 public static void SetAsType (SceneObject sceneObj, PrimitiveType type) {
		 Mesh mesh = GetPrimitiveMesh(type);
		 sceneObj.GetComponent<MeshFilter>().sharedMesh = mesh;
		 sceneObj.GetComponent<MeshCollider>().sharedMesh = mesh;
	 }

     public static PrimitiveType ToType(string str) {
         if (str == "cube") {
             return PrimitiveType.Cube;
         }
         if (str == "sphere") {
             return PrimitiveType.Sphere;
         }
         if (str == "cylinder") {
             return PrimitiveType.Cylinder;
         }
         return PrimitiveType.Cube;
     }
 }
