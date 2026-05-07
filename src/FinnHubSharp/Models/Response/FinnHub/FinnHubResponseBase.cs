namespace FinnHubSharp.Models.Response.FinnHub
{
    public class FinnHubResponseBase
    {
        public int ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseMessage { get; set; }
    }
}