using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace APT_TaskR
{
    public class DBConn
    {
            public static string LoadConnectionString()
            {
                return "Provider=System.Data.SqlClient;Data Source=.\\apt_taskr.db;Version=3";
            }
    }
}
