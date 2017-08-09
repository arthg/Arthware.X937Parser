namespace Arthware.X937Parser.Models
{
    public enum X937Record
    {
        CashLetterHeader10 = 10,
        Return31 = 31,
        ReturnAddendumB33 = 33,
        BundleControl70 = 70,
        CashLetterControl90 = 90,
        FileControl99 = 99
    }

    public enum CashLetterHeaderRecord10Field
    {
        CollectionTypeIndicator = 2
    }

    public static class CollectionTypeIndicators
    {
        public const string FORWARD_PRESENTMENT = "01";
        public const string RETURN = "03";
    }

    public enum ReturnRecord31Field
    {
        PayorBankRoutingNumber = 2,
        PayorBankRoutingNumberCheckDigit = 3,
        OnUsReturnRecord = 4,
        ItemAmount = 5,
        ReturnReason = 6,
        ExternalProcessingCode = 11
    }

    public enum ReturnRecord33Field
    {
        AuxiliaryOnUs = 3
    }

    public enum Record90Field
    {
        BundleCount = 2
    }

    public enum Record99Field
    {
        CashLetterCount = 2
    }

    public enum Record70Field
    {
        ItemsWithinBundleCount = 2
    }
}