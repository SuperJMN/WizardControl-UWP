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
        private Selector selector;

        public WizardItem()
        {
            this.DefaultStyleKey = typeof(WizardItem);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            selector = UIMixin.FindAncestor<Selector>(this);


            var vssHost = selector.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);
            var sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            var selectedItem = selector.SelectedItem;
            var selectedItemContainer = selector.ContainerFromItem(selectedItem);

            if (visualStateChangedEventArgs.NewState.Name == "Full")
            {
                VisualStateManager.GoToState(this, "Expanded", false);
            }
            else
            {
                if (selectedItemContainer == this)
                {
                    VisualStateManager.GoToState(this, "CurrentCompact", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "NonCurrentCompact", false);
                }
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
    }
}
