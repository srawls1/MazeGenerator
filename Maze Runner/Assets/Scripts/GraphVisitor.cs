using System.Collections.Generic;

public interface IGraphVisitor<T>
{
	void VisitNodeOpen(IGraphNode<T> node);
	void VisitNodeClose(IGraphNode<T> node);
}

public static class GraphTraversals
{
	public static void DepthFirstTraversal<T>(this IGraph<T> graph, IGraphNode<T> startingNode, IGraphVisitor<T> visitor)
	{
		Stack<Pair<IGraphNode<T>, IEnumerator<IGraphEdge<T>>>> openNodes = new Stack<Pair<IGraphNode<T>, IEnumerator<IGraphEdge<T>>>>();
		HashSet<IGraphNode<T>> seenNodes = new HashSet<IGraphNode<T>>();

		openNodes.Push(new Pair<IGraphNode<T>, IEnumerator<IGraphEdge<T>>>(startingNode, graph.GetEdges(startingNode).GetEnumerator()));
		seenNodes.Add(startingNode);
		visitor.VisitNodeOpen(startingNode);

		while (openNodes.Count > 0)
		{
			var top = openNodes.Peek();
			if (top.Second.MoveNext())
			{
				IGraphEdge<T> edge = top.Second.Current;
				IGraphNode<T> node = edge.node2;
				if (!seenNodes.Add(node))
				{
					continue;
				}

				openNodes.Push(new Pair<IGraphNode<T>, IEnumerator<IGraphEdge<T>>>(node, graph.GetEdges(node).GetEnumerator()));
				visitor.VisitNodeOpen(node);
			}
			else
			{
				visitor.VisitNodeClose(openNodes.Pop().First);
			}
		}
	}

	public static void BreadthFirstTraversal<T>(this IGraph<T> graph, IGraphNode<T> startingNode, IGraphVisitor<T> visitor)
	{
		Queue<IGraphNode<T>> openNodes = new Queue<IGraphNode<T>>();
		HashSet<IGraphNode<T>> seenNodes = new HashSet<IGraphNode<T>>();
		openNodes.Enqueue(startingNode);
		seenNodes.Add(startingNode);

		while (openNodes.Count > 0)
		{
			IGraphNode<T> node = openNodes.Dequeue();
			visitor.VisitNodeOpen(node);

			foreach (IGraphEdge<T> edge in graph.GetEdges(node))
			{
				if (!seenNodes.Add(edge.node2))
				{
					continue;
				}

				openNodes.Enqueue(edge.node2);
			}
		}
	}
}
