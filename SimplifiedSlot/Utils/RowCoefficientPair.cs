using SimplifiedSlot.SlotSymbols;

namespace SimplifiedSlot.Utils
{
    public class RowCoefficientPair
    {
        public IEnumerable<SlotSymbol> Row { get; }
        public decimal Coefficient { get; }

        public RowCoefficientPair(IEnumerable<SlotSymbol> row, decimal coefficient)
        {
            this.Row = row;
            this.Coefficient = coefficient;
        }
    }
}
