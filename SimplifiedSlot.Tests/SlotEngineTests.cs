using FluentAssertions;
using Moq;
using NUnit.Framework;
using SimplifiedSlot.Tests.Utils;
using SimplifiedSlot.Utils;
using SimplifiedSlot.Utils.Contracts;

namespace SimplifiedSlot.Tests
{
    [TestFixture]
    public class SlotEngineTests
    {
        [Test]
        public void Engine_Run_ShouldThrow_When_Stake_GreaterThanBalance()
        {
            var deposit = 100;
            var stake = 1000;
            var console = new TestConsole();
            console.AddCommand(stake.ToString()); //first command
            var engine = new SlotEngine(deposit, console, new SlotCalculator());

            var action = () => engine.Run();
            action.Should().Throw<ArgumentException>().WithMessage(Errors.STAKE_GREATER_THAN_BALANCE);
        }

        [Test]
        public void Engine_Run_ShouldNot_ReturnErrorMessages_WhenUnhanledError_IsThrown()
        {
            var internalErrorMessage = "secret message for internal error";
            var console = new TestConsole();
            console.AddCommand("10"); //first command

            var slotCalculatorMock = new Mock<ISlotCalculator>();
            slotCalculatorMock.Setup(x => x.GetSpinTotalCoefficient(It.IsAny<IEnumerable<RowCoefficientPair>>())).Throws(new Exception(internalErrorMessage));

            var engine = new SlotEngine(100, console, slotCalculatorMock.Object);

            var action = () => engine.Run();

            action.Should().Throw<Exception>().Which.Message.Should().NotContain(internalErrorMessage);
        }

        [Test]
        public void Engine_Run_ShouldNot_ModifyUserBalance_WhenUnhandledError_Thrown()
        {
            var deposit = 200;
            var console = new TestConsole();
            console.AddCommand("10"); //first command

            var slotCalculatorMock = new Mock<ISlotCalculator>();
            slotCalculatorMock.Setup(x => x.GetSpinTotalCoefficient(It.IsAny<IEnumerable<RowCoefficientPair>>())).Throws(new Exception("test"));

            var engine = new SlotEngine(deposit, console, slotCalculatorMock.Object);

            var action = () => engine.Run();

            action.Should().Throw<Exception>();
            engine.GetUserBalance().Should().Be(deposit);
        }
    }
}
