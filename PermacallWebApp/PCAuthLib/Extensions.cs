using System;

namespace PCAuthLib
{
    public static class Extensions
    {
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (InvalidCastException)
            {
                return -1;
            }

        }

        public static double ToDouble(this object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch (InvalidCastException)
            {
                return -1;
            }

        }

        public static uint ToUInt(this object obj)
        {
            try
            {
                return Convert.ToUInt32(obj);
            }
            catch (InvalidCastException)
            { return 0; }
            catch (FormatException)
            { return 0; }

        }
    }
}