using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace EM.Data.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<ListItem<string>> GetItemsAsString(this Type enumType, string selectedValue = null)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException("Type must be an enum.");
            }

            string[] names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>();
            var items = names.Zip(values, (name, value) =>
                new ListItem<string>
                {
                    Label = GetDisplayName(enumType, name),
                    Value = name,
                    Selected = (string.IsNullOrWhiteSpace(selectedValue) ? false : (selectedValue == name))
                });
            return items;
        }

        public static IEnumerable<ListItem<int>> GetItemsAsInteger(this Type enumType, int? selectedValue = null)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException("Type must be an enum.");
            }

            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException("Type must be an enum.");
            }

            string[] names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>();
            var items = names.Zip(values, (name, value) =>
                new ListItem<int>
                {
                    Label = GetDisplayName(enumType, name),
                    Value = value,
                    Selected = (selectedValue.HasValue ? (selectedValue.Value == value) : false)
                });
            return items;
        }

        public static IEnumerable<ListItem<int>> GetItemsFromDescription(this Type type, string selectedValue = null)
        {
            var result = new List<ListItem<int>>();
            var values = Enum.GetValues(type).Cast<int>();

            values.ToList().ForEach(v =>
            {
                var en = (Enum)Enum.Parse(type, v.ToString());

                var item = new ListItem<int>()
                {
                    Value = (int)v,
                    Label = GetDescription(en)
                };
                result.Add(item);
            });

            return result;
        }

        public static string GetDisplayName(Type enumType, string name)
        {
            string displayName = name;

            try
            {
                var attribute = enumType
               .GetField(name)
               .GetCustomAttributes(inherit: false)
               .OfType<DisplayAttribute>()
               .FirstOrDefault();
                if (attribute != null)
                {
                    displayName = attribute.GetName();
                }
            }
            catch
            {
                ; // do nothing
            }

            return displayName;
        }

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static int GetValueFromDisplayName<T>(this Type enumType, string displayName)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new ArgumentException("Type must be an enum.");
            }

            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                DisplayAttribute attrs = (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

                if (attrs != null && attrs.GetName() == displayName)
                {
                    return (int)value;
                }
            }

            return -1;
        }

        public static TAttribute GetAttribute<TAttribute>(Enum value) where TAttribute : Attribute
        {
            TAttribute attrVal = default(TAttribute);
            var type = value.GetType();
            string name = Enum.GetName(type, value);
            if (!string.IsNullOrWhiteSpace(name))
            {
                attrVal = type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
            }

            return attrVal;
        }
    }
}
