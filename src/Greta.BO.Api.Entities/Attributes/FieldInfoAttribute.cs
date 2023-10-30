using System;

namespace Greta.BO.Api.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfoAttribute: Attribute
    {
        public string Header { get; }
        public int ColumnIndex { get; }
        public ValueFormating Formating { get; }

        public FieldInfoAttribute(
            string header,
            int columnIndex,
            ValueFormating valueFormating = ValueFormating.NoFormating
            )
        {
            Header = header;
            ColumnIndex = columnIndex;
            Formating = valueFormating;
        }

        public string Format(object context) => Formating switch
        {
            ValueFormating.EscapeQuotes when context is string value => $"\"{value.Replace("\"", "\"\"")}\"",
            ValueFormating.DateMMDDYYY when context is DateTime date => date.ToString("MM/dd/yyyy"),
            ValueFormating.DateYYYMMDD when context is DateTime date => date.ToString("yyy/MM/dd"),
            ValueFormating.NoFormating => context.ToString(),
            _ => context?.ToString() ?? String.Empty
        };
    }

    public enum ValueFormating
    {
        // true -> 1
        BooleanToNumber,
        // true -> "1
        // true"
        BooleanToString,
        // some "other" value -> some ""other"" value
        EscapeQuotes,
        // 29/12/2021 -> 2021/12/29
        DateYYYMMDD,
        // 29/12/2021 -> 12/29/2021
        DateMMDDYYY,
        NoFormating
    }
}