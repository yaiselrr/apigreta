using System;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class AdBatch : Batch
    {
        public bool Active { get; set; }
        public DateTime EndTime { get; set; }
        // {    
        //     get 
        //     {        
        //         return _myUtcDateTime;        
        //     }
        //     set
        //     {   
        //         if(value.Kind == DateTimeKind.Utc)      
        //             _myUtcDateTime = value;            
        //         else if (value.Kind == DateTimeKind.Local)         
        //             _myUtcDateTime = value.ToUniversalTime();
        //         else 
        //             _myUtcDateTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);        
        //     }    
        // }
    }
}