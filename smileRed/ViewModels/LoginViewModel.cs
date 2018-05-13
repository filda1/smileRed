using smileRed.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace smileRed.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public Command LoginCommand { get; }
        public object Navigation { get; private set; }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            //this.apiService = new ApiService();
            this.IsRemembered = true;
            this.IsEnabled = true;
            this.Email = "aga5@hotmail.com";
            this.Password = "123";
            LoginCommand = new Command(login)

            { };
        }
        #endregion

        #region Commands
        private async void login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Você tem que validar o Email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Você tem que validar o Password",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            if (this.Email != "aga5@hotmail.com" || this.Password != "123")
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email ou Password errado",
                    "Accept");

                this.Password = string.Empty;
                this.Email = string.Empty;
                return;
            }
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Delivery = new DeliveryViewModel();       
            await Application.Current.MainPage.Navigation.PushAsync(new DeliveryPage());

            this.IsRunning = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;
        }
        #endregion
    }
}
