// PipelineLoader.cs
//
// Loads pipeline definitions from JSON files.
// Supports references to other nodes via @nodeId syntax.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using PipelineCore;

namespace PipelineCore;

public class PipelineDefinition
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ConfigPath { get; set; }
    public List<Node> Nodes { get; set; } = new();
    public List<string> OutputNodes { get; set; } = new();
}

public static class PipelineLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };

    public static Graph Load(string pipelineJsonPath)
    {
        if (!File.Exists(pipelineJsonPath))
            throw new PipelineException($"Pipeline file not found: {pipelineJsonPath}");

        string json = File.ReadAllText(pipelineJsonPath);
        PipelineDefinition? definition = JsonSerializer.Deserialize<PipelineDefinition>(json, JsonOptions);

        if (definition == null)
            throw new PipelineException("Failed to deserialize pipeline definition.");

        var graph = new Graph();

        foreach (Node node in definition.Nodes)
        {
            graph.AddNode(node);
        }

        foreach (Node node in definition.Nodes)
        {
            foreach (string dep in node.DependsOn ?? new List<string>())
            {
                graph.AddEdge(dep, node.Id);
            }
        }

        foreach (string outputId in definition.OutputNodes ?? new List<string>())
        {
            if (graph.TryGetNode(outputId, out Node? node))
            {
                node.IsFinalOutput = true;
            }
        }

        return graph;
    }

    public static PipelineDefinition? LoadDefinition(string pipelineJsonPath)
    {
        if (!File.Exists(pipelineJsonPath))
            throw new PipelineException($"Pipeline file not found: {pipelineJsonPath}");

        string json = File.ReadAllText(pipelineJsonPath);
        return JsonSerializer.Deserialize<PipelineDefinition>(json, JsonOptions);
    }
}
