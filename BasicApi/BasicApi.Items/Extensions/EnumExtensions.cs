using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BasicApi.Items.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

            MemberInfo[] members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException(String.Format("Member '{0}' not found in type '{1}'", value, type.Name));

            MemberInfo member = members[0];
            DisplayAttribute displayAttr = member.GetCustomAttribute<DisplayAttribute>(false);

            if (displayAttr == null)
                return value.ToString();
            else
                return displayAttr.GetName();
        }
    }
}
