using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.Shared
{
    public static class Utils
    {
        public static int ConvertListEnumToIntWithBinary<T>(List<T> listEnum)
        {
            if (!typeof(T).IsEnum) throw new Exception("ConvertListEnumToIntWithBinary: T is not enum");

            var result = 0;
            foreach (var l in listEnum) {
                result |= 1 << Convert.ToInt32(l);
            }
            return result;
        }

        public static List<T> ConvertIntToListEnumWithBinary<T>(int intListEnum)
        {
            if (!typeof(T).IsEnum) throw new Exception("ConvertIntToListEnumWithBinary: T is not enum");

            var result = new List<T>();
            foreach (var val in Enum.GetValues(typeof(T))) {
                if (HasBit(intListEnum, (int)val)) result.Add((T)val);
            }
            return result;
        }

        private static bool HasBit(int value, int bitNumber)
        {
            return (value & (1 << bitNumber)) != 0;
        }
    }
}
