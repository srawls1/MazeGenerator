using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
	private Pair<float, Item>[] itemSpawnFrequencies;
	private IGraph<Vector2> levelGraph;
	private IShortestPathStrategy<Vector2> shortestPathStrategy;
	private DistToLeafVisitor<Vector2> distanceToLeaves;
	private HashSet<Vector2> previouslySpawnedLocations;

	public ItemFactory(IGraph<Vector2> graph, Pair<float, Item>[] itemFrequencies/*,
		IShortestPathStrategy<Vector2> pathingStrategy*/)
	{
		levelGraph = graph;
		itemSpawnFrequencies = itemFrequencies;
		//shortestPathStrategy = pathingStrategy;

		distanceToLeaves = new DistToLeafVisitor<Vector2>(levelGraph);
		IGraphNode<Vector2> startNode = graph.GetNode(Vector2.zero);
		graph.DepthFirstTraversal(startNode, distanceToLeaves);

		previouslySpawnedLocations = new HashSet<Vector2>();
	}

	public void decideSpawn(Vector2 position)
	{
		IGraphNode<Vector2> node = levelGraph.GetNode(position);
		int minDistFromLeaf = distanceToLeaves.GetDistFromLeaf(node);
		float spawnRandom = Random.value * minDistFromLeaf;

		//Debug.Log("Position: " + position + ", distance from leaf: " + minDistFromLeaf +
		//	", spawn random: " + spawnRandom);

		float cumulativeFrequency = 0f;
		foreach (Pair<float, Item> itemFrequency in itemSpawnFrequencies)
		{
			cumulativeFrequency += itemFrequency.First;
			if (cumulativeFrequency > spawnRandom)
			{
				Item.Spawn(itemFrequency.Second, position);
				previouslySpawnedLocations.Add(position);
				break;
			}
		}
	}

	private float shortestDistanceHeuristic(Vector2 step, Vector2 dest)
	{
		return Mathf.Abs(step.x - dest.x) + Mathf.Abs(step.y - dest.y);
	}
}
