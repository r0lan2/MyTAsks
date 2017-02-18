//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Text;
using System.Web.UI.WebControls;

namespace BigLamp.Extensions.WebControls
{
    public static class ButtonExtensions
    {

        /// <summary>
        /// Disables button on click/submit to avoid multiple submits.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="waitMessage"></param>
        /// <remarks></remarks>
        public static void DisableOnClick(this Button btn, string waitMessage)
        {
            DisabledOnClickAdd(btn, waitMessage, string.Empty, string.Empty);
        }
        /// <summary>
        /// Disables button on click/submit to avoid multiple submits.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="waitMessage"></param>
        /// <param name="disabledCssClass"></param>
        /// <remarks></remarks>
        public static void DisableOnClick(this Button btn, string waitMessage, string disabledCssClass)
        {
            DisabledOnClickAdd(btn, waitMessage, string.Empty, disabledCssClass);
        }
        /// <summary>
        /// Disables button on click/submit to avoid multiple submits and pop up warning asking user to confirm deleting.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="waitMessage"></param>
        /// <param name="deleteMessage"></param>
        /// <remarks></remarks>
        public static void DisableOnClickAndShowDeleteWarning(this Button btn, string waitMessage, string deleteMessage)
        {
            DisabledOnClickAdd(btn, waitMessage, deleteMessage, string.Empty);
        }
        /// <summary>
        /// Disables button on click/submit to avoid multiple submits and pop up warning asking user to confirm deleting.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="waitMessage"></param>
        /// <param name="deleteMessage"></param>
        /// <param name="disabledCssClass"></param>
        /// <remarks></remarks>
        public static void DisableOnClickAndShowDeleteWarning(this Button btn, string waitMessage, string deleteMessage, string disabledCssClass)
        {
            DisabledOnClickAdd(btn, waitMessage, deleteMessage, disabledCssClass);
        }
        private static void DisabledOnClickAdd(Button btn, string waitMessage, string deleteMessage, string disabledCssClass)
        {
            var sb = new StringBuilder();
            string validationGroup = btn.ValidationGroup;
            if (btn.CausesValidation)
            {
                sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
                if (string.IsNullOrEmpty(validationGroup))
                {
                    sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
                }
                else
                {
                    sb.Append("if (Page_ClientValidate('" + validationGroup + "') == false) { return false; }} ");
                }
            }

            if (!(string.IsNullOrEmpty(deleteMessage)))
            {
                sb.Append(string.Format("var confirmResult = confirm('{0}');", deleteMessage));
                sb.Append("if (confirmResult == false) { return false; }");
            }

            sb.Append("this.value = '");
            sb.Append(waitMessage);
            sb.Append("';");
            if (System.String.Compare(disabledCssClass, string.Empty, StringComparison.Ordinal) > 0)
            {
                sb.Append("this.className = '");
                sb.Append(disabledCssClass);
                sb.Append("';");
            }
            sb.Append("this.disabled = true;");
            sb.Append(btn.Parent.Page.ClientScript.GetPostBackEventReference(btn, string.Empty));
            sb.Append(";");
            btn.Attributes.Add("onclick", sb.ToString());
        }

    }
}
