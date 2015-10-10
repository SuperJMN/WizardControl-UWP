namespace App4
{
    public class WizardPageViewModel : ViewModelBase
    {
        private bool isValid;
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                {
                    return;
                }
                name = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (value == isValid)
                {
                    return;
                }
                isValid = value;
                OnPropertyChanged();
            }
        }
    }
}