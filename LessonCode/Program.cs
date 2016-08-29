using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonCode
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder bulder = new SqlConnectionStringBuilder();

            bulder.UserID = "lecture";
            bulder.Password = "Qwert123";
            bulder.InitialCatalog = "lecture";
            bulder.DataSource = @"10.70.63.193\SQLEXPRESS";
            bulder.IntegratedSecurity = false;

            using (SqlConnection connection = new SqlConnection(bulder.ToString()))
            {
                try
                {
                    connection.Open(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                    return;
                }

                DataTable dataTable = new DataTable();
                DataColumn idColumn = new DataColumn("Id", typeof(int));
                idColumn.AutoIncrement = true;
                DataColumn nameColumn = new DataColumn("Name", typeof(string));
                dataTable.Columns.Add(idColumn);
                dataTable.Columns.Add(nameColumn);
                dataTable.PrimaryKey = new DataColumn[] { idColumn };

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                using (SqlCommand command = new SqlCommand())
                {                    
                    command.Connection = connection;
                    command.CommandText = "Select * from [dbo].[User]";
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(dataTable);
                }

                using (SqlCommand insertCommand = new SqlCommand())
                {
                    insertCommand.Connection = connection;
                    insertCommand.CommandText = "Insert into [dbo].[User] (Name) Values (@Name)";
                    SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 200, "Name");
                    insertCommand.Parameters.Add(nameParam);
                    dataAdapter.InsertCommand = insertCommand;

                    DataRow dataRow = dataTable.NewRow();
                    dataRow[1] = "Hello from Lesson 9";
                    dataTable.Rows.Add(dataRow);
                    dataAdapter.Update(dataTable);                             
                }

                using (SqlCommand updateCommand = new SqlCommand())
                {
                    updateCommand.CommandText = "Update [dbo].[User] SET Name = @Name where Id = @Id";
                    updateCommand.Connection = connection;
                    updateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 40, "Name");
                    updateCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");
                    dataAdapter.UpdateCommand = updateCommand;
                    dataTable.Rows[0]["Name"] = "Updated on Lesson9";
                    dataAdapter.Update(dataTable);
                }

                using (SqlCommand deleteCommand = new SqlCommand("Delete from [dbo].[User] where Id = @Id"))
                {
                    dataAdapter.DeleteCommand = deleteCommand;
                    deleteCommand.Connection = connection;
                    deleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

                    dataTable.Rows[3].Delete();

                    dataAdapter.Update(dataTable);
                }
            }

            Console.ReadLine();

        }
    }
}
