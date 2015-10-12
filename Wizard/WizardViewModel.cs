namespace App4
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Windows.UI.Popups;
    using GalaSoft.MvvmLight.Command;
    using Wizard;

    public class WizardViewModel
    {
        public WizardViewModel()
        {
            Wizards = new Collection<WizardPageViewModel>
            {
                new WizardPageViewModel {Name = "PRIMERO", IsValid = false},
                new WizardPageViewModel {Name = "SEGUNDO", IsValid = false},
                new WizardPageViewModel {Name = "TERCERO", IsValid = false},
                new WizardPageViewModel {Name = "CUARTO", IsValid = true},
            };

            foreach (var wizardPageViewModel in Wizards)
            {
                wizardPageViewModel.PropertyChanged+= WizardPageViewModelOnPropertyChanged;
            }

            ExecuteCommand = new RelayCommand(async () => await new MessageDialog("Ejecutado!").ShowAsync(), CanExecute );
        }

        private void WizardPageViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsValid")
            {
                ExecuteCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanExecute()
        {
            return Wizards.All(model => model.IsValid);
        }


        public IEnumerable<WizardPageViewModel> Wizards { get; private set; }

        public RelayCommand ExecuteCommand { get; set; }
    }
}