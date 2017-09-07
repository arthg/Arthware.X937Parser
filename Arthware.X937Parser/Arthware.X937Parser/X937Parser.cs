using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Arthware.X937Parser.Models;
using Thinktecture.IO;
using System.Text;
using Thinktecture.IO.Adapters;
using CFS.SkyNet.Common;

namespace Arthware.X937Parser
{
    public sealed class X937Parser : IX937FileParser
    {
        private static int EBCDIC_ENCODING = 37;

        public IEnumerable<X937Return> GetX937Returns(Stream stream)
        {
            var returns = new List<X937Return>();            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (IBinaryReader binaryReader = new BinaryReaderAdapter(stream, Encoding.GetEncoding(EBCDIC_ENCODING)))
            {
                var recordLength = GetRecordLength(binaryReader);
                while (recordLength > 0)
                {
                    var recordTypeBytes = GetRecord(binaryReader, 2);
                    if (recordTypeBytes == X937RecordType.CashLetterHeader10)
                    {
                        var record = recordTypeBytes + GetRecord(binaryReader, recordLength - 2);
                        var collectionTypeIndicator = GetCollectionTypeIndicatorFromCashLetterHeader10(record);
                        if (collectionTypeIndicator == X937CollectionType.ForwardPresentment)
                        {
                            break; // Ok, yes that's crap
                        }
                    }
                    else if (recordTypeBytes == X937RecordType.ReturnRecord31)
                    {
                        var record = recordTypeBytes + GetRecord(binaryReader, recordLength - 2);
                        returns.Add(GetReturnFromReturnRecord31(record));
                    }
                    else if (recordTypeBytes == X937RecordType.ReturnAddendumB33)
                    {
                        // assume that a record 33 is always after a record 31
                        var mostRecentReturn = returns.LastOrDefault();
                        if (mostRecentReturn != null)
                        {
                            var record = recordTypeBytes + GetRecord(binaryReader, recordLength - 2);
                            mostRecentReturn.AuxiliaryOnUs = GetAuxiliaryOnUsFromReturnAddendumB33(record);
                        }
                    }
                    else
                    {
                        binaryReader.BaseStream.Seek(recordLength - 2, SeekOrigin.Current);
                    }

                    recordLength = GetRecordLength(binaryReader);
                }
            }
            return returns;
        }

        private sealed class X937RecordType : StringEnumBase
        {
            private X937RecordType(string recordType)
                : base(recordType)
            {
            }
            public static readonly X937RecordType CashLetterHeader10 = new X937RecordType("10");
            public static readonly X937RecordType ReturnRecord31 = new X937RecordType("31");
            public static readonly X937RecordType ReturnAddendumB33 = new X937RecordType("33");
        }

        private sealed class X937CollectionType : StringEnumBase
        {
            private X937CollectionType(string recordType)
                : base(recordType)
            {
            }
            public static readonly X937CollectionType ForwardPresentment = new X937CollectionType("01");
            public static readonly X937CollectionType Return = new X937CollectionType("03");
        }

        private static string GetAuxiliaryOnUsFromReturnAddendumB33(string record)
        {
            return record.Substring(20, 15);
        }

        private static string GetCollectionTypeIndicatorFromCashLetterHeader10(string record)
        {
            return record.Substring(2, 2);
        }

        private static X937Return GetReturnFromReturnRecord31(string record)
        {
            return new X937Return
            {
                OnUsReturnRecord = record.Substring(11, 20),
                ExternalProcessingCode = record.Substring(68, 1),
                ItemAmount = record.Substring(31, 10),
                PayorBankRoutingNumber = record.Substring(2, 8),
                PayorBankRoutingNumberCheckDigit = record.Substring(10, 1),
                ReturnReason = record.Substring(41, 1)
            };
        }

        private static string GetRecord(IBinaryReader input, int recordLength)
        {
            var recordBinary = input.ReadBytes(recordLength);
            return Encoding.ASCII.GetString(ConvertEbcdicToAscii(recordBinary));
        }

        private static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        {
            return Encoding.Convert(Encoding.GetEncoding(EBCDIC_ENCODING), Encoding.ASCII, ebcdicData);
        }

        private static int GetRecordLength(IBinaryReader input)
        {
            const int RECORD_LENGTH_BYTES = 4;
            var recordLength = input.ReadBytes(RECORD_LENGTH_BYTES);
            if (recordLength.Length == RECORD_LENGTH_BYTES)
            {
                Array.Reverse(recordLength);
                return BitConverter.ToInt32(recordLength, 0);
            }
            return 0;
        }
    }
}
