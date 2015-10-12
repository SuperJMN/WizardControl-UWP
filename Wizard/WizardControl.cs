using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using System.Windows.Input;
    using App4;
    using GalaSoft.MvvmLight.Command;

    public sealed class WizardControl : ListViewBase
    {
        private WizardHost host;

        public WizardControl()
        {
            this.DefaultStyleKey = typeof(WizardControl);

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            SelectionChanged += OnSelectionChanged;

            GoNextCommand = new RelayCommand(() => SelectedIndex++, () => SelectedIndex < Items.Count - 1);
            GoBackCommand = new RelayCommand(() => SelectedIndex--, () => SelectedIndex > 0);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            host = UIMixin.FindAncestor<WizardHost>(this);

            var vssHost = host.GetChildOfType<Border>();
            var stateGroups = VisualStateManager.GetVisualStateGroups(vssHost);
            var sizeModes = stateGroups.First(stateGroup => stateGroup.Name == "SizeModes");
            sizeModes.CurrentStateChanged += SizeModesOnCurrentStateChanged;
        }

        private void SizeModesOnCurrentStateChanged(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            VisualStateManager.GoToState(this, visualStateChangedEventArgs.NewState.Name, false);
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

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {

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
            var container = (ContentControl)element;

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

        public static readonly DependencyProperty GoNextCommandProperty = DependencyProperty.Register(
            "GoNextCommand",
            typeof (ICommand),
            typeof (WizardControl),
            new PropertyMetadata(default(RelayCommand)));

        public RelayCommand GoNextCommand
        {
            get { return (RelayCommand) GetValue(GoNextCommandProperty); }
            set { SetValue(GoNextCommandProperty, value); }
        }

        public static readonly DependencyProperty GoBackCommandProperty = DependencyProperty.Register(
            "GoBackCommand",
            typeof (ICommand),
            typeof (WizardControl),
            new PropertyMetadata(default(RelayCommand)));

        public RelayCommand GoBackCommand
        {
            get { return (RelayCommand) GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }
    }    
}
