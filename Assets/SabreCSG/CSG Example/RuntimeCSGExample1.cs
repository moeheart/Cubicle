#define RUNTIME_CSG

using UnityEngine;
using System.Collections.Generic;
using Sabresaurus.SabreCSG;

public class RuntimeCSGExample1 : MonoBehaviour 
{
	[SerializeField]
	int count = 30; // Number of brushes to create

	[SerializeField]
	Material addMaterial; // Material on additive brushes

	[SerializeField]
	Material subtractMaterial; // Material on subtractive brushes


#if RUNTIME_CSG
	CSGModelBase csgModel;

	void Start()
	{
		// Create a CSG Model component
		csgModel = gameObject.AddComponent<CSGModelBase>();

		// Create a number of brushes
		for (int i = 0; i < count; i++) 
		{
			// Random position for brush
			Vector3 localPosition = Random.insideUnitCircle * 100;
			// All brushes same size
			Vector3 localSize = new Vector3(20,16,20);

			// Random chance of the brush being subtractive
			CSGMode csgMode = CSGMode.Add;
			Material material = addMaterial;
			if(Random.Range(0, 1f) > 0.7f)
			{
				csgMode = CSGMode.Subtract;
				material = subtractMaterial;
			}

			// Create a brush using specified settings
			GameObject newObject = csgModel.CreateBrush(PrimitiveBrushType.Cube, localPosition, localSize, Quaternion.identity, material, csgMode);
			PrimitiveBrush activeBrush = newObject.GetComponent<PrimitiveBrush>();

			// Random chance of moving two of the vertices
			if(Random.Range(0,1f) > 0.7f)
			{
		 		// Pick out the bottom two vertices of the left face (see BrushFactory.GenerateCube() for indexes)
				Polygon[] polygons = activeBrush.GetPolygons();
				List<Vertex> specifiedVertices = new List<Vertex>()
				{
					polygons[1].Vertices[0],
					polygons[1].Vertices[1],
				};
				// Translate those two vertices (and any vertices) that share their position
				VertexUtility.TranslateSpecifiedVertices(activeBrush, specifiedVertices, new Vector3(-10, 0, 0));
			}

			// Random chance of applying a clip plane to the brush
			if(Random.Range(0,1f) > 0.7f)
			{
				ClipUtility.ApplyClipPlane(activeBrush, new Plane(new Vector3(1,2,1), 0), false);
			}
		}

		// Build all the brushes
		csgModel.Build(true, false);
	}

#endif
}
