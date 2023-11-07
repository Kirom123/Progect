using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _1C_winForm
{
    public partial class _1CForm : Form
    {
        public int UserId;
        public string StatusUser;
        private DataBase _dataBase = new DataBase();

        public _1CForm(int id, string status)
        {
            this.UserId = id;
            this.StatusUser = status;
            InitializeComponent();
        }

        private void _1CForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataBaseDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.dataBaseDataSet.Products);
            _dataBase.OpenConnection();

            UpdateTableAllListView();

            this.Text = $"1C({StatusUser})";
        }

        private void UpdateTableListView(ListView view, string nameTable, string columns, ComboBox box)
        {
            _dataBase.updateDataTable(UserId, view, nameTable, columns,StatusUser);
            InitializationComboBox(box, nameTable);
        }

        private void UpdateTableAllListView()
        {
            UpdateTableListView(listView1, "Products", "ProductName", comboBox1);
            UpdateTableListView(listView2, "Provider", "NameProvider", comboBox2);
            UpdateTableListView(listView3, "Store", "NameStore", comboBox3);
            _dataBase.UpdateDataBuyTable(listView4, UserId, StatusUser);
        }

        private void addProduct_Click(object sender, EventArgs e)
        {
            AddTableInfo(productText, "Products", "ProductName", comboBox1);
            UpdateTableAllListView();
        }

        private void addProvider_Click(object sender, EventArgs e)
        {
            AddTableInfo(providerText, "Provider", "NameProvider", comboBox2);
            UpdateTableAllListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTableInfo(storeBox, "Store", "NameStore", comboBox3);
            UpdateTableAllListView();
        }

        private void AddTableInfo(TextBox box, string nameTable, string column, ComboBox combo)
        {
            _dataBase.addDataTable(UserId, box.Text, nameTable, column);
            box.Clear();
            InitializationComboBox(combo, nameTable);
        }

        private void InitializationComboBox(ComboBox box, string nameDB)
        {
            box.Items.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            string request = $"select * from {nameDB} where UserId = '{UserId}'";

            SqlCommand command = new SqlCommand(request, _dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            for (int i = 0; i < table.Rows.Count; i++)
                box.Items.Add(table.Rows[i][2]);
        }

        private void addDocument_Click(object sender, EventArgs e)
        {
            _dataBase.AddDataBuyTable(comboBox1, comboBox2, comboBox3, textBox2, textBox3, UserId, totalSumLabel);

            AddDataThroughComboBox(listView1, comboBox1, "Products", "ProductName");
            AddDataThroughComboBox(listView2, comboBox2, "Provider", "NameProvider");
            AddDataThroughComboBox(listView3, comboBox3, "Store", "NameStore");

            ClearTextAddDocument();
            _dataBase.UpdateDataBuyTable(listView4, UserId, StatusUser);
        }

        private void AddDataThroughComboBox(ListView view, ComboBox box, string tableName, string column)
        {
            if (!box.Items.Contains(box.Text))
            {
                if (checkBox1.Checked == true)
                {
                    _dataBase.addDataTable(UserId, box.Text, tableName, column);
                    UpdateTableListView(view, tableName, column, box);
                }
            }
        }

        private void ClearTextAddDocument()
        {
            comboBox1.Text = null; comboBox2.Text = null;
            comboBox3.Text = null;
            textBox2.Text = null; textBox3.Text = null;
            totalSumLabel.Text = "0";
        }

        private void deleteProduct_Click(object sender, EventArgs e)
        {
            DeleteData(listView1, "Products");
        }

        private void deleteProvider_Click(object sender, EventArgs e)
        {
            DeleteData(listView2, "Provider");
        }

        private void deleteStore_Click(object sender, EventArgs e)
        {
            DeleteData(listView3, "Store");
        }

        private void deleteDocument_Click(object sender, EventArgs e)
        {
            DeleteData(listView4, "BuyDocuments");
        }

        private void DeleteData(ListView list, string nameTable)
        {
            _dataBase.removeData(list.FocusedItem.Text, nameTable, UserId, StatusUser);
            UpdateTableAllListView();
        }

        private void CountTotalSum(object sender, KeyEventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text.Contains(" ")
               || textBox3.Text == "" || textBox3.Text.Contains(" "))
                totalSumLabel.Text = "0";
            else
            {
                try
                {
                    int count = int.Parse(textBox2.Text);
                    int price = int.Parse(textBox3.Text);

                    totalSumLabel.Text = (count * price).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void _1CForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _dataBase.AddDataHistoryUsers(UserId,"HistoryUser");
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditText("Products", "ProductName", Convert.ToInt32(listView1.FocusedItem.Text));
        }

        private void EditText(string nameTable, string nameColumn, int idItem)
        {
            EditTextWindow editTextWindow = new EditTextWindow();
            string updateText;
            editTextWindow.ShowDialog();

            updateText = editTextWindow.editTextBox.Text;

            _dataBase.EditDataTable(nameTable, nameColumn,updateText , UserId, idItem, StatusUser);
            UpdateTableAllListView();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditText("Provider", "NameProvider", Convert.ToInt32(listView2.FocusedItem.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditText("Store", "NameStore", Convert.ToInt32(listView3.FocusedItem.Text));
        }

        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _dataBase.AddDataHistoryUsers(UserId, "HistoryUser");
                EntryForm entryForm = new EntryForm();
                entryForm.Show();
                this.Hide();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dhgffghgfhfghfghfghfghgfh
        }
    }
}