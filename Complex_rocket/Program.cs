//
// SPDX-License-Identifier: CC0-1.0
//
// Complex Rocket Engine - Viewer Integration
// Uses PicoGK official viewer pattern with Library.Go
//

using PicoGK;
using ComplexRocket.Tasks;

Library.Go(0.5f, () =>
{
    Voxels engine = AssemblyTask.AssembleEngine(0.5f);

    Voxels smoothed = SmoothingTask.ApplySmoothing(engine, 1.2f, 0.9f);

    Library.oViewer().Add(smoothed);
});
