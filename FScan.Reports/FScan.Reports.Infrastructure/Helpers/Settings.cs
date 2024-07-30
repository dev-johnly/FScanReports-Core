using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Helpers;

public class Settings
{
    public static IConfiguration Configuration { get; set; }

    static Settings()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        Configuration = builder.Build();
    }

    public struct Config
    {
        public readonly static string DB2CONN = Configuration.GetSection("DB2Conn").Value;

        public readonly static string SMTP_PORT = Configuration.GetSection("SMTPPort").Value;

        public readonly static string SMTP_HOST = Configuration.GetSection("SMTPHost").Value;

        public readonly static string MAIL_FROM = Configuration.GetSection("MailFrom").Value;
    }


}
