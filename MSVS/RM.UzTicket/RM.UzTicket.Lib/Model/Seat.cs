
namespace RM.UzTicket.Lib.Model
{
	public class Seat
	{
		public string CharLine { get; private set; }

		public int Number { get; private set; }

		public decimal? Price { get; private set; }

		public static Seat Create(string charline, int number, decimal? price = null)
		{
			return new Seat { CharLine = charline, Number = number, Price = price };
		}
	}
}
