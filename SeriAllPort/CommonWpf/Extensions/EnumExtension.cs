using System.ComponentModel;
using System.Reflection;

namespace CommonWpf.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            string answer = value.ToString();
            Type type = value.GetType();
            FieldInfo? field = type.GetField(answer);

            if (field != null)
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    answer = attribute.Description;
                }
            }

            return answer;
        }
    }
}
