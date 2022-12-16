
using Microsoft.AspNetCore.Http;

namespace CareerConnect.Infrastructure.Common
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]?> GetDataAsync(this IFormFile? formFile)
        {
            if (formFile == null) return null;
            var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            var data = stream.ToArray();
            await stream.DisposeAsync();
            return data;
        }

        public static bool IsPdfFile(this IFormFile? formFile)
        {
            if (formFile == null) return false;
            return formFile.ContentType == "application/pdf";
        }
    }
}
