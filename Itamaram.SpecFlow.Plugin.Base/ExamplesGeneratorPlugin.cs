using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.UnitTestProvider;

namespace Itamaram.SpecFlow.Plugin.Base
{
    public abstract class ExamplesGeneratorPlugin<T> : IGeneratorPlugin where T : ExamplesGenerator
    {
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            generatorPluginEvents.RegisterDependencies += (_, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<Generator, ITestGenerator>();
            };

            generatorPluginEvents.CustomizeDependencies += (_, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<MissingExamplesParserFactory, IGherkinParserFactory>();
                args.ObjectContainer.Resolve<ExamplesGeneratorContainer>().RegisterGenerator<T>();
            };

            InitializeInternal(generatorPluginEvents, generatorPluginParameters);
        }

        public virtual void InitializeInternal(GeneratorPluginEvents events, GeneratorPluginParameters parameters) { }
    }
}