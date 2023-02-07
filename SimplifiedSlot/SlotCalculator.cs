using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;

namespace SimplifiedSlot
{
    public class SlotCalculator : ISlotCalculator
    {
        private readonly Random random = new();

        public IEnumerable<RowCoefficientPair> GetSpinTotalRows(int rows, int cols)
        {
            var tasks = new List<Task<RowCoefficientPair>>(rows);

            for (int i = 0; i < rows; i++)
            {
                var spinRow = new List<SlotSymbol>();

                tasks.Add(Task.Run(() =>
                {
                    var spinRow = new List<SlotSymbol>();
                    for (int j = 0; j < cols; j++)
                    {
                        var randomNum = this.random.Next(0, 20); //value range [0, 19] 

                        switch (randomNum)
                        {
                            case < 9: spinRow.Add(new AppleSlotSymbol()); break; //45%
                            case < 16: spinRow.Add(new BananaSlotSymbol()); break; //35%
                            case < 18: spinRow.Add(new PineappleSlotSymbol()); break; //15%
                            case <= 19: spinRow.Add(new WildcardSlotSymbol()); break; //5%
                        }
                    }

                    return new RowCoefficientPair(spinRow, this.GetRowWinCoefficient(spinRow));
                }));
            }

            return Task.WhenAll(tasks).Result;
        }

        public decimal GetRowWinCoefficient(IEnumerable<SlotSymbol> row)
        {
            var result = 0m;
            row = row.Where(s => s.Symbol != Constants.WILDCARD_SYMBOL);

            if (row.DistinctBy(s => s.Symbol).Count() == 1)
            {
                result = row.Count() * row.DistinctBy(s => s.Symbol).First().Coefficient;
            }
            return result;
        }

        public decimal GetSpinWinAmount(decimal stakeAmount, decimal spinResultCoef)
        {
            return stakeAmount * spinResultCoef;
        }

        public decimal GetSpinTotalCoefficient(IEnumerable<RowCoefficientPair> spinRows)
        {
            return spinRows.Select(r => r.Coefficient).Sum();
        }
    }

    public interface ISlotCalculator
    {
        IEnumerable<RowCoefficientPair> GetSpinTotalRows(int rows, int cols);
        decimal GetRowWinCoefficient(IEnumerable<SlotSymbol> row);
        decimal GetSpinWinAmount(decimal stakeAmount, decimal spinTotalCoef);
        decimal GetSpinTotalCoefficient(IEnumerable<RowCoefficientPair> spinRows);
    }
}
