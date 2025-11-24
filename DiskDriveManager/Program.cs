using DiskDriveManager.DiskDrive;
using System.Text.Json;

var disks = DiskItem.Load();
string json1 = JsonSerializer.Serialize(disks,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //IgnoreReadOnlyProperties = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
    });
Console.WriteLine(json1);

Console.WriteLine("===============================================");

var partitions = PartitionItem.Load();


Console.ReadLine();
