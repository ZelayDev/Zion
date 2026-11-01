namespace Zion
{
    public record struct MonthInfo(string Name, int DaysCount);

    public static class DateTimeExtension
    {
        private static readonly MonthInfo[] Months =
        [
            new("January", 31),
            new("February", 28),
            new("March", 31),
            new("April", 30),
            new("May", 31),
            new("June", 30),
            new("July", 31),
            new("August", 31),
            new("September", 30),
            new("October", 31),
            new("November", 30),
            new("December", 31)
        ];

        /// <summary>
        ///     Gets month information by index.
        /// </summary>
        /// <param name="Index">The month index (1-12).</param>
        /// <param name="LeapYear">Whether to account for leap year for February.</param>
        /// <returns>Month information including name and day count.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public static MonthInfo GetMonthInfo(int Index, bool LeapYear = false)
        {
            return !Index.IsBetween(1, 12)
                ? throw new ArgumentOutOfRangeException(
                    $"The month index must be between 1 and 12, InputIndex = {Index}")
                : LeapYear && Index == 1 && (DateTime.Now.Year & 3) == 0
                    ? Months[Index] with { DaysCount = 29 }
                    : Months[Index - 1];
        }

        /// <summary>
        ///     Gets month information from a DateTime object.
        /// </summary>
        /// <param name="Date">The DateTime object to examine.</param>
        /// <param name="ConsiderLeapYear">Whether to account for leap year for February.</param>
        /// <returns>Month information including name and day count.</returns>
        public static MonthInfo GetMonthInfo(this DateTime Date)
        {
            return GetMonthInfo(Date.Month, DateTime.IsLeapYear(Date.Year));
        }

        /// <summary>
        ///     Gets the current date as a DateOnly object.
        /// </summary>
        /// <returns>Current date in DateOnly format.</returns>
        public static DateOnly GetNow()
        {
            var Now = DateTime.Today;
            return new DateOnly(Now.Year, Now.Month, Now.Day);
        }
    }
}