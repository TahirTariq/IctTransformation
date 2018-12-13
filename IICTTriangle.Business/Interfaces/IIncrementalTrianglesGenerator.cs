using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIncrementalTrianglesGenerator
    {
        TriangleDataFile GenerateIncrementalTriangles(IncrementalDataFile dataFile);

        /// <summary>
        /// Creates cumulative triangles from incremental triangles
        /// </summary>
        /// <remarks>Deep clones Triangles leaving incremental triangle unaffected</remarks>
        /// <param name="incrementalData"></param>
        /// <returns>cumulative triangles</returns>
        TriangleDataFile GenerateCumulativeTriangles(IncrementalDataFile incrementalData);

        /// <summary>
        /// Creates cumulative triangles from incremental triangles
        /// </summary>
        /// <remarks>Deep clones Triangles leaving incremental triangle unaffected</remarks>
        /// <param name="incrementalData"></param>
        /// <returns>cumulative triangles</returns>
        TriangleDataFile GenerateCumulativeTriangles(TriangleDataFile incrementalData);
    }
}