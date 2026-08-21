// ITask.cs
//
// Core task interface for the PipelineCore module.
// Enforces strict typing of inputs and outputs.
// Every task must be pure, deterministic, and stateless.

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
    TOutput Run(TInput input);
}
