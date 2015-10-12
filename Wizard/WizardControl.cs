 // The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using App4;
    using GalaSoft.MvvmLight.Command;

    public sealed class WizardControl : ListView
    {
        public static readonly DependencyProperty GoNextCommandProperty = DependencyProperty.Register(
            "GoNextCommand",
            typeof (ICommand),
            typeof (WizardControl),
            new PropertyMetadata(default(RelayCommand)));

        public static readonly DependencyProperty GoBackCommandProperty = DependencyProperty.Register(
            "GoBackCommand",
            typeof (ICommand),
            typeof (WizardControl),
            new PropertyMetadata(default(RelayCommand)));

        public static readonly DependencyProperty ExecuteCommandProperty = DependencyProperty.Register(
            "ExecuteCommand",
            typeof (ICommand),
            typeof (WizardControl),
            new PropertyMetadata(default(RelayCommand)));

        private WizardHost host;

        private VisualStateGroup sizeModes;

        public WizardControl()
        {
            DefaultStyleKey = typeof (WizardControl);

            Loaded += OnLoaded;
            SelectionChanged += OnSelectionChanged;

            GoNextCommand = new RelayCommand(() => SelectedIndex++, () => SelectedIndex < Items.Count - 1);
            GoBackCommand = new RelayCommand(() => SelectedIndex--, () => SelectedIndex > 0);
        }

        public RelayCommand GoNextCommand
        {
            get { return (RelayCommand) GetValue(GoNextCommandProperty); }
            set { SetValue(GoNextCommandProperty, value); }
        }

        public RelayCommand GoBackCommand
        {
            get { return (RelayCommand) GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }

        public RelayCommand ExecuteCommand
        {
            get { return (RelayCommand) GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            host = UIMixin.FindAncestor<WizardHost>(this);

            var vssHost = host.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);
            sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            UpdateState();
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (sizeModes.CurrentState.Name == "Full")
            {
                VisualStateManager.GoToState(this, "Full", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Compact", false);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (SelectedItem == null)
            {
                EnsureSelection();
            }

            GoNextCommand.RaiseCanExecuteChanged();
            GoBackCommand.RaiseCanExecuteChanged();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is WizardItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new WizardItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var container = (ContentControl) element;

            container.DataContext = item;
            container.Style = ItemContainerStyle;
            container.ContentTemplate = ItemTemplate;
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            EnsureSelection();
        }

        private void EnsureSelection()
        {
            if (Items.Any())
            {
                SelectedItem = Items.First();
            }
        }
    }
}