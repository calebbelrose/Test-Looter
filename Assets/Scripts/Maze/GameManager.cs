using UnityEngine;
using UnityEditor.AI;
using System.Collections;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject Player;
	[SerializeField] private NavMeshSurface NavMeshSurface;

	private void Start()
	{
		BeginGame();
		NavMeshSurface.BuildNavMesh();
		mazeInstance.SpawnEnemies();
		Player.SetActive(true);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			RestartGame();
	}

	public Maze mazePrefab;

	private Maze mazeInstance;

	private void BeginGame()
	{
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();
	}

	private void RestartGame()
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}