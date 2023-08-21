using CsvHelper;
using CsvHelper.Configuration;
using Finamoid;
using System.Globalization;

namespace Finamoid.Import.Readers
{
    /// <summary>
    /// Reader for the ASN Bank CSV format
    /// Format specification: https://www.asnbank.nl/web/file?uuid=fc28db9c-d91e-4a2c-bd3a-30cffb057e8b&owner=6916ad14-918d-4ea8-80ac-f71f0ff1928e&contentid=852
    /// </summary>
    internal class AsnCsvMutationReader : RawMutationReader
    {
        private static readonly Dictionary<string, Currency> _currencyMap = new()
        {
            { "EUR", Currency.Euro }
        };

        private static readonly Dictionary<string, MutationType> _mutationTypeMap = new()
        {
            { "ACC", MutationType.AcceptGiro },         // ACCEPTGIRO
            { "AFB", MutationType.Cost },               // KOSTEN
            { "AGI", MutationType.AcceptGiro },         // ACCEPTGIRO
            { "AGM", MutationType.AcceptGiro },         // ACCEPTGIRO
            { "BEA", MutationType.PayTerminal },        // PINNEN
            { "BIC", MutationType.Collection },         // INCASSO
            { "BIJ", MutationType.Interest },           // RENTE
            { "BTH", MutationType.BankTransfer },       // OVERBOEKING
            { "BTL", MutationType.BankTransfer },       // OVERBOEKING
            { "BVZ", MutationType.PaymentRequest },     // BETAALVERZOEK
            { "COR", MutationType.Chargeback },         // TERUGBOEKING
            { "EFF", MutationType.Investment },         // BELEGGEN
            { "EIC", MutationType.Collection },         // INCASSO
            { "GEA", MutationType.Atm },                // OPNAME VIA GELDAUTOMAAT
            { "GEW", MutationType.Message },            // BERICHT
            { "IDE", MutationType.Ideal },              // IDEAL
            { "IDM", MutationType.Ideal },              // IDEAL
            { "IIC", MutationType.AutomaticInternal },  // AUTOMATISCHE INTERNE AFSCHRIJVING
            { "INC", MutationType.Collection },         // INCASSO
            { "IOI", MutationType.BankTransfer },       // OVERBOEKING
            { "IOK", MutationType.BankTransfer },       // OVERBOEKING
            { "IOM", MutationType.BankTransfer },       // OVERBOEKING
            { "IOS", MutationType.BankTransfer },       // OVERBOEKING
            { "IPO", MutationType.PeriodicTransfer },   // PERIODIEKE OVERBOEKING
            { "ITP", MutationType.BankTransfer },       // OVERBOEKING
            { "KAS", MutationType.Atm },                // OPNAME
            { "KST", MutationType.Cost },               // KOSTEN
            { "LIC", MutationType.Collection },         // INCASSO
            { "MSC", MutationType.Other },              // OVERIGE
            { "NGI", MutationType.BankTransfer },       // OVERBOEKING
            { "NGM", MutationType.BankTransfer },       // OVERBOEKING
            { "NUL", MutationType.Message },            // BERICHT
            { "OPH", MutationType.AccountCanceled },    // OPHEFFEN REKENING
            { "OVB", MutationType.BankTransfer },       // OVERBOEKING
            { "OVS", MutationType.BankTransfer },       // OVERBOEKING
            { "PCQ", MutationType.BankTransfer },       // OVERBOEKING
            { "POV", MutationType.PeriodicTransfer },   // PERIODIEKE OVERBOEKING
            { "REP", MutationType.Chargeback },         // TERUGBOEKING
            { "RET", MutationType.Chargeback },         // TERUGBOEKING
            { "RNT", MutationType.Interest },           // RENTE
            { "RPI", MutationType.PayTerminal },        // PINNEN
            { "RTI", MutationType.Chargeback },         // TERUGBOEKING
            { "SSR", MutationType.PeriodicTransfer },   // PERIODIEKE OVERBOEKING
            { "SSV", MutationType.PeriodicTransfer },   // PERIODIEKE OVERBOEKING
            { "STO", MutationType.Chargeback },         // TERUGBOEKING
            { "TPP", MutationType.BankTransfer }        // OVERBOEKING
        };

        private readonly CsvConfiguration _configuration = new(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = false
        };

        private const string _dateTimeFormat = "dd-MM-yyyy";

        public override async Task<IEnumerable<Mutation>> ReadAsync(string path)
        {
            var result = new List<Mutation>();
            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, _configuration);

            // Layout:
            //  0 = booking date in dd-MM-yyyy
            //  1 = own account number
            //  2 = balancing account number (optional)
            //  3 = balancing account name
            //  4 = address (unused)
            //  5 = postcode (unused)
            //  6 = city (unused)
            //  7 = account currency
            //  8 = balance before mutation
            //  9 = mutation currency
            // 10 = mutation amount (negative for out, positive for in)
            // 11 = journal date
            // 12 = currency date
            // 13 = internal transaction code
            // 14 = global transcation code (maps to MutationType)
            // 15 = transcation index number (unique together with journal date)
            // 16 = payment reference
            // 17 = description
            // 18 = transcript number
            var row = 0;
            while (await csvReader.ReadAsync())
            {
                row++;
                if (csvReader.Parser.Count == 0)
                {
                    continue;
                }

                csvReader.TryGetField(0, out string? dateTimeString);
                csvReader.TryGetField(2, out string? balancingAccountNumber);
                csvReader.TryGetField(3, out string? balancingAccountName);
                csvReader.TryGetField(9, out string? mutationCurrencyString);
                csvReader.TryGetField(10, out string? mutationAmountString);
                csvReader.TryGetField(11, out string? journalDate);
                csvReader.TryGetField(14, out string? globalTransactionCode);
                csvReader.TryGetField(15, out string? globalTransactionIndex);
                csvReader.TryGetField(16, out string? paymentReference);
                csvReader.TryGetField(17, out string? description);

                // TODO https://github.com/TheAsteroid/finamoid/issues/4: Set row as context, add to logging instead.
                var context = $"File: {path}. Row: {row}. ";
                if (!DateTime.TryParseExact(dateTimeString, _dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
                {
                    throw new InvalidDataException($"{context}Could not parse date '{dateTimeString}' at index 0 with format {_dateTimeFormat}.");
                }

                dateTime = dateTime.ToUniversalTime();

                if (mutationCurrencyString == null || !_currencyMap.TryGetValue(mutationCurrencyString, out var mutationCurrency))
                {
                    throw new InvalidDataException($"{context}Could not map mutation currency '{mutationCurrencyString}' at index 9.");
                }

                if (!decimal.TryParse(mutationAmountString, out var mutationAmount))
                {
                    throw new InvalidDataException($"{context}Could not parse mutation amount at index 10 to decimal.");
                }

                if (journalDate == null)
                {
                    throw new InvalidDataException($"{context}Could not determine journal date at index 11.");
                }

                if (globalTransactionCode == null || !_mutationTypeMap.TryGetValue(globalTransactionCode, out var mutationType))
                {
                    throw new InvalidDataException($"{context}Could not determine mutation type based on on global transaction code '{globalTransactionCode}' at index 14.");
                }

                if (globalTransactionIndex == null)
                {
                    throw new InvalidDataException($"{context}Could not determine global transaction index at index 15.");
                }

                paymentReference = paymentReference?.Trim('\'');
                description = description?.Trim('\'');

                var id = $"{journalDate}_{globalTransactionIndex}";

                result.Add(
                    new Mutation(
                        id,
                        dateTime,
                        new(mutationAmount, mutationCurrency),
                        balancingAccountName,
                        description,
                        paymentReference,
                        balancingAccountNumber,
                        mutationType));
            }

            return result;
        }
    }
}
