//INSTANT C# NOTE: Formerly VB project-level imports:

namespace BigLamp.Extensions.ResourceManager
{
    public static class ResourceManagerExtensions
    {
        /// <summary>
        /// Gets localized string or returns name if localized key is not found.
        /// </summary>
        /// <param name="resourceManager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetStringOrDefault(this System.Resources.ResourceManager resourceManager, string name)
        {
            var value = resourceManager.GetString(name);
            if (!(string.IsNullOrEmpty(value)))
            {
                return value;
            }
            return name;
        }
    }
}
