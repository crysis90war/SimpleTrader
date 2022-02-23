using SimpleTrader.Domain.Exceptions;
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using System;
using System.Threading.Tasks;

namespace SimpleTrader.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;
        private readonly LoginViewModel _loginViewModel;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator, IRenavigator renavigator)
        {
            _authenticator = authenticator;
            _renavigator = renavigator;
            _loginViewModel = loginViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _loginViewModel.ErrorMessage = string.Empty;

            try
            {
                await _authenticator.Login(_loginViewModel.Username, parameter.ToString());
                _renavigator.Renavigate();
            }
            catch (UserNotFoundException)
            {
                _loginViewModel.ErrorMessage = "Username or password does not exist.";
            }
            catch (InvalidPasswordException)
            {
                _loginViewModel.ErrorMessage = "Username or password does not exist.";
            }
            catch (Exception)
            {
                _loginViewModel.ErrorMessage = "Login failed.";
            }
        }
    }
}
