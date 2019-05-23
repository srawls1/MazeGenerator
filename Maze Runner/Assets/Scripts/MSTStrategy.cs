using System.Collections.Generic;

public interface IMSTStrategy<T>
{
	ICollection<IGraphEdge<T>> MinimumSpanningTree(IGraph<T> graph);
}
