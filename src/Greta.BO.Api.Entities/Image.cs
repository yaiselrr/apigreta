using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class Image : BaseEntityLong
    {
        public string Path { get; set; }
        public bool IsPrimary { get; set; }
        public long OwnerId { get; set; }
        public ImageType Type { get; set; }

        public static string getStorageFolder(ImageType Type)
        {
            switch (Type)
            {
                default: return "image";
                case ImageType.PRODUCT: return "productimage";
                case ImageType.CATEGORY: return "categoryimage";
                case ImageType.SCALECATEGORY: return "scategoryimage";
                case ImageType.DEPARTMENT: return "departmentimage";
                case ImageType.VENDORCONTACT: return "vendorcontactimage";
            }
        }
    }
}