using SimplifiedSlot.Utils.Contracts;

namespace SimplifiedSlot.Tests.Utils
{
    internal class TestConsole : IConsole
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
