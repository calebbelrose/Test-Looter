using UnityEngine;

public class MazeDoor : MazePassage
{
	public Transform hinge;

	private bool isMirrored;
	private MazeDoor OtherSideOfDoor { get { return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor; } }

	private static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f), mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	//Sets up door
	public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
	{
		base.Initialize(primary, other, direction);

		if (direction == MazeDirection.South)
			cell.room.AddSouthWall(this);

		if (OtherSideOfDoor != null)
		{
			Vector3 p;

			isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);

			if (child != hinge)
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
		}
	}

	//Opens door
	public void OpenDoor()
    {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
	}

	//Changes the room when player walks through door
    private void OnTriggerEnter(Collider other)
    {
		MazeRoom.ChangeRoom(cell.room);
    }
}