namespace SimplifiedSlot.Utils
{
    public static class Constants
    {
        public const char APPLE_SYMBOL = 'A';
        public const char BANANA_SYMBOL = 'B';
        public const char PINEAPPLE_SYMBOL = 'P';
        public const char WILDCARD_SYMBOL = '*';

        public const decimal APPLE_COEFFICIENT = 0.4m;
        public const decimal BANANA_COEFFICIENT = 0.6m;
        public const decimal PINEAPPLE_COEFFICIENT = 0.8m;
        public const decimal WILDCARD_COEFFICIENT = 0;
    }

    public static class Errors
    {
        public const string STAKE_GREATER_THAN_BALANCE = "Stake amount cannot exceed balance!";
        public const string UNHANDLED_EXCEPTION = "Oops something went wrong...";
    }
}
