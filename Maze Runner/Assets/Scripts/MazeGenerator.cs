using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpawnFrequency
{
	public float frequency;
	public Item item;
}

public class MazeGenerator : MonoBehaviour
{
	[SerializeField] private ItemSpawnFrequency[] itemSpawns;
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject floorPrefab;
	[SerializeField] private int size;

	private void Start()
	{
		GameObject floor = Instantiate(floorPrefab,
			new Vector3((float)(size - 1) / 2, 0, (float)(size - 1) / 2),
			Quaternion.identity);
		floor.transform.localScale = new Vector3(size, .5f, size);

		IGraph<Vector2> graph = GetMazeGraph();
		PlaceWalls(graph);
		PlaceItems(graph);
	}

	private void PlaceItems(IGraph<Vector2> graph)
	{
		Pair<float, Item>[] spawnFrequencies = new Pair<float, Item>[itemSpawns.Length];
		for (int i = 0; i < itemSpawns.Length; ++i)
		{
			spawnFrequencies[i] = new Pair<float, Item>(itemSpawns[i].frequency, itemSpawns[i].item);
		}

		ItemFactory factory = new ItemFactory(graph, spawnFrequencies);
		for (int y = 0; y < size; ++y)
		{
			for (int x = 0; x < size; ++x)
			{
				factory.decideSpawn(new Vector2(x, y));
			}
		}
	}

	private void PlaceWalls(IGraph<Vector2> graph)
	{
		for (int i = 1; i < size; ++i)
		{
			IGraphNode<Vector2> n1 = graph.GetNode(new Vector2(i - 1, 0));
			IGraphNode<Vector2> n2 = graph.GetNode(new Vector2(i, 0));
			IGraphEdge<Vector2> edge = graph.GetEdge(n1, n2);
			if (edge == null)
			{
				PlaceWall(n1, n2);
			}

			n1 = graph.GetNode(new Vector2(0, i - 1));
			n2 = graph.GetNode(new Vector2(0, i));
			edge = graph.GetEdge(n1, n2);
			if (edge == null)
			{
				PlaceWall(n1, n2);
			}
		}

		for (int y = 1; y < size; ++y)
		{
			for (int x = 1; x < size; ++x)
			{
				IGraphNode<Vector2> n1 = graph.GetNode(new Vector2(y - 1, x));
				IGraphNode<Vector2> n2 = graph.GetNode(new Vector2(y, x));
				IGraphEdge<Vector2> edge = graph.GetEdge(n1, n2);
				if (edge == null)
				{
					PlaceWall(n1, n2);
				}

				n1 = graph.GetNode(new Vector2(y, x - 1));
				n2 = graph.GetNode(new Vector2(y, x));
				edge = graph.GetEdge(n1, n2);
				if (edge == null)
				{
					PlaceWall(n1, n2);
				}
			}
		}
	}

	private void PlaceWall(IGraphNode<Vector2> node1, IGraphNode<Vector2> node2)
	{
		Vector3 p1 = new Vector3(node1.storedData.x, 2, node1.storedData.y);
		Vector3 p2 = new Vector3(node2.storedData.x, 2, node2.storedData.y);
		Vector3 midpoint = 0.5f * (p1 + p2);
		Quaternion rot = Quaternion.FromToRotation(Vector3.right, p2 - p1);

		GameObject wall = Instantiate(wallPrefab, midpoint, rot);
	}

	private IGraph<Vector2> GetMazeGraph()
	{
		IGraph<Vector2> graph = new AdjacencyListGraph<Vector2>();
		IGraphNode<Vector2>[,] nodes = new IGraphNode<Vector2>[size, size];

		for (int y = 0; y < size; ++y)
		{
			for (int x = 0; x < size; ++x)
			{
				nodes[y, x] = graph.AddNode(new Vector2(x, y));
			}
		}

		for (int i = 1; i < size; ++i)
		{
			graph.AddUndirectedEdge(nodes[i - 1, 0], nodes[i, 0], Random.Range(0f, 1f));
			graph.AddUndirectedEdge(nodes[0, i - 1], nodes[0, i], Random.Range(0f, 1f));
		}

		for (int y = 1; y < size; ++y)
		{
			for (int x = 1; x < size; ++x)
			{
				graph.AddUndirectedEdge(nodes[y - 1, x], nodes[y, x], Random.Range(0f, 1f));
				graph.AddUndirectedEdge(nodes[y, x - 1], nodes[y, x], Random.Range(0f, 1f));
			}
		}

		IMSTStrategy<Vector2> strategy = new KruskalsAlgorithm<Vector2>();
		return new MSTResultGraph<Vector2>(graph, strategy);
	}
}
