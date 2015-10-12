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
        private WizardControl selector;

        public WizardItem()
        {
            this.DefaultStyleKey = typeof(WizardItem);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            selector = UIMixin.FindAncestor<WizardControl>(this);
            PreviousCommand = selector.GoBackCommand;
            NextCommand = selector.GoNextCommand;

            var vssHost = selector.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);
            
            sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            UpdateStates();
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
            selector.SelectionChanged += SelectorOnSelectionChanged;
        }

        private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateStates();
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            UpdateStates();
        }

        private void UpdateStates()
        {
            var selectedItem = selector.SelectedItem;
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
            var id = selector.IndexFromContainer(this);
            var last = selector.Items.Count - 1;

            if (id == last)
            {
                VisualStateManager.GoToState(this, "Last", false);
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

            var id = selector.IndexFromContainer(this);
            var current = selector.SelectedIndex;

            if (id <  current)
            {
                VisualStateManager.GoToState(this, "Previous", false);
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
            typeof (bool),
            typeof (WizardItem),
            new PropertyMetadata(default(bool)));

        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }
    }
}
