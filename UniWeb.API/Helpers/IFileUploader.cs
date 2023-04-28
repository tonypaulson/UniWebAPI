using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Helpers
{
    /// <summary>
    /// IFileUploader
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// UploadCloudinary
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadCloudinary(string imageData, string folderName);

        /// <summary>
        /// DeleteCloudinaryResource
        /// </summary>
        /// <param name="publicIds"></param>
        /// <returns></returns>
        Task<DelResResult> DeleteCloudinaryResource(List<string> publicIds);

        /// <summary>
        /// UploadStreamCloudinary
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadStreamCloudinary(Stream imageData, string folderName);
    }
}
