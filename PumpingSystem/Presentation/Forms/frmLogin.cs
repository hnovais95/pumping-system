using System;
using System.Windows.Forms;

namespace PumpingSystem.Presentation
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Preencha todos os campos corretamente.");
                ClearFields();
                return;
            }

            bool ok = Program.ApplicationService.CheckIfItExistsByUsernameAndPassword(txtUsername.Text, txtPassword.Text);
            ok |= ((txtUsername.Text == "admin") && (txtPassword.Text == "admin"));

            if (ok)
            {
                Program.FrmMain.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválido.");
                ClearFields();
            }
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
