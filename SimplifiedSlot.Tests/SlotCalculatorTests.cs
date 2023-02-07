using FluentAssertions;
using NUnit.Framework;
using SimplifiedSlot.SlotSymbols;
using SimplifiedSlot.Utils;

namespace SimplifiedSlot.Tests
{
    [TestFixture]
    public class SlotCalculatorTests
    {
        private ISlotCalculator slotCalculator;

        [SetUp]
        public void SetUp()
        {
            this.slotCalculator = new SlotCalculator();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1.5)]
        [TestCase(1.97943721)]
        public void GetSpinWinAmount_Returns_CorrectAmount(decimal coefficient)
        {
            var stake = 10;
            var expected = stake * coefficient;

            var result = this.slotCalculator.GetSpinWinAmount(stake, coefficient);
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

            var result = this.slotCalculator.GetRowWinCoefficient(row);
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

            var result = this.slotCalculator.GetRowWinCoefficient(row);
            result.Should().Be(0);
        }

        [TestCaseSource("_singleTypeOnly")]
        public void GetRowWinCoefficient_Returns_SymbolCoefficient_MultipliedBy_NumberOfSymbols_WhenOnlyOneTypePresent(IEnumerable<SlotSymbol> row)
        {
            var result = this.slotCalculator.GetRowWinCoefficient(row);
            var expected = row.Count() * GetCoefficientBySymbolType(row.First().GetType());
            result.Should().Be(expected);
        }

        [TestCaseSource("_applesAndWildcards")]
        public void GetRowWinCoefficient_Returns_SymbolCoefficient_MultipliedBy_NumberOfSymbols_Present_When_Only_SymbolsAndWildcards_Present(IEnumerable<SlotSymbol> row)
        {
            var result = this.slotCalculator.GetRowWinCoefficient(row);
            var expected = row.Where(s => s.Symbol == Constants.APPLE_SYMBOL).Count() * Constants.APPLE_COEFFICIENT;
            result.Should().Be(expected);
        }

        [TestCase(0, -1, 1)]
        [TestCase(1.5, 3.2, -1)]
        [TestCase(1.53123, 9.3213124, 0.12321)]
        public void GetSpinTotalCoefficient_Returns_SumOfAllIndividualRowCoefficients(decimal coef_1, decimal coef_2, decimal coef_3)
        {
            var rowPairs = new List<RowCoefficientPair>
            {
                new RowCoefficientPair(new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol()}, coef_1),
                new RowCoefficientPair(new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol()}, coef_2),
                new RowCoefficientPair(new List<SlotSymbol> { new AppleSlotSymbol(), new AppleSlotSymbol()}, coef_3),
            };

            var expected = coef_1 + coef_2 + coef_3;
            var result = this.slotCalculator.GetSpinTotalCoefficient(rowPairs);

            result.Should().Be(expected, "Regardless of correct/incorrect combination. The method's responsible only to sum the provided coefficients.");
        }

        [TestCase(1, 0)]
        [TestCase(2, 2)]
        [TestCase(4, 55)]
        [TestCase(3, -5)]
        public void GetSpinTotalRows_ReturnsSize_DependingOn_Provided_RowsAndColumns(int rows, int cols)
        {
            var result = this.slotCalculator.GetSpinTotalRows(rows, cols);

            result.Should().HaveCount(rows, "Each row is a separate element inside the result collection");
            result.Should().AllSatisfy(r => r.Row.Count().Equals(cols));
        }

        [TestCase(10)]
        [TestCase(10000)] //bigger matrix for more results (~15sec)
        public void GetSpinTotalRows_ReturnsCoefficients_EqualTo_GetRowWinCoefficient(int size)
        {
            var result = this.slotCalculator.GetSpinTotalRows(size, size);

            result.Should().AllSatisfy(r => r.Coefficient.Equals(this.slotCalculator.GetRowWinCoefficient(r.Row)));
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