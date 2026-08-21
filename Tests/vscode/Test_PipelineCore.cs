// Test_PipelineCore.cs
//
// Unit tests for PipelineCore module components.
// Tests ITask, Node, Graph, TaskRegistry, Scheduler, PipelineLoader.

using System.Collections.Generic;
using PipelineCore;
using FFSC_PicoGK.Pipeline;
using FFSC_PicoGK.Pipeline.Nodes;

namespace Tests.vscode;

public static class Test_PipelineCore
{
    public static int RunAll()
    {
        int passed = 0;
        int failed = 0;

        void AssertTrue(string name, bool condition)
        {
            if (condition)
            {
                Console.WriteLine($"[PASS] {name}");
                passed++;
            }
            else
            {
                Console.WriteLine($"[FAIL] {name}");
                failed++;
            }
        }

        void AssertEqual<T>(string name, T expected, T actual)
        {
            bool equal = EqualityComparer<T>.Default.Equals(expected, actual);
            AssertTrue(name, equal);
        }

        void AssertNotNull<T>(string name, T? value) where T : class
        {
            AssertTrue(name, value != null);
        }

        // ITask tests
        {
            var loadParams = new LoadParamsNode();
            AssertEqual("LoadParamsNode.Id", "load_params", loadParams.Id);
            AssertEqual("LoadParamsNode.Name", "Load Engine Parameters", loadParams.Name);
        }

        // TaskRegistry tests
        {
            var registry = new TaskRegistry();
            var loadParams = new LoadParamsNode();
            registry.Register("load_params", loadParams);

            AssertTrue("TaskRegistry.Contains('load_params')", registry.Contains("load_params"));
            AssertTrue("TaskRegistry.NotContains('missing')", !registry.Contains("missing"));
            AssertNotNull("TaskRegistry.Get<LoadParams>", registry.Get<string, FFSC_PicoGK.Models.EngineParams>("load_params"));

            bool duplicateError = false;
            try
            {
                registry.Register("load_params", loadParams);
            }
            catch (PipelineException)
            {
                duplicateError = true;
            }
            AssertTrue("TaskRegistry.DuplicateThrows", duplicateError);
        }

        // Graph tests
        {
            var graph = new Graph();
            graph.AddNode(new Node { Id = "a", Name = "Node A" });
            graph.AddNode(new Node { Id = "b", Name = "Node B" });
            graph.AddNode(new Node { Id = "c", Name = "Node C" });

            graph.AddEdge("a", "b");
            graph.AddEdge("b", "c");

            AssertEqual("Graph.NodeCount", 3, graph.NodeIds.Count);
            AssertTrue("Graph.Contains('a')", graph.TryGetNode("a", out _));
            AssertTrue("Graph.NotContains('z')", !graph.TryGetNode("z", out _));

            var sorted = graph.TopologicalSort();
            AssertEqual("Graph.TopologicalSort.Count", 3, sorted.Count);
            AssertEqual("Graph.Sorted[0].Id", "a", sorted[0].Id);
            AssertEqual("Graph.Sorted[1].Id", "b", sorted[1].Id);
            AssertEqual("Graph.Sorted[2].Id", "c", sorted[2].Id);

            bool cycleError = false;
            try
            {
                graph.AddEdge("c", "a");
                graph.TopologicalSort();
            }
            catch (PipelineException)
            {
                cycleError = true;
            }
            AssertTrue("Graph.CycleDetected", cycleError);
        }

        // Node tests
        {
            var node = new Node
            {
                Id = "test",
                Name = "Test Node",
                TaskType = "Some.Type",
                DependsOn = new List<string> { "dep1", "dep2" },
                IsEntryPoint = true,
                IsFinalOutput = false
            };

            AssertEqual("Node.Id", "test", node.Id);
            AssertEqual("Node.Name", "Test Node", node.Name);
            AssertEqual("Node.DependsOn.Count", 2, node.DependsOn.Count);
            AssertTrue("Node.IsEntryPoint", node.IsEntryPoint);
            AssertTrue("Node.NotFinalOutput", !node.IsFinalOutput);
        }

        // Unit struct tests
        {
            var u1 = Unit.Value;
            var u2 = Unit.Value;
            AssertTrue("Unit.Value is same", Unit.Value.Equals(Unit.Value));
        }

        // PipelineLoader tests
        {
            string pipelinePath = "Pipeline/pipeline.json";
            if (System.IO.File.Exists(pipelinePath))
            {
                var graph = PipelineLoader.Load(pipelinePath);
                AssertTrue("PipelineLoader.Load != null", graph != null);
                AssertTrue("PipelineLoader.NodeCount > 0", graph.NodeIds.Count > 0);
                AssertTrue("PipelineLoader.HasFinalOutput", graph.NodeIds.Contains("final_assembly"));
            }
            else
            {
                AssertTrue("PipelineLoader.FileExists", false);
            }
        }

        Console.WriteLine($"\n=== PipelineCore Tests: {passed} passed, {failed} failed ===");
        return failed == 0 ? 0 : 1;
    }
}
