using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;

namespace SimplifiedSlot
{
    internal class SlotEngine
    {
        private const int CONSOLE_ROWS_COUNT = 4;
        private const int CONSOLE_COLUMNS_COUNT = 3;

        public SlotEngine() { }

        public void Run()
        {
            Console.WriteLine("Please deposit money you would like to play with:");
            var depositAmount = decimal.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("Enter stake amount:");
                var stakeAmount = int.Parse(Console.ReadLine());

                depositAmount -= stakeAmount;

                var spinResultCoefficient = GetSpinSlotCoefficient();

                var spinResult = CalculateSpinResult(stakeAmount, spinResultCoefficient);

                Console.WriteLine($"You have won: {spinResult}");

                depositAmount += (decimal)spinResult;

                Console.WriteLine($"Current balance is: {depositAmount}" + Environment.NewLine);
            }
        }

        private double GetSpinSlotCoefficient()
        {
            var random = new Random();
            var tasks = new List<Task<double>>(CONSOLE_ROWS_COUNT);

            for (int i = 0; i < CONSOLE_ROWS_COUNT; i++)
            {
                var spinRow = new List<SlotSymbol>();

                tasks.Add(Task.Run(() =>
                {
                    var spinRow = new List<SlotSymbol>();
                    for (int j = 0; j < CONSOLE_COLUMNS_COUNT; j++)
                    {
                        var randomNum = random.Next(0, 100); //value range [0, 99] 

                        switch (randomNum)
                        {
                            case < 45: spinRow.Add(new AppleSlotSymbol()); break;
                            case < 80: spinRow.Add(new BananaSlotSymbol()); break;
                            case < 95: spinRow.Add(new PineappleSlotSymbol()); break;
                            case < 100: spinRow.Add(new WildcardSlotSymbol()); break;
                        }
                    }
                    var temp = GetRowWinCoefficient(spinRow);
                    Console.WriteLine(string.Join("", spinRow.Select(s => s.Symbol)));
                    return temp;
                }));
            }

            return Task.WhenAll(tasks).Result.Sum(); //could be awaited
        }

        private double GetRowWinCoefficient(IEnumerable<SlotSymbol> spinRow)
        {
            var result = 0d;
            spinRow = spinRow.Where(s => s.Symbol != Constants.WILDCARD_SYMBOL);

            if (spinRow.DistinctBy(s => s.Symbol).Count() == 1)
            {
                result = spinRow.Count() * spinRow.DistinctBy(s => s.Symbol).First().Coefficient;
            }
            return result;
        }

        private double CalculateSpinResult(int stakeAmount, double spinResultCoef)
        {
            return stakeAmount * spinResultCoef;
        }
    }
}
