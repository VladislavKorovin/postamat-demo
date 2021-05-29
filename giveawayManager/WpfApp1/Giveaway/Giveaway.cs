using req_class_namespace;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using WpfApplication1.ExceptionHandlersNamespace;

namespace WpfApplication1.GiveawayNamespace
{
    public class Giveaway
    {
        public int id;        
        public String currentStatus;
        public int lockerNumber;
        public int contact;
        public String caseNumber;
                
        public Giveaway(SqlDataReader row)
        {            
            this.id = (int)row[GiveawayConstants.ID];               
            this.currentStatus = (String)row[GiveawayConstants.CURRENT_STATUS];
            this.lockerNumber = (int)row[GiveawayConstants.LOCKER_NUMBER];
            this.contact = (int)row[GiveawayConstants.CONTACT];
            this.caseNumber = (String)row[GiveawayConstants.CASE_NUMBER];
        }

        public Giveaway(int id, string currentStatus, int lockerNumber, int contact, string caseNumber)
        {
            this.id = id;
            this.currentStatus = currentStatus;
            this.lockerNumber = lockerNumber;
            this.contact = contact;
            this.caseNumber = caseNumber;
        }
    }

    public class GiveawayConstants {
        public static String ID = "id";               
        public static String CURRENT_STATUS = "currentStatus";
        public static String LOCKER_NUMBER = "lockerNumber";
        public static String CONTACT = "contact";
        public static String CASE_NUMBER = "caseNumber";
    }




}
