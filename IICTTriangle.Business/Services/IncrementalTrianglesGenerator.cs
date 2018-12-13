using System;
using System.Collections.Generic;
using System.Linq;
using Ict.Business.Interfaces;
using Ict.Business.Models;

namespace Ict.Business.Services
{
    public class IncrementalTrianglesGenerator : IIncrementalTrianglesGenerator
    {                       
        public TriangleDataFile GenerateIncrementalTriangles(IncrementalDataFile dataFile)
        {
            Validate(dataFile);

            Dictionary<string, Triangle> productTriangles = CreateIncrementalTriangles(dataFile);

            Initialize(productTriangles, dataFile);

            return new TriangleDataFile
            (
                dataFile.EarliestOriginYear,
                dataFile.DevelopmentYears,
                productTriangles
            );
        }

        /// <summary>
        /// Creates cumulative triangles from incremental triangles
        /// </summary>
        /// <remarks>Deep clones Triangles leaving incremental triangle unaffected</remarks>
        /// <param name="incrementalData"></param>
        /// <returns>cumulative triangles</returns>
        public TriangleDataFile GenerateCumulativeTriangles(IncrementalDataFile incrementalData)
        {         
            TriangleDataFile triangleDataFile = GenerateIncrementalTriangles(incrementalData);

            return GenerateCumulativeTriangles(triangleDataFile);
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
            (
                incrementalData.EarliestOriginYear,
                incrementalData.DevelopmentYears,
                triangles
            );           
        }

        private void Validate(IncrementalDataFile dataFile)
        {
            if (dataFile == null)
                throw new ArgumentNullException(nameof(dataFile));

            if(!dataFile.IsValid)
                throw new ArgumentException("Incremental data file is not valid for further processing");
        }

        /// <summary>
        /// Create triangle for each distinct product
        /// </summary>
        /// <returns>triangle dictionary</returns>
        private Dictionary<string, Triangle> CreateIncrementalTriangles(IncrementalDataFile dataFile)
        {
            var productTriangles = new Dictionary<string, Triangle>();

            int earliestOriginYear = dataFile.EarliestOriginYear;
            int developmentYears = dataFile.DevelopmentYears;

            foreach (string product in dataFile.Products)
            {
                productTriangles.Add(product, new Triangle(earliestOriginYear, developmentYears));
            }

            return productTriangles;
        }

        /// <summary>
        /// Initialize each product triangle from relevant data.
        /// </summary>
        private void Initialize(Dictionary<string, Triangle> productTriangles, IncrementalDataFile dataFile)
        {
            foreach (string product in dataFile.Products)
            {
                Triangle productTriangle = productTriangles[product];
                List<IncrementalRecord> productRecords = dataFile.Claims
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