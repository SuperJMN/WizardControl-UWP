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
            PreviousCommand = wizardControl.GoBackCommand;
            NextCommand = wizardControl.GoNextCommand;
            ExecuteCommand = wizardControl.ExecuteCommand;

            var vssHost = wizardControl.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);

            sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            UpdateSizeStates();
            UpdateIsValidStates();
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
            wizardControl.SelectionChanged += WizardControlOnSelectionChanged;
        }

        private void WizardControlOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateSizeStates();
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            UpdateSizeStates();
        }

        private void UpdateSizeStates()
        {
            var selectedItem = wizardControl.SelectedItem;
            if (sizeModes.CurrentState.Name == "Full")
            {
                SetExpandedStateBasedOnOrder();
            }
            else
            {

                SetCompactStateBasedOnOrder(selectedItem);
            }
        }

        private void SetExpandedStateBasedOnOrder()
        {
            var id = wizardControl.IndexFromContainer(this);
            var last = wizardControl.Items.Count - 1;

            if (id == last)
            {
                VisualStateManager.GoToState(this, "LastExpanded", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", false);
            }
        }

        private void SetCompactStateBasedOnOrder(object selectedItem)
        {
            if (selectedItem == null)
            {
                GoToNone();
            }

            var id = wizardControl.IndexFromContainer(this);
            var last = wizardControl.Items.Count - 1;
            var current = wizardControl.SelectedIndex;

            if (id < current)
            {
                VisualStateManager.GoToState(this, "Previous", false);
            }
            else if (id == last)
            {
                VisualStateManager.GoToState(this, "LastCompact", false);
            }
            else if (id == current)
            {
                VisualStateManager.GoToState(this, "Current", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Next", false);
            }
        }

        private void GoToNone()
        {
            VisualStateManager.GoToState(this, "None", false);
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
