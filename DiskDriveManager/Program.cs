
using DiskDriveManager.DiskDrive;
using DiskDriveManager.Functions.EnumParser;
using System.Text.Json;


var diskInfo = DiskDriveHelper.GetInfo();
string json = JsonSerializer.Serialize(diskInfo,
    new JsonSerializerOptions()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //IgnoreReadOnlyProperties = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
        //DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
    });
Console.WriteLine(json);




Console.ReadLine();
