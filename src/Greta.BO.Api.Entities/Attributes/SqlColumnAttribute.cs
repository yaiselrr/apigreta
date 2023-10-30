using System;

namespace Greta.BO.Api.Entities.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SqlColumnAttribute : Attribute
{
    public string ColumnName { get; set; }

    public SqlColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}