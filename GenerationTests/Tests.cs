using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using AvroHelper;
using AvroHelper.Generators;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GenerationTests;

[UsesVerify] // 👈 Adds hooks for Verify into XUnit
public class BigQueryMapperGeneratorSnapshotTests
{
    [Fact]
    public Task GeneratesMappingPartialCorrectly()
    {
        // The source code to test
        var source = /* lang=csharp */@"using AvroHelper;
using System;

namespace Fake.Namespace;

[GeneratedAvroRecord]
public partial class MeterReadingEntry
{
    [AvroColumn(0, ColumnName = ""PropertyID"")]
    public string? PropertyId { get; set; }

    [AvroColumn(1, ColumnName = ""Property_RentRoll_ID"")]
    public string? RentRollId { get; set; }

    [AvroColumn(3, ColumnName = ""header_serial_number"")]
    public string? SerialNumber { get; set; }

    [AvroColumn(2, ColumnName = ""Gallons"")]
    public double? Gallons { get; set; }

    [AvroColumn(4, ColumnName = ""Events"")] public double? Events { get; set; }

    [AvroColumn(5, ColumnName = ""Flowtime"")]
    public double? FlowTime { get; set; }

    [AvroColumn(6, ColumnName = ""hour"", UnderlyingType = ""long"", LogicalType = ""timestamp-micros"")]
    public DateTime? Hour { get; set; }

    [AvroColumn(7, ColumnName = ""Running_hours"")]
    public long? RunningHours { get; set; }

    [AvroColumn(8, ColumnName = ""CatchupFlag"")]
    public string? CatchupFlag { get; set; }

    [AvroColumn(9, ColumnName = ""Leak_Status"")]
    public string? LeakStatus { get; set; }

    [AvroColumn(10, ColumnName = ""Leak_Details"")]
    public string? LeakDetails { get; set; }

    [AvroColumn(11, ColumnName = ""toilet_leak"")]
    public double? ToiletLeak { get; set; }

    [AvroColumn(12, ColumnName = ""Miscellaneous_Leaks"")]
    public double? MiscellaneousLeak { get; set; }

    [AvroColumn(13, ColumnName = ""daily_leak_gallon_total"")]
    public double? DailyGallonsLeaked { get; set; }

    [AvroColumn(14, ColumnName = ""daily_leak_status"")]
    public string? DailyLeakStatus { get; set; }

    [AvroColumn(15, ColumnName = ""Leak_Gallons_22_hours"")]
    public double? LeakGallons22Hours { get; set; }

    [AvroColumn(16, ColumnName = ""DateLeakStarted"", UnderlyingType = ""int"", LogicalType = ""date"")]
    public DateTime? DateLeakStarted { get; set; }

    [AvroColumn(17, ColumnName = ""Total_gallons_since_leak"")]
    public double? GallonsSinceLastLeak { get; set; }

    [AvroColumn(18, ColumnName = ""last_leaking_at"", UnderlyingType = ""long"", LogicalType = ""timestamp-micros"")]
    public DateTime? LastLeakingAt { get; set; }

    [AvroColumn(19, ColumnName = ""Developer"")]
    public string? DeveloperName { get; set; }

    [AvroColumn(20, ColumnName = ""GPD_Filter"")]
    public string? GpdFilter { get; set; }

    [AvroColumn(21, ColumnName = ""FlowtimeHHmm"")]
    public string? FlowTimeFormatted { get; set; }

    [AvroColumn(22, ColumnName = ""ran_more_than_22h"")]
    public string? RanMoreThan22Hours { get; set; }

    [AvroColumn(23, ColumnName = ""hourly_reading_status_num"")]
    public long? HourlyReadingStatus { get; set; }

    [AvroColumn(24, ColumnName = ""Days_Repeating"")]
    public long? DaysRepeating { get; set; }

    [AvroColumn(25, ColumnName = ""Unit_Details"")]
    public string? UnitDetails { get; set; }

    [AvroColumn(26, ColumnName = ""Hours_running_55min_Daily"")]
    public long? HoursRunningMoreThan55Minutes { get; set; }

    [AvroColumn(27, ColumnName = ""Leak_Gallons"")]
    public double? LeakGallons { get; set; }

    [AvroColumn(28, ColumnName = ""Orange_Leak_Gallons"")]
    public double? WarningLeakGallons { get; set; }

    [AvroColumn(29, ColumnName = ""Red_Leak_Gallons"")]
    public double? UrgentLeakGallons { get; set; }

    [AvroColumn(30, ColumnName = ""Latest_Leak_Status"")]
    public string? LatestLeakStatus { get; set; }

    [AvroColumn(31, ColumnName = ""duration_since_last_reading"")]
    public string? TimeSinceLastReading { get; set; }

    [AvroColumn(32, ColumnName = ""DPOE_hourly_events_hot_cold"")]
    public string? DpoeHourlyEvents { get; set; }

    [AvroColumn(33, ColumnName = ""DPOE_hourly_gallons_hot_cold"")]
    public string? DpoeHourlyGallons { get; set; }

    [AvroColumn(34, ColumnName = ""DPOE_hourly_flowtime_hot_cold"")]
    public string? DpoeHourlyFlowTime { get; set; }
}
";

        // Pass the source code to our helper and snapshot test the output
        return TestHelper.Verify(source);
    }
}

public static class TestHelper
{
    public static Task Verify(string source)
    {
        // Parse the provided string into a C# syntax tree
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Create a Roslyn compilation for the syntax tree.
        var assemblies = ReferenceAssemblies.Net60.Append(
            MetadataReference.CreateFromFile(typeof(GeneratedAvroRecordAttribute).GetTypeInfo().Assembly.Location))
            .Append(
                MetadataReference.CreateFromFile(typeof(AvroColumnAttribute).GetTypeInfo().Assembly.Location));
        var compilation = CSharpCompilation.Create("compilation",
            new[] { syntaxTree },
            assemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var diag = compilation.GetDiagnostics();
        Assert.Empty(diag.Where(d => d.Severity == DiagnosticSeverity.Error));

        // directly create an instance of the generator
        // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
        var generator = new AvroRecordGenerator();

        // Create the driver that will control the generation, passing in our generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the generation pass
        // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        var runResult = driver.GetRunResult();

        // Use verify to snapshot test the source generator output!
        return Verifier.Verify(runResult);
    }
}

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
    }
}