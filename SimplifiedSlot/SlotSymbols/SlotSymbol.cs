using SimplifiedSlot.Utils;

namespace SimplifiedSlot.SlotSymbols
{
    public abstract class SlotSymbol
    {
        public decimal Coefficient { get; }
        public char Symbol { get; }

        public SlotSymbol(char symbol, decimal coefficient)
        {
            Coefficient = coefficient;
            Symbol = symbol;
        }
    }

    public class AppleSlotSymbol : SlotSymbol
    {
        public AppleSlotSymbol()
            : base(Constants.APPLE_SYMBOL, Constants.APPLE_COEFFICIENT)
        { }
    }

    public class PineappleSlotSymbol : SlotSymbol
    {
        public PineappleSlotSymbol()
            : base(Constants.PINEAPPLE_SYMBOL, Constants.PINEAPPLE_COEFFICIENT)
        { }
    }

    public class BananaSlotSymbol : SlotSymbol
    {
        public BananaSlotSymbol()
            : base(Constants.BANANA_SYMBOL, Constants.BANANA_COEFFICIENT)
        { }
    }

    public class WildcardSlotSymbol : SlotSymbol
    {
        public WildcardSlotSymbol()
            : base(Constants.WILDCARD_SYMBOL, Constants.WILDCARD_COEFFICIENT)
        { }
    }
}
