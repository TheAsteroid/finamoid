using Finamoid.Abstractions.Import;
using IbanNet;
using System.Globalization;

namespace Finamoid.Import
{
    public class BankStatementFormatDetector : IBankStatementFormatDetector
    {
        private readonly IIbanParser _ibanParser;

        public BankStatementFormatDetector(IIbanParser ibanParser)
        {
            _ibanParser = ibanParser;
        }

        public async Task<BankStatementType> DetectAsync(string path)
        {
            var extension = Path.GetExtension(path);

            if (extension == ".csv")
            {
                await foreach (var line in File.ReadLinesAsync(path))
                {
                    if (line == null)
                    {
                        return BankStatementType.Undefined;
                    }

                    return
                        IsAsnCsv(line) ? BankStatementType.AsnCsv :
                        IsIngCsv(line) ? BankStatementType.IngCsv :
                        IsIngSsv(line) ? BankStatementType.IngSsv :
                        BankStatementType.Undefined;
                }
            }

            if (extension == ".xml")
            {
                return
                    IsCamt053() ? BankStatementType.Camt053 :
                    BankStatementType.Undefined;
            }

            return BankStatementType.Undefined;
        }

        private bool IsAsnCsv(string firstLine)
        {
            var columns = firstLine.Split(',');
            if (columns.Length < 2)
            {
                return false;
            }

            return
                DateTime.TryParseExact(columns[0], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) &&
                _ibanParser.TryParse(columns[1], out _);
        }

        private static bool IsIngCsv(string firstLine)
        {
            return
                firstLine == "\"Datum\",\"Naam / Omschrijving\",\"Rekening\",\"Tegenrekening\",\"Code\",\"Af Bij\",\"Bedrag (EUR)\",\"Mutatiesoort\",\"Mededelingen\"" ||
                firstLine == "\"Date\",\"Name / Description\",\"Account\",\"Counterparty\",\"Code\",\"Debit/credit\",\"Amount (EUR)\",\"Transaction type\",\"Notifications\"";
        }

        private static bool IsIngSsv(string firstLine)
        {
            return
                firstLine == "\"Datum\";\"Naam / Omschrijving\";\"Rekening\";\"Tegenrekening\";\"Code\";\"Af Bij\";\"Bedrag (EUR)\";\"Mutatiesoort\";\"Mededelingen\"" ||
                firstLine == "\"Date\";\"Name / Description\";\"Account\";\"Counterparty\";\"Code\";\"Debit/credit\";\"Amount (EUR)\";\"Transaction type\";\"Notifications\"";
        }

        private static bool IsCamt053()
        {
            // TODO https://github.com/TheAsteroid/finamoid/issues/3: Get xmlns from Document root without reading full XML.
            // For now assume all XMLs are CAMT053.
            return true;
        }
    }
}
