using Newtonsoft.Json;


namespace TelegramReceiver.API.Models
{
    public class Video
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        [JsonProperty("file_unique_id")]
        public string FileUniqueId { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Duration { get; set; }
        public PhotoSize Thumb { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }
    }    
}