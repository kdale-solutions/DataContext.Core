using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Core.ValueTypes
{
	public readonly struct ModifiedData
	{
		public readonly object OldVal { get; init; }
		public readonly object NewVal { get; init; }

		public ModifiedData(object oldVal, object newVal)
		{
			OldVal = oldVal;
			NewVal = newVal;
		}
	}
}
