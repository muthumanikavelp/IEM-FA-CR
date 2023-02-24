using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using IEM.App_Start;
namespace PrintCheque
{

    [NoDirectAccess]
    public class clsPrintSettings
    {
        private  StreamWriter rdr, reader;
        private string Bold_On = "_G";
        private string Bold_Off = "_H";
        private string Width_On = "_W1";
        private string Width_Off = "_W0";
        private int ColWidth = 96;
        private DataSet ds;
        private int Spliteline = 1000;
        private string splitexline="";
        ArrayList writelist = new ArrayList();
        ArrayList Startlist = new ArrayList();
        public clsPrintSettings(DataSet ds)
        {
            string FileLocation = ConfigurationManager.AppSettings["ChequePrintingNew"];
            rdr = new System.IO.StreamWriter(Path.Combine(FileLocation, "cheque.txt"));
            this.ds = ds;
        }
        public void Close()
        {
            rdr.Close();
        }

        // added by sugumar 2016-07-15
        public void PrintCheque()
        {
            string amtinword1, amtinword2;
            string Bankid = ds.Tables[3].Rows[0]["paybank_gid"].ToString();

            if (ds.Tables[1].Rows.Count> 0)

                if (ds.Tables[2].Rows.Count >0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        int RowHeight = Convert.ToInt32(ds.Tables[2].Rows[0]["chqpring_Rowheight"].ToString());
                        ColWidth = Convert.ToInt32(ds.Tables[2].Rows[0]["chqprint_Colwidth"].ToString());

                        amtinword1 = "";
                        amtinword2 = "";

                        for (int j = 1; j <= RowHeight; j++)
                        {
                            var rows = ds.Tables[2].AsEnumerable().Where(r => r.Field<int>("chqprint_Ylocation") == j);

                            if (rows.Count() == 0)
                            {
                                rdr.WriteLine("");
                            }
                            else
                            {
                                string linetxt, fldname, s, fldvalue, txt, Date, DateFormat;
                                int x, y, l, n;

                                linetxt = "";
                                fldname = "";
                                Date = ""; DateFormat = "";

                                foreach (DataRow row1 in rows)
                                {
                                    x = Int32.Parse(row1.ItemArray[2].ToString());
                                    y = Int32.Parse(row1.ItemArray[3].ToString());
                                    l = Int32.Parse(row1.ItemArray[5].ToString());

                                    if (linetxt.Length < x)
                                    {
                                        s = new string(' ', x - linetxt.Length - 1);
                                        linetxt += s;
                                    }
                                    else
                                    {
                                        if (x > 1) linetxt = linetxt.Substring(0, x - 1);
                                    }

                                    switch (row1.ItemArray[1].ToString())
                                    {
                                        case "Date":
                                            fldname = "CHQDate";
                                            break;
                                        case "Name":
                                            fldname = "Beneficiary";
                                            break;
                                        case "Amount":
                                            fldname = "PvAmount";
                                            break;
                                        case "AmtInWordsLine1":
                                            fldname = "AmountInWords";
                                            break;
                                        case "AmtInWordsLine2":
                                            fldname = "AmountInWords";
                                            break;
                                        case "RefNo":
                                            fldname = "PvNo";
                                            break;
                                    }

                                    fldvalue = ds.Tables[1].Rows[i][fldname].ToString();

                                    if (Bankid == "18")
                                    {
                                        if (fldname == "CHQDate")
                                        {
                                            Date = ds.Tables[1].Rows[i][fldname].ToString();
                                            if (Date != "")
                                            {
                                                DateFormat = string.Format(" {0} {1}  {2} {3}  {4} {5}  {6} {7}", Date[0], Date[1], Date[3], Date[4], Date[6], Date[7], Date[8], Date[9]);
                                                fldvalue = DateFormat;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fldname == "CHQDate")
                                        {
                                            Date = ds.Tables[1].Rows[i][fldname].ToString();
                                            if (Date != "")
                                            {
                                                DateFormat = string.Format(" {0} {1} {2} {3} {4} {5} {6} {7}", Date[0], Date[1], Date[3], Date[4], Date[6], Date[7], Date[8], Date[9]);
                                                fldvalue = DateFormat;
                                            }
                                        }
                                    }

                                    if (fldname == "AmountInWords")
                                    {
                                        if (amtinword1 == "")
                                        {
                                            fldvalue = fldvalue.Replace("  "," ");

                                            if (l >= fldvalue.Length)
                                            {
                                                amtinword1 = fldvalue;
                                                amtinword2 = "";
                                            }
                                            else
                                            {
                                                n = fldvalue.Substring(l - 1, 2).IndexOfAny(new char[] { ' ', '-' });
                                                if (n == -1)
                                                {
                                                    txt = fldvalue.Substring(0, l);
                                                    txt = new string(txt.ToCharArray().Reverse().ToArray());
                                                    n = txt.IndexOfAny(new char[] { ' ', '-' });

                                                    if (n != -1)
                                                    {
                                                        l = l - n;
                                                    }

                                                }
                                                amtinword1 = fldvalue.Substring(0, l);
                                                amtinword2 = fldvalue.Substring(l, fldvalue.Length - amtinword1.Length).Trim();
                                                fldvalue = amtinword1;
                                            }
                                        }
                                        else
                                        {
                                            fldvalue = amtinword2;
                                        }
                                    }

                                    fldvalue = fldvalue.Substring(0, (l < fldvalue.Length) ? l : fldvalue.Length);
                                    linetxt += fldvalue;

                                    if (linetxt.Length < ColWidth)
                                    {
                                        s = new string(' ', ColWidth - linetxt.Length);
                                        linetxt += s;
                                    }
                                    else
                                    {
                                        linetxt = linetxt.Substring(0, ColWidth);
                                    }
                                }

                                rdr.WriteLine(linetxt);
                            }
                        }
                        
                    }
                }
        }

        
      // end

        string cou1;
        private string GetLeftFormat(string Cont, int Lenght,int Strleng)
        {
           
            string space = "";
            int rloc = Lenght + Cont.Length;
            if (rloc <= ColWidth)
            {
                int nos;
                for (nos = 0; nos < Lenght; nos++)
                {
                    space += " ";
                    cou1 = space + Cont;
                }
                Cont = cou1;
            }
            else
            {
                rloc = ColWidth - Lenght;
                string str = Cont.Substring(0, rloc);
                int totcount = ColWidth - rloc;
                for (rloc = 0; rloc < totcount; rloc++)
                {
                    space += " ";
                    cou1 = space + str;
                }
                Cont = cou1;
            }
            if (Cont.Length > Strleng)
            {
                Cont = Cont.Substring(0, Strleng);
            }
            
            return (Cont);
        }

        private string GetLeftFormatAmount(string Cont, int Lenght)
        {
            writelist.Clear();
            string space = "";
            int rloc = Lenght + Cont.Length;
            if (rloc <= ColWidth)
            {
                int nos;
                for (nos = 0; nos < Lenght; nos++)
                {
                    space += " ";
                    cou1 = space + Cont;
                }
                Cont = cou1;
            }
            else
            {
                string[] array = Cont.Split(' ');
                string word = "";
                
                int word1 = Cont.Length - Lenght;

                int wordstart = ColWidth - Lenght;
                //word += Lenght;
                int rloc1 = Lenght + word1; 
                int rloc2 = ColWidth - Lenght;
                int totcount = ColWidth - rloc2;

                for (int k = 0; k < array.Length; k++)
                {
                    //int arraylent=0;
                    if (wordstart > word.Length)
                    {
                       // space += " ";
                        word += array[k] + " ";
    
                    }
                    else
                    {
                        splitexline += array[k] + " ";
                    }
                }

                    //if (rloc <= ColWidth)
                    //{
                    //    for (rloc = 0; rloc < rloc1; rloc++)
                    //    {
                    //        space += " ";
                    //        cou1 = space + word;
                    //    }
                    //}
                    //else
                    //{
                    //word = array[0] + " " + array[1] + " " + array[2] + " " + array[3];
                    //word1 = word.Length;
                    //for (rloc = 0; rloc < word1; rloc++)
                    //{
                    //    space += " ";
                    //    cou1 = space + word;
                    //}

                    //for (int RowHeight = 4; RowHeight < array.Length; RowHeight++)
                    //{
                    //    string amountword = array[RowHeight];
                    //    splitexline = splitexline + amountword;
                    //}
                //}
                Cont = word;
                writelist.Add(Cont);
                writelist.Add(splitexline);
                Spliteline = Spliteline + 1;
               
            }
            return (Cont);
        }

        private string GetFormatedText(string Cont, int Length) 
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                for (nos = 0; nos < rLoc; nos++)
                    Cont = Cont + " ";
            }
            return (Cont);
        }
        private string GetRightFormatedText(string Cont, int Length)
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                string space = "";
                for (nos = 0; nos < rLoc; nos++)
                    space += " ";
                Cont = space + Cont;
            }
            return (Cont);
        }
        private string GetCenterdFormatedText(string Cont, int Length)
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                string space = "";
                for (nos = 0; nos < rLoc; nos++)
                    space += " ";
                Cont = space + Cont;
            }
            return (Cont);
        }

        private string GetRupees(string Cont, int Length)
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                string space = "";
                for (nos = 0; nos < rLoc; nos++)
                    space += "*";
                Cont = space + Cont;
            }
            return (Cont);
        }

        public void PrintLine()
        {
            int i;
            string Lstr = "";
            for (i = 1; i <= ColWidth; i++)
            {
                Lstr = Lstr + "-";
            }
            rdr.WriteLine(Lstr);

        }

        public void PrintSpace(int Length)
        {
            int i;
            string Lstr = "";
            for (i = 1; i <= Length; i++)
            {
                Lstr = Lstr + " ";
            }
            rdr.Write(Lstr);

        }

        public void SkipLine(int LineNos)
        {
            int i;
            for (i = 1; i <= LineNos; i++)
            {
                rdr.WriteLine("");
            }
        }

        public void ReverseSkip(int LineNos)
        {
            int i;
            for (i = 1; i <= LineNos; i++)
            {
                rdr.WriteLine(Convert.ToChar(27) + "j" + Convert.ToChar(36 * LineNos));
            }
        }

        public void PrintBuffer()
        {
            string FileLocation = ConfigurationManager.AppSettings["ChequePrintingLocal"];
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.Refresh();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Verb = "print";
            process.StartInfo.FileName =FileLocation+"cheque.txt";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.Start();
        }

        public void Printdos()
        {
            string strFilePath = @"C:\WEB\cheque.txt";
            var psi = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = "c:\\temp\\"
                };
            using (var proc = Process.Start(psi))
            {
                var sIn = proc.StandardInput;

                using (var strm = File.OpenText(strFilePath))
                {
                    while (strm.Peek() != -1)
                    {
                        sIn.WriteLine(strm.ReadLine());
                    }
                }
                sIn.WriteLine(String.Format("# {0} run successfully. Exiting", strFilePath));
                //sIn.WriteLine("type cheque.txt >prn");
                sIn.WriteLine("EXIT");

                var results = proc.StandardOutput.ReadToEnd().Trim();
                string fmtStdOut = "<font face=courier size=0>{0}</font>";
             
                Console.Write(String.Format(fmtStdOut, results.Replace(System.Environment.NewLine, "<br>")));
               

            }

        }
    }
}
