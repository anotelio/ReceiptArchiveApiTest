namespace ReceiptArchiveApi.Contracts.Requests
{
    public class Wrequest
    {
        public int IdRequest { get; set; }

        public string TitleRequest { get; set; }

        public WrequestData WrequestData { get; set; }

        public IEnumerable<WrequestCol> WrequestCol { get; set; }
    }

    public class WrequestData
    {
        public string IdData { get; set; }
    }

    public class WrequestCol
    {
        public string IdCol { get; set; }
    }
}
