// ITask.cs
//
// Core task interface for the PipelineCore module.
// Enforces strict typing of inputs and outputs.

namespace PipelineCore;

public readonly struct Unit
{
    public static readonly Unit Value = new Unit();
    public Unit() { }
}

public interface ITask<in TInput, out TOutput>
{
    string Id { get; }
    string Name { get; }
    TOutput Execute(TInput input);
}
