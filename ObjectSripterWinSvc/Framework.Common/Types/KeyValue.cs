namespace Framework.Common.Types
{
    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return this.Key ?? string.Empty;
        }
    }
}
