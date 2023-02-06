using SimplifiedSlot.Utils.Contracts;
using System.Globalization;

namespace SimplifiedSlot.Utils
{
    public static class ConsoleHelpers
    {
        /// <summary>
        /// Takes the dot separator as the valid decimal separator. For more precise use-case scenarios, other options should be considered.
        /// </summary>
        /// <returns></returns>
        public static decimal ReadAmountFromString(IConsole console)
        {
            decimal amount;
            while (true)
            {
                var input = console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    console.WriteLine("Please enter a valid amount:");
                else if (!decimal.TryParse(input.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out amount))
                    console.WriteLine("Please enter a valid amount:");
                else if (amount < 1m)
                    console.WriteLine("Please enter an amount bigger than 1:");
                else
                    break;
            }
            return amount;
        }
    }
}
