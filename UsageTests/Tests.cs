using System.Reflection;
using Avro;
using Avro.Specific;
using AvroHelper;

namespace UsageTests;

public class Tests
{
    [Fact]
    public void ShouldHaveGet()
    {
        var method = typeof(MeterReadingEntry).GetMethod("Get");
        Assert.NotNull(method);
        Assert.False(method!.IsStatic);
    }
    
    [Fact]
    public void ShouldHavePut()
    {
        var method = typeof(MeterReadingEntry).GetMethod("Put");
        Assert.NotNull(method);
        Assert.Equal(typeof(void), method!.ReturnType);
        Assert.False(method!.IsStatic);
    }

    [Fact]
    public void ShouldImplementInterface()
    {
        var @interfaces = typeof(MeterReadingEntry).GetInterfaces();
        var matchingInterface =
            @interfaces.FirstOrDefault(i => i == typeof(ISpecificRecord));
        Assert.NotNull(@matchingInterface);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(null)]
    public void ShouldStoreDouble(double? value)
    {
        var entry = new MeterReadingEntry();
        entry.Put(5, value);
        Assert.Equal(value, entry.FlowTime);
        Assert.Equal(value, entry.Get(5));
    }
    
    [Theory]
    [InlineData("potato")]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldStoreString(string? value)
    {
        var entry = new MeterReadingEntry();
        entry.Put(3, value);
        Assert.Equal(value, entry.SerialNumber);
        Assert.Equal(value, entry.Get(3));
    }
    
    [Theory]
    [InlineData(735)]
    [InlineData(0)]
    [InlineData(null)]
    public void ShouldStoreLong(long? value)
    {
        var entry = new MeterReadingEntry();
        entry.Put(26, value);
        Assert.Equal(value, entry.HoursRunningMoreThan55Minutes);
        Assert.Equal(value, entry.Get(26));
    }
    
    [Theory]
    [InlineData("2021-05-08")]
    [InlineData(null)]
    public void ShouldStoreDateTime(string? value)
    {
        DateTime? dt = DateTime.TryParse(value, out var parsed) ? parsed : null;
        var entry = new MeterReadingEntry();
        entry.Put(6, dt);
        Assert.Equal(dt, entry.Hour);
        Assert.Equal(dt, entry.Get(6));
    }

    [Fact]
    public void ShouldParseSchema()
    {
        var field = typeof(MeterReadingEntry).GetField("___schemaJson", BindingFlags.NonPublic | BindingFlags.Static);
        var json = field!.GetValue(null) as string;
        var schema = Schema.Parse(json);
        Assert.NotNull(schema);
    }
}

[GeneratedAvroRecord]
public partial class MeterReadingEntry
{
    [AvroColumn(0, ColumnName = "PropertyID")]
    public string? PropertyId { get; set; }

    [AvroColumn(1, ColumnName = "Property_RentRoll_ID")]
    public string? RentRollId { get; set; }

    [AvroColumn(3, ColumnName = "header_serial_number")]
    public string? SerialNumber { get; set; }

    [AvroColumn(2, ColumnName = "Gallons")]
    public double? Gallons { get; set; }

    [AvroColumn(4, ColumnName = "Events")] public double? Events { get; set; }

    [AvroColumn(5, ColumnName = "Flowtime")]
    public double? FlowTime { get; set; }

    [AvroColumn(6, ColumnName = "hour", UnderlyingType = "long", LogicalType = "timestamp-micros")]
    public DateTime? Hour { get; set; }

    [AvroColumn(7, ColumnName = "Running_hours")]
    public long? RunningHours { get; set; }

    [AvroColumn(8, ColumnName = "CatchupFlag")]
    public string? CatchupFlag { get; set; }

    [AvroColumn(9, ColumnName = "Leak_Status")]
    public string? LeakStatus { get; set; }

    [AvroColumn(10, ColumnName = "Leak_Details")]
    public string? LeakDetails { get; set; }

    [AvroColumn(11, ColumnName = "toilet_leak")]
    public double? ToiletLeak { get; set; }

    [AvroColumn(12, ColumnName = "Miscellaneous_Leaks")]
    public double? MiscellaneousLeak { get; set; }

    [AvroColumn(13, ColumnName = "daily_leak_gallon_total")]
    public double? DailyGallonsLeaked { get; set; }

    [AvroColumn(14, ColumnName = "daily_leak_status")]
    public string? DailyLeakStatus { get; set; }

    [AvroColumn(15, ColumnName = "Leak_Gallons_22_hours")]
    public double? LeakGallons22Hours { get; set; }

    [AvroColumn(16, ColumnName = "DateLeakStarted", UnderlyingType = "int", LogicalType = "date")]
    public DateTime? DateLeakStarted { get; set; }

    [AvroColumn(17, ColumnName = "Total_gallons_since_leak")]
    public double? GallonsSinceLastLeak { get; set; }

    [AvroColumn(18, ColumnName = "last_leaking_at", UnderlyingType = "long", LogicalType = "timestamp-micros")]
    public DateTime? LastLeakingAt { get; set; }

    [AvroColumn(19, ColumnName = "Developer")]
    public string? DeveloperName { get; set; }

    [AvroColumn(20, ColumnName = "GPD_Filter")]
    public string? GpdFilter { get; set; }

    [AvroColumn(21, ColumnName = "FlowtimeHHmm")]
    public string? FlowTimeFormatted { get; set; }

    [AvroColumn(22, ColumnName = "ran_more_than_22h")]
    public string? RanMoreThan22Hours { get; set; }

    [AvroColumn(23, ColumnName = "hourly_reading_status_num")]
    public long? HourlyReadingStatus { get; set; }

    [AvroColumn(24, ColumnName = "Days_Repeating")]
    public long? DaysRepeating { get; set; }

    [AvroColumn(25, ColumnName = "Unit_Details")]
    public string? UnitDetails { get; set; }

    [AvroColumn(26, ColumnName = "Hours_running_55min_Daily")]
    public long? HoursRunningMoreThan55Minutes { get; set; }

    [AvroColumn(27, ColumnName = "Leak_Gallons")]
    public double? LeakGallons { get; set; }

    [AvroColumn(28, ColumnName = "Orange_Leak_Gallons")]
    public double? WarningLeakGallons { get; set; }

    [AvroColumn(29, ColumnName = "Red_Leak_Gallons")]
    public double? UrgentLeakGallons { get; set; }

    [AvroColumn(30, ColumnName = "Latest_Leak_Status")]
    public string? LatestLeakStatus { get; set; }

    [AvroColumn(31, ColumnName = "duration_since_last_reading")]
    public string? TimeSinceLastReading { get; set; }

    [AvroColumn(32, ColumnName = "DPOE_hourly_events_hot_cold")]
    public string? DpoeHourlyEvents { get; set; }

    [AvroColumn(33, ColumnName = "DPOE_hourly_gallons_hot_cold")]
    public string? DpoeHourlyGallons { get; set; }

    [AvroColumn(34, ColumnName = "DPOE_hourly_flowtime_hot_cold")]
    public string? DpoeHourlyFlowTime { get; set; }
}