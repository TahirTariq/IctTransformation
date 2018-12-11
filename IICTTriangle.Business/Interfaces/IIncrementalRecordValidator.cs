using IctTriangle.Business.Models;

namespace IctTriangle.Business.Interfaces
{
    public interface IIncrementalRecordValidator
    {        
        bool Validate(IncrementalRecord fileRecord);
    }
}
