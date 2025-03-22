using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Square_Note.Services
{
    /// <summary>
    /// Mise à jour fournies par Les Majesticiels
    /// </summary>
    class MajesticielsUpdater
    {
        private static MajesticielsUpdater? _Instance;

        private static readonly string UpdaterUrl = "https://www.lesmajesticiels.org/api/update";
        private static readonly string ProductSlug = "squarenote";
        private static readonly string PlatformSlug = "win64";
        private static readonly string ChannelSlug = "beta";

        public delegate void UpdateAvailableEventHandler(object sender, MajesticielsUpdate e);
        public event UpdateAvailableEventHandler? UpdateAvailable;

        public MajesticielsUpdate? UpdateData;
        public string? UpdateLocation;

        public static MajesticielsUpdater Instance
        {
            get
            {
                _Instance ??= new MajesticielsUpdater();
                return _Instance;
            }
        }

        public async void CheckUpdatesNow()
        {
            if(UpdateData is not null)
            {
                UpdateAvailable?.Invoke(this, UpdateData);
                return;
            }

            HttpClient client = new();
            string? CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            if (CurrentVersion is null) return;

            try
            {
                MajesticielsUpdate? response = await client.GetFromJsonAsync<MajesticielsUpdate>(
                    $"{UpdaterUrl}?product={ProductSlug}&platform={PlatformSlug}&channel={ChannelSlug}&current_version={CurrentVersion}"
                );

                Console.WriteLine(response);
                if (response == null) return;
                UpdateData = response;

                // Informer les suiveurs que la mise à jour est disponible
                if (response.UpdateAvailable)
                {
                    UpdateAvailable?.Invoke(this, response);
                }

                // Télécharger la nouvelle version dans le répertoire temporaire
                if (response.NewVersion?.DownloadUrl is not null)
                {
                    var fileResponse = await client.GetStreamAsync(response.NewVersion.DownloadUrl);
                    var fileLocation = $"{Path.GetTempPath()}{response.NewVersion.DownloadUrl[(response.NewVersion.DownloadUrl.LastIndexOf('/') + 1)..]}";

                    using var fs = new FileStream(fileLocation, FileMode.Create);
                    await fileResponse.CopyToAsync(fs);

                    UpdateLocation = fileLocation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class MajesticielsUpdate
    {
        [JsonPropertyName("updateAvailable")]
        public bool UpdateAvailable { get; set; }

        [JsonPropertyName("isProductDiscontinued")]
        public bool IsProductDiscontinued { get; set; }

        [JsonPropertyName("newVersion")]
        public MajesticielsUpdateVersion? NewVersion { get; set; }
    }

    public class MajesticielsUpdateVersion
    {
        [JsonPropertyName("number")]
        public required string VersionNumber { get; set; }

        [JsonPropertyName("releaseDate")]
        public required string ReleaseDate { get; set; }

        [JsonPropertyName("releaseNotes")]
        public required MajesticielsUpdateVersionReleaseNotes ReleaseNotes { get; set; }

        [JsonPropertyName("downloadUrl")]
        public required string DownloadUrl { get; set; }

        [JsonPropertyName("updateArgs")]
        public required string UpdateArgs { get; set; }
    }

    public class MajesticielsUpdateVersionReleaseNotes
    {
        [JsonPropertyName("added")]
        public required string[] AddedNotes { get; set; }

        [JsonPropertyName("fixed")]
        public required string[] FixedNotes { get; set; }

        [JsonPropertyName("removed")]
        public required string[] RemovedNotes { get; set; }

        [JsonPropertyName("improved")]
        public required string[] ImprovedNotes { get; set; }
    }
}
