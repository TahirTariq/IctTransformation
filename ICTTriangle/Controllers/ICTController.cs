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
        public IIctTransformationService TransformationService { get; }

        public IctController(IIctTransformationService transformationService)
        {
            TransformationService = transformationService;
        }

        public IctController()
        {
            TransformationService = new IctTransformationService();
        }

        [HttpPost, Route("api/ict/accumulate")]
        public async Task<IHttpActionResult> Accumulate()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            string csvData = await GetFileContent();

            IncrementalDataFile incrementalDataFile = TransformationService.ReadIncrementalCsvData(csvData);

            if (!incrementalDataFile.IsValid)
            {
                // TODO : can improve here
                return BadRequest(incrementalDataFile.Status.ToString());
            }

            TriangleDataFile cumulativeTriangleDataFile =
                TransformationService.CreateCumulativeData(incrementalDataFile);

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