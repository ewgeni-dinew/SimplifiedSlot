namespace SimplifiedSlot
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Please deposit money you would like to play with:");
            var depositAmount = decimal.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("Enter stake amount:");
                var stakeAmount = int.Parse(Console.ReadLine());

                depositAmount -= stakeAmount;

                var spinResultCoefficient = GetSpinSlotCoefficient();

                var spinResult = CalculateSpinResult(stakeAmount, spinResultCoefficient);

                Console.WriteLine($"You have won: {spinResult}");

                depositAmount += spinResult;

                Console.WriteLine($"Current balance is: {depositAmount}" + Environment.NewLine);
            }
        }

        private static double GetSpinSlotCoefficient()
        {
            var random = new Random();
            var spinResultCoef = 0d;

            for (int i = 0; i < 4; i++)
            {
                var spinRow = new List<char>();

                for (int j = 0; j < 3; j++)
                {
                    var randomNum = random.Next(0, 100);
                    if (randomNum <= 45)
                    {
                        spinRow.Add('A');
                    }
                    else if (randomNum <= 80)
                    {
                        spinRow.Add('B');
                    }
                    else if (randomNum <= 95)
                    {
                        spinRow.Add('P');
                    }
                    else
                    {
                        spinRow.Add('*');
                    }
                }

                //check for win
                if (spinRow.Where(x => x != '*').Distinct().Count() == 1)
                {
                    Console.WriteLine($"{string.Join("", spinRow)} WIN WIN WIN");
                    spinResultCoef += spinRow.Where(x => x != '*').Count() * GetValueFromKey(spinRow.Where(x => x != '*').Distinct().First());
                }
                else
                {
                    Console.WriteLine(string.Join("", spinRow));
                }
            }

            return spinResultCoef;
        }

        private static double GetValueFromKey(char enumerable)
        {
            return enumerable switch
            {
                'A' => 0.4,
                'B' => 0.6,
                'P' => 0.8,
                _ => 0,
            };
        }

        private static decimal CalculateSpinResult(int stakeAmount, double spinResultCoef)
        {
            return ((decimal)(stakeAmount * spinResultCoef));
        }
    }
}