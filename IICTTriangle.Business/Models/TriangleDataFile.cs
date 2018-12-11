using System.Collections.Generic;

namespace IctTriangle.Business.Models
{
    public class TriangleDataFile
    {
        public int EarliestOriginYear { get; set; }

        public int DevelopmentYears { get; set; }

        public Dictionary<string, Triangle> ProductTriangles { get; set; }
    }
}
