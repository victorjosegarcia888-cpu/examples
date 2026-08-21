// GeometryII_ShapeGenerator.cs
//
// GeometryII module - Shape generation using LEAP71 ShapeKernel patterns.
// This module provides high-level shape generation tasks compatible with
// the PipelineCore ITask interface.

using PipelineCore;
using PicoGK;

namespace GeometryII.Tasks;

public record ShapeGeneratorInput(
    double Radius = 0.1,
    double Height = 0.2,
    int Segments = 32,
    string ShapeType = "Sphere");

public class GeometryII_ShapeGeneratorTask : ITask<ShapeGeneratorInput, Voxels>
{
    public string Id => "geometryii_shape_generator";
    public string Name => "GeometryII Shape Generator";

    public Voxels Run(ShapeGeneratorInput input)
    {
        return input.ShapeType.ToLower() switch
        {
            "sphere" => GenerateSphere(input.Radius, input.Segments),
            "cylinder" => GenerateCylinder(input.Radius, input.Height, input.Segments),
            "cone" => GenerateCone(input.Radius, input.Height, input.Segments),
            "box" => GenerateBox(input.Radius, input.Height),
            _ => GenerateSphere(input.Radius, input.Segments)
        };
    }

    public Voxels Execute(ShapeGeneratorInput input)
    {
        return Run(input);
    }

    private Voxels GenerateSphere(double radius, int segments)
    {
        Voxels voxels = new Voxels();
        int count = segments;
        for (int i = 0; i < count; i++)
        {
            float angle = i * 2.0f * (float)Math.PI / count;
            float x = (float)(radius * Math.Cos(angle));
            float y = (float)(radius * Math.Sin(angle));
            for (int j = 0; j < count / 2; j++)
            {
                float z = (float)(radius * (j / (double)(count / 2) - 0.5));
                float r = (float)(radius * 0.05);
                voxels += Voxels.voxSphere(new Vector3(x, y, z), r);
            }
        }
        return voxels;
    }

    private Voxels GenerateCylinder(double radius, double height, int segments)
    {
        Voxels voxels = new Voxels();
        int count = segments;
        for (int i = 0; i < count; i++)
        {
            float angle = i * 2.0f * (float)Math.PI / count;
            float x = (float)(radius * Math.Cos(angle));
            float y = (float)(radius * Math.Sin(angle));
            for (int j = 0; j < 10; j++)
            {
                float z = (float)((j / 9.0 - 0.5) * height);
                voxels += Voxels.voxSphere(new Vector3(x, y, z), (float)(radius * 0.05));
            }
        }
        return voxels;
    }

    private Voxels GenerateCone(double radius, double height, int segments)
    {
        Voxels voxels = new Voxels();
        int count = segments;
        for (int i = 0; i < count; i++)
        {
            float angle = i * 2.0f * (float)Math.PI / count;
            for (int j = 0; j < 10; j++)
            {
                float t = j / 9.0f;
                float r = (float)(radius * (1.0 - t));
                float x = r * (float)Math.Cos(angle);
                float y = r * (float)Math.Sin(angle);
                float z = (float)((t - 0.5) * height);
                voxels += Voxels.voxSphere(new Vector3(x, y, z), (float)(radius * 0.05));
            }
        }
        return voxels;
    }

    private Voxels GenerateBox(double radius, double height)
    {
        Voxels voxels = new Voxels();
        float size = (float)radius;
        for (float x = -size; x <= size; x += size * 0.1f)
        {
            for (float y = -size; y <= size; y += size * 0.1f)
            {
                for (float z = (float)(-height / 2); z <= height / 2; z += size * 0.1f)
                {
                    voxels += Voxels.voxSphere(new Vector3(x, y, z), size * 0.05f);
                }
            }
        }
        return voxels;
    }
}
