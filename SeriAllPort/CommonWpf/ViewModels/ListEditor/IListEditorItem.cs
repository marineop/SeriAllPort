namespace CommonWpf.ViewModels.ListEditor
{
    public interface IListEditorItem : IHasName
    {
        bool CanNotDelete { get; }
    }
}
