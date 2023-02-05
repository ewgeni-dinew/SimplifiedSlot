namespace SimplifiedSlot.SlotSymbols
{
    internal abstract class SlotSymbol
    {
        public double Coefficient { get; }
        public char Symbol { get; }

        public SlotSymbol(char symbol, double coefficient)
        {
            Coefficient = coefficient;
            Symbol = symbol;
        }
    }
}
