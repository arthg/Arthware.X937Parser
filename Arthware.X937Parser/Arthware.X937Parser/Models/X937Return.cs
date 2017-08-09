namespace Arthware.X937Parser.Models
{
    public sealed class X937Return
    {
        /*
          Aba = PayorBankRoutingNumber + PayorBankRoutingNumberCheckDigit
          OnUsReturnRecord:
            The Payor’s account number as well as a check number or transaction code if present, separated by ‘/’
            {Account Number}/{Trans Code or Check Number}
        */
        public string PayorBankRoutingNumber { get; set; }
        public string PayorBankRoutingNumberCheckDigit { get; set; }
        public string OnUsReturnRecord { get; set; }
        public string ItemAmount { get; set; }
        public string ReturnReason { get; set; }
        public string ExternalProcessingCode { get; set; }
        public string AuxiliaryOnUs { get; set; }
    }
}