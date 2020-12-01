using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour
{
	public MazeCell cell, otherCell;
	public MazeDirection direction;

	[SerializeField] private Renderer[] renderers;

	//Hides the wall so the player can see what's behind it
	public void HideWall()
    {
		Alpha(0.2f);

		if (otherCell != null)
			(otherCell.GetEdge(direction.GetOpposite()) as MazeCellEdge).Alpha(0.2f);
	}

	//Shows the wall
	public void ShowWall()
    {
		Alpha(1.0f);

		if (otherCell != null)
			(otherCell.GetEdge(direction.GetOpposite()) as MazeCellEdge).Alpha(1.0f);
	}

	//Sets the alpha of the wall
	private void Alpha(float alpha)
	{
		foreach (Renderer renderer in renderers)
		{
			Color color = renderer.material.color;
			color.a = alpha;
			renderer.material.color = color;
		}
	}

	//Sets up the edge
    public virtual void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
}