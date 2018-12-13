using System.Collections.Generic;

namespace Ict.Business.Models
{
    public class TriangleDataFile
    {
        public TriangleDataFile(int earliestOriginYear, int developmentYears, Dictionary<string, Triangle> triangles)
        {
            EarliestOriginYear = earliestOriginYear;
            DevelopmentYears = developmentYears;
            ProductTriangles = triangles;
        }

        public int EarliestOriginYear { get; }

        public int DevelopmentYears { get; }

        public Dictionary<string, Triangle> ProductTriangles { get; }      
    }
}
