using Newtonsoft.Json;
using req_class_namespace;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1;
using WpfApplication1;

namespace req_class_namespace
{

    public class User
    {
        public int ldap;
        public string fio;
        public string mail;
        public string title;
        public string phone;

        public void GetADUserByLdap(string indentity)
        {

            SearchResult result;

            string ADpath = "LDAP://ru1000";
            DirectoryEntry rootEnt = new DirectoryEntry(ADpath);
            DirectorySearcher searcher = new DirectorySearcher(rootEnt);

            searcher.PropertiesToLoad.Add("cn");
            searcher.Filter = ("(SAMAccountName=" + indentity + "*)");
            using (StreamWriter file = new StreamWriter("logs.txt", true))
            {
                file.WriteLine("#AD# " + DateTime.Now.ToString() + " Trying to find user " + indentity + " in AD ");
            }

            result = searcher.FindOne();
            using (StreamWriter file = new StreamWriter("logs.txt", true))
            {
                file.WriteLine("#AD# " + DateTime.Now.ToString() + " Search successful");
            }
            if (result == null)
            {
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#AD# " + DateTime.Now.ToString() + " No users found");
                }
                ldap = 0;
            }
            else
            {
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#AD# " + DateTime.Now.ToString() + " User found");
                }
                ldap = Int32.Parse(indentity);
                fio = result.GetDirectoryEntry().Properties["displayname"].Value.ToString();
                if (result.GetDirectoryEntry().Properties["title"].Value != null)
                {
                    title = result.GetDirectoryEntry().Properties["title"].Value.ToString();
                }
                if (result.GetDirectoryEntry().Properties["mail"].Value != null)
                {
                    mail = result.GetDirectoryEntry().Properties["mail"].Value.ToString();
                }
                if (result.GetDirectoryEntry().Properties["mobile"].Value != null)
                {
                    phone = result.GetDirectoryEntry().Properties["mobile"].Value.ToString();
                }
                else
                {
                    phone = "не указан";
                }
            }



        }
    }

    public class Constants
    {
        public string info_url;
        public string creayion_url;
        public string x_api_key;
        public string authorization;
        public string login;
        public string password;
        public string db_connection_string;
        public vaultSecret vaultData;

        public Constants()
        {
            var client = new RestClient();
            var request = new RestRequest();
            IRestResponse response;
            vaultClass vault = new vaultClass();
            vaultSecret secret = new vaultSecret();
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client = new RestClient("https://vault.lmru.adeo.com/v1/auth/approle/login");
                request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "4c608e1f-e570-d741-16f4-82102ab40560");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\n\t\"role_id\":\"19cf6581-765e-1d4f-1dd3-c6e6edf256b2\",\n\t\"secret_id\":\"d20e02ac-8b5c-6fe8-09da-289c6394a7c3\"\n}", ParameterType.RequestBody);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Getting token from Vault");
                }
                response = client.Execute(request);
                vault = JsonConvert.DeserializeObject<vaultClass>(response.Content);
                if (vault != null)
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Token recieved");
                    }



                client = new RestClient("https://vault.lmru.adeo.com/v1/LM-HQ%20L1/EQUERRY_TEST");
                request = new RestRequest(Method.GET);
                request.AddHeader("postman-token", "1556c450-1669-8c0b-254a-71c402ed9ed0");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("x-vault-token", vault.auth.client_token);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Getting secret from Vault");
                }
                response = client.Execute(request);
                secret = JsonConvert.DeserializeObject<vaultSecret>(response.Content);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Secret recieved");
                }
            }

            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    Thread windowCloser = new Thread(() =>
                    {
                        Thread.Sleep(15000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (Window w in App.Current.Windows)
                            {
                                if (w != App.Current.MainWindow)
                                    w.Close();
                            }
                        });

                    });
                    windowCloser.Start();

                    //popup_window vault_error = new popup_window("Произошла ошибка", "Хранилище секретов Vault недоступно", "", "Ок", Visibility.Hidden, "no_connection");
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#!!!VAULT!!!# " + DateTime.Now.ToString() + " Exception: " + e.Message);
                    }
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w != Application.Current.MainWindow)
                        {
                            w.Close();
                        }
                    }

                });

            }

            using (StreamWriter file1 = new StreamWriter("logs.txt", true))
            {
                file1.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Creating Constants object");
            }
            info_url = secret.data.info_url;
            creayion_url = secret.data.creation_url;
            x_api_key = secret.data.x_api_key;
            authorization = secret.data.authorization;
            login = secret.data.login;
            password = secret.data.password;
            db_connection_string = secret.data.db_connection_string;
            vaultData = secret;
            using (StreamWriter file1 = new StreamWriter("logs.txt", true))
            {
                file1.WriteLine("#VAULT# " + DateTime.Now.ToString() + " Object Constants has been created successfully");
            }
        }


    };



    public class req_class
    {
        public string CaseNumber { get; set; }
        public string CaseId { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Contact { get; set; }
        public string ContactId { get; set; }
        public string ContactLogin { get; set; }
        public string ContactServicePact { get; set; }
        public string ContactServicePactName { get; set; }
        public string Shop { get; set; }
        public string Email { get; set; }
        public string BP { get; set; }
        public string BS { get; set; }
        public string ServiceItem { get; set; }
        public string TS { get; set; }
        public string Urgency { get; set; }
        public string Impact { get; set; }
        public string GroupName { get; set; }
        public string OwnerName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string TechDesc { get; set; }
        public string SourceName { get; set; }
        public string Major { get; set; }
        public string ExtNumber { get; set; }
        public string RegisteredDate { get; set; }
        public string FirstSaveDate { get; set; }
        public string ResponseDate { get; set; }
        public string RespondedOnDate { get; set; }
        public string SolutionDate { get; set; }
        public string SolutionProvidedOnDate { get; set; }
        public string ClosureDate { get; set; }
        public string CreateGroupName { get; set; }
        public string CreatedByName { get; set; }
        public string Solution { get; set; }
        public string RootCause { get; set; }
        public string ClosureCode { get; set; }
        public string SolutionType { get; set; }
        public string TechResolution { get; set; }
        public string SatisfactionLevel { get; set; }
        public string SatisfactionLevelComment { get; set; }
        public string UsrNeedClassifyML { get; set; }

        public int pr;

        public req_class(string BP, string BS, string ServiceItem, string Contact, string ShortDescr, string Description, int Priority)
        {
            this.BP = BP;
            this.BS = BS;
            this.ServiceItem = ServiceItem;
            this.Contact = Contact;
            this.ShortDesc = ShortDescr;
            this.Description = Description;
            this.pr = Priority;
        }

        public req_class()
        {

        }

        public bool create_req(int Priority)
        {

            Constants constants = new Constants();

            DateTime t1 = new DateTime();
            t1 = DateTime.Now;
            try
            {
                var client = new RestClient(constants.creayion_url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("password", constants.password);
                request.AddHeader("login", constants.login);
                request.AddHeader("x-api-key", constants.x_api_key);
                string data = "{\n\t\"Action\":\"Create\",\n\t\"Title\":\"" + this.ShortDesc + "\",\n\t\"Description\":\"" + this.Description + "\",\n\t\"BP\":\"" + this.BP + "\",\n\t\"BS\":\"" + this.BS + "\",\n\t\"ServiceItem\":\"" + this.ServiceItem + "\",\n\t\"Contact\":\"" + this.Contact + "\"\n}";
                request.AddParameter("application/json", data, ParameterType.RequestBody);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Trying to create inc for " + this.Contact);
                }
                IRestResponse response = client.Execute(request);

                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Response recieved");
                }

                Regex regex = new Regex("[A-Z]{3}[0-9]{6}_[0-9]{4}");
                MatchCollection matchCollection = regex.Matches(response.Content.ToString());

                req_class createdReq = new req_class();
                createdReq = JsonConvert.DeserializeObject<req_class>(response.Content);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Inc created: " + matchCollection[0]);
                }
                this.CaseNumber = createdReq.CaseNumber;
                Thread dbThread = new Thread(() =>
                {
                    Task.Delay(3000);
                    req_class created_req = new req_class();
                    try
                    {
                        client = new RestClient(constants.info_url);
                        request = new RestRequest(Method.POST);
                        request.AddHeader("postman-token", "d3a81b68-caad-ff47-3c61-d6771687a3de");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("x-api-key", constants.x_api_key);
                        request.AddHeader("authorization", "Basic " + constants.authorization);
                        request.AddHeader("content-type", "application/json; charset=utf-8");
                        request.AddParameter("application/json; charset=utf-8", "{\n\t\"Action\":\"Info\",\n\t\"CaseNumber\": \"" + matchCollection[0] + "\"\n}", ParameterType.RequestBody);
                        using (StreamWriter file = new StreamWriter("logs.txt", true))
                        {
                            file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Trying to get information of " + matchCollection[0]);
                        }
                        response = client.Execute(request);
                        using (StreamWriter file = new StreamWriter("logs.txt", true))
                        {
                            file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Response recieved");
                        }

                        DateTime t2 = new DateTime();
                        t2 = DateTime.Now;

                        // MessageBox.Show((t2.Second - t1.Second).ToString());


                        regex = new Regex(@"\{\""CaseNumber"":.+?\}");
                        matchCollection = regex.Matches(response.Content);
                        created_req = JsonConvert.DeserializeObject<req_class>(matchCollection[0].ToString());

                    }
                    catch (Exception e)
                    {



                        using (StreamWriter file = new StreamWriter("logs.txt", true))
                        {
                            file.WriteLine("#!!!EXCEPTION!!!# " + DateTime.Now.ToString() + " Exception of subthread: " + e.Message + " Response: " + response.Content);
                        }
                    }
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#INC CREATION# " + DateTime.Now.ToString() + " Response parced successfully");
                    }

                    this.Description = created_req.Description;
                    this.Email = created_req.Email;
                    this.ExtNumber = created_req.ExtNumber;
                    this.BP = created_req.BP;
                    this.BS = created_req.BS;
                    this.CaseId = created_req.CaseId;
                    this.CaseNumber = created_req.CaseNumber;
                    this.ClosureCode = created_req.ClosureCode;
                    this.ClosureDate = created_req.ClosureDate;
                    this.Contact = created_req.Contact;
                    this.ContactId = created_req.ContactId;
                    this.ContactLogin = created_req.ContactLogin;
                    this.ContactServicePact = created_req.ContactServicePact;
                    this.ContactServicePactName = created_req.ContactServicePactName;
                    this.CreatedByName = created_req.CreatedByName;
                    this.CreateGroupName = created_req.CreateGroupName;
                    this.FirstSaveDate = created_req.FirstSaveDate;
                    this.GroupName = created_req.GroupName;
                    this.Impact = created_req.Impact;
                    this.Major = created_req.Major;
                    this.OwnerName = created_req.OwnerName;
                    this.Priority = created_req.Priority;
                    this.RegisteredDate = created_req.RegisteredDate;
                    this.RespondedOnDate = created_req.RespondedOnDate;
                    this.ResponseDate = created_req.ResponseDate;
                    this.RootCause = created_req.RootCause;
                    this.SatisfactionLevel = created_req.SatisfactionLevel;
                    this.SatisfactionLevelComment = created_req.SatisfactionLevelComment;
                    this.ServiceItem = created_req.ServiceItem;
                    this.Shop = created_req.Shop;
                    this.ShortDesc = created_req.ShortDesc;
                    this.Solution = created_req.Solution;
                    this.SolutionDate = created_req.SolutionDate;
                    this.SolutionProvidedOnDate = created_req.SolutionProvidedOnDate;
                    this.SolutionType = created_req.SolutionType;
                    this.SourceName = created_req.SourceName;
                    this.Status = created_req.Status;
                    this.TechDesc = created_req.TechDesc;
                    this.TechResolution = created_req.TechResolution;
                    this.TS = created_req.TS;
                    this.Urgency = created_req.Urgency;
                    this.UsrNeedClassifyML = created_req.UsrNeedClassifyML;

                    db_request db_Request = new db_request(this, Priority, false);
                    db_Request.AddToDB();

                });
                dbThread.Start();
                return true;
            }
            catch (Exception e)
            {
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#!!!EXCEPTION!!!# " + DateTime.Now.ToString() + " Exception: " + e.Message);

                    Thread windowCloser = new Thread(() =>
                    {
                        Thread.Sleep(15000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (Window w in App.Current.Windows)
                            {
                                if (w != App.Current.MainWindow)
                                    w.Close();
                            }
                        });

                    });
                    windowCloser.Start();

                    //popup_window popup_Window = new popup_window("Ошибка", "Произошла ошибка при создании заявки. Коллеги из HelpDesk оповещены и скоро тебе помогут.", "", "Понимаю...", Visibility.Hidden, "no_connection");
                   // popup_Window.ShowDialog();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w != App.Current.MainWindow)
                            w.Close();

                    }
                    return false;
                }
            }
        }

    }

    public class Resp
    {
        public req_class[] Result { get; set; }
    }


    public class db_request
    {
        public Constants constants;
        public string request_number { get; set; }
        public string request_id { get; set; }
        public string contact { get; set; }
        public string status { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime solution_date { get; set; }
        public int priority { get; set; }
        public int stp_ldap { get; set; }
        public bool flag { get; set; }
        public int window { get; set; }

        public bool change_status { get; set; }
        public bool need_creation { get; set; }

        public void AddToDB()
        {

            try
            {
                SqlConnection sql_qlient = new SqlConnection(constants.db_connection_string);
                string sql_query1 = "select * from dbo.requests where request_number = @request_number";
                SqlCommand command1 = new SqlCommand(sql_query1, sql_qlient);
                command1.Parameters.AddWithValue("@request_number", request_number);
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Opening SqlConnection (is inc in queue?)");
                }
                sql_qlient.Open();
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Connection opened successfully");
                    file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Trying to execute sql-command (is inc in queue?)");
                }
                SqlDataReader reader = command1.ExecuteReader();
                using (StreamWriter file = new StreamWriter("logs.txt", true))
                {
                    file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Executed successfully");
                }
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader[2].ToString() == "Отложено")
                    {
                        reader.Close();
                        string sql_query2 = "update dbo.requests set status1 = @new_status where request_number = @request_number";
                        SqlCommand command2 = new SqlCommand(sql_query2, sql_qlient);
                        command2.Parameters.AddWithValue("@new_status", "Из отложенных");
                        command2.Parameters.AddWithValue("@request_number", request_number);
                        using (StreamWriter file = new StreamWriter("logs.txt", true))
                        {
                            file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Trying to change inc's status");
                        }
                        command2.ExecuteNonQuery();
                        using (StreamWriter file = new StreamWriter("logs.txt", true))
                        {
                            file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Changed successfully");
                        }
                        change_status = true;
                        need_creation = false;
                        sql_qlient.Close();

                    }
                    else
                    {
                        need_creation = false;
                        change_status = false;
                    }
                }
                else
                {
                    reader.Close();
                    string sql_query = "INSERT INTO dbo.requests values (@request_number, @contact, @status, @creation_date, @solution_date, @priority, @stp_ldap, @flag, @window, @request_id)";
                    SqlCommand comand = new SqlCommand(sql_query, sql_qlient);
                    //sql_qlient.Open();
                    comand.Parameters.AddWithValue("@request_number", request_number);
                    comand.Parameters.AddWithValue("@request_id", request_id);
                    comand.Parameters.AddWithValue("@contact", contact);
                    comand.Parameters.AddWithValue("@status", status);
                    comand.Parameters.AddWithValue("@creation_date", creation_date);
                    comand.Parameters.AddWithValue("@solution_date", solution_date);
                    comand.Parameters.AddWithValue("@priority", priority);
                    comand.Parameters.AddWithValue("@stp_ldap", stp_ldap);
                    comand.Parameters.AddWithValue("@flag", true);
                    comand.Parameters.AddWithValue("@window", window);
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Trying to add inc to the database");
                    }
                    comand.ExecuteNonQuery();
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#DATABASE# " + DateTime.Now.ToString() + " Added successfully");
                    }
                    sql_qlient.Close();
                    change_status = false;
                    need_creation = true;
                }


            }
            catch (Exception e)
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    using (StreamWriter file = new StreamWriter("logs.txt", true))
                    {
                        file.WriteLine("#!!!EXCEPTION!!!# " + DateTime.Now.ToString() + " Database connection error. Exception: " + e.Message);
                    }

                    Thread windowCloser = new Thread(() =>
                    {
                        Thread.Sleep(15000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (Window w in App.Current.Windows)
                            {
                                if (w != App.Current.MainWindow)
                                    w.Close();
                            }
                        });

                    });
                    windowCloser.Start();

                    //popup_window db_error = new popup_window("Ошибка", String.Format("База данных системы недоступна. Cотрудники HelpDesk свяжутся с тобой в ближайшее время."), "Понимаю", "Понимаю", Visibility.Hidden, "no_connection");
                    //db_error.ShowDialog();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w != Application.Current.MainWindow)
                        {
                            w.Close();
                        }
                    }
                });
            }
        }

        public db_request(req_class req, int priority, bool flag)
        {
            request_number = req.CaseNumber;
            request_id = req.CaseId;
            contact = req.Contact;
            status = "В очереди";
            creation_date = DateTime.Now;
            solution_date = DateTime.MaxValue;
            this.priority = priority;
            stp_ldap = 0;
            this.flag = flag;
            window = 0;
            constants = new Constants();
        }

    }

}




public class Result
{
    public string CaseNumber { get; set; }
    public string CaseId { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Contact { get; set; }
    public string ContactId { get; set; }
    public string ContactLogin { get; set; }
    public string ContactServicePact { get; set; }
    public string ContactServicePactName { get; set; }
    public string Shop { get; set; }
    public string Email { get; set; }
    public string BP { get; set; }
    public string BS { get; set; }
    public string ServiceItem { get; set; }
    public object TS { get; set; }
    public string Urgency { get; set; }
    public string Impact { get; set; }
    public string GroupName { get; set; }
    public object OwnerName { get; set; }
    public string ShortDesc { get; set; }
    public string Description { get; set; }
    public string TechDesc { get; set; }
    public string SourceName { get; set; }
    public string Major { get; set; }
    public string ExtNumber { get; set; }
    public string WaitingText { get; set; }
    public DateTime RegisteredDate { get; set; }
    public DateTime FirstSaveDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    public object RespondedOnDate { get; set; }
    public DateTime? SolutionDate { get; set; }
    public object SolutionProvidedOnDate { get; set; }
    public object ClosureDate { get; set; }
    public string CreateGroupName { get; set; }
    public string CreatedByName { get; set; }
    public string Solution { get; set; }
    public object RootCause { get; set; }
    public object ClosureCode { get; set; }
    public object SolutionType { get; set; }
    public string TechResolution { get; set; }
    public object SatisfactionLevel { get; set; }
    public string SatisfactionLevelComment { get; set; }
    public bool UsrNeedClassifyML { get; set; }



}

public class Root
{
    public string Action { get; set; }
    public List<req_class> Result { get; set; }
}