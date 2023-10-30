using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Greta.BO.BusinessLogic.TypeConverters
{
    public class BooleanConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "Yes" || text == "Y" || text == "1" || text == "y" || text == "true" || text == "TRUE") return true;
            return false;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return null;
        }
    }
}