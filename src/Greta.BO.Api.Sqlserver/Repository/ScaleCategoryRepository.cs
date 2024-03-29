﻿using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;

namespace Greta.BO.Api.Sqlserver.Repository
{
    public class ScaleCategoryRepository : OperationBase<long, string, ScaleCategory>, IScaleCategoryRepository
    {
        public ScaleCategoryRepository(IAuthenticateUser<string> authenticatetUser, SqlServerContext context)
            : base(authenticatetUser, context)
        {
        }

        public async Task<ScaleCategory> GetByCategory(int categoryId, bool track = true)
        {
            return track
                ? await Context.Set<ScaleCategory>().FirstOrDefaultAsync(e => e.CategoryId == categoryId)
                : await Context.Set<ScaleCategory>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.CategoryId == categoryId);
        }
    }
}