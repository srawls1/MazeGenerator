using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTResultGraph<T> : IGraph<T>
{
	#region Private Variables

	private IGraph<T> baseGraph;
	private HashSet<IGraphEdge<T>> mstEdges;

	#endregion // Private Variables

	#region Public Properties

	public int EdgeCount
	{
		get { return mstEdges.Count; }
	}

	public IEnumerable<IGraphEdge<T>> Edges
	{
		get { return mstEdges; }
	}

	public int NodeCount
	{
		get { return baseGraph.NodeCount; }
	}

	public IEnumerable<IGraphNode<T>> Nodes
	{
		get { return baseGraph.Nodes; }
	}

	#endregion // Public Properties

	#region Constructor

	public MSTResultGraph(IGraph<T> wholeGraph, IMSTStrategy<T> strategy)
	{
		baseGraph = wholeGraph;
		mstEdges = new HashSet<IGraphEdge<T>>(strategy.MinimumSpanningTree(baseGraph));
	}

	#endregion // Constructor

	#region Public Functions

	public IGraphEdge<T> AddDirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1)
	{
		throw new NotImplementedException();
	}

	public IGraphNode<T> AddNode(T elem)
	{
		throw new NotImplementedException();
	}

	public IGraphEdge<T> AddUndirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1)
	{
		throw new NotImplementedException();
	}

	public IGraphEdge<T> GetEdge(IGraphNode<T> node1, IGraphNode<T> node2)
	{
		IGraphEdge<T> edge = baseGraph.GetEdge(node1, node2);
		if (mstEdges.Contains(edge))
		{
			return edge;
		}

		return null;
	}

	public IEnumerable<IGraphEdge<T>> GetEdges(IGraphNode<T> node)
	{
		foreach (IGraphEdge<T> edge in baseGraph.GetEdges(node))
		{
			if (mstEdges.Contains(edge))
			{
				yield return edge;
			}
		}
	}

	public IGraphNode<T> GetNode(T elem)
	{
		return baseGraph.GetNode(elem);
	}

	#endregion // Public Functions
}
