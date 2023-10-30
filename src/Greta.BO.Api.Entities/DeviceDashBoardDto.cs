using System;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.Api.Entities
{
    public class DeviceDashBoardDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Connected { get; set; }
    }
}