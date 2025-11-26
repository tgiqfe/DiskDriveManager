using DiskDriveManager.DiskDrive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DiskDriveManager.SampleCodes
{
    internal class Sample01
    {
        public static void Test01()
        {
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
            string json2 = JsonSerializer.Serialize(partitions,
                new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    //IgnoreReadOnlyProperties = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                });
            Console.WriteLine(json2);

        }

        public static void Test02()
        {
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

            var json2 = JsonSerializer.Serialize(DriveItem.Load(),
                new JsonSerializerOptions()
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    //IgnoreReadOnlyProperties = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
                    //DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                });
            Console.WriteLine(json2);

        }
    }
}
