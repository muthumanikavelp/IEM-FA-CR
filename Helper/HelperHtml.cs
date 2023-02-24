using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IEM.Helper
{
    public static class HelperHtml
    {
        public static IHtmlString NoRecFound()
        {
            string htmlString = "<span>No Records found!</span>";
            return new HtmlString(htmlString);
        }

        public static IHtmlString NoAuditCheckList()
        {
            string htmlString = "<span>No Audit Check List found!</span>";
            return new HtmlString(htmlString);
        }

        public static string ConvertINRAmount(string Amount)
        {
            try
            {
                decimal amoumnt = 0;
                amoumnt = Convert.ToDecimal(Amount);
                CultureInfo cuInfo = new CultureInfo("hi-IN");
                string Isretrunstr;
                Isretrunstr = (amoumnt.ToString("C", cuInfo)).Remove(0, 2).Trim();
                return Isretrunstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}