using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.Extensions
{
    public static class SubArrayExtension
    {
        public static T[] SubArray<T>(this T[] data, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;
            T[] result = new T[length];
            Array.Copy(data, startIndex, result, 0, length);
            return result;
        }
    }
}
