using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using System.Web.Services;

namespace cgt.webservice
{
    public class SBR : System.Web.Services.WebService
    {
        static string connect = WebConfigurationManager.ConnectionStrings["sbr"].ToString();
        SqlConnection _connection = new SqlConnection(connect);
        SqlParameterCollection p;
        SqlCommand _command;
        SqlDataReader _reader;
        List<string> returnData;

        [WebMethod]
        public string[] GetList(string prefixText, int count, string contextKey)
        {
            _connection.Open();
            _command = new SqlCommand("[dbo].[AutoComplete]", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            p = _command.Parameters;
            p.Add("Key1", SqlDbType.Text).Value = prefixText;
            p.Add("Type", SqlDbType.Text).Value = contextKey;
            returnData = new List<string>();
            _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);
            while (_reader.Read())
            {
                returnData.Add(_reader["Label"].ToString());
            }
            _command.Dispose();
            _reader.Dispose();
            _connection.Dispose();
            return returnData.ToArray();
        }

        [WebMethod]
        public string[] GetHR(string Type, string K1, string K2, string K3, string K4, string K5)
        {
            _connection.Open();
            _command = new SqlCommand("[dbo].[AutoComplete]", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            p = _command.Parameters;
            p.Add("Type", SqlDbType.Text).Value = Type;
            p.Add("Key1", SqlDbType.Text).Value = K1;
            p.Add("Key2", SqlDbType.Text).Value = K2;
            p.Add("Key3", SqlDbType.Text).Value = K3;
            p.Add("Key4", SqlDbType.Text).Value = K4;
            p.Add("Key5", SqlDbType.Text).Value = K5;
            returnData = new List<string>();
            _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);
            while (_reader.Read())
            {
                returnData.Add(_reader["Label"].ToString());
            }
            _command.Dispose();
            _reader.Dispose();
            _connection.Dispose();
            return returnData.ToArray();
        }

        [WebMethod]
        public string DelAttachment(string s1)
        {
            return DeleteAttachments("\\\\10.2.1.33\\prototype\\attachment\\" + s1);
        }

        public string DeleteAttachments(string path)
        {
            string r = "DELETE ERROR";
            if (File.Exists(path))
            {
                File.Delete(path);
                r = "DELETE SUCCESS, REOPEN ATTACHMENT WINDOWS TO SEE THE RESULT";
            }
            return r;
        }
    }
}
