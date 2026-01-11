using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.ValueObjects
{
    public sealed class FlatType
    {
        public string Value { get; private set; } = null!;

        private FlatType() { }

        private FlatType(string value)
        {
            Value = value;
        }

        public static FlatType Apartment => new("Apartment");
        public static FlatType Villa => new("Villa");
        public static FlatType Office => new("Office");

        public static FlatType From(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Flat type is required");

            return new FlatType(value);
        }

        public override string ToString() => Value;
    }
}
