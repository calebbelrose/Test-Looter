using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour 
{
	public MazeCell cellPrefab;
	public IntVector2 size;
	public MazePassage passagePrefab;
	public MazeWall[] wallPrefabs;
	public MazeRoomSettings[] roomSettings;
	public List<GameObject> enemies = new List<GameObject>();
	public MazeDoor doorPrefab;
	[Range(0f, 1f)] public float doorProbability;
	[Range(0f, 1f)] public float enemyProbability;

	private List<Vector3> enemyLocations = new List<Vector3>();
	private MazeRoom firstRoom;
	private MazeCell[,] cells;
	private List<MazeRoom> rooms = new List<MazeRoom>();

	//Creates room
	private MazeRoom CreateRoom(int indexToExclude)
	{
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();

		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);

		if (newRoom.settingsIndex == indexToExclude)
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;

		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	//Does first generation step
	private void DoFirstGenerationStep(List<MazeCell> activeCells)
	{
		MazeCell newCell = CreateCell(RandomCoordinates);

		firstRoom = CreateRoom(-1);
		newCell.Initialize(firstRoom);
		activeCells.Add(newCell);
	}

	//Creates passage
	private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;

		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;

		if (passage is MazeDoor)
			otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
		else
			otherCell.Initialize(cell.room);

		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	//Gets cell
	public MazeCell GetCell(IntVector2 coordinates)
	{
		return cells[coordinates.x, coordinates.y];
	}

	//Generates 
	public void Generate()
	{
		List<MazeCell> activeCells = new List<MazeCell>();

		cells = new MazeCell[size.x, size.y];
		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0)
			DoNextGenerationStep(activeCells);

		MazeRoom.ChangeRoom(firstRoom);
	}

	//Spawns enemies
	public void SpawnEnemies()
	{
		foreach (Vector3 location in enemyLocations)
			Instantiate(enemies[Random.Range(0, enemies.Count)], location, Quaternion.identity);
	}

	//Creates passage in room
	private void CreatePassageInSameRoom(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;

		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());

		if (cell.room != otherCell.room)
		{
			MazeRoom roomToAssimilate = otherCell.room;

			cell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}

	//Does next generation step
	private void DoNextGenerationStep(List<MazeCell> activeCells)
	{
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		MazeDirection direction;
		IntVector2 coordinates;

		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}

		direction = currentCell.RandomUninitializedDirection;
		coordinates = currentCell.coordinates + direction.ToIntVector2();

		if (ContainsCoordinates(coordinates))
		{
			MazeCell neighbor = GetCell(coordinates);

			if (neighbor == null)
			{
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex)
				CreatePassageInSameRoom(currentCell, neighbor, direction);
			else
				CreateWall(currentCell, neighbor, direction);
		}
		else
			CreateWall(currentCell, null, direction);

		if (Random.value < enemyProbability)
			enemyLocations.Add(currentCell.transform.position);
	}

	//Creates cell
	private MazeCell CreateCell(IntVector2 coordinates)
	{
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;

		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.y - size.y * 0.5f + 0.5f);
		return newCell;
	}

	//Creates wall
	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		(Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall).Initialize(cell, otherCell, direction);

		if (otherCell != null)
			(Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall).Initialize(otherCell, cell, direction.GetOpposite());
	}

	//Random coordinates in maze
	public IntVector2 RandomCoordinates
	{
		get
		{
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.y));
		}
	}

	//Whether the maze contains coordinates
	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}
}