// Exceptions.cs
//
// Custom exceptions for PipelineCore module.

namespace PipelineCore;

public class PipelineException : Exception
{
    public PipelineException(string message) : base(message) { }
    public PipelineException(string message, Exception inner) : base(message, inner) { }
}
