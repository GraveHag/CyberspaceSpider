using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace CS_Core
{
    public sealed class HtmlParserService : IHtmlParserService, IService
    {
        private readonly IConfiguration Configuration;
        private readonly IBrowsingContext Context;
        public HtmlParserService()
        {
            Configuration = AngleSharp.Configuration.Default;
            Context = BrowsingContext.New(Configuration);
        }

        public async Task<IList<Uri>> ExtractDomainsFromContentAsync(string htmlContent)
        {
            IList<Uri> links = await ExtractLinksFromContentAsync(htmlContent);
            return links.Select(link => link.Host).Distinct().ToUriList();
        }

        public async Task<IList<Uri>> ExtractLinksFromContentAsync(string htmlContent)
        {
            IDocument document = await Context.OpenAsync(req => req.Content(htmlContent));
            IHtmlCollection<IElement> linkElements = document.Links;

            return linkElements.Select(el => ((IHtmlAnchorElement)el).Href).ToUriList();
        }

        public async Task<IList<MetaTagModel>> ExtractMetaTagsFromContentAsync(string htmlContent)
        {
            IDocument document = await Context.OpenAsync(req => req.Content(htmlContent));
            IHtmlCollection<IElement> metaTagElements = document.QuerySelectorAll("meta");

            if (metaTagElements.Length == 0) return new List<MetaTagModel>();

            List<MetaTagModel> metas = metaTagElements
                .Where(p => Primitive.MetaFilter.Contains(p.GetAttribute("name")))
                .Select(p => new MetaTagModel() { Name = p.GetAttribute("name")!, Content = p.GetAttribute("content") ?? string.Empty})
                .Distinct(new MetaTagComparator())
                .ToList();

            return metas;
        }
    }
}
