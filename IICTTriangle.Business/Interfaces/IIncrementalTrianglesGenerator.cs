using IctTriangle.Business.Models;

namespace IctTriangle.Business.Interfaces
{
    public interface IIncrementalTrianglesGenerator
    {
        TriangleDataFile GenerateIncrementalTriangles();
        TriangleDataFile GenerateCumulativeTriangles(TriangleDataFile incrementalData);
    }
}