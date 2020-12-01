﻿using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public IntVector2 coordinates;
	public MazeRoom room;

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
	private int initializedEdgeCount;

	public bool IsFullyInitialized { get { return initializedEdgeCount == MazeDirections.Count; } }

	//Sets up the cell
	public void Initialize(MazeRoom room)
	{
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	//Returns random uninitialized direction
	public MazeDirection RandomUninitializedDirection
	{
		get
		{
			int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);

			for (int i = 0; i < MazeDirections.Count; i++)
			{
				if (edges[i] == null)
				{
					if (skips == 0)
						return (MazeDirection)i;

					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

	//Sets an edge of the cell
	public void SetEdge(MazeDirection direction, MazeCellEdge edge)
	{
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	//Gets an edge of the cell
	public MazeCellEdge GetEdge(MazeDirection direction)
	{
		return edges[(int)direction];
	}
}