// GeometryAerospikeNode.cs
//
// Node wrapper for aerospike geometry generation.

using PipelineCore;
using PicoGK;
using FFSC_PicoGK.Geometry.Aerospike;

namespace FFSC_PicoGK.Pipeline.Nodes;

public class GeometryAerospikeNode : ITask<Unit, Voxels>
{
    public string Id => "geom_aerospike";
    public string Name => "Aerospike Geometry";

    public Voxels Execute(Unit input)
    {
        return Geometry_Aerospike.Create(0.55, 0.15);
    }
}
