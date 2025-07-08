using System.Windows.Markup;

namespace CommonWpf.Extensions
{
    public class EnumAllExtension : MarkupExtension
    {
        public EnumAllExtension(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType), "enumType must not be null");
            }
            else
            {
                _enumType = enumType;
            }
        }

        private Type _enumType;
        public Type EnumType
        {
            get => _enumType;
            private set
            {
                if (_enumType != value)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value), "enumType must not be null");
                    }
                    else
                    {
                        if (!value.IsEnum)
                        {
                            throw new ArgumentException("Type must be Enum");
                        }
                        else
                        {
                            _enumType = value;
                        }
                    }
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Array values = EnumType.GetEnumValues();
            EnumMember[] enumMembers = new EnumMember[values.Length];
            for (int i = 0; i < enumMembers.Length; ++i)
            {
                object? value = values.GetValue(i);
                enumMembers[i] = new EnumMember
                {
                    Value = value as Enum,
                    Description = (value as Enum)?.GetDescription()
                };
            }

            return enumMembers;
        }
    }

    public class EnumMember
    {
        public string? Description { get; set; }
        public Enum? Value { get; set; }
    }
}
