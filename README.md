# Finamoid - Bank statement management
Finamoid is a personal finance manager for categorizing and aggregating bank statements, aimed at household bank accounts.

## Idea
Analyzing bank statements can be a tedious task. This application aims to provide better insight into your finances by categorizing and aggregating your payment transactions. These are some questions this application aims to answer:
* How much did I spend on groceries last month?
* How much do I spend on fixed costs every month, and how fixed are these costs really?
* How much did I put into my savings account the last half year?
* In which categories am I overspending?

## Features
✅ Import bank statements (mutations) from several formats. Currently ASN Bank CSV is implemented.

✅ Import categories from CSV to kickstart the application while not having a UI to manage categories.

✅ Categorize mutations based on filters (partial, exact, bank account number).

✅ Aggregate categorized mutations into sum by time (e.g. spendings per category per month).

✅ Interactive CLI to do all above steps while the UI is still under construction.

✅ 100% offline, with the option to store data encrypted (untested).

![CLI](https://github.com/TheAsteroid/finamoid/assets/41196601/f8a6e023-e0d4-4fd0-b341-c599809b6c75)

## To do
🔴 Define license type

🔴 Let user set password, generate recovery codes.

🔴 Test two-tier AES encryption.

🔴 Support more bank statement formats: ING CSV, CAMT.053 XML (ISO 20022 industry standard).

🔴 Analysis on aggregations: end of month balance, moving average income / outcome, etc.

🔴 User interface (.NET MAUI) for features that are already done.

🔴 User interface (.NET MAUI) for new features: charts and tables ([Microcharts](https://github.com/microcharts-dotnet/Microcharts)https://github.com/microcharts-dotnet/Microcharts)

🔴 Export aggregations to CSV, optionally XLSX.

🔴 Write unit / integration tests
