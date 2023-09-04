using System;

namespace HelpCheck_API.Dtos
{
    public class FileDetails
    {
        public Guid ID { get; set; }
        public int No { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTimeOffset? FileDate { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
    }
}