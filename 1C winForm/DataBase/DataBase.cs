using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace _1C_winForm
{
    internal class DataBase
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["keyDB"].ConnectionString);
        
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public void EditDataTable(string tableName,string productName, string editText, int UserId, int idSelectItem, string userStatus)
        {
            try
            {
                string request = $"update {tableName} set {productName} = N'{editText}' where UserId = '{UserId}' and id = '{idSelectItem}'";

                if(userStatus == "директор" || userStatus == "администратор")
                {
                    request = $"update {tableName} set {productName} = N'{editText}' where id = '{idSelectItem}'";
                }
                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable table = new DataTable();

                SqlCommand command = new SqlCommand(request, connection);

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void updateDataTable(int UserId, ListView view, string nameTable, string columns,string userStatus)
        {
            view.Items.Clear();

            string request = $"select * from {nameTable} where UserId = '{UserId}'";

            if(userStatus == "директор" || userStatus == "администратор")
            {
                request = $"select * from {nameTable}";
            }


            SqlDataReader reader = null;

            try
            {
                SqlCommand command = new SqlCommand(request, connection);

                reader = command.ExecuteReader();

                ListViewItem item = null;

                while (reader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(reader["id"]),
                        Convert.ToString(reader["UserId"]),
                        Convert.ToString(reader[$"{columns}"])});


                    view.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        public void addDataTable(int UserId, string boxText, string nameTable, string column)
        {
            if (boxText == "")
                MessageBox.Show("Некорректно введены данные");
            else if (boxText != "")
            {
                try
                {
                    string request = $"insert into {nameTable} (UserId, {column}) values ('{UserId}', N'{boxText}')";

                    SqlDataAdapter adapter = new SqlDataAdapter();

                    DataTable table = new DataTable();

                    SqlCommand command = new SqlCommand(request, connection);

                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void removeData(string box, string tableName, int UserId,string userStatus)
        {
            try
            {
                string request = $"Delete from {tableName} Where id = '{box}' and UserId = '{UserId}'";

                if (userStatus == "директор" || userStatus == "администратор")
                {
                    request = $"Delete from {tableName} Where id = '{box}'";
                }
                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable table = new DataTable();

                SqlCommand command = new SqlCommand(request, connection);

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddDataBuyTable(ComboBox box1, ComboBox box2, ComboBox box3, TextBox text1, TextBox text2, int UserId, Label label)
        {
            if (box1.Text == "" || box2.Text == "" || box3.Text == "" ||
            text1.Text == "" || text2.Text == "" ||
                text1.Text.Contains(" ") || text2.Text.Contains(" "))
            {
                MessageBox.Show("Введены не все данные");
            }
            else
            {
                string request = $"insert into BuyDocuments (UserId, Product, Provider, Store, ProductCounts, ProductPrice, TotalSum) values ('{UserId}'," +
                $" N'{box1.Text}', N'{box2.Text}', N'{box3.Text}'," +
                $" '{text1.Text}', '{text2.Text}', '{label.Text}')";

                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable table = new DataTable();

                SqlCommand command = new SqlCommand(request, connection);

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }

        }

        public void UpdateDataBuyTable(ListView view, int UserId,string userStatus)
        {
            view.Items.Clear();

            string request = $"select * from BuyDocuments where UserId = '{UserId}'";

            if(userStatus == "директор" || userStatus == "администратор")
            {
                request = $"select * from BuyDocuments";
            }


            SqlDataReader reader = null;

            try
            {
                SqlCommand command = new SqlCommand(request, connection);

                reader = command.ExecuteReader();

                ListViewItem item = null;

                while (reader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(reader["id"]),
                        Convert.ToString(reader["UserId"]),
                        Convert.ToString(reader["Product"]),
                        Convert.ToString(reader["Provider"]),
                        Convert.ToString(reader["Store"]),
                        Convert.ToString(reader["ProductCounts"]),
                        Convert.ToString(reader["ProductPrice"]),
                        Convert.ToString(reader["TotalSum"]),});

                    view.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        public void AddDataHistoryUsers(int userId, string nameTable)
        {
            try
            {
                string request = $"insert into {nameTable} (UserId) values ({userId})";
                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable table = new DataTable();

                SqlCommand command = new SqlCommand(request, connection);

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}