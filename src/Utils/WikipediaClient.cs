
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Wikirace.Data;

namespace Wikirace.Utils;


public class WikipediaSearchResult {
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}



public class WikipediaClient {
    private readonly HttpClient _httpClient;

    public WikipediaClient() {
        _httpClient = new HttpClient();
    }

    public async Task<List<WikipediaSearchResult>> SearchAsync(string query) {
        var url = new Uri("https://en.wikipedia.org/w/api.php")
            .AddQuery("action", "query")
            .AddQuery("format", "json")
            .AddQuery("generator", "prefixsearch")
            .AddQuery("prop", "pageprops|pageimages|description")
            .AddQuery("ppprop", "displaytitle")
            .AddQuery("piprop", "thumbnail")
            .AddQuery("pithumbsize", "75")
            .AddQuery("pilimit", "4")
            .AddQuery("gpssearch", query)
            .AddQuery("gpsnamespace", "0")
            .AddQuery("gpslimit", "4");

        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonObject>(json);
        var pages = result?["query"]?["pages"];

        if (pages == null) {
            return [];
        }

        var searchResults = new List<WikipediaSearchResult>();

        var sortedPages = pages.AsObject().OrderBy(p => p.Value!["index"]?.GetValue<int>());


        foreach (var page in sortedPages) {
            var title = page.Value?["title"]?.GetValue<string>();
            var description = page.Value?["description"]?.GetValue<string>();
            var imageUrl = page.Value?["thumbnail"]?["source"]?.GetValue<string>();

            searchResults.Add(new WikipediaSearchResult {
                Title = title,
                Description = description,
                ImageUrl = imageUrl
            });
        }

        return searchResults;
    }

}



public static class WikipediaClientExt {
    public static IServiceCollection AddWikipediaRepo(this IServiceCollection services) {
        services.AddSingleton<WikipediaClient>();
        return services;
    }
}
