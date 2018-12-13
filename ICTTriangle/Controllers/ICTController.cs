using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ict.Business.Helper;
using Ict.Business.Interfaces;
using Ict.Business.Models;
using Ict.Business.Services;

namespace IctTriangle.Controllers
{
    public class IctController : ApiController
    {
        private readonly IIncrementalTrianglesGenerator _trianglesGenerator;
        private readonly IIncrementalReaderService _readerService;
        public IctController(IIncrementalReaderService readerService, IIncrementalTrianglesGenerator trianglesGenerator)
        {
            _readerService = readerService;
            _trianglesGenerator = trianglesGenerator;
        }

        public IctController()
        {
            _trianglesGenerator = new IncrementalTrianglesGenerator();
            _readerService = new IncrementalReaderService();
        }

        [HttpPost, Route("api/ict/accumulate")]
        public async Task<IHttpActionResult> Accumulate()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            string csvData = await GetFileContent();

            IncrementalDataFile incrementalDataFile = _readerService.ReadCsvData(csvData);

            if (!incrementalDataFile.IsValid)
            {                
                return BadRequest(incrementalDataFile.Status.ToString());
            }

            TriangleDataFile cumulativeTriangleDataFile =
                _trianglesGenerator.GenerateCumulativeTriangles(incrementalDataFile);

            string cumulativeCsv = cumulativeTriangleDataFile.ToCsvString();

            return Ok(cumulativeCsv);
        }

        private async Task<string> GetFileContent()
        {
            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {                
                return await file.ReadAsStringAsync();
            }

            return null;
        }

    }
}