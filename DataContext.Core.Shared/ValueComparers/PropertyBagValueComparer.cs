using DataContext.Core.Configuration.EqualityComparers;
using DataContext.Core.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

#nullable enable
#pragma warning disable CS8600, CS8601, CS8602
namespace DataContext.Core.Configuration.ValueComparers
{
    public class PropertyBagValueComparer : ValueComparer<Dictionary<string, object>>
    {
        public PropertyBagValueComparer()
            : base(
                (x, y) => PropertyBagEqualityComparer.AreEqual(x, y),
                x => x.GetHashCode()
                  )
        { }
    }
}
