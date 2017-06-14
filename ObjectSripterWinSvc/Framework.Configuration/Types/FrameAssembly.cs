namespace Framework.Configuration.Types
{
    public struct FrameAssembly
    {

        public FrameAssembly(string typeName, string nameSpaceStr, string className)
        {
            this.TypeName = typeName ?? string.Empty;
            this.Namespace = nameSpaceStr ?? string.Empty;
            this.ClassName = className ?? string.Empty;
        }

        public string TypeName { get; private set; }

        public string Namespace { get; private set; }

        public string ClassName { get; private set; }
    }
}
