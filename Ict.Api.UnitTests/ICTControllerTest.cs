using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using IctTriangle.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ict.Api.UnitTests
{
    [TestClass]
    public class IctControllerTest
    {
       
        [TestMethod]
        public void InvalidFile_BlankFile_Test()
        {
            // Arrange
            string csvData = @"";

            HttpControllerContext controllerContext = CreateControllerContext(csvData);
            string expected = "NoProductsFound";

            // Act
            IctController controller = new IctController {ControllerContext = controllerContext};
            var taskResult = controller.Accumulate();

            taskResult.Wait();

            var result = taskResult.Result as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Message, expected);
        }

        

        [TestMethod]
        public void InvalidFile_NoProduct_Test()
        {
            // Arrange
            string csvData = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            ,1995,1995,100";

            HttpControllerContext controllerContext = CreateControllerContext(csvData);
            string expected = "InvalidRowsFound, NoProductsFound";

            // Act
            IctController controller = new IctController {ControllerContext = controllerContext};
            var taskResult = controller.Accumulate();
            taskResult.Wait();
            var result = taskResult.Result as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Message, expected);
        }


        [TestMethod]
        public void SimpleFileUpload_TestAsync()
        {
            // Arrange
            string csvData = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            P1,1995,1995,100";

            HttpControllerContext controllerContext = CreateControllerContext(csvData);
            string expected = "1995, 1\r\nP1,100\r\n";

            // Act
            IctController controller = new IctController {ControllerContext = controllerContext};
            var taskResult = controller.Accumulate();
            taskResult.Wait();
            var result = taskResult.Result as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content, expected);
        }

         [TestMethod]
        public void Basic_Product_File_Csv_Test()
        {
            // Arrange
            string csvData = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Basic,1995,1995,100
                            basic,1995,1996,50
                            Basic,1995,1997,200
                            Basic,1996,1996,80
                            basic,1996,1997,40
                            Basic,1997,1997,120
                          ";

                     
            HttpControllerContext controllerContext = CreateControllerContext(csvData);
            string expected = "1995, 3\r\nBasic,100,150,350,80,120,120\r\n";            

            // Act
            IctController controller = new IctController {ControllerContext = controllerContext};
            var taskResult = controller.Accumulate();
            taskResult.Wait();
            var result = taskResult.Result as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content, expected);
        }

        [TestMethod]
        public void Multiple_Products_File_Csv_Test()
        {
            string csvData = @"Product, EarliestOriginYear, DevelopmentYear, IncrementalValue
                            Comp, 1992, 1992, 110.0
                            Comp, 1992, 1993, 170.0
                            Comp, 1993, 1993, 200.0
                            Non-Comp, 1990, 1990, 45.2
                            Non-Comp, 1990, 1991, 64.8
                            Non-Comp, 1990, 1993, 37.0
                            Non-Comp, 1991, 1991, 50.0
                            Non-Comp, 1991, 1992, 75.0
                            Non-Comp, 1991, 1993, 25.0
                            Non-Comp, 1992, 1992, 55.0
                            Non-Comp, 1992, 1993, 85.0
                            Non-Comp, 1993, 1993, 100.0
                          ";

            HttpControllerContext controllerContext = CreateControllerContext(csvData);
            string expected =
                "1990, 4\r\nComp,0,0,0,0,0,0,0,110,280,200\r\nNon-Comp,45.2,110,110,147,50,125,150,55,140,100\r\n";
            // Act
            IctController controller = new IctController {ControllerContext = controllerContext};
            var taskResult = controller.Accumulate();
            taskResult.Wait();
            var result = taskResult.Result as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content, expected);
        }

        /// <summary>
        /// Just a poor method to test
        /// </summary>
        /// <param name="contentString"></param>
        /// <returns></returns>
        private HttpControllerContext CreateControllerContext(string contentString)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "");

            var content = new MultipartFormDataContent();
            var contentBytes = new ByteArrayContent(Encoding.ASCII.GetBytes(contentString));

            contentBytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            contentBytes.Headers.Add("Content-Type", "text/plain");
            contentBytes.Headers.Add("Content-Length", contentString.Length.ToString());
            content.Add(contentBytes);
            request.Content = content;

            return new HttpControllerContext(new HttpConfiguration(), new HttpRouteData(new HttpRoute("")), request);
        }
    }
}
