using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Core.Models.Pager;

namespace Greta.BO.Api.Test.Helpers
{
    internal class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            CreateMap(typeof(Pager<>), typeof(Pager<>));
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Family, FamilyModel>().ReverseMap();
            CreateMap<GiftCard, GiftCardModel>().ReverseMap();
            CreateMap<Vendor, VendorOrderVendorModel>().ReverseMap();
            CreateMap<BOUser, VendorOrderUserModel>().ReverseMap();
            CreateMap<Store, VendorOrderStoreModel>().ReverseMap();
            CreateMap<VendorOrder, VendorOrderModel>().ReverseMap();
            CreateMap<Vendor, VendorModel>().ReverseMap();
            CreateMap<MixAndMatch, MixAndMatchModel>().ReverseMap();           
            CreateMap<MixAndMatch, MixAndMatchGetAllModel>().ReverseMap();           
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, ProductSearchModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<Role, RoleGetAllDto>();

            CreateMap<ScaleLabelType, ScaleLabelTypeModel>().ReverseMap();
            CreateMap<Department, DepartmentModel>().ReverseMap();
            CreateMap<VendorProduct, VendorProductModel>().ReverseMap();
            CreateMap<StoreProduct, StoreProductModel>().ReverseMap();
            CreateMap<KitProduct, KitProductModel>().ReverseMap();

            CreateMap<Fee, FeeGetAllModel>().ReverseMap();
            CreateMap<Fee, FeeFilterModel>().ReverseMap();
        

            CreateMap<Entities.Report, ReportModel>().ReverseMap();
            CreateMap<StoreProduct, InventoryResponseModel>().ReverseMap();
            CreateMap<StoreProduct, BinLocationResponseModel>().ReverseMap();
            CreateMap<Product, ProductInventoryModel>().ReverseMap();
            CreateMap<VendorProduct, VendorProductForInventory>().ReverseMap();
            CreateMap<BinLocation, BinLocationInventoryModel>().ReverseMap();
            CreateMap<Profile, ProfilesListModel>().ReverseMap();
            CreateMap<Profiles, ProfilesListModel>().ReverseMap();
            CreateMap<Greta.BO.Api.Entities.Profiles, Greta.BO.BusinessLogic.Models.Dto.ProfilesModel>().ReverseMap();
            CreateMap<ClientApplication, ClientApplicationModel>().ReverseMap();
            CreateMap<FunctionGroup, FunctionGroupModel>().ReverseMap();
            CreateMap<Greta.BO.Api.Entities.FunctionGroup, Greta.BO.BusinessLogic.Models.Dto.FunctionGroupModel>().ReverseMap();
            CreateMap<Permission, PermissionModel>().ReverseMap();
            // Permission

            CreateMap<LoyaltyDiscount, LoyaltyDiscountModel>().ReverseMap();
            CreateMap<LoyaltyDiscount, LoyaltyDiscountCreateModel>().ReverseMap();
            CreateMap<LoyaltyDiscount, LoyaltyDiscountUpdateModel>().ReverseMap();
            CreateMap<LoyaltyDiscount, LoyaltyDiscountFilterModel>().ReverseMap();
            CreateMap<LoyaltyDiscount, LoyaltyDiscountGetAllModel>().ReverseMap();
            CreateMap<LoyaltyDiscount, LoyaltyDiscountGetByIdModel>().ReverseMap();

            CreateMap<CutListTemplate, CutListTemplateModel>().ReverseMap()
                .ForMember(vm => vm.ScaleProducts, m => m.MapFrom(u => u.ScaleProductIds.Select(x => new ScaleProduct { Id = x }).ToList()));
            CreateMap<ScaleProduct, ScaleProductModel>().ReverseMap(); CreateMap<ScaleProduct, ScaleProductLiteModel>().ReverseMap();
            CreateMap<Product, RapidProductModel>().ReverseMap();
            CreateMap<ProductModel, RapidProductModel>().ReverseMap();     

        }
    }
}