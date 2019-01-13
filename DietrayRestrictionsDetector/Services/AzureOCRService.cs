using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DietrayRestrictionsDetector.Services
{
    public class AzureOCRService : IOCRService
    {
        private const int numberOfCharsInOperationId = 36;
        string subscriptionKey = "YOURKET";
        string cognitiveServiceEndPoint = "https://francecentral.api.cognitive.microsoft.com/";
        // For printed text, change to TextRecognitionMode.Printed
        TextRecognitionMode textRecognitionMode = TextRecognitionMode.Printed;
        ComputerVisionClient computerVision;

        public AzureOCRService()
        {
            computerVision = new ComputerVisionClient(
         new ApiKeyServiceClientCredentials(subscriptionKey),
         new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = cognitiveServiceEndPoint;
        }
        public async Task<string[]> DetectTextInImageAsync(Stream image)
        {
            return await ExtractLocalTextAsync(image);

        }
        // Recognize text from a local image
        private async Task<string[]> ExtractLocalTextAsync(
            Stream imageStream)
        {

            // Start the async process to recognize the text
            RecognizeTextInStreamHeaders textHeaders =
                await computerVision.RecognizeTextInStreamAsync(
                    imageStream, textRecognitionMode);

            return await GetTextAsync(computerVision, textHeaders.OperationLocation);

        }
        private async Task<string[]> GetTextAsync(
            ComputerVisionClient computerVision, string operationLocation)
        {

            string[] results;
            // Retrieve the URI where the recognized text will be
            // stored from the Operation-Location header
            string operationId = operationLocation.Substring(
                operationLocation.Length - numberOfCharsInOperationId);

            TextOperationResult result =
                await computerVision.GetTextOperationResultAsync(operationId);
            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {

                await Task.Delay(1000);
                result = await computerVision.GetTextOperationResultAsync(operationId);
            }
            var lines = result.RecognitionResult.Lines;
            return lines.Select(l => l.Text).ToArray();
        }
    }
}

