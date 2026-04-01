using BrandInspector.Presenters.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrandInspector.Views
{
    public partial class LoginForm : Form , ILoginView
    {
        public ILoginPresenter Presenter;
        public LoginForm()
        {
            InitializeComponent();
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
 
        private void LoginForm_Load(object sender, EventArgs e) { }
        private async void LoginBtn_Click_1(object sender, EventArgs e)
        {
            var response = await  Presenter.Login(usernameTxt.Text, passwordTxt.Text);
            if(!string.IsNullOrEmpty(response))
                ShowError(response);
        }
        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }

     


    }
}
