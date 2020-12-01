using UnityEngine;

public class MazeWall : MazeCellEdge
{
	public Renderer wall;

	//Sets up the wall
	public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		base.Initialize(cell, otherCell, direction);
		wall.material = cell.room.settings.wallMaterial;

		if (direction == MazeDirection.South)
			cell.room.AddSouthWall(this);
	}
}