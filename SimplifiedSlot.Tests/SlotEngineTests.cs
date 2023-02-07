using FluentAssertions;
using NUnit.Framework;
using SimplifiedSlot.Utils;
using SimplifiedSlot.Utils.Contracts;

namespace SimplifiedSlot.Tests
{
    [TestFixture]
    public class SlotEngineTests
    {
        [Test]
        public void Engine_ShouldThrow_When_Stake_GreaterThanBalance()
        {
            var deposit = 100;
            var stake = 1000;
            var console = new TestConsole();
            console.AddCommand(stake.ToString()); //first command
            var engine = new SlotEngine(deposit, console, new SlotCalculator());

            var action = () => engine.Run();
            action.Should().Throw<ArgumentException>().WithMessage(Errors.STAKE_GREATER_THAN_BALANCE);
        }

        //test for invalid input
    }

    class TestConsole : IConsole
    {
        private readonly Queue<string> _commands = new();

        public void AddCommand(string command)
        {
            this._commands.Enqueue(command);
        }

        public string ReadLine()
        {
            return this._commands.Dequeue();
        }

        public void WriteLine(string message) { }
    }
}
