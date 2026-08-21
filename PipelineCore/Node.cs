// Node.cs
//
// Node definition that wraps an ITask with metadata
// for graph construction and execution.
// Supports InputFactory for complex multi-dependency input resolution.

using System;
using System.Collections.Generic;

namespace PipelineCore;

public class Node
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TaskType { get; set; }
    public List<string> DependsOn { get; set; } = new();
    public Dictionary<string, object?> Input { get; set; } = new();
    public string? OutputKey { get; set; }
    public Func<PipelineContext, object>? InputFactory { get; set; }
    public bool IsEntryPoint { get; set; }
    public bool IsFinalOutput { get; set; }

    public override string ToString()
    {
        return $"Node(Id={Id}, Name={Name}, TaskType={TaskType}, DependsOn=[{string.Join(", ", DependsOn)}])";
    }
}
