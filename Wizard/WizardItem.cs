using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls.Primitives;
    using App4;
    using GalaSoft.MvvmLight.Command;

    public sealed class WizardItem : ContentControl
    {
        private WizardControl wizardControl;

        public WizardItem()
        {
            this.DefaultStyleKey = typeof(WizardItem);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            wizardControl = UIMixin.FindAncestor<WizardControl>(this);
            PreviousCommand = new RelayCommand(() => wizardControl.GoBackCommand.Execute(null), () => wizardControl.GoBackCommand.CanExecute(null));
            NextCommand = new RelayCommand(() => wizardControl.GoNextCommand.Execute(null), () => wizardControl.GoNextCommand.CanExecute(null) && IsValid);
            ExecuteCommand = wizardControl.ExecuteCommand;

            var vssHost = wizardControl.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);

            sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
            wizardControl.SelectionChanged += WizardControlOnSelectionChanged;

            UpdateSizeStates();
            UpdateIsValidStates();
            UpdateFinishStates();
        }

        private void UpdateFinishStates()
        {
            if (IsLast)
            {
                VisualStateManager.GoToState(this, "Last", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "NonLast", false);
            }
        }

        public bool IsLast => wizardControl.IndexFromContainer(this) == wizardControl.Items.Count - 1;
        public bool IsBeforeCurrent => wizardControl.IndexFromContainer(this) < wizardControl.SelectedIndex;
        public bool IsAfterCurrent => wizardControl.IndexFromContainer(this) > wizardControl.SelectedIndex;
        public bool IsCurrent => wizardControl.IndexFromContainer(this) == wizardControl.SelectedIndex;

        private void WizardControlOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateSizeStates();
            PreviousCommand.RaiseCanExecuteChanged();
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            UpdateSizeStates();
        }

        private void UpdateSizeStates()
        {
            if (IsBeforeCurrent)
            {
                VisualStateManager.GoToState(this, "BeforeCurrent", false);
            }
            else if (IsAfterCurrent)
            {
                VisualStateManager.GoToState(this, "AfterCurrent", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Current", false);
            }
        }

        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
          "NextCommand",
          typeof(ICommand),
          typeof(WizardItem),
          new PropertyMetadata(null));

        public static readonly DependencyProperty PreviousCommandProperty = DependencyProperty.Register(
            "PreviousCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(null));

        private VisualStateGroup sizeModes;

        public RelayCommand NextCommand
        {
            get { return (RelayCommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        public RelayCommand PreviousCommand
        {
            get { return (RelayCommand)GetValue(PreviousCommandProperty); }
            set { SetValue(PreviousCommandProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(
            "IsValid",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var source = (WizardItem)dependencyObject;
            
            source.UpdateIsValidStates();
            source.NextCommand.RaiseCanExecuteChanged();
            source.PreviousCommand.RaiseCanExecuteChanged();
        }

        private void UpdateIsValidStates()
        {
            if (IsValid)
            {
                VisualStateManager.GoToState(this, "Valid", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Invalid", false);
            }
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty ExecuteCommandProperty = DependencyProperty.Register(
            "ExecuteCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(default(RelayCommand)));

        public RelayCommand ExecuteCommand
        {
            get { return (RelayCommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }
    }
}
