using Ict.Business.Interfaces;
using Ict.Business.Models;

namespace Ict.Business.DataReaders
{
    public class IncrementalRecordDefaultValidator : IIncrementalRecordValidator
    {
        private const int MinYear = 1900;
        private const int MaxDevelopmentYear = 10000;

        public bool Validate(IncrementalRecord record)
        {
            if (record == null) return false;
            
            if(string.IsNullOrWhiteSpace(record.Product))
                return false;

            if (record.DevelopmentYear <= MinYear &&
                record.DevelopmentYear > MaxDevelopmentYear)
            {
                return false;
            }

            if (record.OriginYear <= MinYear &&
                record.OriginYear > MaxDevelopmentYear)
                return false;

            if (record.IncrementalValue < 0)
                return false;

            return true;
        }
    }
}
