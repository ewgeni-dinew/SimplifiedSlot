using FluentAssertions;
using NUnit.Framework;
using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;

namespace SimplifiedSlot.Tests
{
    [TestFixture]
    public class SlotEngineHelpersTests
    {
        private ISlotEngineHelper slotEngineHelper;

        [SetUp]
        public void SetUp()
        {
            this.slotEngineHelper = new SlotEngineHelper();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1.5)]
        [TestCase(1.97943721)]
        public void GetSpinWinAmount_Returns_CorrectAmount(decimal coefficient)
        {
            var stake = 10;
            var expected = stake * coefficient;

            var result = this.slotEngineHelper.GetSpinWinAmount(stake, coefficient);
            result.Should().Be(expected);
        }

        [Test]
        public void GetRowWinCoefficient_Returns_0_When_AllSymbols_AreWildcards()
        {
            var row = new List<SlotSymbol>
            {
                new WildcardSlotSymbol(),
                new WildcardSlotSymbol(),
                new WildcardSlotSymbol(),
            };

            var result = this.slotEngineHelper.GetRowWinCoefficient(row);
            result.Should().Be(0);
        }

        [Test]
        public void GetRowWinCoefficient_Returns_0_When_MoreThanOneSymbolType_IsPresent()
        {
            var row = new List<SlotSymbol>
            {
                new WildcardSlotSymbol(),
                new AppleSlotSymbol(),
                new BananaSlotSymbol(),
                new BananaSlotSymbol(),
            };

            var result = this.slotEngineHelper.GetRowWinCoefficient(row);
            result.Should().Be(0);
        }

        [TestCaseSource("_singleTypeOnly")]
        public void GetRowWinCoefficient_Returns_SymbolCoefficient_MultipliedBy_NumberOfSymbols_WhenOnlyOneTypePresent(IEnumerable<SlotSymbol> row)
        {
            var result = this.slotEngineHelper.GetRowWinCoefficient(row);
            var expected = row.Count() * GetCoefficientBySymbolType(row.First().GetType());
            result.Should().Be(expected);
        }

        [TestCaseSource("_applesAndWildcards")]
        public void GetRowWinCoefficient_Returns_SymbolCoefficient_MultipliedBy_NumberOfSymbols_Present_When_Only_SymbolsAndWildcards_Present(IEnumerable<SlotSymbol> row)
        {
            var result = this.slotEngineHelper.GetRowWinCoefficient(row);
            var expected = row.Where(s => s.Symbol == Constants.APPLE_SYMBOL).Count() * Constants.APPLE_COEFFICIENT;
            result.Should().Be(expected);
        }

        private decimal GetCoefficientBySymbolType(Type type)
        {
            if (type == typeof(AppleSlotSymbol))
                return Constants.APPLE_COEFFICIENT;
            else if (type == typeof(BananaSlotSymbol))
                return Constants.BANANA_COEFFICIENT;
            else if (type == typeof(PineappleSlotSymbol))
                return Constants.PINEAPPLE_COEFFICIENT;
            else
                return 0;
        }

        private static readonly object[] _applesAndWildcards =
        {
            new object[] { new List<SlotSymbol> { new AppleSlotSymbol(), new WildcardSlotSymbol(), new WildcardSlotSymbol(), new WildcardSlotSymbol() } },
            new object[] { new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol(), new WildcardSlotSymbol(), new WildcardSlotSymbol() } },
            new object[] { new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol(), new AppleSlotSymbol(), new WildcardSlotSymbol() } },
        };

        private static readonly object[] _singleTypeOnly =
        {
            new object[] { new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol(), new AppleSlotSymbol(), new AppleSlotSymbol() } },
            new object[] { new List<SlotSymbol> { new BananaSlotSymbol(), new BananaSlotSymbol(), new BananaSlotSymbol(), new BananaSlotSymbol() } },
        };
    }
}