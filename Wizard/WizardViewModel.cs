namespace App4
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class WizardViewModel
    {
        public WizardViewModel()
        {
            Wizards = new Collection<WizardPageViewModel>()
            {
                new WizardPageViewModel {Name = "PRIMERO", IsValid = true},
                new WizardPageViewModel {Name = "SEGUNDO", IsValid = true},
                new WizardPageViewModel {Name = "TERCERO", IsValid = true},
                //new WizardPageViewModel {Name = "CUARTO", IsValid = true},
            };
        }

        public IEnumerable<WizardPageViewModel> Wizards { get; private set; }
    }
}