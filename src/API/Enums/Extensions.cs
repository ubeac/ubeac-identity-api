using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace uBeac.Enums;

public static class Extensions
{
    public static IEnumerable<EnumModel> GetEnums(this Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(type => type.IsEnum))
        {
            var enumAttributes = type.GetCustomAttributes<EnumAttribute>().ToList();

            var enumModel = new EnumModel
            {
                Name = type.Name,
                Description = string.Join("\n", enumAttributes.Where(x => !string.IsNullOrEmpty(x.Description)).Select(x => x.Description)),
                Values = new List<EnumValueModel>()
            };

            foreach (var enumValue in Enum.GetValues(type))
            {
                var value = enumValue.ToString();

                var fieldInfo = type.GetField(value);
                var displayAttributes = fieldInfo.GetCustomAttributes<DisplayAttribute>().ToList();

                enumModel.Values.Add(new EnumValueModel
                {
                    Value = enumValue,
                    DisplayName =  displayAttributes.Select(x => x.Name).FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                    Name = Enum.GetName(type, enumValue),
                    Description = string.Join("\n", displayAttributes.Select(x => x.Description))
                });
            }

            yield return enumModel;
        }
    }

    public static IEnumerable<EnumModel> GetEnums(this IEnumerable<AssemblyName> assemblyNames)
        => assemblyNames.Select(Assembly.Load).SelectMany(assembly => assembly.GetEnums());
}