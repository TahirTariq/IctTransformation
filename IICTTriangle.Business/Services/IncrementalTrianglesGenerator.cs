using System;
using System.Collections.Generic;
using System.Linq;
using IctTriangle.Business.Interfaces;
using IctTriangle.Business.Models;

namespace IctTriangle.Business.Services
{
    public class IncrementalTrianglesGenerator : IIncrementalTrianglesGenerator
    {
        private IncrementalDataFile IncrementalDataFile { get; }

        public IncrementalTrianglesGenerator(IncrementalDataFile dataFile)
        {
            IncrementalDataFile = dataFile ?? throw new ArgumentNullException(nameof(dataFile));

            if(!dataFile.IsValid)
                throw new ArgumentException("Incremental data file is not valid for further processing");
        }

        public TriangleDataFile GenerateIncrementalTriangles()
        {
            Dictionary<string, Triangle> productTriangles = CreateIncrementalTriangles();

            Initialize(productTriangles);

            return new TriangleDataFile
            {
                ProductTriangles = productTriangles,
                DevelopmentYears = IncrementalDataFile.DevelopmentYears,
                EarliestOriginYear = IncrementalDataFile.EarliestOriginYear
            };
        }

        /// <summary>
        /// Creates cumulative triangles from incremental triangles
        /// </summary>
        /// <remarks>Deep clones Triangles leaving incremental triangle unaffected</remarks>
        /// <param name="incrementalData"></param>
        /// <returns>cumulative triangles</returns>
        public TriangleDataFile GenerateCumulativeTriangles(TriangleDataFile incrementalData)
        {
            var triangles = new Dictionary<string, Triangle>();
             
            foreach (var productTriangle in incrementalData.ProductTriangles)
            {
                Triangle accumulateTriangle = productTriangle.Value.Accumulate();
                triangles.Add(productTriangle.Key, accumulateTriangle);
            }

            return new TriangleDataFile
            {
                ProductTriangles = triangles,
                DevelopmentYears = incrementalData.DevelopmentYears,
                EarliestOriginYear = incrementalData.EarliestOriginYear
            };
        }

        /// <summary>
        /// Create triangle for each distinct product
        /// </summary>
        /// <returns>triangle dictionary</returns>
        private Dictionary<string, Triangle> CreateIncrementalTriangles()
        {
            var productTriangles = new Dictionary<string, Triangle>();

            int earliestOriginYear = IncrementalDataFile.EarliestOriginYear;
            int developmentYears = IncrementalDataFile.DevelopmentYears;

            foreach (string product in IncrementalDataFile.Products)
            {
                productTriangles.Add(product, new Triangle(earliestOriginYear, developmentYears));
            }

            return productTriangles;
        }

        /// <summary>
        /// Initialize each product triangle from relevant data.
        /// </summary>
        private void Initialize(Dictionary<string, Triangle> productTriangles)
        {
            foreach (string product in IncrementalDataFile.Products)
            {
                Triangle productTriangle = productTriangles[product];
                List<IncrementalRecord> productRecords = IncrementalDataFile.Claims
                    .Where(c => string.Compare(c.Product, product, StringComparison.InvariantCultureIgnoreCase) == 0)
                    .ToList();

                foreach (IncrementalRecord record in productRecords)
                {
                    productTriangle[record.OriginYear, record.DevelopmentYear] = record.IncrementalValue;
                }
            }
        }

        
    }
}