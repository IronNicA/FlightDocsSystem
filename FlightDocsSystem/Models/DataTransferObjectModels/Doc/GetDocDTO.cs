namespace FlightDocsSystem.Models.DataTransferObjectModels.Doc
{
    public class GetDocDTO
    {
        public int? Id { get; set; }
        public string? DocTitle { get; set; }
        public float? DocumentVer { get; set; }
        public string? DocType { get; set; }
        public string? Creator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
