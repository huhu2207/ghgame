using System;

namespace MinGH.Extensions
{
	/// <remarks>
	/// A simple extenstion that returns a sub array given a start and end index.
	/// This extention works on all array types.
	/// </remarks>
    public static class SubArrayExtension
    {
		/// <summary>
		/// Creates and returns a new sub array using an input array, a start index, 
		/// and an end index.
		/// </summary>
		/// <returns>
		/// An array of type T that contains only the specified section.
		/// </returns>
        public static T[] SubArray<T>(this T[] data, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;
            T[] result = new T[length];
            Array.Copy(data, startIndex, result, 0, length);
            return result;
        }
    }
}
