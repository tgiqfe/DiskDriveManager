
using DiskDriveManager.DiskDrive;
using DiskDriveManager.Functions.EnumParser;
using System.Text.Json;


var partisions = PartitionItem.Load();
string json = JsonSerializer.Serialize(partisions,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //IgnoreReadOnlyProperties = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
    });
Console.WriteLine(json);


Console.ReadLine();
