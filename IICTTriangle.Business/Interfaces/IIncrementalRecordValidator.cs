using Ict.Business.Models;

namespace Ict.Business.Interfaces
{
    public interface IIncrementalRecordValidator
    {        
        bool Validate(IncrementalRecord fileRecord);
    }
}
