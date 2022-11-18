using System;

namespace AvroHelper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GeneratedAvroRecordAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AvroColumnAttribute : Attribute
    {
        public int Index { get; }

        public string ColumnName { get; set; }

        public string UnderlyingType { get; set; }

        public string LogicalType { get; set; }

        public AvroColumnAttribute(int index)
        {
            if (index < 0)
                throw new ArgumentException("Index cannot be negative", nameof(index));

            Index = index;
        }
    }
}