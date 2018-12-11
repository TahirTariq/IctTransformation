using System.Collections.Generic;
using Ict.Business.Helper;

namespace Ict.Business.Models
{
    public class TriangleDataFile
    {
        public int EarliestOriginYear { get; set; }

        public int DevelopmentYears { get; set; }

        public Dictionary<string, Triangle> ProductTriangles { get; set; }      
    }
}
