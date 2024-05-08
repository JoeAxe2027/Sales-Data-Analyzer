using System.Globalization;
public class SalesDataProcessor
{
    public static List<SaleRecord> LoadSalesData(string filePath)
    {
        var salesRecords = new List<SaleRecord>();
        var lineNumber = 0;

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    var columns = line.Split(',');
                    if (columns.Length != 11)
                        throw new FormatException($"Row {lineNumber} contains {columns.Length} values. It should contain 11.");
                    
                    var record = new SaleRecord
                    {
                        InvoiceID = columns[0],
                        Branch = columns[1],
                        City = columns[2],
                        CustomerType = columns[3],
                        Gender = columns[4],
                        ProductLine = columns[5],
                        UnitPrice = decimal.Parse(columns[6], CultureInfo.InvariantCulture),
                        Quantity = int.Parse(columns[7]),
                        Date = DateTime.Parse(columns[8]),
                        Payment = columns[9],
                        Rating = double.Parse(columns[10], CultureInfo.InvariantCulture)
                    };
                    salesRecords.Add(record);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the sales data file: {ex.Message}");
            Environment.Exit(1);
        }

        return salesRecords;
    }

}
