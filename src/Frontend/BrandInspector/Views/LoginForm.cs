using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrandInspector.Views
{
    public partial class LoginForm : Form , ILoginView
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public event EventHandler LoginClicked;

        public void OpenView()
        {
            Application.Run(this);
        }

        public void CloseView()
        {
            Close();
        }

        public void ShowError(string message)
        {
            throw new NotImplementedException();
        }

        private void LoginForm_Load(object sender, EventArgs e) { }
       




    }
}
