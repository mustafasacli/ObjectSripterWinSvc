using Framework.Common.Interfaces;

namespace Framework.Common.Factory
{
    internal class CoreDictionaryFactory : ICoreDictionaryFactory
    {
        public ICoreDictionary GetNew() { return new CoreDictionary(); }
    }
}
