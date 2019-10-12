using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistToLeafVisitor<T> : IGraphVisitor<T>
{
	private IGraph<T> graph;
	private Dictionary<IGraphNode<T>, int> distsFromLeafs;

	public DistToLeafVisitor(IGraph<T> graph)
	{
		this.graph = graph;
		distsFromLeafs = new Dictionary<IGraphNode<T>, int>();
	}

	public void VisitNodeOpen(IGraphNode<T> node)
	{
	}

	public void VisitNodeClose(IGraphNode<T> node)
	{
		int minDist = int.MaxValue;
		foreach (IGraphEdge<T> edge in graph.GetEdges(node))
		{
			int dist;
			IGraphNode<T> otherNode = node == edge.node1 ? edge.node2 : edge.node1;
			if (distsFromLeafs.TryGetValue(otherNode, out dist) && dist < minDist)
			{
				minDist = dist;
			}
		}

		if (minDist == int.MaxValue)
		{
			minDist = 0;
		}

		distsFromLeafs[node] = minDist + 1;
	}

	private void PrintDistanceDictionary()
	{
		Debug.Log(string.Format("[{0}]",
			string.Join(", ", distsFromLeafs.Select((distanceEntry) =>
				string.Format("{{ {0} => {1} }}",
					distanceEntry.Key.storedData.ToString(),
					distanceEntry.Value)
			))
		));
	}

	public int GetDistFromLeaf(IGraphNode<T> node)
	{
		int dist;
		if (distsFromLeafs.TryGetValue(node, out dist))
		{
			return dist;
		}
		else
		{
			return -1;
		}
	}
}
