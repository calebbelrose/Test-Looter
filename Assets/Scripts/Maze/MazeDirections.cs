using UnityEngine;

public static class MazeDirections
{
	public const int Count = 4;

	//Random direction
	public static MazeDirection RandomValue
	{
		get
		{
			return (MazeDirection)Random.Range(0, Count);
		}
	}

	//Opposite directions
	private static MazeDirection[] opposites = {
		MazeDirection.South,
		MazeDirection.West,
		MazeDirection.North,
		MazeDirection.East
	};

	//Returns opposite direction
	public static MazeDirection GetOpposite(this MazeDirection direction)
	{
		return opposites[(int)direction];
	}

	//Rotations
	private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	//Returns rotation
	public static Quaternion ToRotation(this MazeDirection direction)
	{
		return rotations[(int)direction];
	}

	//Vectors
	private static IntVector2[] vectors = {
		new IntVector2(0, 5),
		new IntVector2(5, 0),
		new IntVector2(0, -5),
		new IntVector2(-5, 0)
	};

	//Returns vector
	public static IntVector2 ToIntVector2(this MazeDirection direction)
	{
		return vectors[(int)direction];
	}
}