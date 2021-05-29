namespace WpfApplication1
{
    public class vaultClass
    {
        public string request_id { get; set; }
        public string lease_id { get; set; }
        public string renewable { get; set; }
        public string lease_duration { get; set; }
        public string data { get; set; }
        public string wrap_info { get; set; }
        public string warnings { get; set; }
        public auth auth;


    }

    public class vaultSecret
    {
        public data data;
    }

    public class auth
    {
        public string client_token { get; set; }
        public string accessor { get; set; }
        public string[] policies { get; set; }
        public string[] token_policies { get; set; }
    }

    public class data
    {          
        public string authorization { get; set; }
        public string creation_url { get; set; }
        public string db_connection_string { get; set; }
        public string info_url { get; set; }
        public string login { get; set; }
        public string mailBoxAddress { get; set; }
        public string mailBoxLogin { get; set; }
        public string mailBoxPassword { get; set; }
        public string password { get; set; }
        public string testApiKey { get; set; }
        public string testCreationUrl { get; set; }
        public string testLogin { get; set; }
        public string testPassword { get; set; }
        public string x_api_key { get; set; }
    }
}
