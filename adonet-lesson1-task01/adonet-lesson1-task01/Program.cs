using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;  // ✅ Для .NET Core/5/6/7/8

namespace AdoNetSample1
{
    class Program
    {
        SqlConnection conn = null;

        public Program()
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;
            Initial Catalog=Library;
            Integrated Security=SSPI;";
        }

        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.InsertQuery();
        }

        public void InsertQuery()
        {
            try
            {
                conn.Open();
                string insertString = @"INSERT INTO Authors (FirstName, LastName)
                                      VALUES ('Roger', 'Zelazny')";
                SqlCommand cmd = new SqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Данные успешно добавлены!");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
