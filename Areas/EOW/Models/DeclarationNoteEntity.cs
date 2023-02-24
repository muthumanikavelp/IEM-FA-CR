using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class DeclarationNoteEntity
    {
    }
    public class EOW_DeclarationNote
    {
        public int declnote_gid { get; set; }
        public string declnote_docsubtype_gid { get; set; }
        //public string declnote_at { get; set; }
        public string declnote_onsubmission { get; set; }
        public string declnote_approval { get; set; }
        public string declnote_desc { get; set; }
        public string declnote_active { get; set; }
        public int declnote_insert_by { get; set; }
        public string declnote_insert_date { get; set; }
        public int declnote_update_by { get; set; }
        public string declnote_update_date { get; set; }
        public int selecteddoctype_gid { get; set; }
        public int selecteddocsubtype_gid { get; set; }
        public string declnote_name { get; set; }
        public string declnote_periodfrom { get; set; }
        public string declnote_periodto { get; set; }
        public int declnote_d { get; set; }
        //doctype
        public int doctype_gid { get; set; }
        public string doctype_code { get; set; }
        public string doctype_name { get; set; }

        //docsubtype
        public int docsubtype_gid { get; set; }
        public string docsubtype_code { get; set; }
        public string docsubtype_name { get; set; }

        public SelectList GetDoctype { get; set; }
        public SelectList GetDocsubtype { get; set; }

    }
    public class GetDoctype
    {
        public int doctypegid { get; set; }
        public string doctypecode { get; set; }
        public string doctypename { get; set; }
    }
    public class GetDocsubtype
    {
        public int docsubtypegid { get; set; }
        public string doctypecode { get; set; }
        public string docsubtypename { get; set; }
    }
}