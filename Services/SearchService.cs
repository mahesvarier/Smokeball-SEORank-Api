namespace Smokeball_SEORank_Api.Services;

public class SeoRankService : ISeoRankService
{
    public async Task<string> PerformSeoRankSearch(string keywords, string url)
    {
        return await Task.FromResult("1, 10, 33");
    }
}