using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using log4net.Config;

[assembly: LogSqlAspects.LogAdoNetAttribute(
    AttributeTargetAssemblies = "System.Data",
    AttributeTargetTypes = "System.Data.*Command", //@"regex:^System\.Data\.(Common\.DbCommand|IDbCommand|SqlClient\.SqlCommand)$",
    AttributeTargetMembers = "Execute*"
    )]

namespace LogSqlAspects.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            try
            {
                IDbCommand sqlCommand = new SqlCommand("SELECT everything FROM everywhere WHERE id=@id");
                sqlCommand.Parameters.Add(new SqlParameter("@id", "1"));
                sqlCommand.ExecuteReader();
            }
            catch
            {
                // sure it throws, what did you expect
                // but command text should be logged to console
            }
        }
    }
}
