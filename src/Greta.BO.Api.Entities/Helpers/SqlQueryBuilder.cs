using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Helpers;

public enum SqlOperationType
{
    Insert,
    InsertOrUpdate,
    Update,
    Delete
}

public class SqlQueryBuilder //<TLiteEntity> where TLiteEntity : BaseEntityLong
{
    public List<string> Queries { get; set; }

    public SqlQueryBuilder()
    {
        Queries = new();
    }

    public void SimpleQuery<TLiteEntity>(TLiteEntity entity, SqlOperationType type)
        where TLiteEntity : BaseEntityLong
    {
        switch (type)
        {
            case SqlOperationType.InsertOrUpdate:
                InsertOrUpdate(entity);
                break;
            case SqlOperationType.Insert:
                Insert(entity);
                break;
            case SqlOperationType.Update:
                Update(entity);
                break;
            case SqlOperationType.Delete:
                Delete(entity);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void Insert<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var query = GenerateInsertQuery(entity);
        Queries.Add(query);
    }

    public void Update<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var tableName = GetTableName(entity);
        var query = GenerateUpdateQuery(tableName, entity);
        Queries.Add(query);
    }

    public void InsertOrUpdate<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var query = GenerateInsertOrUpdateQuery(entity);
        Queries.Add(query);
    }

    public void Delete<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var query = GenerateDeleteQuery( entity);
        Queries.Add(query);
    }

    private string GenerateInsertQuery<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var tableName = GetTableName(entity);
        var queryBuilder = new StringBuilder();
        var type = entity.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToList();

        var columnNames = new List<string>();
        var columnValues = new List<string>();

        //Initial data
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(List<long>))
                continue;

            var columnName = GetColumnName(property);
            var value = GetSqlValue(property.GetValue(entity));

            if (value == null)
                continue;
            columnNames.Add(columnName);
            columnValues.Add(value);
        }

        queryBuilder.AppendLine(
            $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", columnValues)}) ");
        queryBuilder.AppendLine($"GO--Separator");
        
        //check the relations and generate
        foreach (var property in properties)
        {
            if (property.PropertyType != typeof(List<long>))
                continue;
            
            var (fkTableName, columnName) = GetFkTableName(property);
            var fkColumnName = GetFkColumnName(property);
            
            if(fkTableName == null || columnName == null || fkColumnName == null)
                continue;
            
            var fkValues = (List<long>)property.GetValue(entity);
            
            //delete all elements relates with this
            queryBuilder.AppendLine($"DELETE FROM {fkTableName} WHERE {columnName} = {entity.Id}");
            queryBuilder.AppendLine($"GO--Separator");
            
            if (fkValues != null)
            {
                foreach (var fkValue in fkValues)
                {
                    queryBuilder.AppendLine($"INSERT INTO {fkTableName} ( {columnName}, {fkColumnName} ) VALUES ({entity.Id}, {fkValue}) ");
                    queryBuilder.AppendLine($"GO--Separator");
                }
            }
        }

        return queryBuilder.ToString();
    }

    private string GenerateUpdateQuery<TLiteEntity>(string tableName, TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        var type = entity.GetType();
        var properties = type.GetProperties();

        var setStatements = string.Join(", ", properties.Select(p => $"{GetColumnName(p)} = '{p.GetValue(entity)}'"));

        return $"UPDATE {tableName} SET {setStatements} WHERE Id = {entity.Id}";
    }

    private string GenerateInsertOrUpdateQuery<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        return GenerateInsertOrUpdateQuery(entity, entity.GetType());
    }

    private string GenerateInsertOrUpdateQuery(object entity, Type type)
    {
        var tableName = GetTableName(type);
        var queryBuilder = new StringBuilder();
        var isDerivate = false;
        var baseType = type.BaseType;
        
        if (baseType != null && baseType != typeof(BaseEntityLong))
        {
            isDerivate = true;
            var baseQuery = GenerateInsertOrUpdateQuery(entity, baseType);
            queryBuilder.AppendLine(baseQuery);
        }
        
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToList();

        var baseIdProperty = properties.FirstOrDefault(x => x.Name == "Id");// type.GetProperty("Id");
        var idValue = baseIdProperty!.GetValue(entity);
        
        var columnNames = new List<string>();
        var columnValues = new List<string>();
        var joinedValues = new List<string>();

        //Initial data
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(List<long>))
                continue;
            
            if(property.Name != "Id" && isDerivate && property.DeclaringType != type)
                continue;

            var columnName = GetColumnName(property);
            var value = GetSqlValue(property.GetValue(entity));

            if (value == null)
                continue;
            columnNames.Add(columnName);
            columnValues.Add(value);
            joinedValues.Add($"{columnName} = {value}");
        }

        queryBuilder.AppendLine($"IF EXISTS (SELECT 1 FROM {tableName} WHERE Id = {idValue}) ");
        queryBuilder.AppendLine($"BEGIN ");
        queryBuilder.AppendLine($"UPDATE {tableName} SET {string.Join(", ", joinedValues)} WHERE Id = {idValue} ");
        queryBuilder.AppendLine($"END ");
        queryBuilder.AppendLine($"ELSE ");
        queryBuilder.AppendLine($"BEGIN ");
        queryBuilder.AppendLine(
            $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", columnValues)}) ");
        queryBuilder.AppendLine($"END");
        queryBuilder.AppendLine($"GO--Separator");
        
        //check the relations and generate
        foreach (var property in properties)
        {
            if (property.PropertyType != typeof(List<long>))
                continue;
            
            var (fkTableName, columnName) = GetFkTableName(property);
            var fkColumnName = GetFkColumnName(property);
            
            if(fkTableName == null || columnName == null || fkColumnName == null)
                continue;
            
            var fkValues = (List<long>)property.GetValue(entity);
            
            if (fkValues != null)
            {
                //delete all elements relates with this
                queryBuilder.AppendLine($"DELETE FROM {fkTableName} WHERE {columnName} = {idValue}");
                queryBuilder.AppendLine($"GO--Separator");
                queryBuilder.AppendLine();

                var haveInserts = false;
                foreach (var fkValue in fkValues)
                {
                    if(!haveInserts)
                        haveInserts = true;
                    queryBuilder.AppendLine($"INSERT INTO {fkTableName} ( {columnName}, {fkColumnName} ) VALUES ({idValue}, {fkValue}) ");
                }
                if(haveInserts)
                    queryBuilder.AppendLine($"GO--Separator");
            }
        }

        return queryBuilder.ToString();
    }

    private string GenerateDeleteQuery<TLiteEntity>(TLiteEntity entity)
        where TLiteEntity : BaseEntityLong
    {
        return GenerateDeleteQuery(entity, entity.GetType());
    }
    
    private string GenerateDeleteQuery(object entity, Type type)
    {
        var tableName = GetTableName(type);
        var queryBuilder = new StringBuilder();
        var baseType = type.BaseType;
        
        if (baseType != null && baseType != typeof(BaseEntityLong))
        {
            var baseQuery = GenerateDeleteQuery(entity, baseType);
            queryBuilder.AppendLine(baseQuery);
            queryBuilder.AppendLine("GO--Separator");
            return queryBuilder.ToString();
        }
        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToList();

        var baseIdProperty = properties.FirstOrDefault(x => x.Name == "Id");
        var idValue = baseIdProperty!.GetValue(entity);
        
        queryBuilder.AppendLine($"DELETE FROM {tableName} WHERE Id = {idValue}");
        queryBuilder.AppendLine("GO--Separator");
        
        return queryBuilder.ToString();
    }

    private string GetTableName(object entity)
    {
        var type = entity.GetType();
        return GetTableName(type);
    }
    
    private string GetTableName(Type type)
    {
        var tableAttribute = type.GetCustomAttribute<SqlTableAttribute>();
        if (tableAttribute == null)
        {
            throw new Exception($"Definition for entity {type.FullName} does not have a {nameof(SqlTableAttribute)}");
        }

        return tableAttribute.TableName;
    }

    private string GetColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
        return columnAttribute != null ? columnAttribute.ColumnName : property.Name;
    }

    private (string, string) GetFkTableName(PropertyInfo property)
    {
        var tableAttribute = property.GetCustomAttribute<SqlFkTableAttribute>();
        var tableName = tableAttribute?.TableName;//property.Name;
        var columnName = tableAttribute?.ColumnName;//property.Name;
        
        return (tableName, columnName);
    }

    private string GetFkColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttribute<SqlFkColumnAttribute>();
        return columnAttribute?.ColumnName;//property.Name;
    }

    private string GetSqlValue(object value)
    {
        if (value == null)
        {
            return "NULL";
        }

        var type = value.GetType();

        if (type == typeof(string))
        {
            return $"'{EscapeSqlString((string)value)}'";
        }

        if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte))
        {
            return value.ToString();
        }

        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);
        }

        if (type == typeof(bool))
        {
            return ((bool)value) ? "1" : "0";
        }

        if (type == typeof(DateTime))
        {
            var dateTime = (DateTime)value;
            //2023-08-23 14:37:08.000
            return $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}'";
        }

        if (type == typeof(Guid))
        {
            var guid = (Guid)value;
            return $"'{guid.ToString()}'";
        }

        if (type.IsEnum)
        {
            return ((int)value).ToString();
        }

        throw new NotSupportedException($"Unsupported property type: {type.Name}");
    }

    private string EscapeSqlString(string value)
    {
        return value.Replace("'", "''");
    }

    public void SaveToFile(string filePath, string type, string version)
    {
        File.WriteAllText(filePath, SaveString(type, version));
    }

    public string SaveString(string type, string version)
    {
        var sqlQueryFile = new StringBuilder();
        //Header
        // sqlQueryFile.AppendLine($"-- {type} synchronization SQL file");
        // sqlQueryFile.AppendLine("-- Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
        // sqlQueryFile.AppendLine("-- Created by: Greta.BO.Api");
        // sqlQueryFile.AppendLine();

        foreach (string query in Queries)
        {
            sqlQueryFile.AppendLine(query);
            sqlQueryFile.AppendLine();
        }
        
        sqlQueryFile.AppendLine($"-- Updating DeviceConfiguration SynchroVersion to {version}");
        sqlQueryFile.AppendLine($"UPDATE DeviceConfiguration SET SynchroVersion= {version} WHERE Id = 1 ");
        sqlQueryFile.AppendLine("GO--Separator");
        sqlQueryFile.AppendLine();

        return sqlQueryFile.ToString();
    }
}