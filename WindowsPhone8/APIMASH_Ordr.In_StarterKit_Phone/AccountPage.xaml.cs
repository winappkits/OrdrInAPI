using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class AccountPage : PhoneApplicationPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameText.Text == null || firstNameText.Text.Trim() == "")
            {
                MessageBox.Show("First name is needed to create an account.");
                return;
            }
            if (lastNameText.Text == null || lastNameText.Text.Trim() == "")
            {
                MessageBox.Show("Last name is needed to create an account.");
                return;
            }
            if (emailText.Text == null || emailText.Text.Trim() == "")
            {
                MessageBox.Show("Email address is needed to create an account.");
                return;
            }
            if (this.passwordText.Password == null || passwordText.Password.Trim() == "")
            {
                MessageBox.Show("Password is needed to create an account.");
                return;
            }
            if (passwordText.Password.Trim().Length < 2)
            {
                MessageBox.Show("Password needs to be at least 2 characters long.");
                return;
            }
            if (passwordText.Password != passwordCheckText.Password)
            {
                MessageBox.Show("The passwords must match to create an account.");
                return;
            }

            Globals.Instance.ordrin.CreateAccount(firstNameText.Text, lastNameText.Text, emailText.Text, passwordText.Password);
            NavigationService.GoBack();
        }

        private void loginPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordTextWatermark.Opacity = 0;
            passwordText.Opacity = 100;
        }

        private void loginPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckPasswordWatermark();
        }

        public void CheckPasswordWatermark()
        {
            var passwordEmpty = string.IsNullOrEmpty(passwordText.Password);
            passwordTextWatermark.Opacity = passwordEmpty ? 100 : 0;
            passwordText.Opacity = passwordEmpty ? 0 : 100;
        }

        private void passwordCheck_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordCheckTextWatermark.Opacity = 0;
            passwordCheckText.Opacity = 100;
        }

        private void passwordCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckPasswordCheckWatermark();
        }

        public void CheckPasswordCheckWatermark()
        {
            var passwordEmpty = string.IsNullOrEmpty(passwordCheckText.Password);
            passwordCheckTextWatermark.Opacity = passwordEmpty ? 100 : 0;
            passwordCheckText.Opacity = passwordEmpty ? 0 : 100;
        }

        private void firstNameText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // if the enter key is pressed
            if (e.Key == Key.Enter)
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
                this.Focus();
            }
        }

    }
}