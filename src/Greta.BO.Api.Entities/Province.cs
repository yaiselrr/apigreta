namespace Greta.BO.Api.Entities
{
    public class Province : BaseEntityLong
    {
        public string Name { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
    }
}