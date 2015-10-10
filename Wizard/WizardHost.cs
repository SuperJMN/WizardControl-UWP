using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using System.Collections;

    public sealed class WizardHost : Control
    {
        public WizardHost()
        {
            this.DefaultStyleKey = typeof(WizardHost);
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs.NewSize.Width < 700)
            {
                VisualStateManager.GoToState(this, "Compact", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Full", false);
            }
        }

        public static readonly DependencyProperty WizardsProperty = DependencyProperty.Register(
            "Wizards",
            typeof (IEnumerable),
            typeof (WizardHost),
            new PropertyMetadata(default(IEnumerable)));

        public IEnumerable Wizards
        {
            get { return (IEnumerable) GetValue(WizardsProperty); }
            set { SetValue(WizardsProperty, value); }
        }

        public static readonly DependencyProperty WizardTemplateProperty = DependencyProperty.Register(
            "WizardTemplate",
            typeof (DataTemplate),
            typeof (WizardHost),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate WizardTemplate
        {
            get { return (DataTemplate) GetValue(WizardTemplateProperty); }
            set { SetValue(WizardTemplateProperty, value); }
        }


    }
}
