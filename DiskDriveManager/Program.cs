
using DiskDriveManager.DiskDrive;
using System.Text.Json;

var info = DiskDriveHelper.GetInfo();
var json = JsonSerializer.Serialize(info,
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
