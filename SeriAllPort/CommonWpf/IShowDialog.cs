using System.Windows;

namespace CommonWpf
{
    public interface IShowDialog
    {
        bool ShowDialog(object dataContext,
            string title = "Dialog",
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            bool showInTaskbar = false,
            double width = 800,
            double height = 600);
    }
}
