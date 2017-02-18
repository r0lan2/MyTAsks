//INSTANT C# NOTE: Formerly VB project-level imports:

namespace BigLamp.Extensions.HttpRequest
{
    public static class HttpRequestExtension
    {
        public static bool IsNorwegian(this System.Web.HttpRequest request)
        {
            return request.UserLanguages != null && (request.UserLanguages[0].Contains("no") || request.UserLanguages[0].Contains("nb"));
        }

        public static bool IsSpanish(this System.Web.HttpRequest request)
        {
            return request.UserLanguages != null && request.UserLanguages[0].Contains("es");
        }
    }
}
