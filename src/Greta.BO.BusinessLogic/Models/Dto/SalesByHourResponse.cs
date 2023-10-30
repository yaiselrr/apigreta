using System;
using System.Collections.Generic;
using System.Linq;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class SalesByHourResponse
    {
        public decimal TotalAmount { get; set; }
        public int WeekCount { get; set; }
        public decimal Average { get; set; }
        public decimal AllMonth { get; set; }
        public List<SalesByHourItemResponse> Items { get; set; } = new List<SalesByHourItemResponse>();

        public static SalesByHourResponse operator +(SalesByHourResponse a, SalesByHourResponse b)
        {
            if (a.Items.Count == 0 && b.Items.Count == 0) return a;
            if (a.Items.Count == 0 ) return b;
            if ( b.Items.Count == 0) return a;
            var finalList = new List<SalesByHourItemResponse>();
            var result = new SalesByHourResponse();

            var large = a.Items.Count > b.Items.Count ? a.Items.ToList() : b.Items.ToList();
            
            foreach (var r in a.Items)
            {
                var bI = b.Items.FirstOrDefault(x => x.InitialHour == r.InitialHour);
                if (bI != null)
                {
                    finalList.Add(new SalesByHourItemResponse()
                    {
                        InitialHour = r.InitialHour,
                        HourInterval = r.HourInterval,
                        Amount = r.Amount + bI.Amount
                    });
                }
                else
                {
                    finalList.Add(new SalesByHourItemResponse()
                    {
                        InitialHour = r.InitialHour,
                        HourInterval = r.HourInterval,
                        Amount = r.Amount 
                    });
                }
            }
            foreach (var r in b.Items)
            {
                var bI = a.Items.FirstOrDefault(x => x.InitialHour == r.InitialHour);
                if (bI == null)
                {
                    finalList.Add(new SalesByHourItemResponse()
                    {
                        InitialHour = r.InitialHour,
                        HourInterval = r.HourInterval,
                        Amount = r.Amount
                    });
                }
            }
            finalList.Sort();
            result.Items = finalList;
            result.TotalAmount = result.Items.Sum(x => x.Amount);
            return result;
        }
    }

    public class SalesByHourItemResponse: IComparable<SalesByHourItemResponse>
    {
        public int InitialHour { get; set; }
        public string HourInterval { get; set; }
        public decimal Amount { get; set; }

        public string DayOfWeek { get; set; }

        public int CustomerCount { get; set; }

        public decimal Average { get; set; }
        public int CompareTo(SalesByHourItemResponse other)
        {
            if (other == null)
                return 1;
            else
                return this.InitialHour.CompareTo(other.InitialHour);
        }
        public override int GetHashCode()
        {
            return InitialHour;
        }
        public bool Equals(SalesByHourItemResponse other)
        {
            if (other == null) return false;
            return (this.InitialHour.Equals(other.InitialHour));
        }
    }
}