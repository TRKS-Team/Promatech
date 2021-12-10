using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Promatech
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings SerializeSettings = new JsonSerializerSettings();

        public static T JsonDeserialize<T>(this object source)
        {
            return source.ToJsonString().JsonDeserialize<T>();
        }

        public static string ToJsonString<T>(this T source)
        {
            return JsonConvert.SerializeObject(source, SerializeSettings);
        }

        public static T JsonDeserialize<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source, SerializeSettings);
        }

        public static string ToJsonString<T>(this T source, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(source, settings);
        }

        public static T JsonDeserialize<T>(this string source, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(source, settings);
        }

    }
  public static class HttpClientExtensions
  {
      public static async Task<T> DownloadJsonAsync<T>(this HttpClient hc, string url)
      {
          var msg = new HttpRequestMessage(HttpMethod.Get, url);
          var data = new HttpResponseMessage();
          try
          {
              data = await hc.SendAsync(msg);
              data.EnsureSuccessStatusCode();

          }
          catch (WebException e)
          {
              // TODO 异常处理
          }
          var stream = await data.Content.ReadAsStreamAsync();
          using var sr = new StreamReader(stream);
          using var reader = new JsonTextReader(sr);
          var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
          return serializer.Deserialize<T>(reader);

      }
    public static async Task DownloadAsync(
      this HttpClient client,
      string requestUri,
      string destination,
      IProgress<double>? progress = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(destination)));
      using (FileStream fs = File.OpenWrite(destination))
        await client.DownloadAsync(requestUri, (Stream) fs, progress, cancellationToken).ConfigureAwait(false);
    }

    public static async Task DownloadAsync(
      this HttpClient client,
      string requestUri,
      Stream destination,
      IProgress<double>? progress = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      using (HttpResponseMessage response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
      {
        long? contentLength = response.Content.Headers.ContentLength;
        response.EnsureSuccessStatusCode();
        using (Stream download = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
          if (progress == null || !contentLength.HasValue)
          {
            await download.CopyToAsync(destination).ConfigureAwait(false);
          }
          else
          {
            await download.CopyToAsync(destination, 81920, (IProgress<long>) new Progress<long>((Action<long>) (totalBytes => progress.Report((double) totalBytes / (double) contentLength.Value))), cancellationToken).ConfigureAwait(false);
            progress.Report(1.0);
          }
        }
      }
    }

    public static async Task CopyToAsync(
      this Stream source,
      Stream destination,
      int bufferSize = 8192,
      IProgress<long>? progress = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (!source.CanRead)
        throw new ArgumentException("Has to be readable", nameof (source));
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      if (!destination.CanWrite)
        throw new ArgumentException("Has to be writable", nameof (destination));
      byte[] buffer = bufferSize >= 0 ? new byte[bufferSize] : throw new ArgumentOutOfRangeException(nameof (bufferSize));
      long totalBytesRead = 0;
      while (true)
      {
        IProgress<long> progress1;
        do
        {
          int bytesRead;
          if ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
          {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
            totalBytesRead += (long) bytesRead;
            progress1 = progress;
          }
          else
            goto label_15;
        }
        while (progress1 == null);
        progress1.Report(totalBytesRead);
      }
label_15:;
    }

    public static async Task<string> PostStringAsync(
      this HttpClient client,
      string address,
      HttpContent data)
    {
      HttpResponseMessage httpResponseMessage = await client.PostAsync(address, data).ConfigureAwait(false);
      httpResponseMessage.EnsureSuccessStatusCode();
      return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    public static Task<string> PostStringAsync(
      this HttpClient client,
      string address,
      Dictionary<string, string> data)
    {
      return client.PostStringAsync(address, (HttpContent) new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) data));
    }

    public static async Task<T> GetJsonAsync<T>(this HttpClient client, string address) => (await client.GetStringAsync(address).ConfigureAwait(false)).JsonDeserialize<T>();

    public static async Task<T> PostJsonAsync<T>(
      this HttpClient client,
      string address,
      HttpContent data)
    {
      HttpResponseMessage httpResponseMessage = await client.PostAsync(address, data).ConfigureAwait(false);
      httpResponseMessage.EnsureSuccessStatusCode();
      return (await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)).JsonDeserialize<T>();
    }

    public static Task<T> PostJsonAsync<T>(
      this HttpClient client,
      string address,
      Dictionary<string, string> data)
    {
      return client.PostJsonAsync<T>(address, (HttpContent) new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) data));
    }
  }
}
