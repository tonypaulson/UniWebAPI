namespace UniWeb.API.Services
{
    public interface IAppConfiguration
    {

        string AwsAccessKey { get; set; }
        string AwsSecretAccessKey { get; set; }
        string BucketName { get; set; }
        string Region { get; set; }
    }
}
