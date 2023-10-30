using System;
using System.Text.Json;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Helpers;
using Greta.BO.Api.Entities.Lite;

namespace Greta.BO.Api.Entities.Entensions;


public static class LiteSqlConverters
{

    public static void ProcessDetail(this SynchroDetail detail, ref SqlQueryBuilder builder)
    {
        var entity = detail.Entity;
        if (!detail.Entity.StartsWith("Lite"))
        {
            entity = $"Lite{detail.Entity}";
        }
        var type = Type.GetType($"Greta.BO.Api.Entities.Lite.{entity}");
        var obj = JsonSerializer.Deserialize(detail.Data, type) as BaseEntityLong;
        obj.ToSqlQuery(ref builder, detail.Type switch
        {
            SynchroType.DELETE => SqlOperationType.Delete,
            _ => SqlOperationType.InsertOrUpdate
        });
        
    }
    public static void ToSqlQuery(this BaseEntityLong gen, ref SqlQueryBuilder builder, SqlOperationType type)
    {
        builder.SimpleQuery(gen, type);
    }
}