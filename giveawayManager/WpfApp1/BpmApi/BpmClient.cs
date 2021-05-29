using Newtonsoft.Json;
using req_class_namespace;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.BpmApi
{
    class BpmClient
    {
        private Constants webConstants = new Constants();

        public static ItsmIncident testGetInfoByCaseNumber(String caseNumber) {
            var client = new RestClient("https://test-api-internal-op.apigee.lmru.tech/technical/tickets/v1/info");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("x-api-key", "fCvGXSXdpJ4pHtLLlAdQGItA3F6smlaf");
            request.AddHeader("Authorization", "Basic V1M6Z1dwXnohRE1jMk5h");
            request.AddHeader("Cookie", "NSC_ESNS=1b951ba3-ab2e-10af-9678-0050569c6c6f_1479420078_0136585033_00000000009051704530");
            request.AddParameter("application/json; charset=utf-8", "{\n\t\"Action\":\"Info\",\n\t\"CaseNumber\": \n\t[\n\t\t{\"number\":\""+caseNumber+"\"}\n\t\n\t]\n\t\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);            
            return JsonConvert.DeserializeObject<ItsmIncident>(response.Content);
        }

        public static ItsmCreationResponse testCreateIncidentGiveaway(ItsmCreationRequest creationRequest) {
            var client = new RestClient("https://test-api-internal-op.apigee.lmru.tech/technical/tickets/v1/ticket");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-api-key", "pjYLXPzh00WefwTg7548VlTW2I88a4ml");
            request.AddHeader("login", "API TEST");
            request.AddHeader("password", "123");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "NSC_ESNS=1b8d6741-a8c9-10af-9678-0050569c6c6f_2570215249_3375898110_00000000009051321811");
            request.AddParameter("application/json", 
                "{\r\n    \"Action\": \"Create\",\r\n    \"Title\": \""+creationRequest.Title+ "\",\r\n    \"Description\": \"" + creationRequest.Description + "\",\r\n    \"BP\": \"" + creationRequest.BP + "\",\r\n    \"BS\": \"" + creationRequest.BS + "\",\r\n    \"ServiceItem\": \"" + creationRequest.ServiceItem + "\",\r\n    \"Contact\": \"" + creationRequest.Contact + "\"    \r\n} ", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            return JsonConvert.DeserializeObject<ItsmCreationResponse>(response.Content);
            
        }

    }

    public class Result
    {
        public string CaseNumber { get; set; }
        public string CaseId { get; set; }
        public string Status { get; set; }
        public object ApproveStatus { get; set; }
        public string Priority { get; set; }
        public string Contact { get; set; }
        public string ContactId { get; set; }
        public string ContactLogin { get; set; }
        public string ContactServicePact { get; set; }
        public string ContactServicePactName { get; set; }
        public string Shop { get; set; }
        public string ShopNumber { get; set; }
        public string Email { get; set; }
        public string BP { get; set; }
        public string BS { get; set; }
        public string ServiceItem { get; set; }
        public string ServiceItemId { get; set; }
        public object TS { get; set; }
        public string Urgency { get; set; }
        public string Impact { get; set; }
        public string GroupName { get; set; }
        public string OwnerName { get; set; }
        public string Owner { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string TechDesc { get; set; }
        public string SourceName { get; set; }
        public string Major { get; set; }
        public string ExternalNumber { get; set; }
        public string WaitingText { get; set; }        
    }

    public class ItsmIncident
    {
        public string Action { get; set; }
        public List<Result> Result { get; set; }
    }

    public class ItsmCreationRequest
    {
        public ItsmCreationRequest(string title, string description, string bP, string bS, string serviceItem, string contact)
        {
            Title = title;
            Description = description;
            BP = bP;
            BS = bS;
            ServiceItem = serviceItem;
            Contact = contact;            
        }

        public string Action { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BP { get; set; }
        public string BS { get; set; }
        public string ServiceItem { get; set; }
        public string Contact { get; set; }
        public string ContEmail { get; set; }
        public object ContPhone { get; set; }
        public string TechDesc { get; set; }
        public object DopEmails { get; set; }
        public string ExternalNumber { get; set; }
    }

    public class ItsmCreationResponse
    {
        public string Action { get; set; }
        public string CaseNumber { get; set; }
        public string CaseId { get; set; }
    }

    public class ItsmInfoResponse { 
    
    }
}
