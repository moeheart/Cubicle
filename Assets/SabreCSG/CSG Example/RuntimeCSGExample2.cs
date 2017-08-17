#define RUNTIME_CSG

using UnityEngine;
using System.Collections.Generic;
using Sabresaurus.SabreCSG;

public class RuntimeCSGExample2 : MonoBehaviour 
{
	// Number of cells to create in the X and Z axis
	[SerializeField]
	int cellXCount = 10;
	[SerializeField]
	int cellZCount = 10;

	// Size of each individual size
	[SerializeField]
	Vector3 cellSize = new Vector3(8,8,8);

	// 2D dimension array specifying whether each cell has a floor
	bool[,] cellVisitable;

	CSGModelBase csgModel;

#if RUNTIME_CSG

	void CreateTile(int x, int z)
	{
		Vector3 position = new Vector3(x * cellSize.x,0,z * cellSize.z);

		Vector3 brushPosition = position + new Vector3(0,-cellSize.y/2f + 0.5f,0);
		Vector3 brushSize = new Vector3(cellSize.x, 1, cellSize.z);
		// Create a floor if the cell is visitable 
		if(cellVisitable[x,z])
		{
			csgModel.CreateBrush(PrimitiveBrushType.Cube, brushPosition, brushSize);
		}

		// Create a wall in the X direction if it is visitable and on the edge, of if the visitable status has changed from the last cell
		if((x == 0 && cellVisitable[x,z]) || (x > 0 && cellVisitable[x,z] != cellVisitable[x-1,z]))
		{
			// Wall (X Negative)
			brushPosition = position + new Vector3(-cellSize.x/2f, 0,0);
			brushSize = new Vector3(1, cellSize.y-2, cellSize.z);
			csgModel.CreateBrush(PrimitiveBrushType.Cube, brushPosition, brushSize);
		}

		// Create a wall in the Z direction if it is visitable and on the edge, of if the visitable status has changed from the last cell
		if((z == 0 && cellVisitable[x,z]) || (z > 0 && cellVisitable[x,z] != cellVisitable[x,z-1]))
		{
			// Wall (Z Negative)
			brushPosition = position + new Vector3(0,0, -cellSize.z/2f);
			brushSize = new Vector3(cellSize.x, cellSize.y-2, 1);
			csgModel.CreateBrush(PrimitiveBrushType.Cube, brushPosition, brushSize);
		}

		// Create a wall on the opposite side of the cell if it is visitable and on the opposite boundary
		if(x == cellXCount-1 && cellVisitable[x,z])
		{
			// Wall (X Positive)
			brushPosition = position + new Vector3(+cellSize.x/2f, 0,0);
			brushSize = new Vector3(1, cellSize.y-2, cellSize.z);
			csgModel.CreateBrush(PrimitiveBrushType.Cube, brushPosition, brushSize);
		}

		// Create a wall on the opposite side of the cell if it is visitable and on the opposite boundary
		if(z == cellZCount-1 && cellVisitable[x,z])
		{
			// Wall (Z Positive)
			brushPosition = position + new Vector3(0,0, +cellSize.z/2f);
			brushSize = new Vector3(cellSize.x, cellSize.y-2, 1);
			csgModel.CreateBrush(PrimitiveBrushType.Cube, brushPosition, brushSize);
		}
	}

	void Start()
	{
		csgModel = gameObject.AddComponent<CSGModelBase>();

		// Generate a 2 dimensional array specifying whether a cell has a floor
		cellVisitable = new bool[cellXCount,cellZCount];
		for (int x = 0; x < cellXCount; x++) 
		{
			for (int z = 0; z < cellZCount; z++) 
			{
				cellVisitable[x,z] = (Random.Range(0f,1f) > 0.7f);
			}	
		}

		// Create brush geometry for each tile
		for (int x = 0; x < cellXCount; x++) 
		{
			for (int z = 0; z < cellZCount; z++) 
			{
				CreateTile(x, z);
			}	
		}
		// Finally build the brushes into geometry
		csgModel.Build(true, false);
	}

#endif
}
