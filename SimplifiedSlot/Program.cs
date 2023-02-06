using SimplifiedSlot.Utils;

namespace SimplifiedSlot
{
    internal class Program
    {
        static void Main()
        {
            var console = new ConsoleWrapper();
            console.WriteLine("Please deposit money you would like to play with:");
            var depositAmount = ConsoleHelpers.ReadAmountFromString(console);

            var engine = new SlotEngine(depositAmount, console, new SlotEngineHelper());
            engine.Run();
        }
    }
}