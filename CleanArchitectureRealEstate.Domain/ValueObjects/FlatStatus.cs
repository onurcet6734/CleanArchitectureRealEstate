using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.ValueObjects
{
    public sealed class FlatStatus
    {
        public string Value { get; private set; } = null!;

        private FlatStatus() { }

        private FlatStatus(string value)
        {
            Value = value;
        }

        public static FlatStatus ForSale => new("ForSale");
        public static FlatStatus ForRent => new("ForRent");
        public static FlatStatus Sold => new("Sold");

        public static FlatStatus From(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Flat status is required");

            return new FlatStatus(value);
        }

        public override string ToString() => Value;
    }
}
