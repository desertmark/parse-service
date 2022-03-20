using ExcelDataReader;
namespace parse_service.Managers;

public class ParseManager : IParseManager
{
    private readonly ILogger<dynamic> logger;

    public ParseManager(ILogger<dynamic> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Parse an .xls Excel file. Assumes it contains a Table that needs to be parsed into JSON format.
    /// </summary>
    public List<dynamic> FromXls(IFormFile file, ParseOptions options)
    {
        try
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var stream = this.GetMemoryStream(file);
            var reader = ExcelReaderFactory.CreateReader(stream);
            var header = this.ReadHeader(reader, options.headerIndex);
            var rows = this.ReadBody(reader, header);
            return rows;
        }
        catch (Exception error)
        {
            logger.LogError("Error on ParseManager.FromXls", error, file, options);
            throw error;
        }
    }

    /// <summary>
    /// Read rows and build json
    /// </summary>
    private List<dynamic> ReadBody(IExcelDataReader reader, List<String> header)
    {
        try
        {
            var rows = new List<dynamic>();
            while (reader.Read())
            {
                var row = new Json();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var colName = header[i];
                    try
                    {
                        var cellValue = reader.GetValue(i).ToString();
                        row.setKey(colName, cellValue);
                    }
                    catch (NullReferenceException error)
                    {
                        // Row exists but is empty.
                        logger.LogWarning("Empty row found while reading the file.", error, reader, header);
                    }
                }
                if (row.hasKeys()) {
                    rows.Add(row.obj);
                }

            }
            return rows;
        }
        catch (Exception error)
        {
            logger.LogError("Error on ParseManager.ReadBody", error, reader, header);
            throw error;
        }
    }

    /// <summary>
    /// Skip all rows until you hit the table's header
    /// </summary>
    private void SkipEmptyRows(IExcelDataReader reader, int headerIndex)
    {
        try
        {
            int index = 0;
            while (reader.Read() && (index < headerIndex))
            {
                index++;
            }
        }
        catch (Exception error)
        {
            logger.LogError("Error on ParseManager.SkipEmptyRows", error, reader, headerIndex);
            throw error;
        }
    }
    /// <summary>
    /// Gets the header of the table as list of strings.
    /// </summary>
    private List<String> ReadHeader(IExcelDataReader reader, int headerIndex)
    {
        try
        {
            this.SkipEmptyRows(reader, headerIndex);
            var header = new List<String>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                try
                {
                    var colName = reader.GetValue(i).ToString();
                    header.Add(colName);
                }
                catch (NullReferenceException)
                {
                    throw new HeaderNotFoundException(headerIndex);
                }
            }
            return header;
        }
        catch (Exception error)
        {
            logger.LogError("Error on ParseManager.ReadHeader", error, reader, headerIndex);
            throw error;
        }
    }

    /// <summary>
    /// Gets a memory stream from IFormFile ready to be read.
    /// </summary>
    private MemoryStream GetMemoryStream(IFormFile file)
    {
        try
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            return stream;
        }
        catch (Exception error)
        {
            logger.LogError("Error on ParseManager.GetMemoryStream", error, file);
            throw error;
        }
    }

}

