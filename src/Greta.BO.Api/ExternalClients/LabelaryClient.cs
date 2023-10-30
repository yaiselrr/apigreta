using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.ExternalClients;

public class LabelaryClient: IDisposable
{
    private readonly ILogger<LabelaryClient> _logger;
    private readonly string _apiEndpoint;
    private readonly HttpClient _httpClient;

    public LabelaryClient(
        ILogger<LabelaryClient> logger = default,
        string apiEndpoint = "http://api.labelary.com/v1/printers")
    {
        _logger = logger;
        _apiEndpoint = apiEndpoint;
        _httpClient = new HttpClient();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(true);  // Violates rule
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient?.Dispose();
        }
    }

    public async Task<byte[]> GetPreviewAsync(
        string zplData,
        PrintDensity printDensity,
        LabelSize labelSize)
    {
        var dpi = printDensity.ToString().Substring(2);
        var zpl = Encoding.UTF8.GetBytes(zplData);

        using var byteContent = new ByteArrayContent(zpl);
        using (var response = await _httpClient.PostAsync($"{_apiEndpoint}/{dpi}/labels/{labelSize.WidthInInch}x{labelSize.HeightInInch}/0/", byteContent))
        {
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError($"Cannot get a preview {response.StatusCode}");
                return Array.Empty<byte>();
            }

            var data = await response.Content.ReadAsByteArrayAsync();
            return data;
        }
    }
    
    public enum PrintDensity
    {
        /// <summary>
        /// 152 dpi
        /// </summary>
        PD6dpmm,

        /// <summary>
        /// 203 dpi
        /// </summary>
        PD8dpmm,

        /// <summary>
        /// 300 dpi
        /// </summary>
        PD12dpmm,

        /// <summary>
        /// 600 dpi
        /// </summary>
        PD24dpmm
    }
    
    public class LabelSize
    {
        private readonly double _width;
        private readonly double _height;
        private readonly Measure _measure;
        private readonly double _millimeterToInch = 25.4;

        public LabelSize(double width, double height, Measure measure)
        {
            _width = width;
            _height = height;
            _measure = measure;
        }

        public double WidthInInch
        {
            get
            {
                if (_measure == Measure.Inch)
                {
                    return _width;
                }

                return Math.Round(_width / _millimeterToInch, 0);
            }
        }

        public double HeightInInch
        {
            get
            {
                if (_measure == Measure.Inch)
                {
                    return _height;
                }

                return Math.Round(_height / _millimeterToInch, 0);
            }
        }
    }
    
    public enum Measure
    {
        /// <summary>
        /// Millimeter
        /// </summary>
        Millimeter,

        /// <summary>
        /// Inch
        /// </summary>
        Inch
    }
}