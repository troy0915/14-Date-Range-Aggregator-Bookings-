using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Booking
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public Booking(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date must be on or after start date.");

        Start = start.Date;
        End = end.Date;
    }

    public IEnumerable<DateTime> GetDays()
    {
        for (var day = Start; day <= End; day = day.AddDays(1))
        {
            yield return day;
        }
    }
}


namespace _14__Date_Range_Aggregator__Bookings_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int year = 2025, month = 8; 
            var bookings = new List<Booking>
        {
            new Booking(new DateTime(year, month, 1), new DateTime(year, month, 3)),
            new Booking(new DateTime(year, month, 2), new DateTime(year, month, 5)), 
            new Booking(new DateTime(year, month, 10), new DateTime(year, month, 12))
        };

            int daysInMonth = DateTime.DaysInMonth(year, month);
            int[] dailyCounts = new int[daysInMonth + 1]; 

            foreach (var booking in bookings)
            {
                foreach (var day in booking.GetDays())
                {
                    if (day.Month == month && day.Year == year)
                    {
                        dailyCounts[day.Day]++;
                    }
                }
            }

            Console.WriteLine("=== Daily Booking Counts (Including Overlaps) ===");
            for (int day = 1; day <= daysInMonth; day++)
            {
                Console.WriteLine($"{month}/{day}: {dailyCounts[day]} booking(s)");
            }

            var mergedRanges = MergeBookings(bookings);
            Console.WriteLine("\n=== Merged Booking Ranges (No Double-Count) ===");
            foreach (var range in mergedRanges)
            {
                Console.WriteLine($"{range.Start:yyyy-MM-dd} to {range.End:yyyy-MM-dd}");
            }
        }

        public static List<Booking> MergeBookings(List<Booking> bookings)
        {
            if (!bookings.Any()) return new List<Booking>();

            var sorted = bookings.OrderBy(b => b.Start).ToList();
            var merged = new List<Booking> { sorted[0] };

            foreach (var current in sorted.Skip(1))
            {
                var last = merged.Last();
                if (current.Start <= last.End.AddDays(1)) 
                {
                    var newEnd = current.End > last.End ? current.End : last.End;
                    merged[merged.Count - 1] = new Booking(last.Start, newEnd);
                }
                else
                {
                    merged.Add(current);
                }
            }

            return merged;
        }
    }
}





