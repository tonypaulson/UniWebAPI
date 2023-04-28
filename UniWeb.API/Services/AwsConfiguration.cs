namespace UniWeb.API.Services
{
    public class AwsConfiguration:IAppConfiguration
    {
        public AwsConfiguration(IConfiguration configuration)
        {
            BucketName = configuration.GetValue<string>("Aws:BucketName");
            Region = configuration.GetValue<string>("Aws:Region");
            AwsAccessKey = configuration.GetValue<string>("Aws:AwsAccessKey");
            AwsSecretAccessKey = configuration.GetValue<string>("Aws:AwsSecretAccessKey");

        }

        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
      
    }
}
