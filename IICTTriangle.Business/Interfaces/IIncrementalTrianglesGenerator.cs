using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIncrementalTrianglesGenerator
    {
        TriangleDataFile GenerateIncrementalTriangles();
        TriangleDataFile GenerateCumulativeTriangles(TriangleDataFile incrementalData);
    }
}