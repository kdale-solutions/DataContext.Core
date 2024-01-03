using DataContext.Core.Interfaces.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataContext.Core.Entities
{
	public abstract partial class TransactionalEntity : BaseEntity<int>, ITransactionalEntity
	{
		[NotMapped]
		public Guid? TempId { get; set; } = null;

		[JsonIgnore]
        public byte[] RowVersion { get; set; }

		[NotMapped]
		public Dictionary<string, object> ModificationBag { get; set; }
	}
}
