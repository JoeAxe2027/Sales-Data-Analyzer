class Program
{
    static void Main(string[] args)
    {
        string dataFilePath = "supermarket_sales.csv";
        string reportFilePath = "report.txt";

        var salesRecords = SalesDataProcessor.LoadSalesData(dataFilePath);

        GenerateReport(salesRecords, reportFilePath);
    }

    static void GenerateReport(List<SaleRecord> salesRecords, string reportFilePath)
    {
        using (var writer = new StreamWriter(reportFilePath))
        {
            // All Total Sales
            var totalSales = salesRecords.Sum(r => r.UnitPrice * r.Quantity);
            writer.WriteLine($"Total Sales: {totalSales:C}");

            // Listing Unique Product Lines
            var uniqueProductLines = salesRecords.Select(r => r.ProductLine).Distinct();
            writer.WriteLine("\nUnique Product Lines:");
            foreach (var line in uniqueProductLines)
            {
                writer.WriteLine(line);
            }

            // Total Sales by Product Line
            var salesByProductLine = salesRecords
                .GroupBy(r => r.ProductLine)
                .Select(group => new
                {
                    ProductLine = group.Key,
                    TotalSales = group.Sum(r => r.UnitPrice * r.Quantity)
                });
            writer.WriteLine("\nTotal Sales by Product Line:");
            foreach (var item in salesByProductLine)
            {
                writer.WriteLine($"{item.ProductLine}: {item.TotalSales:C}");
            }

            // Total Sales per City
            var salesByCity = salesRecords
                .GroupBy(r => r.City)
                .Select(group => new
                {
                    City = group.Key,
                    TotalSales = group.Sum(r => r.UnitPrice * r.Quantity)
                });
            writer.WriteLine("\nTotal Sales per City:");
            foreach (var city in salesByCity)
            {
                writer.WriteLine($"{city.City}: {city.TotalSales:C}");
            }

            // Product Line with the Highest Unit Price
            var highestPriceProductLine = salesRecords
                .OrderByDescending(r => r.UnitPrice)
                .First();
            writer.WriteLine($"\nHighest Unit Price Product Line: {highestPriceProductLine.ProductLine} at {highestPriceProductLine.UnitPrice:C}");
            writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");

            // Total Sales per Month
            var salesByMonth = salesRecords
                .GroupBy(r => new { r.City, Month = r.Date.Month })
                .Select(group => new
                {
                    CityMonth = group.Key,
                    TotalSales = group.Sum(r => r.UnitPrice * r.Quantity)
                });
            writer.WriteLine("\nTotal Sales per Month:");
            foreach (var month in salesByMonth)
            {
                writer.WriteLine($"{month.CityMonth.City} {month.CityMonth.Month}: {month.TotalSales:C}");
                
            }
            writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");


            // Sales per Payment Type
            var salesByPaymentType = salesRecords
                .GroupBy(r => r.Payment)
                .Select(group => new
                {
                    PaymentType = group.Key,
                    TotalSales = group.Sum(r => r.UnitPrice * r.Quantity)
                });
            writer.WriteLine("\nTotal Sales per Payment Type:");
            foreach (var payment in salesByPaymentType)
            {
                writer.WriteLine($"{payment.PaymentType}: {payment.TotalSales:C}");
            }
                        writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");


            // Sales transactions by a certain member
            var transactionsByMemberType = salesRecords
                .GroupBy(r => r.CustomerType)
                .Select(group => new
                {
                    MemberType = group.Key,
                    Transactions = group.Count()
                });
            writer.WriteLine("\nNumber of Transactions per Member Type:");
            foreach (var member in transactionsByMemberType)
            {
                writer.WriteLine($"{member.MemberType}: {member.Transactions}");
            }

            // Average rating for product line
            var averageRatingByProductLine = salesRecords
                .GroupBy(r => r.ProductLine)
                .Select(group => new
                {
                    ProductLine = group.Key,
                    AverageRating = group.Average(r => r.Rating)
                });
            writer.WriteLine("\nAverage Rating per Product Line:");
            foreach (var product in averageRatingByProductLine)
            {
                writer.WriteLine($"{product.ProductLine}: {product.AverageRating:F2}");
            }
                        writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");


            // Total number of products sold in each city
            var quantityByCity = salesRecords
                .GroupBy(r => r.City)
                .Select(group => new
                {
                    City = group.Key,
                    TotalQuantity = group.Sum(r => r.Quantity)});
            writer.WriteLine("\nTotal Quantity of Products Sold per City:");
            foreach (var city in quantityByCity)
            {
                writer.WriteLine($"{city.City}: {city.TotalQuantity}");
            }
                        writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");


            // Total Number of Transactions
            var transactionCountByPayment = salesRecords
                .GroupBy(r => r.Payment)
                .Select(group => new
                {
                    PaymentType = group.Key,
                    Transactions = group.Count()
                });
            writer.WriteLine("\nTotal Number of Transactions per Payment Type:");
            foreach (var payment in transactionCountByPayment)
            {
                writer.WriteLine($"{payment.PaymentType}: {payment.Transactions}");
            }
                        writer.WriteLine("--------------------------------------------------------------------------------------------------------------------");


            // Tax
            var taxesByTransaction = salesRecords
                .Select(r => new
                {
                    InvoiceNumber = r.InvoiceID,
                    ProductLine = r.ProductLine,
                    TotalSales = r.UnitPrice * r.Quantity,
                    TaxAmount = r.UnitPrice * r.Quantity * 0.05m 
                })
                .OrderBy(r => r.ProductLine);
            writer.WriteLine("\nTaxes for Each Transaction:");
            foreach (var transaction in taxesByTransaction)
            {
                writer.WriteLine($"Invoice {transaction.InvoiceNumber}, {transaction.ProductLine} - Total Sales: {transaction.TotalSales:C}, Tax: {transaction.TaxAmount:C}");
            }
        }
    }
}


//references
//Used AI to help out line code, as seen by comments in code
//groupBy function helps group elements with similar values