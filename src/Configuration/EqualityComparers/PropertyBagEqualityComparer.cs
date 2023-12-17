using Global.Utilities;

#nullable enable
#pragma warning disable CS8600, CS8601, CS8602
namespace DataContext.Core.Configuration.EqualityComparers
{
	public class PropertyBagEqualityComparer : IEqualityComparer<Dictionary<string, object>>
    {
		public static bool AreEqual(Dictionary<string, object>? x, Dictionary<string, object>? y)
		{
			if (x.IsNullOrEmpty() != y.IsNullOrEmpty()) return false;
			if (x.IsNullOrEmpty()) return true;

			if (x.Keys.Count != y.Keys.Count || x.Keys.Except(y.Keys).Count() > 0) return false;

			foreach (var key in x.Keys)
			{
				if (object.Equals(x[key], y[key])) return false;
			}

			return true;
		}

		public int Compare(Dictionary<string, object>? x, Dictionary<string, object>? y)
        {
            var xNull = x.IsNullOrEmpty();
            var yNull = y.IsNullOrEmpty();

            if (xNull && yNull) return 0;
            else if (xNull && !yNull) return -1;
            else if (!xNull && yNull) return 1;

            var xKeys = x.Keys.ToHashSet();
            var yKeys = y.Keys.ToHashSet();

            if (KeysEqual(xKeys, yKeys)) return 0;
            else if (x.Keys.Count > y.Keys.Count) return -1;
            else return 1;
        }

        private bool KeysEqual(HashSet<string> keysA, HashSet<string> keysB)
        {
            if (keysA.Count == keysB.Count)
            {
                keysA.ExceptWith(keysB);
                return keysA.Count == 0;
            }
            else
            {
                return false;
            }
        }

        private bool ValuesEqual(Dictionary<string, object> x, Dictionary<string, object> y)
        {
            foreach (var key in x.Keys)
            {
                if (!ReferenceEquals(x[key], y[key])) return false;
            }

            return true;
        }

        public bool Equals(Dictionary<string, object>? x, Dictionary<string, object>? y)
        {
            if (x.IsNullOrEmpty() != y.IsNullOrEmpty()) return false;
            if (x.IsNullOrEmpty()) return true;

            if (!KeysEqual(x.Keys.ToHashSet(), y.Keys.ToHashSet())) return false;

            return ValuesEqual(x, y);
        }

        public int GetHashCode(Dictionary<string, object> x)
        {
            return x.Keys.Count ^ x.GetHashCode();
        }
    }
}
