using SimplifiedSlot.Utils.Contracts;

namespace SimplifiedSlot.Utils
{
    public class ConsoleWrapper : IConsole
    {
        public void WriteLine(string message) => Console.WriteLine(message);

        public string ReadLine() => Console.ReadLine();
    }
}
