using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.SL.Core.Extensions.TypeExt;
using CMM.Core.SL.Core.Helpers;
using System.Reflection;

namespace CMM.Core.SL.Core.Extensions.Enum
{
    /// <summary>
    /// Расширение класса Enum
    /// </summary>
    internal static class EnumExtension
    {
        /// <summary>
        /// Получить значение атрибута Description для текущего значения переменной типа перечисление
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static string GetBaseDescription(this System.Enum _enum)
        {
            Type _enumType = _enum.GetType();

            MemberInfo[] memberInfo = _enumType
                .GetMember(_enum.ToString());

            if ((memberInfo != null 
                && memberInfo.Length > 0))
            {
                var _attributes = memberInfo[0]
                    .GetCustomAttributes(
                        typeof(System.ComponentModel.DescriptionAttribute),
                        false);

                if ((_attributes != null 
                    && _attributes.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_attributes
                        .ElementAt(0))
                        .Description;
                }
            }

            return _enum.ToString();
        }

        /// <summary>
        /// Получить локализованное значение атрибута Description для текущего значения переменной
        /// типа перечисление с учетом текущих настроек пользователя
        /// </summary>
        /// <param name="_enum">Перечисление</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <param name="descriptionsTypeNamePrefix">Префикс имени типа, хранящего
        /// строковые константы, описывающие элементы данного перечисления</param>
        /// <returns></returns>
        public static string GetLocalizedDescription(
            this System.Enum _enum,
            UserSettingsModel settings,
            string descriptionsTypeNamePrefix)
        {
            var desctiption = _enum.GetBaseDescription();

            var desctiptionsType = AssemblyHelper.GetTypeByNameAndSettings(
                descriptionsTypeNamePrefix,
                settings);

            return desctiptionsType.GetConstString(desctiption);
        }

        /// <summary>
        /// Проверить соответствие целочисленного представления значения переменной перечисления
        /// списку элементов перечисления
        /// </summary>
        /// <param name="_enum">Перечисление</param>
        /// <returns></returns>
        public static bool IsValid(this System.Enum _enum)
        {
            int[] validEnumValues = (int[])System.Enum
                .GetValues(_enum.GetType());

            int currentEnumValue = int.Parse(
                _enum.ToString("d"));

            return validEnumValues
                .Any(v => v == currentEnumValue);
        }
    }
}
