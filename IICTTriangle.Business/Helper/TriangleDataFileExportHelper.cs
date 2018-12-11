using System.Collections.Generic;
using System.IO;
using Ict.Business.Models;

namespace Ict.Business.Helper
{
    public static class TriangleDataFileExportHelper
    {
        public static string ToCsvString(this TriangleDataFile dataFile)
        {
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);

                sw.WriteLine($"{dataFile.EarliestOriginYear}, {dataFile.DevelopmentYears}");

                foreach (var productTriangle in dataFile.ProductTriangles)
                {
                    sw.WriteLine(ToCsvLine(productTriangle));
                }

                sw.Flush();
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
              
        }

        private static string ToCsvLine(KeyValuePair<string, Triangle> productTriangle)
        {
            return $"{productTriangle.Key},{productTriangle.Value.ToCsvString()}";
        }
    }
}
