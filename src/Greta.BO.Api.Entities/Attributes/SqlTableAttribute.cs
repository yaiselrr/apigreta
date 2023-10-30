using System;

namespace Greta.BO.Api.Entities.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class SqlTableAttribute : Attribute
{
    public string TableName { get; set; }

    public SqlTableAttribute(string tableName)
    {
        TableName = tableName;
    }
}