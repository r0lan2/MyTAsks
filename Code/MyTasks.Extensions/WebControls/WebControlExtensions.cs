//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace BigLamp.Extensions.WebControls
{
    public static class WebControlExtensions
    {
        /// <summary>
        /// Pop up warning asking user to confirm deleting.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="deleteMessage"></param>
        /// <remarks></remarks>
        public static void ShowDeleteWarning(this WebControl btn, string deleteMessage)
        {
            AddDeleteConfirmation(btn, deleteMessage);
        }
        private static void AddDeleteConfirmation(WebControl btn, string deleteMessage)
        {
            string js = string.Format("return confirm('{0}');", deleteMessage);

            if ((!(btn is Button)) & (!(btn is ImageButton)))
            {
                throw new NotSupportedException("Confirmation pop up can only be added to Button and ImageButton");
            }

            if (btn.Attributes[HtmlTextWriterAttribute.Onclick.ToString()] == null)
            {
                btn.Attributes.Add(HtmlTextWriterAttribute.Onclick.ToString(), js);
            }
            else
            {
                btn.Attributes[HtmlTextWriterAttribute.Onclick.ToString()] = btn.Attributes[HtmlTextWriterAttribute.Onclick.ToString()] + ";" + js;
            }

        }
    }
}
