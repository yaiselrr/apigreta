using System;

namespace Greta.BO.Api.Entities.Attributes;
[AttributeUsage( AttributeTargets.Property)]
public class SqlFkTableAttribute: Attribute
{
    public string TableName { get; set; }
    public string ColumnName { get; }

    public SqlFkTableAttribute(string tableName, string columnName)
    {
        TableName = tableName;
        ColumnName = columnName;
    }
}