using System;
using System.Collections.Generic;
using System.Linq;
using IctTriangle.Business.Enums;
using IctTriangle.Business.Interfaces;
using IctTriangle.Business.Models;

namespace IctTriangle.Business.Services
{
    /// <summary>
    /// This is a wrapper over <see cref="IIncrementalRecordProvider"/>
    /// </summary>
    public class IncrementalDataFileGenerator : IncrementalDataFile
    {
        private readonly IIncrementalRecordProvider _incrementalRecordProvider;

        public IncrementalDataFileGenerator(IIncrementalRecordProvider reader)
        {
            _incrementalRecordProvider = reader ?? throw new ArgumentNullException(nameof(reader));

            Claims = reader.GetRecords().ToList();

            Products = GetDistinctProducts();

            Status = Validate();

            IsValid = isValid(Status);

            if (!IsValid) return;

            EarliestOriginYear = CalculateOriginYear();
            DevelopmentYears = CalculateDevelopmentYears(EarliestOriginYear);
        }

        private IncrementalDataFileStatus Validate()
        {
            IncrementalDataFileStatus status = IncrementalDataFileStatus.NoError;

            if (_incrementalRecordProvider.HasErrors)
            {
                status |= IncrementalDataFileStatus.InvalidRowsFound;
            }

            if (Claims.Count <= 0)
            {
                status |= IncrementalDataFileStatus.NoProductsFound;
                return status;
            }

            if (Products.Count <= 0)
            {
                status |= IncrementalDataFileStatus.NoProductsFound;
            }

            return status;
        }

        private bool isValid(IncrementalDataFileStatus status)
        {
            return !status.HasFlag(IncrementalDataFileStatus.NoProductsFound);
        }

        private List<string> GetDistinctProducts()
        {
            // converted to lower case to ignore case.
            return Claims.Select(c => c.Product)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }

        private int CalculateOriginYear()
        {
            var minOrigin = Claims.Min(c => c.OriginYear);
            var minDevelopmentYear = Claims.Min(c => c.DevelopmentYear);
            return Math.Min(minOrigin, minDevelopmentYear);
        }

        private int CalculateDevelopmentYears(int originYear)
        {
            var maxOrigin = Claims.Max(c => c.OriginYear);
            var maxDevelopmentYear = Claims.Max(c => c.DevelopmentYear);
            return Math.Max(maxOrigin, maxDevelopmentYear) - originYear + 1;
        }
    }
}
