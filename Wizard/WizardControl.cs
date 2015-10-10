using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Wizard
{
    using Windows.UI.Xaml.Controls.Primitives;
    using App4;

    public sealed class WizardControl : ListViewBase
    {
        private WizardHost host;

        public WizardControl()
        {
            this.DefaultStyleKey = typeof(WizardControl);

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            SelectionChanged += OnSelectionChanged;
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
    }    
}
