using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Amazon.S3.Transfer;
using UniWeb.API.Entities;
using System;
using System.Net;

namespace UniWeb.API.Services
{
    public class Aws3Services:IAws3Services
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey,string region, string bucketName)
        {
            _bucketName = bucketName;
            _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey,RegionEndpoint.GetBySystemName(region));
        }

        public async Task<byte[]> DownloadFileAsync(int patientId,int tenantId,string file)
        {
            MemoryStream ms = null;

            try
            {
                var filepath = "/";
                if (patientId == 0)
                {
                    filepath = "Tenant" + tenantId + "/";
                }
                else
                {
                    filepath = "Tenant" + tenantId + "patient" + patientId + "/";
                }


                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = filepath + file
                };

                using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                return ms.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UploadFileAsync(int tenantId,int patientId, IFormFile file)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var filepath = "/";
                  
                    if(file.ContentType== "application/json")
                    {
                        filepath = "Tenant" + tenantId + "/";
                    }
                    else
                    {
                        filepath = "Tenant" + tenantId + "patient" + patientId + "/";
                    }
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = filepath + file.FileName,
                        BucketName = _bucketName,
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                     await fileTransferUtility.UploadAsync(uploadRequest);
                    return true;
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        
         public async Task<bool> DeleteFileAsync(int patientId,int tenantId,string fileName)
        {
            var filepath = "/";

            if(patientId == 0)
            {
                filepath = "Tenant" + tenantId + "/";
            }
            else
            {
                filepath = "Tenant" + tenantId + "patient" + patientId + "/";
            }


            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = filepath + fileName
            };


            await _awsS3Client.DeleteObjectAsync(request);
            return true;
        }

    }
}
