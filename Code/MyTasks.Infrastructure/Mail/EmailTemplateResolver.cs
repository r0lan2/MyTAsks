// <copyright file=EmailTemplateResolver.cs company=Justin Spradlin>
//      Copyright (c) 2011 Justin Spradlin.  ALL RIGHTS RESERVED
// </copyright>
// <product></product>
// <author>jspradlin</author>
// <created>Friday, April 29, 2011</created>
// <lastedit>Friday, April 29, 2011</lastedit>
namespace MyTasks.Infrastructure.Mail
{
    using RazorEngine;
    using RazorEngine.Templating;
    /// <summary>
    /// Summary description for TemplateResolver
    /// </summary>
    public class EmailTemplateResolver
    {
        #region Methods

        public static string GetEmailBody(string template,string templateKey, object model)
        {
           // var key = new NameOnlyTemplateKey("EmailTemplate", ResolveType.Global, null);
           // Engine.Razor.AddTemplate(key.Name, new LoadedTemplateSource(template));
           // Engine.Razor.Compile(key, model);
           // var body = Engine.Razor.RunCompile(key,null,null,model);
           //// var body = Razor.Parse(template, model);
            var body =
                Engine.Razor.RunCompile(template, templateKey, null, model);
            return body;

        }

        #endregion Methods
    }
}