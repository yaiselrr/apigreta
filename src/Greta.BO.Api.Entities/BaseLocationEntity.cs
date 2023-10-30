using System;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.Api.Entities
{
    public class BaseLocationEntityLong : IEntityLocationBase<long, string>
    {
        #region Base

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion Base

        #region Location

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }
        public long ProvinceId { get; set; }
        public long CountryId { get; set; }
        public string Zip { get; set; }

        #endregion Location
    }
}