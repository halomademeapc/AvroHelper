# AvroHelper
Source generator to make implementing [ISpecificRecord](https://avro.apache.org/docs/1.9.2/api/csharp/html/interfaceAvro_1_1Specific_1_1ISpecificRecord.html) less of a pain.

## Usage

**Install NuGet package**
```bash
dotnet add package AvroHelper.Generators
```

**Apply attribute to your partial class**

Any property with an `AvroColumnAttribute` will be included in mapping.  

```csharp
using AvroHelper

[GeneratedAvroRecord]
public partial class MyRowDto
{
  public string SomeId { get; set; }
  
  [AvroColumn(0, ColumnName = "some_RiDiCuLoUs_name")]
  public string SensibleName { get; set; }

  [AvroColumn(0, ColumnName = "DATE_COLUMN", UnderlyingType = "int", LogicalType = "date")]
  public DateTime? DateColumn { get; set; }
}
```

The souce generator will implement [ISpecificRecord](https://avro.apache.org/docs/1.9.2/api/csharp/html/interfaceAvro_1_1Specific_1_1ISpecificRecord.html) with implementations of the Schema, Get, and Put.  It will only add the Schema if your class does not already have a Schema Get method, so if you don't want to use the generator's schema, just implement that yourself.

```csharp
var datumReader = new SpecificDatumReader<MyRowDto>(schema, schema);
using var stream = avroRows.SerializedBinaryRows.Memory.AsStream();
var decoder = new BinaryDecoder(stream);

while (stream.Position < stream.Length)
{
    var @record = datumReader.Read(null!, decoder);
    yield return @record;
}
```
