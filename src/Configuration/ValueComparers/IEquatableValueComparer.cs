using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

#nullable enable
#pragma warning disable CS8600, CS8601, CS8602
namespace DataContext.Core.Configuration.ValueComparers
{
    public class IEquatableValueComparer<T> : ValueComparer<T> where T : IEquatable<T>
    {
        public IEquatableValueComparer()
            : base(
                (x, y) => x.Equals(y),
                x => x.GetHashCode()
            ) { }
    }
}
