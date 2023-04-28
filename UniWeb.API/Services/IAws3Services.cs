namespace UniWeb.API.Services
{
    public interface IAws3Services
    {

        Task<byte[]> DownloadFileAsync(int patientId, int tenantId, string file);

        Task<bool> UploadFileAsync(int tenantId, int patientId, IFormFile file);
        //Task<string> UploadFileAsync(IFormFile file);

        Task<bool> DeleteFileAsync(int tenantId, int patientId, string fileName);
    }
}
