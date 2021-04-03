using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;
using TechTalk.SpecFlow.Parser;

namespace Itamaram.SpecFlow.Plugin.Base
{
    public static class Extensions
    {
        public static TableRow ToTableRow(this IEnumerable<string> cells, SpecFlowDocument doc)
        {
            return cells.ToTableRow(doc.SpecFlowFeature.Location);
        }

        public static TableRow ToTableRow(this IEnumerable<string> cells, Location loc)
        {
            return new TableRow(loc, cells.Select(c => new TableCell(loc, c)).ToArray());
        }

        public static string SanitiseTag(this Tag tag)
        {
            return tag.Name.StartsWith("@") ? tag.Name.Substring(1) : tag.Name;
        }

        public static SpecFlowDocument Clone(this SpecFlowDocument d,
            SpecFlowFeature feature = null,
            IEnumerable<Comment> comments = null,
            string sourceFilePath = null
        )
        {
            return new SpecFlowDocument(
                feature ?? d.SpecFlowFeature,
                (comments ?? d.Comments)?.ToArray(),
                new SpecFlowDocumentLocation(sourceFilePath ?? d.SourceFilePath)
            );
        }

        public static SpecFlowFeature Clone(this SpecFlowFeature f,
            IEnumerable<Tag> tags = null,
            Location location = null,
            string language = null,
            string keyword = null,
            string name = null,
            string description = null,
            IEnumerable<IHasLocation> children = null
        )
        {
            return new SpecFlowFeature(
                (tags ?? f.Tags)?.ToArray(),
                location ?? f.Location,
                language ?? f.Language,
                keyword ?? f.Keyword,
                name ?? f.Name,
                description ?? f.Description,
                (children ?? f.Children)?.ToArray()
            );
        }

        public static ScenarioOutline Clone(this ScenarioOutline o,
            IEnumerable<Tag> tags = null,
            Location location = null,
            string keyword = null,
            string name = null,
            string description = null,
            IEnumerable<Step> steps = null,
            IEnumerable<Examples> examples = null
        )
        {
            return new ScenarioOutline(
                (tags ?? o.Tags)?.ToArray(),
                location ?? o.Location,
                keyword ?? o.Keyword,
                name ?? o.Name,
                description ?? o.Description,
                (steps ?? o.Steps)?.ToArray(),
                (examples ?? o.Examples)?.ToArray()
            );
        }

        public static Examples Clone(this Examples x,
            IEnumerable<Tag> tags = null,
            Location location = null,
            string keyword = null,
            string name = null,
            string description = null,
            TableRow header = null,
            IEnumerable<TableRow> body = null
        )
        {
            return new Examples(
                (tags ?? x.Tags)?.ToArray(),
                location ?? x.Location,
                keyword ?? x.Keyword,
                name ?? x.Name,
                description ?? x.Description,
                header ?? x.TableHeader,
                (body ?? x.TableBody)?.ToArray()
            );
        }
    }
}
