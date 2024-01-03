namespace DataContext.Core.ValueTypes
{
	public struct SearchUrl
    {
        public string Value { get; set; }

        public int HashCode => Hash();

        public SearchUrl(string value)
        {
            Value = value;
        }

        public static implicit operator SearchUrl(string value)
        {
            return new SearchUrl(value);
        }

        public static implicit operator string(SearchUrl value)
        {
            return value.Value;
        }

        public new SearchUrl ToString()
        {
            return this.Value;
        }

        public int Hash()
        {
            var code = 0;

			for (int i = 0, j = this.Value.Length - 1; i <= j; i++, j--)
			{
                code += (i != j ? (this.Value[i] + this.Value[j]) : this.Value[i]);
			}

            return code % Int32.MaxValue;
        }
    }
}
