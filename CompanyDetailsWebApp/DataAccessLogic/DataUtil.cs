using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CompanyDetailsWebApp.DataAccessLogic
{
    public class DataUtil
    {
        string _sConnectionString;
        private readonly IConfiguration _configuration;

        public DataUtil(IConfiguration configuration)
        {
            _configuration = configuration;
            _sConnectionString = GetConnectionString(_configuration);
        }
        public static string GetConnectionString(IConfiguration _configuration)
        {
            string sServerName = string.Empty;
            string sDatabaseName = string.Empty;
            string sUserID = string.Empty;
            string sPassword = string.Empty;
            string sMaxPoolSize = "50";
            string connectionTimeOut = "10";
            try
            {
                //https://waspsource.beesys.com/Products/KC/-/issues/754
                if (!string.IsNullOrEmpty(_configuration["AppSettings:SERVER_NAME"]))
                    sServerName = _configuration["AppSettings:SERVER_NAME"].ToString();

                if (!string.IsNullOrEmpty(_configuration["AppSettings:DATABASE_NAME"]))
                    sDatabaseName = _configuration["AppSettings:DATABASE_NAME"].ToString();
                if (!string.IsNullOrEmpty(_configuration["AppSettings:USER_ID"]))
                    sUserID = _configuration["AppSettings:USER_ID"].ToString();
                if (!string.IsNullOrEmpty(_configuration["AppSettings:PASSWORD"]))
                    sPassword = _configuration["AppSettings:PASSWORD"].ToString();
                sMaxPoolSize = "100";
                connectionTimeOut = "100";

                if (_configuration["AppSettings:MAXPOOLSIZE"] != null)
                    sMaxPoolSize = _configuration["AppSettings:MAXPOOLSIZE"].ToString();
                if (_configuration["AppSettings:CONNECTION_TIMEOUT"] != null)
                    connectionTimeOut = _configuration["AppSettings:CONNECTION_TIMEOUT"].ToString();


                //System.Diagnostics.Trace.WriteLine($"@@KC@@ MaxPoolSize : {sMaxPoolSize}");
                return "server=" + sServerName
                    + ";database=" + sDatabaseName
                    + ";uid=" + sUserID
                    + ";pwd=" + sPassword
                    + ";Max Pool Size=" + sMaxPoolSize
                    + ";connection Timeout=" + connectionTimeOut
                    + ";MultipleActiveResultSets=true"
                    + ";Persist Security Info=true"
                    + ";Encrypt=true"
                    + ";TrustServerCertificate = true"
                    + ";";
            }//end of try
            catch (Exception ex)
            {
                //LogWriterCore.WriteLog("GetConnectionString", ex);
                return null;
            }//end of catch
        }

        public DataTable FetchCompanyByID(string companyId)
        {
            DataTable dTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetCompanyDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Counter", 1));
                        command.Parameters.Add(new SqlParameter("@Company_Id", companyId));
                        try
                        {
                            connection.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            adapter.Fill(dTable);
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTable;
        }


    }
}
