using CommonWpf.ViewModels.ListEditor;

namespace CommonWpf
{
    public static class NameHelper
    {
        public static string GetUniqueName(IList<string> currentList, string prefix)
        {
            HashSet<string> table = [];
            foreach (string s in currentList)
            {
                if (s.StartsWith(prefix))
                {
                    table.Add(s);
                }
            }

            int numberNow = 0;
            while (true)
            {
                ++numberNow;
                string name = $"{prefix}{numberNow}";
                if (!table.Contains(name))
                {
                    break;
                }
            }

            return $"{prefix}{numberNow}";
        }

        public static string GetUniqueName<T>(IList<T> currentList, string prefix) where T : IHasName
        {
            List<string> names = [];
            foreach (T t in currentList)
            {
                names.Add(t.Name);
            }

            return GetUniqueName(names, prefix);
        }
    }
}
