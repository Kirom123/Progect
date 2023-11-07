using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _1C_winForm
{
    public partial class RegistrForm : Form
    {

        DataBase data = new DataBase();

        public RegistrForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nameUser = textBox1.Text;
            var passwordUser = textBox2.Text;
            var statusUser = textBox3.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();


            string request = $"select id, Login, Password from TableUserStatus where Login = '{nameUser}' and Password = '{passwordUser}'";

            SqlCommand command = new SqlCommand(request, data.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Такой пользователь уже есть попробуй другой логин");
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                request = $"insert into TableUserStatus(Login,Password,Status) values('{nameUser.ToLower()}', '{passwordUser.ToLower()}',N'{statusUser.ToLower()}')";

                command = new SqlCommand(request, data.GetConnection());

                data.OpenConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно зарегистрировались, теперь авторизуйтесь");
                    EntryForm form = new EntryForm();
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Не удалось зарегистрироваться");
                }
                data.CloseConnection();
            }
        }

        private void RegistrForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
