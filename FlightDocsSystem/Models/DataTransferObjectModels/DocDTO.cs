using System.Text.Json.Serialization;

namespace FlightDocsSystem.Models.DataTransferObjectModels;
public class DocDTO
{
    [JsonIgnore]
    public int? Id { get; set; }
    public string? DocTitle { get; set; }
    public string? DocumentVer { get; set; }
    public string? Creator { get; set; }
    public DateTime CreateDate { get; set; }
    public string? Note { get; set; }
    public string? FileName { get; set; }
}