using System;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;

namespace Greta.BO.BusinessLogic.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsValid(this string val, string msg, ref HandlerMessage handler)
        {
            if(string.IsNullOrWhiteSpace(val))
            {
                handler.Message = msg;
                return false;
            }
            return true;
        }

        public static bool IsValid(this long val, string msg, ref HandlerMessage handler)
        {
            if(val == 0)
            {
                handler.Message = msg;
                return false;
            }
            return true;
        }

        public static bool IsValid(this int val, string msg, ref HandlerMessage handler)
        {
            if(val == 0)
            {
                handler.Message = msg;
                return false;
            }
            return true;
        }
    }
}
