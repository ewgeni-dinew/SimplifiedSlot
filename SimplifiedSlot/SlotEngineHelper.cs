using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;

namespace SimplifiedSlot
{
    public class SlotEngineHelper : ISlotEngineHelper
    {
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
    }

    public interface ISlotEngineHelper
    {
        decimal GetRowWinCoefficient(IEnumerable<SlotSymbol> row);
        decimal GetSpinWinAmount(decimal stakeAmount, decimal spinResultCoef);
    }
}
