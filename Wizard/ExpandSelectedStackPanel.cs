namespace Wizard
{
    using System.Linq;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using App4;

    public class ExpandSelectedStackPanel : Panel
    {
        private Selector selector;

        public ExpandSelectedStackPanel()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            selector = UIMixin.FindAncestor<Selector>(this);
            selector.SelectionChanged += SelectorOnSelectionChanged;
        }

        private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {            
            InvalidateArrange();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;
            double width;
            width = finalSize.Width;

            
            var selectedContainer = GetSelectedContainer(SelectedItem);

            double selectedItemHeight = 0D;
            if (selectedContainer != null)
            {

                var totalHeight = Children.Sum(element => element.DesiredSize.Height);
                selectedItemHeight = finalSize.Height - (totalHeight - selectedContainer.DesiredSize.Height);
            }

            foreach (var child in Children)
            {
                if (child != selectedContainer)
                {
                    var height = child.DesiredSize.Height;
                    child.Arrange(new Rect(x, y, width, height));
                    y += height;
                }
                else
                {
                    var height = selectedItemHeight;
                    child.Arrange(new Rect(x, y, width, height));
                    y += height;
                }
            }

            return finalSize;
        }

        private object SelectedItem => selector?.SelectedItem;

        private UIElement GetSelectedContainer(object item)
        {
            if (item == null)
            {
                return null;
            }

            return (UIElement)selector.ContainerFromItem(item);
        }

        protected override Size MeasureOverride(Size availableSize)
        {


            foreach (var child in Children)
            {
                child.Measure(availableSize);
            }

            //var requiredSize = Children.Select(element => element.DesiredSize).Aggregate((a, b) => new Size(a.Width + b.Width, a.Height + b.Height));
            //return new Size(finalSize.Width, requiredSize.Height);
            return availableSize;
        }
    }
}