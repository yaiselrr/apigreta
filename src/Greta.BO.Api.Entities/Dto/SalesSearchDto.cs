using System;
using System.Collections.Generic;

namespace Greta.BO.Api.Entities.Dto
{
    public class SalesSearchRequestDto
    {
        public string CardNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public long? DeviceId { get; set; }
        public DateTime? SaleDay { get; set; }
        public string Invoice { get; set; }
    }
    
    public class SalesSearchResponseDto
    {
        public List<SalesSearchResponseDetailDto> Sales { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }
    public class SalesSearchResponseDetailDto
    {
        public long SaleId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Register { get; set; }
        public DateTime SaleDay { get; set; }
    }
}