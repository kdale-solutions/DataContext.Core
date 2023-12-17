using DataContext.Core.Interfaces.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataContext.Core.Entities
{
	public abstract class TransactionalEntity : BaseEntity<int>, ITransactionalEntity
	{
		[JsonIgnore]
        public byte[] RowVersion { get; set; }

		[NotMapped]
		[JsonIgnore]
		public Dictionary<string, object> ModificationBag { get; set; }
	}
}
