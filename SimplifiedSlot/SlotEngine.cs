using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;
using SimplifiedSlot.Utils.Contracts;

namespace SimplifiedSlot
{
    public class SlotEngine
    {
        private const int CONSOLE_ROWS_COUNT = 4;
        private const int CONSOLE_COLUMNS_COUNT = 3;
        private const string NUMBER_FORMAT = "f2";

        private decimal userBalance = 0;
        private readonly IConsole console;
        private readonly ISlotEngineHelper slotEngineHelper;

        public SlotEngine(decimal userBalance, IConsole console, ISlotEngineHelper slotEngineHelper)
        {
            this.userBalance = userBalance;
            this.console = console;
            this.slotEngineHelper = slotEngineHelper;
        }

        public void Run()
        {
            while (true)
            {
                var tempBalance = this.userBalance;
                try
                {
                    this.console.WriteLine("Enter stake amount:");

                    var stakeAmount = ConsoleHelpers.ReadAmountFromString(this.console);

                    if (stakeAmount > this.userBalance)
                        throw new ArgumentException(Errors.STAKE_GREATER_THAN_BALANCE);

                    this.userBalance -= stakeAmount;

                    var spinResultCoefficient = GetSpinSlotCoefficient();

                    var spinResult = this.slotEngineHelper.GetSpinWinAmount(stakeAmount, spinResultCoefficient);

                    this.console.WriteLine($"You have won: {spinResult.ToString(NUMBER_FORMAT)}");

                    this.userBalance += spinResult;

                    this.console.WriteLine($"Current balance is: {this.userBalance.ToString(NUMBER_FORMAT)}" + Environment.NewLine);
                }
                catch (ArgumentException ex)
                {
                    //Argument exceptions should be regarded as handled...
                    throw ex;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Errors.UNHANDLED_EXCEPTION);
                }
                finally
                {
                    if (this.userBalance != tempBalance)
                        this.userBalance = tempBalance;
                }
            }
        }

        public decimal GetSpinSlotCoefficient()
        {
            var random = new Random();
            var tasks = new List<Task<decimal>>(CONSOLE_ROWS_COUNT);

            for (int i = 0; i < CONSOLE_ROWS_COUNT; i++)
            {
                var spinRow = new List<SlotSymbol>();

                tasks.Add(Task.Run(() =>
                {
                    var spinRow = new List<SlotSymbol>();
                    for (int j = 0; j < CONSOLE_COLUMNS_COUNT; j++)
                    {
                        var randomNum = random.Next(0, 20); //value range [0, 19] 

                        switch (randomNum)
                        {
                            case < 9: spinRow.Add(new AppleSlotSymbol()); break; //45%
                            case < 16: spinRow.Add(new BananaSlotSymbol()); break; //35%
                            case < 18: spinRow.Add(new PineappleSlotSymbol()); break; //15%
                            case <= 19: spinRow.Add(new WildcardSlotSymbol()); break; //5%
                        }
                    }

                    this.console.WriteLine(string.Join("", spinRow.Select(s => s.Symbol)));

                    return this.slotEngineHelper.GetRowWinCoefficient(spinRow);
                }));
            }

            return Task.WhenAll(tasks).Result.Sum();
        }
    }
}
