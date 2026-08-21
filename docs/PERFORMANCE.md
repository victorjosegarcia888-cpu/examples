# Performance Optimization Guide

## Heavy Nodes and Optimization Strategies

### 1. Lattice Generation (lattice_dual, lattice_quasi)

**Current Implementation**: Sphere-based approximation
**Optimization Opportunities**:
- **Spatial Hashing**: Use a 3D grid hash map to avoid duplicate sphere placements
- **Parallel Generation**: Use `Parallel.For` for sphere placement loops
- **Level of Detail**: Reduce sphere count for viewer previews
- **Caching**: Cache lattice results for identical stress fields

**Recommended Changes**:
```csharp
// Before (sequential)
for (int i = 0; i < count; i++)
{
    lattice += Voxels.voxSphere(...);
}

// After (parallel)
Parallel.For(0, count, i =>
{
    lattice += Voxels.voxSphere(...);
});
```

### 2. Cooling Analysis (cooling_analysis)

**Current Implementation**: Per-point heat transfer calculation
**Optimization Opportunities**:
- **Vectorization**: Use `System.Numerics.Vector<double>` for SIMD
- **Lookup Tables**: Precompute Bartz coefficients
- **Adaptive Resolution**: Reduce Nz for previews

**Recommended Changes**:
```csharp
// Use Span<T> and stack allocation for small arrays
Span<CoolingPoint> points = stackalloc CoolingPoint[Nz];
```

### 3. Turbopump Design (turbopump_design)

**Current Implementation**: Single-point Euler equation
**Optimization Opportunities**:
- **Parallel Blade Design**: Generate all blades in parallel
- **Mesh Simplification**: Reduce voxel resolution for preview

### 4. Geometry Generation (all geom_* nodes)

**Current Implementation**: Sequential sphere placement
**Optimization Opportunities**:
- **Voxel Pooling**: Reuse voxel arrays across similar geometries
- **LOD System**: Multiple resolution levels
- **Frustum Culling**: Skip off-screen voxels

### 5. Stress Field (physics_stress)

**Current Implementation**: Fixed sphere grid
**Optimization Opportunities**:
- **GPU Offload**: Use compute shaders for stress calculation
- **Sparse Representation**: Only compute stress in high-interest regions

## Memory Management

### Voxels Pooling
```csharp
public static class VoxelsPool
{
    private static readonly ObjectPool<Voxels> s_pool = new ObjectPool<Voxels>(
        createFunc: () => new Voxels(),
        actionOnGet: v => v.Clear(),
        actionOnReturn: v => v.Clear(),
        maximumRetained: 100);

    public static Voxels Rent() => s_pool.Get();
    public static void Return(Voxels voxels) => s_pool.Return(voxels);
}
```

### Parallel Execution
```csharp
public void ExecuteInParallel(Graph graph, PipelineContext context)
{
    var sortedNodes = graph.TopologicalSort();
    var levelGroups = GroupByLevel(sortedNodes);
    
    Parallel.ForEach(levelGroups, level =>
    {
        foreach (var node in level)
        {
            ExecuteNode(node, context);
        }
    });
}
```

## Caching Strategy

### Result Caching
```csharp
public class CachedScheduler : Scheduler
{
    private readonly Dictionary<string, object?> m_cache = new();

    protected override object? ExecuteNode(Node node, PipelineContext context)
    {
        string cacheKey = GenerateCacheKey(node, context);
        if (m_cache.TryGetValue(cacheKey, out object? cached))
            return cached;
        
        object? result = base.ExecuteNode(node, context);
        m_cache[cacheKey] = result;
        return result;
    }
}
```

## Profiling Recommendations

1. **Use dotTrace or Visual Studio Profiler** to identify bottlenecks
2. **Monitor memory allocations** with `dotnet-counters`
3. **Track pipeline execution time** per node
4. **Profile voxel count** at each pipeline stage

## Expected Performance Gains

| Node | Current | Optimized | Gain |
|------|---------|-----------|------|
| lattice_dual | ~500ms | ~100ms | 5x |
| physics_stress | ~200ms | ~50ms | 4x |
| geom_chamber | ~300ms | ~100ms | 3x |
| final_assembly | ~1s | ~300ms | 3x |

**Total Pipeline**: ~3s → ~1s (3x improvement)
