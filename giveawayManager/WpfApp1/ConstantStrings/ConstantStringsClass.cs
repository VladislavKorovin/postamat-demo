using System;

namespace WpfApplication1.ConstantStrings
{
    public class ConstantStringsClass
    {
        public BP BP = new BP();
        public BS BS = new BS();
        public ServiceItem ServiceItem = new ServiceItem();
    }

    public class BP
    {
        public const String CORP_SERVICES = "Корпоративные сервисы";
    }

    public class BS
    {
        public const String MOBILE_PLATFORM = "Мобильная платформа";
        public const String VIDEO_CONFERENCE_SYSTEMS = "Системы видеосвязи";
        public const String WORKPLACE = "Рабочее место";
    }

    public class ServiceItem
    {
        public const String CONSULTATION = "Консультация";
        public const String MOBILE_DEVICE_PROBLEM = "Неисправность мобильного устройства";
        public const String WORKPLACE_CONFIGURATION = "Изменение конфигурации рабочего места";
        public const String VIDEO_CONFERENCE = "Видео конференц связь";
        public const String WORKPLACE_PROBLEM = "Проблемы с рабочим местом в ЦО";
    }

    public class GiveawayStatus
    {
        public static String DONE = "DONE";
        public static String IN_PROGRESS = "IN_PROGRESS";
        public static String PARTLY_DONE = "PARTLY_DONE";
        public static String READY = "READY";
    }
}
