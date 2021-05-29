using req_class_namespace;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using WpfApplication1.BpmApi;
using WpfApplication1.ConstantStrings;
using WpfApplication1.ExceptionHandlersNamespace;
using WpfApplication1.Mail;

namespace WpfApplication1.GiveawayNamespace
{
    public static class GiveawayProcessor
    {

        public static Giveaway getGiveawayById(int id)
        {
            Constants webConstants = new Constants();
            SqlDataReader dbReader = null;
            SqlConnection dbConnection = new SqlConnection(webConstants.db_connection_string);

            String sqlRequestString = "select * from giveaways where id = @id";
            SqlCommand sqlRequset = new SqlCommand(sqlRequestString, dbConnection);
            sqlRequset.Parameters.AddWithValue("@id", id);

            try { dbConnection.Open(); }
            catch { ExceptionHandlers.databaseConnectionExceptionHandler(); }

            try { dbReader = sqlRequset.ExecuteReader();
                
            }
            catch { ExceptionHandlers.databaseReadExceptionHandler(); }



            if (dbReader.HasRows)
            {
                dbReader.Read();
                dbConnection.Close();
                return new Giveaway(dbReader);
            }
            else
                dbConnection.Close();
            return null;
        }

        public static void setGiveawayStatus(Giveaway giveaway) {

        }

        public static int generateId()
        {
            Random random = new Random();
            while (true)
            {
                int id = random.Next(10000, 99999);
                Giveaway giveaway = getGiveawayById(id);
                if (giveaway == null)
                    return id;
            }
        }

        public static void createGiveaway(ItsmIncident incident) {
            int id = generateId();

            Regex reg = new Regex(@"\d{8}");
            MatchCollection matches = reg.Matches(incident.Result.First().Contact);

            Giveaway giveaway = new Giveaway(
                id,
                GiveawayStatus.READY,
                1,
                Int32.Parse(matches[0].ToString()),
                incident.Result.First().CaseNumber
                );

           

            Constants webConstants = new Constants();
            
            SqlConnection dbConnection = new SqlConnection(webConstants.db_connection_string);

            String sqlRequestString = "insert into giveaways values(@id, 1, 1, @currentStatus, @contact, @caseNumber, 60080049)";
            SqlCommand sqlRequset = new SqlCommand(sqlRequestString, dbConnection);
            sqlRequset.Parameters.AddWithValue("@id", id);
            sqlRequset.Parameters.AddWithValue("@currentStatus", giveaway.currentStatus);
            sqlRequset.Parameters.AddWithValue("@contact", giveaway.contact);
            sqlRequset.Parameters.AddWithValue("@caseNumber", giveaway.caseNumber);

            try { dbConnection.Open(); }
            catch { ExceptionHandlers.databaseConnectionExceptionHandler(); }

            try { sqlRequset.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch { ExceptionHandlers.databaseReadExceptionHandler(); }
            MailProcessor.sendMailMessageGiveawayReady(giveaway);

            



        }
    }
}
