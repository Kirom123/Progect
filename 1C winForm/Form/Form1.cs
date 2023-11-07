using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _1C_winForm
{
    public partial class EntryForm : Form
    {

        DataBase dataBase = new DataBase();

        public EntryForm()
        {
            InitializeComponent();
        }

        private void EntryForm_Load(object sender, EventArgs e)
        {
            dataBase.OpenConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            var nameUser = logBox.Text;
            var passwordUser = passBox.Text;


            string request = $"select id, Login, Password, Status from TableUserStatus where Login = '{nameUser}' and Password = '{passwordUser}'";

            SqlCommand command = new SqlCommand(request, dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);



            if (table.Rows.Count == 1)
            {
                int id = Convert.ToInt32(table.Rows[0][0]);
                string statusUser = Convert.ToString(table.Rows[0][3]);
                MessageBox.Show($"Вход успешен, ваш id: {id}");
                _1CForm form = new _1CForm(id,statusUser);
                form.Show();
                dataBase.CloseConnection();
                this.Hide();
            }
            else
                MessageBox.Show("Такого аккаунта не существует");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistrForm form = new RegistrForm();
            form.Show();
            this.Hide();
        }

        private void EntryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            string request = $"select UserId from HistoryUser where id = (select MAX(id) from HistoryUser)";

            SqlCommand command = new SqlCommand(request, dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            int id = Convert.ToInt32(table.Rows[0][0]);
            string statusUser = Convert.ToString(table.Rows[0][3]);

            _1CForm form = new _1CForm(id, statusUser);
            form.Show();
            dataBase.CloseConnection();
            this.Hide();
        }
    }
}
