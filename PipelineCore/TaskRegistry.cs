// TaskRegistry.cs
//
// Central registry for all ITask instances.
// Maps string IDs to typed task implementations.

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PipelineCore;

public class TaskRegistry
{
    private readonly ConcurrentDictionary<string, Type> m_idToTaskType = new();
    private readonly ConcurrentDictionary<string, object> m_idToInstance = new();

    public void Register<TInput, TOutput>(string id, ITask<TInput, TOutput> task) where TOutput : notnull
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new PipelineException("Task id cannot be null or empty.");

        if (m_idToTaskType.ContainsKey(id))
            throw new PipelineException($"Task already registered with id: {id}");

        m_idToTaskType[id] = typeof(ITask<TInput, TOutput>);
        m_idToInstance[id] = task;
    }

    public void RegisterInstance(string id, object instance)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new PipelineException("Task id cannot be null or empty.");

        m_idToInstance[id] = instance;
    }

    public ITask<TInput, TOutput>? Get<TInput, TOutput>(string id)
    {
        if (m_idToInstance.TryGetValue(id, out object? obj) && obj is ITask<TInput, TOutput> task)
            return task;

        return null;
    }

    public bool Contains(string id)
    {
        return m_idToTaskType.ContainsKey(id);
    }

    public IReadOnlyCollection<string> RegisteredIds => m_idToTaskType.Keys.ToList();
}
