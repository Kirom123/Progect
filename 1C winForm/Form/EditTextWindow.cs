using System;
using System.Windows.Forms;

namespace _1C_winForm
{
    public partial class EditTextWindow : Form
    {
        public EditTextWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (editTextBox.Text == "" || editTextBox.Text.Length >= 1 && editTextBox.Text.Contains(" "))
                MessageBox.Show("Вы не ввели значение!!!");
            else
                this.Close();
        }
    }
}
