using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gherkin.Ast;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.CodeDom;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Generator.UnitTestConverter;
using TechTalk.SpecFlow.Parser;

namespace Itamaram.SpecFlow.Plugin.Base
{
    public class Generator : TestGenerator
    {
        private readonly ExamplesGeneratorContainer container;

        public Generator(
            SpecFlowConfiguration specFlowConfiguration,
            ProjectSettings projectSettings,
            ITestHeaderWriter testHeaderWriter,
            ITestUpToDateChecker testUpToDateChecker,
            IFeatureGeneratorRegistry featureGeneratorRegistry,
            CodeDomHelper codeDomHelper,
            IGherkinParserFactory gherkinParserFactory,
            ExamplesGeneratorContainer container
            ) : base(
                specFlowConfiguration,
                projectSettings,
                testHeaderWriter,
                testUpToDateChecker,
                featureGeneratorRegistry,
                codeDomHelper,
                gherkinParserFactory
                )
        {
            this.container = container;
        }

        protected override SpecFlowDocument ParseContent(IGherkinParser parser, TextReader contentReader,
            SpecFlowDocumentLocation documentLocation)
        {
            var doc = base.ParseContent(parser, contentReader, documentLocation);
            var children = doc.SpecFlowFeature.Children.Select(c => ProcessScenarioDefinition(c, doc));
            return doc.Clone(doc.SpecFlowFeature.Clone(children: children));
        }

        private IHasLocation ProcessScenarioDefinition(IHasLocation definition, SpecFlowDocument doc)
        {
            if (definition is ScenarioOutline outline)
                return outline.Clone(examples: outline.Examples.Select(e => HandleTags(e, doc)));

            return definition;
        }

        private Examples HandleTags(Examples example, SpecFlowDocument doc)
        {
            if (example.Tags == null)
                return example;

            var unhandled = example.Tags.ToList();

            var header = new ExamplesHeader(example.TableHeader.Cells.Select(c => c.Value));

            var rows = new List<TableRow>();

            foreach (var generator in container.GetGenerators())
            {
                foreach (var tag in example.Tags)
                {
                    var name = tag.SanitiseTag();
                    if (!generator.Handles(name))
                        continue;

                    unhandled.Remove(tag);

                    var generated = generator.GetRows(name, doc.SourceFilePath, header)
                        .Select(r => r.ToTableRow(doc))
                        .ToList();

                    if (!generated.Any())
                        throw new SemanticParserException($"Generator '{name}' returned no examples", tag.Location);

                    rows.AddRange(generated);
                }
            }

            return !rows.Any()
                ? example
                : example.Clone(tags: unhandled, body: example.TableBody.Concat(RowSorter.MaybeSortRows(example.TableHeader, rows)));
        }
    }
}