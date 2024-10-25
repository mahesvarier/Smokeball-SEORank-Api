using System;

namespace Smokeball_SEORank_Api.Services;

public interface ISeoRankService
{
    Task<string> PerformSeoRankSearch(string keywords, string url);
}