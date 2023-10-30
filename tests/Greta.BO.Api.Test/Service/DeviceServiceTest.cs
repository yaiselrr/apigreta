using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.DeviceSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service
{
    public class DeviceServiceTest
    {

        readonly DeviceRepository _repository;
        readonly DeviceService _service;

        public DeviceServiceTest()
        {
            var builder = new DbContextOptionsBuilder<SqlServerContext>();
            var options = builder.UseInMemoryDatabase(nameof(DeviceServiceTest)).Options;
            var context = new SqlServerContext(options, TestsSingleton.Auth);

            _repository = new DeviceRepository(TestsSingleton.Auth, context);
            _service = new DeviceService(_repository, Mock.Of<ILogger<DeviceService>>());
        }


        [Fact]
        public void Get_NormalCall_ResultOk()
        {
            // Arrange

            // Act
            var result = _service.Get().Result;

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task GetId_CallNotExist_ReturnNull()
        {
            // Arrange
            long id = 1000;
            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetId_CallNegative_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Get(id);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task GetId_NormalCall_ResultSameAsInsert()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            var id = await _repository.CreateAsync(device);
            // Act
            var result = await _service.Get(id);

            // Assert
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task Delete_NormalCall_ResultSucess()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            var id = await _repository.CreateAsync(device);

            // Act
            var result = await _service.Delete(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Delete(id);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task ChangeState_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.ChangeState(id, true);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task ChangeState_NormalCall_ResultTrue()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            var id = await _repository.CreateAsync(device);

            // Act
            var result = await _service.ChangeState(id, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Post_NormalCall_ResultSameId()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            // Act
            var result = await _service.Post(device);

            // Assert
            Assert.Equal(device.Name, (result as Device).Name);
        }

        [Fact]
        public async Task Put_NormalCall_ChangeSuccess()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            var id = await _repository.CreateAsync(device);

            var deviceUpdate = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                State = true
            };

            // Act
            var result = await _service.Put(id, deviceUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var deviceUpdate = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.Put(id, deviceUpdate);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterDevice_OutRange_ThrowBusinessLogicException()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name ",
                DeviceId = "101A",
            };

            var id = await _repository.CreateAsync(device);

            var spec = new DeviceFilterSpec(
               new DeviceSearchModel()
            );

            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.FilterSpec(
                    0, 0, null);
            });

            // Assert
            Assert.Equal("Page parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task FilterDevice_NormalCall_ResultOk()
        {
            // Arrange
            var cats = new List<Device>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new Device()
                {
                    Name = "_fil_ Device Name " + i,
                    DeviceId = "101A"
                });
            }

            await _repository.CreateRangeAsync<Device>(cats);

            var spec = new DeviceFilterSpec(
                new DeviceSearchModel()
                {
                    Name = "_fil_"
                }
            );

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
        }

        [Fact]
        public async Task SignalRConnected_NormalCall_ChangeSuccess()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            // Act
            var result = await _service.SignalRConnected(device.DeviceId, null);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SignalRConnected_Null()
        {
            // Arrange

            // Act
            var result = await _service.SignalRConnected(null, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignalRDisconnected_NormalCall_ChangeSuccess()
        {
            // Arrange

            // Act
            var result = await _service.SignalRDisconnected(null);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task SignalRDisconnected_Null()
        {
            // Arrange

            // Act
            var result = await _service.SignalRDisconnected("testing");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetConnectionIdById_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                SignalRConnectionId = null
            };

            var dev = await _service.Post(device);
            // Act
            var result = await _service.GetConnectionIdById(dev.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetConnectionIdById_Null()
        {
            // Arrange

            // Act
            var result = await _service.GetConnectionIdById(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetConnectionIdByDeviceId_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                SignalRConnectionId = null
            };

            var dev = await _service.Post(device);
            // Act
            var result = await _service.GetConnectionIdByDeviceId(dev.DeviceId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetConnectionIdByDeviceId_Null()
        {
            // Arrange

            // Act
            var result = await _service.GetConnectionIdByDeviceId(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetConnectionIdByGuid_NormalCall()
        {
            // Arrange
            var gu = new Guid();
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = null
            };

            await _repository.CreateAsync(device);
            // Act
            var result = await _service.GetConnectionIdByGuid(gu.ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePong_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = null,
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.UpdatePong(device.DeviceId, null);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task UpdatePong_NormalCall_Null()
        {
            // Arrange

            // Act
            var result = await _service.UpdatePong("", null);

            // Assert
            Assert.False((bool)result);
        }

        [Fact]
        public async Task PutName_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = null,
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.PutName(device.Id, device);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task PutName_NormalCall_Null_CallWithNotFound_ThrowBusinessLogicException()
        {
            // Arrange
            long id = -1000;
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = null,
                State = true
            };
            // Act
            var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
            {
                var result = await _service.PutName(id, device);
            });

            // Assert
            Assert.Equal("Id parameter out of bounds", exception.Message);
        }

        [Fact]
        public async Task GetDeviceConnectedByStore_NormalCall_ResultOk()
        {
            // Arrange
            var cats = new List<Device>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new Device()
                {
                    Name = "_fil_ Device Name " + i,
                    DeviceId = "101A" + i,
                    StoreId = 1,
                    SignalRConnectionId = "202Test"
                    
                });
            }
            await _repository.CreateRangeAsync<Device>(cats);

            var result = await _service.GetDeviceConnectedByStore(1);

            // Assert
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task PutConfiguration_NormalCall_ChangeSuccess()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A"
            };

            var id = await _repository.CreateAsync(device);

            var deviceUpdate = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                State = true
            };

            // Act
            var result = await _service.PutConfiguration(id, deviceUpdate);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public async Task GetIdByGuid_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = null,
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.GetIdByGuid(device.GuidId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByDeviceLic_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Id = 12,
                Name = "Device Name 107",
                DeviceId = "101AB",
                GuidId = Guid.Empty,
                SignalRConnectionId = null,
                Store = new Store()
                {
                    Id = 16,
                    Name = "Store 16"
                },
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.GetByDeviceLic(device.DeviceId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllConnectedByStore_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Id = 13,
                Name = "Device Name 107",
                DeviceId = "101A",
                GuidId = Guid.Empty,
                SignalRConnectionId = "Testing",
                StoreId = 17,
                Store = new Store()
                {
                    Id = 17,
                    Name = "Store 17"
                },
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.GetAllConnectedByStore(device.StoreId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetDeviceByConnectionId_NormalCall()
        {
            // Arrange
            var device = new Device()
            {
                Id = 190,
                Name = "Device Name 107",
                DeviceId = "101AWX",
                GuidId = Guid.Empty,
                SignalRConnectionId = "Testing",
                StoreId = 20,
                Store = new Store()
                {
                    Id = 20,
                    Name = "Store 20"
                },
                State = true
            };

            await _repository.CreateAsync(device);

            // Act
            var result = await _service.GetDeviceByConnectionId(device.SignalRConnectionId);

            // Assert
            Assert.NotNull(result);
        }


    }
}