namespace ImageManager.API.Helpers
{
    public class AppSettings
    {
        public string AuthTokenSecret { get; set; }
        public string AzureAccountName { get; set; }
        public string AzureAccountKey { get; set; }
        public string AzureAccountConnString { get; set; }
        public string AzureAccountContainer { get; set; }
    }
}