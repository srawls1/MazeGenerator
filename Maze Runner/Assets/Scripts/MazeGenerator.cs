using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject floorPrefab;
	[SerializeField] private int size;

	private void Start()
	{
		GameObject floor = Instantiate(floorPrefab,
			new Vector3((float)(size - 1) / 2, 0, (float)(size - 1) / 2),
			Quaternion.identity);
		floor.transform.localScale = new Vector3(size, .5f, size);

		IGraph<Vector2> graph = new AdjacencyListGraph<Vector2>();
		HashSet<IGraphEdge<Vector2>> mazeTree = GetMazeTree(graph);
		DebugDrawTree(mazeTree);
		PlaceWalls(graph, mazeTree);
	}

	private void DebugDrawTree(HashSet<IGraphEdge<Vector2>> mazeTree)
	{
		foreach (IGraphEdge<Vector2> edge in mazeTree)
		{
			Vector3 p1 = new Vector3(edge.node1.storedData.x, 5, edge.node1.storedData.y);
			Vector3 p2 = new Vector3(edge.node2.storedData.x, 5, edge.node2.storedData.y);
			Debug.DrawLine(p1, p2, Color.red, 60f);
		}
		Debug.Log(mazeTree.Count);
	}

	private void PlaceWalls(IGraph<Vector2> graph, HashSet<IGraphEdge<Vector2>> mazeTree)
	{
		for (int i = 1; i < size; ++i)
		{
			IGraphNode<Vector2> n1 = graph.GetNode(new Vector2(i - 1, 0));
			IGraphNode<Vector2> n2 = graph.GetNode(new Vector2(i, 0));
			IGraphEdge<Vector2> edge = graph.GetEdge(n1, n2);
			if (!mazeTree.Contains(edge))
			{
				PlaceWall(edge);
			}

			n1 = graph.GetNode(new Vector2(0, i - 1));
			n2 = graph.GetNode(new Vector2(0, i));
			edge = graph.GetEdge(n1, n2);
			if (!mazeTree.Contains(edge))
			{
				PlaceWall(edge);
			}
		}

		for (int y = 1; y < size; ++y)
		{
			for (int x = 1; x < size; ++x)
			{
				IGraphNode<Vector2> n1 = graph.GetNode(new Vector2(y - 1, x));
				IGraphNode<Vector2> n2 = graph.GetNode(new Vector2(y, x));
				IGraphEdge<Vector2> edge = graph.GetEdge(n1, n2);
				if (!mazeTree.Contains(edge))
				{
					PlaceWall(edge);
				}

				n1 = graph.GetNode(new Vector2(y, x - 1));
				n2 = graph.GetNode(new Vector2(y, x));
				edge = graph.GetEdge(n1, n2);
				if (!mazeTree.Contains(edge))
				{
					PlaceWall(edge);
				}
			}
		}
	}

	private void PlaceWall(IGraphEdge<Vector2> edge)
	{
		Vector3 p1 = new Vector3(edge.node1.storedData.x, 2, edge.node1.storedData.y);
		Vector3 p2 = new Vector3(edge.node2.storedData.x, 2, edge.node2.storedData.y);
		Vector3 midpoint = 0.5f * (p1 + p2);
		Quaternion rot = Quaternion.FromToRotation(Vector3.right, p2 - p1);

		GameObject wall = Instantiate(wallPrefab, midpoint, rot);
	}

	private HashSet<IGraphEdge<Vector2>> GetMazeTree(IGraph<Vector2> graph)
	{

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

		return new HashSet<IGraphEdge<Vector2>>(strategy.MinimumSpanningTree(graph));
	}
}
