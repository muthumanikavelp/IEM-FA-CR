using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.EOW.Models
{
    public interface DeclarationNoteRepository
    {
        IEnumerable<EOW_DeclarationNote> DisplayGrid();
        //IEnumerable<EOW_DeclarationNote> DisplayGridById(int id);
        EOW_DeclarationNote DisplayGridById(int id);
        IEnumerable<EOW_DeclarationNote> DisplayGrid(string delnotename,string docsubtype,string periodfrom,string periodto);
        IEnumerable<GetDoctype> GetDoctype();
        IEnumerable<GetDocsubtype> GetDocsubtype(int doctypegid);
        string InsertDecNote(EOW_DeclarationNote decnote);
        string UpdateDecnote(EOW_DeclarationNote decnote);
        string DeleteNote(int declnotegid);
        DataTable getdoctypecode(int doctypegid);
        DataTable getdocsubtypename(int doctypegid);
        
    }
}