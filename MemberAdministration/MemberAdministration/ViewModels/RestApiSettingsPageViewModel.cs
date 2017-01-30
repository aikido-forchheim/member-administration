using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MemberAdministration.ViewModels
{
    public class RestApiSettingsPageViewModel : BindableBase
    {
        private readonly ILogger _logger;
        private readonly IAccountService _accountService;

        public string ApiUrl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        public ICommand SaveCommand { get; private set; }

        public RestApiSettingsPageViewModel(ILogger logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;

            SaveCommand = new DelegateCommand<object>(this.OnSave, this.CanSave);
        }

        private void OnSave(object state)
        {
            try
            {
                _accountService.StoreRestApiAccount(ApiUrl, UserName, Password);

                Message = "Account-Informationen erfolgreich gespeichert...";
            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }
        }
        private bool CanSave(object state)
        {
            return true;
        }
    }
}
