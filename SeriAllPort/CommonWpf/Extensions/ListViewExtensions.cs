using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace CommonWpf.Extensions
{
    public static class ListViewExtensions
    {
        public static readonly DependencyProperty AutoScrollToEndProperty =
            DependencyProperty.RegisterAttached(
                "AutoScrollToEnd",
                typeof(bool),
                typeof(ListViewExtensions),
                new PropertyMetadata(false, OnAutoScrollToEndChanged));

        public static bool GetAutoScrollToEnd(DependencyObject obj) =>
            (bool)obj.GetValue(AutoScrollToEndProperty);

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value) =>
            obj.SetValue(AutoScrollToEndProperty, value);

        private static void OnAutoScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ListView listView)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                listView.Loaded += (_, _) => HookCollectionChanged(listView);
                listView.TargetUpdated += (_, _) => HookCollectionChanged(listView);
            }
        }

        private static void HookCollectionChanged(ListView listView)
        {
            if (listView.ItemsSource is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged += (_, args) =>
                {
                    bool autoScrollToEnd = GetAutoScrollToEnd(listView);
                    if (autoScrollToEnd)
                    {
                        if (args.Action == NotifyCollectionChangedAction.Add && listView.Items.Count > 0)
                        {
                            listView.ScrollIntoView(listView.Items[^1]);
                        }
                    }
                };
            }
        }
    }
}