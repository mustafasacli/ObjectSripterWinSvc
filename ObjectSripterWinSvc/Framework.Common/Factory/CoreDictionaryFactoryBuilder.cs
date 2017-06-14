using Framework.Common.Interfaces;
using System;

namespace Framework.Common.Factory
{
    public class CoreDictionaryFactoryBuilder
    {
        private static Lazy<CoreDictionaryFactoryBuilder> pInstance =
            new Lazy<CoreDictionaryFactoryBuilder>(() => new CoreDictionaryFactoryBuilder());

        private CoreDictionaryFactoryBuilder()
        { }

        public static CoreDictionaryFactoryBuilder Instance { get { return pInstance.Value; } }

        private static Lazy<ICoreDictionaryFactory> pFactoryInstance =
            new Lazy<ICoreDictionaryFactory>(() => new CoreDictionaryFactory());

        public ICoreDictionaryFactory DictionaryFactry { get { return pFactoryInstance.Value; } }
    }
}
