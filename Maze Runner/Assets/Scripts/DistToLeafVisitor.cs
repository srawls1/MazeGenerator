using System.Collections.Generic;

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
			if (distsFromLeafs.TryGetValue(node, out dist) && dist < minDist)
			{
				minDist = dist;
			}
		}

		if (minDist == int.MaxValue)
		{
			minDist = 0;
		}

		distsFromLeafs[node] = minDist;
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
