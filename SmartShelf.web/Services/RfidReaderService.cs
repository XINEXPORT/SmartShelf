using System;
using System.Collections.Generic;
using ThingMagic;

/*
Class: TagReadResult

Description:
Represents a single RFID tag read result.
This is a clean, application-friendly model extracted from ThingMagic's TagReadData.

Fields:
- EPC       : Unique EPC identifier of the RFID tag
- Antenna   : Antenna number that detected the tag
- Rssi      : Signal strength (used for distance estimation / localization)
- Timestamp : Time when the tag was read
*/
public class TagReadResult
{
    public required string EPC { get; set; }
    public int Antenna { get; set; }
    public int Rssi { get; set; }
    public DateTime Timestamp { get; set; }
}

/*
Class: RfidReaderService

Description:
Handles all communication with the ThingMagic RFID reader.

Responsibilities:
- Connect to reader
- Configure reader settings
- Read tag data
- Return clean application-level objects

IMPORTANT:
- This is the ONLY class that talks to hardware
- Do NOT add business logic, database calls, or API formatting
- Acts as a boundary between hardware and application logic
*/
public class RfidReaderService
{
    private Reader _reader;

    /*
    Method: Connect

    Description:
    Establishes connection to the RFID reader using a URI.

    Example URIs:
    - Serial (Windows): tmr:///com4
    - Serial (Linux):   tmr:///dev/ttyUSB1
    - Network reader:   tmr://192.168.1.100

    Steps:
    1. Create reader instance
    2. Attempt connection
    3. Handle baud rate fallback (serial readers)
    4. Configure region
    5. Enable metadata
    6. Set read plan
    */
    public void Connect(string readerUri)
    {
        _reader = Reader.Create(readerUri);

        try
        {
            _reader.Connect();
        }
        catch (Exception ex)
        {
            /*
            If connection times out on a serial reader,
            attempt baud rate probing (ThingMagic-specific behavior)
            */
            if (ex.Message.Contains("The operation has timed out") && _reader is SerialReader sr)
            {
                int baud = 0;

                sr.probeBaudRate(ref baud);
                _reader.ParamSet("/reader/baudRate", baud);
                _reader.Connect();
            }
            else
            {
                throw;
            }
        }

        // Ensure region is set (required)
        if ((Reader.Region)_reader.ParamGet("/reader/region/id") == Reader.Region.UNSPEC)
        {
            var regions = (Reader.Region[])_reader.ParamGet("/reader/region/supportedRegions");

            if (regions.Length == 0)
                throw new Exception("No supported regions found.");

            _reader.ParamSet("/reader/region/id", regions[0]);
        }

        // Enable metadata (RSSI, antenna, timestamp, etc.)
        string model = (string)_reader.ParamGet("/reader/version/model");
        if (!model.Equals("Mercury6"))
        {
            _reader.ParamSet("/reader/metadata", SerialReader.TagMetadataFlag.ALL);
        }

        /*
        Configure read plan:
        - null antenna list → use all antennas
        - GEN2 → UHF RFID protocol
        - 1000 → internal timing parameter
        */
        var plan = new SimpleReadPlan(null, TagProtocol.GEN2, null, null, 1000);
        _reader.ParamSet("/reader/read/plan", plan);
    }

    /*
    Method: ReadTags

    Description:
    Reads RFID tags for a fixed duration and returns clean results.

    Behavior:
    - Blocking call (waits for durationMs)
    - Collects ALL tags seen during the time window

    Parameters:
    - durationMs (int) : Read duration in milliseconds (default = 500)

    Returns:
    - List<TagReadResult> : Processed tag data (not raw ThingMagic objects)
    */
    public List<TagReadResult> ReadTags(int durationMs = 500)
    {
        if (_reader == null)
            throw new InvalidOperationException("Reader not connected. Call Connect() first.");

        var results = new List<TagReadResult>();

        TagReadData[] tagReads = _reader.Read(durationMs);

            foreach (var tr in tagReads)
            {
                results.Add(new TagReadResult
                {
                    EPC = tr.EpcString,
                    Antenna = tr.Antenna,
                    Rssi = tr.Rssi,
                    Timestamp = tr.Time
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading tags: " + ex.Message);
        }

        return results;
    }

    /*
    Method: Disconnect

    Description:
    Releases the reader and frees hardware resources.

    Notes:
    - Always call this when shutting down the application
    */
    public void Disconnect()
    {
        _reader?.Destroy();
    }
}
