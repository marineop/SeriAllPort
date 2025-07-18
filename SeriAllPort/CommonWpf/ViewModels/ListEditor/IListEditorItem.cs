namespace CommonWpf.ViewModels.ListEditor
{
    public interface IListEditorItem
    {
        string Name { get; }

        bool CanNotDelete { get; }
    }
}
