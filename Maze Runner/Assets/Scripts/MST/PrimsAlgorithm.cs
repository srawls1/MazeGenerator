using System;
using System.Linq;
using System.Collections.Generic;

public class PrimsAlgorithm<T> : EdgeRelaxingAlgorithm<T>, IMSTStrategy<T>
{
	

	public ICollection<IGraphEdge<T>> MinimumSpanningTree(IGraph<T> graph)
	{
		IEnumerable<IGraphNode<T>> nodes = graph.Nodes;
		IGraphNode<T> node = nodes.FirstOrDefault();
		if (node == null)
		{
			return new List<IGraphEdge<T>>();
		}

		IDictionary<IGraphNode<T>, IGraphEdge<T>> connectingEdges = RelaxEdges(graph, node);
		return connectingEdges.Values;
	}

	protected override float GetCost(float prevNodeCost, IGraphEdge<T> edge)
	{
		return edge.weight;
	}
}
