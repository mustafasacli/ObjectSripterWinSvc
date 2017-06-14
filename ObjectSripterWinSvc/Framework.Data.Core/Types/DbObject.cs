namespace Framework.Data.Core.Types
{    
    public struct DbObject
    {
        public string NAME { get; set; }

        public string OWNER { get; set; }

        public string TYPENAME { get; set; }

        public override string ToString()
        {
            return $"OWNER: {OWNER}\nNAME: {NAME}\nTYPENAME: {TYPENAME}\n----------------------\n";
        }
    }
}
