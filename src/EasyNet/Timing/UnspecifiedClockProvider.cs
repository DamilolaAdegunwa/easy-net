using System;

namespace EasyNet.Timing
{
    public class UnspecifiedClockProvider : IClockProvider
    {
	    internal UnspecifiedClockProvider()
	    {
	    }

        public DateTime Now => DateTime.Now;

        public DateTimeKind Kind => DateTimeKind.Unspecified;

        public bool SupportsMultipleTimezone => false;

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime;
        }
    }
}
