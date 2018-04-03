using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            this.userLoginValidation = new MockUserLoginValidation();
            LoginSuccessful += LoginControl_LoginSuccessful;
            LoginFailed += LoginControl_LoginFailed;
        }

        public event LoginSuccessfulHandler LoginSuccessful;

        public delegate void LoginSuccessfulHandler(object sender, string token);

        public event EventHandler LoginFailed;

        private IUserLoginValidation userLoginValidation;

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string token = await this.userLoginValidation.ValidateCredentials(this.tbUsername.Text.Trim(), this.tbPassword.Password.Trim());
            if (!string.IsNullOrEmpty(token))
            {
                this.ShowErrorMsg(false);
                if (LoginSuccessful != null)
                {
                    LoginSuccessful(this, token);
                }

            }
            else
            {

                if (LoginFailed != null) { LoginFailed(this, new EventArgs()); }
            }
            //if (LoginSuccessful != null) { LoginSuccessful(this, ""); }
        }
        public void ShowErrorMsg(bool show, string msg = null)
        {
            //this.lblError.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            //this.lblError.Content = msg == null ? "" : msg;
        }

        private void tbUserName_TextChanged(object sender, EventArgs e)
        {
            this.ShowErrorMsg(false);
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            this.ShowErrorMsg(false);
        }
        private void LoginControl_LoginFailed(object sender, EventArgs e)
        {
            ShowErrorMsg(true, "Username or password is invalid!");
        }

        private void LoginControl_LoginSuccessful(object sender, string token)
        {
            EventPage objEvent= new EventPage(this.tbUsername.Text.Trim(), token);

            objEvent.Show();
            this.Close();

        }
    }
    public interface IUserLoginValidation
    {
        Task<string> ValidateCredentials(string username, string password);
    }

    public class MockUserLoginValidation : IUserLoginValidation
    {
        public Task<string> ValidateCredentials(string username, string password)
        {
            // if (username == "admin" && password == "admin") return Task.FromResult("12345");

            var client = new RestClient("http://data.meetpoint.si/rest/v1/DataAPI/Authenticate/json");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "multipart/form-data;");
            //request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\""+username+"\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"Password\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            request.AddParameter("Username", username);
            request.AddParameter("Password", password);

            IRestResponse response = client.Execute(request);

            var res = SimpleJson.DeserializeObject<ServiceResponse>(response.Content);
            if (res.serviceStatus == "OK")
            {
                if (res.data.authStatus == "OK")
                    return Task.FromResult(res.data.authToken);
                else
                    return Task.FromResult(default(string));
            }
            else

                return Task.FromResult(default(string));
        }
    }
}
