using System;
using System.Collections.Generic;
using UnityEngine;

public class KruskalsAlgorithm<T> : IMSTStrategy<T>
{
	private class QueueType : ComparablePair<float, IGraphEdge<T>>, IComparable<QueueType>
	{
		public QueueType(float first, IGraphEdge<T> second)
			: base(first, second) { }

		public int CompareTo(QueueType other) { return base.CompareTo(other); }
	}

	public ICollection<IGraphEdge<T>> MinimumSpanningTree(IGraph<T> graph)
	{
		DisjointSet<IGraphNode<T>> nodes = new DisjointSet<IGraphNode<T>>();
		foreach (IGraphNode<T> node in graph.Nodes)
		{
			nodes.Add(node);
		}

		PriorityQueue<QueueType> edges = new PriorityQueue<QueueType>(graph.EdgeCount);
		foreach (IGraphEdge<T> edge in graph.Edges)
		{
			edges.Add(new QueueType(edge.weight, edge));
		}

		List<IGraphEdge<T>> ret = new List<IGraphEdge<T>>(graph.NodeCount);
		while (edges.Count > 0)
		{
			IGraphEdge<T> edge = edges.Pop().Second;
			if (nodes.AreUnited(edge.node1, edge.node2))
			{
				continue;
			}
			ret.Add(edge);
			nodes.Union(edge.node1, edge.node2);
		}

		return ret;
	}
}
