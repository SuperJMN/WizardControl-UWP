 // The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using System;
    using System.Collections;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using GalaSoft.MvvmLight.Command;

    public sealed class WizardHost : Control
    {
        public static readonly DependencyProperty WizardsProperty = DependencyProperty.Register(
            "Wizards",
            typeof (IEnumerable),
            typeof (WizardHost),
            new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty WizardTemplateProperty = DependencyProperty.Register(
            "WizardTemplate",
            typeof (DataTemplate),
            typeof (WizardHost),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty SelectedWizardProperty = DependencyProperty.Register(
            "SelectedWizard",
            typeof (object),
            typeof (WizardHost),
            new PropertyMetadata(default(object)));

        public WizardHost()
        {
            DefaultStyleKey = typeof (WizardHost);
            //SizeChanged += OnSizeChanged;
            LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, object o)
        {
            if (ActualWidth < 700)
            {
                VisualStateManager.GoToState(this, "Compact", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Full", false);
            }
        }

        public IEnumerable Wizards
        {
            get { return (IEnumerable) GetValue(WizardsProperty); }
            set { SetValue(WizardsProperty, value); }
        }

        public DataTemplate WizardTemplate
        {
            get { return (DataTemplate) GetValue(WizardTemplateProperty); }
            set { SetValue(WizardTemplateProperty, value); }
        }

        public object SelectedWizard
        {
            get { return GetValue(SelectedWizardProperty); }
            set { SetValue(SelectedWizardProperty, value); }
        }

        public static readonly DependencyProperty WizardItemStyleProperty = DependencyProperty.Register(
            "WizardItemStyle",
            typeof (Style),
            typeof (WizardHost),
            new PropertyMetadata(default(Style)));

        public Style WizardItemStyle
        {
            get { return (Style) GetValue(WizardItemStyleProperty); }
            set { SetValue(WizardItemStyleProperty, value); }
        }

        public static readonly DependencyProperty ExecuteCommandProperty = DependencyProperty.Register(
            "ExecuteCommand",
            typeof (ICommand),
            typeof (WizardHost),
            new PropertyMetadata(default(RelayCommand)));

        public RelayCommand ExecuteCommand
        {
            get { return (RelayCommand) GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }
    }
}