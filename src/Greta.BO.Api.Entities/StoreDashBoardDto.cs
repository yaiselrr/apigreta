using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.Api.Entities
{
    public class StoreDashBoardDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DeviceDashBoardDto> Devices { get; set; }
    }
}