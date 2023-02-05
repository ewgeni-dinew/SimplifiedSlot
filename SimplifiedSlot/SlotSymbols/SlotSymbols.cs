using SimplifiedSlot.Utils;

namespace SimplifiedSlot.SlotSymbols
{
    internal class AppleSlotSymbol : SlotSymbol
    {
        public AppleSlotSymbol()
            : base(Constants.APPLE_SYMBOL, Constants.APPLE_COEFFICIENT)
        { }
    }

    internal class PineappleSlotSymbol : SlotSymbol
    {
        public PineappleSlotSymbol()
            : base(Constants.PINEAPPLE_SYMBOL, Constants.PINEAPPLE_COEFFICIENT)
        { }
    }

    internal class BananaSlotSymbol : SlotSymbol
    {
        public BananaSlotSymbol()
            : base(Constants.BANANA_SYMBOL, Constants.BANANA_COEFFICIENT)
        { }
    }

    internal class WildcardSlotSymbol : SlotSymbol
    {
        public WildcardSlotSymbol()
            : base(Constants.WILDCARD_SYMBOL, Constants.WILDCARD_COEFFICIENT)
        { }
    }
}
