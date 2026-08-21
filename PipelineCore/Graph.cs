// Graph.cs
//
// Directed acyclic graph (DAG) for pipeline execution.
// Supports topological sorting and cycle detection.

using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineCore;

public class Graph
{
    private readonly Dictionary<string, Node> m_nodes = new();
    private readonly Dictionary<string, List<string>> m_adjacency = new();
    private readonly Dictionary<string, int> m_inDegree = new();

    public IReadOnlyDictionary<string, Node> Nodes => m_nodes;
    public IReadOnlyList<string> NodeIds => m_nodes.Keys.ToList();

    public void AddNode(Node node)
    {
        if (m_nodes.ContainsKey(node.Id))
            throw new PipelineException($"Duplicate node id: {node.Id}");

        m_nodes[node.Id] = node;
        m_adjacency[node.Id] = new List<string>();
        m_inDegree[node.Id] = 0;
    }

    public void AddEdge(string fromId, string toId)
    {
        if (!m_nodes.ContainsKey(fromId) || !m_nodes.ContainsKey(toId))
            throw new PipelineException($"Cannot add edge: node not found ({fromId} -> {toId})");

        m_adjacency[fromId].Add(toId);
        m_inDegree[toId]++;
    }

    public List<Node> TopologicalSort()
    {
        var queue = new Queue<string>();
        var inDegreeCopy = new Dictionary<string, int>(m_inDegree);

        foreach (var kvp in inDegreeCopy)
        {
            if (kvp.Value == 0)
                queue.Enqueue(kvp.Key);
        }

        var sorted = new List<Node>();
        while (queue.Count > 0)
        {
            string currentId = queue.Dequeue();
            sorted.Add(m_nodes[currentId]);

            foreach (string neighbor in m_adjacency[currentId])
            {
                inDegreeCopy[neighbor]--;
                if (inDegreeCopy[neighbor] == 0)
                    queue.Enqueue(neighbor);
            }
        }

        if (sorted.Count != m_nodes.Count)
            throw new PipelineException("Cycle detected in pipeline graph.");

        return sorted;
    }

    public bool TryGetNode(string id, out Node? node)
    {
        return m_nodes.TryGetValue(id, out node);
    }
}
