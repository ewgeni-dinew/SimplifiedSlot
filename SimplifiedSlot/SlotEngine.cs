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
        private readonly ISlotCalculator calculator;

        public SlotEngine(decimal userBalance, IConsole console, ISlotCalculator calculator)
        {
            this.userBalance = userBalance;
            this.console = console;
            this.calculator = calculator;
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

                    var spinRowPairs = this.calculator.GetSpinTotalRows(CONSOLE_ROWS_COUNT, CONSOLE_COLUMNS_COUNT);

                    PrintSpinRows(spinRowPairs);

                    var spinTotalCoefficient = this.calculator.GetSpinTotalCoefficient(spinRowPairs);

                    var spinResult = this.calculator.GetSpinWinAmount(stakeAmount, spinTotalCoefficient);

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
                    if (this.userBalance != tempBalance)
                        this.userBalance = tempBalance;
                    throw new ArgumentException(Errors.UNHANDLED_EXCEPTION);
                }
            }
        }

        public decimal GetUserBalance()
        {
            return this.userBalance;
        }

        private void PrintSpinRows(IEnumerable<RowCoefficientPair> rowPairs)
        {
            foreach (var rowPair in rowPairs)
                this.console.WriteLine(string.Join("", rowPair.Row.Select(r => r.Symbol)));
        }
    }
}
