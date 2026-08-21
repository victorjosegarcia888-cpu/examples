// Scheduler.cs
//
// Executes a pipeline graph by processing nodes in topological order.
// Resolves dependencies and passes typed outputs between tasks.

using System;
using System.Collections.Generic;

namespace PipelineCore;

public class PipelineContext
{
    private readonly Dictionary<string, object?> m_results = new();

    public void Set<T>(string nodeId, T result)
    {
        m_results[nodeId] = result;
    }

    public T? Get<T>(string nodeId)
    {
        if (m_results.TryGetValue(nodeId, out object? value) && value is T typed)
            return typed;

        return default;
    }

    public bool TryGet<T>(string nodeId, out T? result)
    {
        if (m_results.TryGetValue(nodeId, out object? value) && value is T typed)
        {
            result = typed;
            return true;
        }

        result = default;
        return false;
    }

    public void Clear()
    {
        m_results.Clear();
    }
}

public class Scheduler
{
    private readonly TaskRegistry m_registry;

    public Scheduler(TaskRegistry registry)
    {
        m_registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    public void Execute(Graph graph, Action<string, object?>? onNodeComplete = null)
    {
        if (graph == null)
            throw new PipelineException("Graph cannot be null.");

        List<Node> sortedNodes = graph.TopologicalSort();
        var context = new PipelineContext();

        foreach (Node node in sortedNodes)
        {
            if (!m_registry.Contains(node.Id))
                throw new PipelineException($"Task not registered: {node.Id}");

            ExecuteNode(node, context, onNodeComplete);
        }
    }

    public TOutput? ExecuteAndGetResult<TOutput>(Graph graph, string outputNodeId)
    {
        TOutput? result = default;
        Execute(graph, (nodeId, output) =>
        {
            if (nodeId == outputNodeId && output is TOutput typed)
                result = typed;
        });

        return result;
    }

    private void ExecuteNode(Node node, PipelineContext context, Action<string, object?>? onNodeComplete)
    {
        object? input = ResolveInput(node, context);
        object? output = InvokeTask(node, input);

        if (node.OutputKey != null)
            context.Set(node.OutputKey, output);

        onNodeComplete?.Invoke(node.Id, output);
    }

    private object? InvokeTask(Node node, object? input)
    {
        string taskTypeName = node.TaskType ?? throw new PipelineException($"TaskType not set for node: {node.Id}");

        Type taskType = Type.GetType(taskTypeName) ?? throw new PipelineException($"Cannot load task type: {taskTypeName}");

        var executeMethod = taskType.GetMethod("Execute") ?? throw new PipelineException($"Execute method not found in: {taskTypeName}");

        return executeMethod.Invoke(null, new[] { input }) ?? throw new PipelineException($"Task {node.Id} returned null.");
    }

    private object? ResolveInput(Node node, PipelineContext context)
    {
        if (node.InputFactory != null)
            return node.InputFactory(context);

        if (node.DependsOn == null || node.DependsOn.Count == 0)
            return Unit.Value;

        if (node.DependsOn.Count == 1)
        {
            string depId = node.DependsOn[0];
            if (!context.TryGet<object>(depId, out object? value))
                throw new PipelineException($"Dependency not found: {depId} for node {node.Id}");

            return value;
        }

        var values = new object[node.DependsOn.Count];
        for (int i = 0; i < node.DependsOn.Count; i++)
        {
            string depId = node.DependsOn[i];
            if (!context.TryGet<object>(depId, out object? value))
                throw new PipelineException($"Dependency not found: {depId} for node {node.Id}");

            values[i] = value;
        }

        return values;
    }
}
