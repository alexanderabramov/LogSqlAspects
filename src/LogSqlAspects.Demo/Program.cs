using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using log4net.Config;

// I'd expect one entry to cover IDbCommand, Common.DbCommand, derivatives and implementers,
// but it does not do so
//[assembly: LogSqlAspects.LogAdoNetAttribute(
//    AttributeTargetAssemblies = "System.Data",
//    AttributeTargetTypes = "System.Data.*DbCommand",
//    AttributeTargetMembers = "Execute*" // ExecuteScalar, NonQuery, Reader
//    )]

[assembly: LogSqlAspects.LogAdoNetAttribute(
    AttributeTargetAssemblies = "System.Data",
    AttributeTargetTypes = "System.Data.IDbCommand",
    AttributeTargetMembers = "Execute*"
    )]
//[assembly: LogSqlAspects.LogAdoNetAttribute(
//    AttributeTargetAssemblies = "System.Data",
//    AttributeTargetTypes = "System.Data.Common.DbCommand",
//    AttributeTargetMembers = "Execute*"
//    )]
//[assembly: LogSqlAspects.LogAdoNetAttribute(
//    AttributeTargetAssemblies = "System.Data",
//    AttributeTargetTypes = "System.Data.SqlClient.SqlCommand",
//    AttributeTargetMembers = "Execute*"
//    )]

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
