using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using UniWeb.API.Services;

namespace UniWeb.API.Helpers
{
    public class FileUploader : IFileUploader
    {
        private readonly IServiceProvider _services;

        private readonly Account account = null;

        /// <summary>
        /// FileUploader
        /// </summary>
        public FileUploader(IServiceProvider services)
        {
            _services = services;
            var config = (ConfigurationService)_services.GetService(typeof(ConfigurationService));
            var cloudinaryConfig = config.GetCloudinaryConfig();
            account = new Account(
                            cloudinaryConfig.Cloud,
                            cloudinaryConfig.API_key,
                            cloudinaryConfig.API_Secret);
        }

        /// <summary>
        /// DeleteCloudinaryResource
        /// </summary>
        /// <param name="publicIds"></param>
        /// <returns></returns>
        public async Task<DelResResult> DeleteCloudinaryResource(List<string> publicIds)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            var delResParams = new DelResParams()
            {
                PublicIds = publicIds
            };
            return await cloudinary.DeleteResourcesAsync(delResParams);
        }

        /// <summary>
        /// UploadCloudinary
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<ImageUploadResult> UploadCloudinary(string imageData, string folderName)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), imageData),
                Folder = folderName
            };

            return await cloudinary.UploadAsync(uploadParams);
        }

        /// <summary>
        /// UploadStreamCloudinary
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<ImageUploadResult> UploadStreamCloudinary(Stream imageData, string folderName)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), imageData),
                Folder = folderName
            };

            return await cloudinary.UploadAsync(uploadParams);
        }
    }
}
