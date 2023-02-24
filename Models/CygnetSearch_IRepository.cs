using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Models
{
    public interface CygnetSearch_IRepository
    {       
        IEnumerable<CygnetSearchModel> SelectInvoiceSearch(CygnetSearchModel data); 
    }
}