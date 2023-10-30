using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using System;
using Greta.BO.Api.Test.Helpers;

namespace Greta.BO.Api.Test.Repository
{
    public class ProvinceServiceTest
    {
        public ProvinceServiceTest()
        {
        }

        private ProvinceRepository Get_Repository()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();

            var options = builder.UseInMemoryDatabase(nameof(ProvinceServiceTest)).Options;

            var context = new SqlServerContext(options, TestsSingleton.Auth);

            var repository = new ProvinceRepository(TestsSingleton.Auth, context);
            return repository;
        }

        // Test getByCountry
        // Country not exist
        //Country  id is < 1
        //Country id exist
    }
}
