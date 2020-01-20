using Newtonsoft.Json;


namespace TelegramReceiver.API.Model
{
    class PhotoSize
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        [JsonProperty("file_unique_id")]
        public string FileUniqueId { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }
    }
}