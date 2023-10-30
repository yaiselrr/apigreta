using System.Net;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Wix.Extentions;
using Greta.BO.Wix.Interfaces;
using Greta.BO.Wix.Models;
using RestSharp;

namespace Greta.BO.Wix.Implementations;

public class MediaClient : BaseClient, IMediaClient
{
    private readonly RestClient _client;

    public MediaClient(RestClient client, WixOptions options) : base(options)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    // public async Task<string> Import(string refreshToken, LiteMedia media)
    public async Task<OnlineWixMedia> Import(string refreshToken, OnlineWixMedia media)
    {
        var request = new RestRequest($"/site-media/v1/files/import").AddJsonBody(new
        {

            mimeType = "image/jpeg",
            displayName = media.DisplayName,
            url = media.Url
         // parentFolderId ="25284aa06584441ea94338fdcfbaba12",
         // private = false,
        });

        request.AddOrUpdateParameter(await RefreshAccess(refreshToken).ConfigureAwait(false));

        var response = await _client.PostAsync<OnlineWixMediaResponse>(request);
        
        return response.OnlineWixMedia;
    }

}
