using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject
{
	public int settingsIndex;
	public MazeRoomSettings settings;

	private List<MazeCell> cells = new List<MazeCell>();
	private List<MazeCellEdge> southWalls = new List<MazeCellEdge>();
	
	private static MazeRoom currentRoom;

	//Adds a cell to the room
	public void Add(MazeCell cell)
	{
		cell.room = this;
		cells.Add(cell);
	}

	//Adds the wall to the list of south walls
	public void AddSouthWall(MazeCellEdge wall)
    {
		southWalls.Add(wall);
	}

	//Assimilates room
	public void Assimilate(MazeRoom room)
	{
		for (int i = 0; i < room.cells.Count; i++)
			Add(room.cells[i]);
	}

	//Reshows the walls in the previous room hides the walls in the current room
	public static void ChangeRoom(MazeRoom newRoom)
    {
		if (currentRoom != null)
			foreach (MazeCellEdge wall in currentRoom.southWalls)
				wall.ShowWall();

		foreach (MazeCellEdge wall in newRoom.southWalls)
			wall.HideWall();

		currentRoom = newRoom;
	}
}