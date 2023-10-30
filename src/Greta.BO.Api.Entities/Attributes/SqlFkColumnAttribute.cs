using System;

namespace Greta.BO.Api.Entities.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class SqlFkColumnAttribute: Attribute
{
    public string ColumnName { get; set; }

    public SqlFkColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}