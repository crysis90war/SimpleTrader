using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.ViewModels;
using System;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly IAuthenticator _authenticator;
        private readonly LoginViewModel _loginViewModel;

        public event EventHandler CanExecuteChanged;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator)
        {
            _authenticator = authenticator;
            _loginViewModel = loginViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            bool success = await _authenticator.Login(_loginViewModel.Username, parameter.ToString());
        }
    }
}
