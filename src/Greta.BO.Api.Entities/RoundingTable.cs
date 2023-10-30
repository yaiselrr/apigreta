namespace Greta.BO.Api.Entities;

public class RoundingTable: BaseEntityLong
{
    public int EndWith { get; set; }
    public int ChangeBy { get; set; }
}