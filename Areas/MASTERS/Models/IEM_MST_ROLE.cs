using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;
using System.IO;


namespace IEM.Areas.MASTERS.Models
{
    public class IEM_MST_ROLE : Iiem_mst_role
    {
        string result;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmn = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<iem_mst_role> getrole()
        {
            try
            {
                string strquery = "";
                List<iem_mst_role> objOrgType = new List<iem_mst_role>();
                iem_mst_role objModel;
                GetCon();
                //strquery = "Select role_gid,role_code,role_name,rolegroup_name from iem_mst_trole as a ";
                //strquery += " inner join iem_mst_trolegroup as b on a.role_rolegroup_gid=b.rolegroup_gid where role_isremoved='N'";
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_mst_trole_new", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Roleexport"] = dt;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    objModel = new iem_mst_role();
                    objModel.role_gid = Convert.ToInt32(dt.Rows[i]["role_gid"].ToString());
                    objModel.role_code = dt.Rows[i]["role_code"].ToString();
                    objModel.role_name = dt.Rows[i]["role_name"].ToString();
                    objModel.rolegroup_name = dt.Rows[i]["rolegroup_name"].ToString();
                    objModel.role_assignedto = dt.Rows[i]["role_assignedto"].ToString();
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }


        }
        public IEnumerable<Getrolegroup> Getrolegroupvl()
        {

            try
            {
                string strquery = "";
                List<Getrolegroup> objOrgType = new List<Getrolegroup>();
                Getrolegroup objModel;
                GetCon();
                DataTable dt = new DataTable();

                strquery = "select '0' as rolegroup_gid,'--Select Group--' as rolegroup_name union all select rolegroup_gid,rolegroup_name from iem_mst_trolegroup where rolegroup_isremoved='N'";

                cmd = new SqlCommand(strquery, con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    objModel = new Getrolegroup();

                    objModel.rolegroup_gid = Convert.ToInt32(dt.Rows[i]["rolegroup_gid"].ToString());
                    objModel.rolegroup_name = dt.Rows[i]["rolegroup_name"].ToString();

                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }
        }
        public iem_mst_role Iem_Mst_Role(string id)
        {
            string strquery;
            iem_mst_role objModel = new iem_mst_role();
            GetCon();
            DataTable dt = new DataTable();

            strquery = "Select role_gid,role_code,role_name,rolegroup_name,role_rolegroup_gid,role_assignedto from iem_mst_trole as a ";
            strquery += " inner join iem_mst_trolegroup as b on a.role_rolegroup_gid=b.rolegroup_gid where role_isremoved='N' and rolegroup_isremoved='N' and role_gid=" + id + "";
            cmd = new SqlCommand(strquery, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            HttpContext.Current.Session["role_gid"] = dt.Rows[0][0].ToString();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                objModel.role_gid = Convert.ToInt32(dt.Rows[0]["role_gid"].ToString());
                objModel.role_code = dt.Rows[0]["role_code"].ToString();
                objModel.role_name = dt.Rows[0]["role_name"].ToString();
                objModel.rolegroup_name = dt.Rows[0]["rolegroup_name"].ToString();
                objModel.role_rolegroup_gid = Convert.ToInt32(dt.Rows[0]["role_rolegroup_gid"]);
                objModel.role_assignedto = dt.Rows[0]["role_assignedto"].ToString();

            }
            string stquery1 = "select rolemenu_role_gid,rolemenu_menu_gid from iem_mst_trolemenu where rolemenu_role_gid=" + id + " and rolemenu_access='Y' and rolemenu_isremoved='N'";
            cmd = new SqlCommand(strquery, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            objModel.menu = menuView(Convert.ToInt32(id));

            return objModel;
        }

        public string update(iem_mst_role obj, string[] SelectedState, string[] unselectedrole)
        {
            int res = 0;
            string id = HttpContext.Current.Session["role_gid"].ToString();

            string result = "";

            string strquery = "";


            //strquery = "select * from iem_mst_trole where role_code='" + obj.role_code + "' or role_name='" + obj.role_name + "' and role_gid <> '" + id + "'";

            DataTable dt = new DataTable();


            strquery = "update iem_mst_trole set role_code='" + obj.role_code + "',role_name='" + obj.role_name + "',role_rolegroup_gid='" + obj.rolegroup_gid + "',role_assignedto='" + obj.role_assignedto + "' where role_gid='" + id + "'";
            GetCon();
            cmd = new SqlCommand(strquery, con);
            res = cmd.ExecuteNonQuery();


            strquery = "delete from iem_mst_trolemenu where rolemenu_role_gid='" + id + "'";
            GetCon();
            cmd = new SqlCommand(strquery, con);
            res = cmd.ExecuteNonQuery();

            if (res >= 0)
            {
                //string word = SelectedState[0].ToString();
                //string[] words = word.Split(',');

                //string[] words = SelectedState.Split(' ');


                foreach (string words in SelectedState)
                {

                    strquery = "";
                    strquery = "insert into iem_mst_trolemenu (rolemenu_role_gid,rolemenu_menu_gid,rolemenu_access) values ('" + id + "','" + words + "','Y')";

                    GetCon();
                    cmd = new SqlCommand(strquery, con);
                    da = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    //cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value = Convert.ToInt16(selctedstate);
                    //cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = 1;
                    //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDHOLIDAYSTATE";
                    //result = (string)cmd.ExecuteScalar();
                }



                foreach (string word in unselectedrole)
                {
                    strquery = "";
                    strquery = " insert into iem_mst_trolemenu (rolemenu_role_gid,rolemenu_menu_gid,rolemenu_access) values ('" + id + "','" + word + "','N')";
                    GetCon();
                    cmd = new SqlCommand(strquery, con);
                    da = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                }


                result = "Record Update Successfully";


            }
            //}
            //else
            //{
            //    result = "Please Select Another Role";
            //}

            return result;

        }

        public List<menu> menuView(int id)
        {
            List<menu> menu = new List<menu>();

            List<menu> all = new List<menu>();

            /*iem_mst_role obj = new iem_mst_role();
            obj.menu = new List<menu>();
            obj.RoleList = new List<iem_mst_role>();*/

            GetCon();

            DataTable dt = new DataTable();

            cmd = new SqlCommand("pr_iem_mst_trolemenu", con);
            cmd.Parameters.AddWithValue("@role_gid", id);
            cmd.Parameters.AddWithValue("@action", "SelectView");
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);

            da.Fill(dt);


            menu mn;

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {

                mn = new menu();

                mn.menu_gid = Convert.ToInt32(dt.Rows[i]["menu_gid"].ToString());

                mn.menu_name = dt.Rows[i]["menu_name"].ToString();

                mn.menu_parent_gid = Convert.ToInt32(dt.Rows[i]["menu_parent_gid"].ToString());

                mn.menu_link = dt.Rows[i]["menu_link"].ToString();
                if (dt.Rows[i]["rolemenu_access"].ToString() == "Y")
                    mn.menu_access = true;
                else
                    mn.menu_access = false;
                menu.Add(mn);
            }

            dt.Rows.Clear();

            all = menu.OrderBy(a => a.menu_parent_gid).ToList();

            return all;
        }

        public IEnumerable<iem_mst_role> searchrole(string rolecode, string rolename, string rolegroupname)
        {
            try
            {
                List<iem_mst_role> objOrgType = new List<iem_mst_role>();
                iem_mst_role objModel;

                GetCon();
                DataTable dt = new DataTable();

                string strquery = "";
                strquery = " Select role_gid,role_code,role_name,rolegroup_name from iem_mst_trole as a";
                strquery += " inner join iem_mst_trolegroup as b on a.role_rolegroup_gid=b.rolegroup_gid ";
                strquery += " where role_isremoved='N' and role_code like '%" + rolecode + "%' and role_name like '%" + rolename + "%' and rolegroup_name like '%" + rolegroupname + "%'";


                cmd = new SqlCommand(strquery, con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Roleexport"] = dt;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    objModel = new iem_mst_role();
                    objModel.role_gid = Convert.ToInt32(dt.Rows[i]["role_gid"].ToString());
                    objModel.role_code = dt.Rows[i]["role_code"].ToString();
                    objModel.role_name = dt.Rows[i]["role_name"].ToString();
                    objModel.rolegroup_name = dt.Rows[i]["rolegroup_name"].ToString();

                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string AddHolidayState(iem_mst_role sub, string[] SelectedState, string[] unselectedrole)
        {
            try
            {
                GetCon();

                int res = 0;

                string strquery = "";

                strquery = "select * from iem_mst_trole where role_code='" + sub.role_code + "' and role_name='" + sub.role_name + "' and role_isremoved='N'";

                DataTable dt = new DataTable();

                cmd = new SqlCommand(strquery, con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {

                    strquery = "";

                    strquery = "select * from iem_mst_trole as a inner join iem_mst_trolegroup as b on a.role_rolegroup_gid=b.rolegroup_gid where rolegroup_multiple_flag<>'N' and  role_rolegroup_gid='" + sub.rolegroup_gid + "' and role_isremoved='N'";

                    dt.Clear();

                    cmd = new SqlCommand(strquery, con);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {

                        strquery = "";
                        strquery = "insert into iem_mst_trole (role_code,role_name,role_rolegroup_gid,role_insert_by,role_insert_date,role_assignedto) values ('" + sub.role_code + "','" + sub.role_name + "','" + sub.rolegroup_gid + "','" + cmn.GetLoginUserGid() + "',getdate(),'"+ sub.role_assignedto +"')";

                        GetCon();

                        cmd = new SqlCommand(strquery, con);
                        da = new SqlDataAdapter(cmd);

                        res = cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        result = "Please Select Another Role";
                    }

                    if (res != 0)
                    {

                        strquery = "";

                        strquery = "select role_gid from iem_mst_trole where role_code='" + sub.role_code + "' and role_name='" + sub.role_name + "'";

                        cmd = new SqlCommand(strquery, con);

                        da = new SqlDataAdapter(cmd);

                        da.Fill(dt);

                        string reader = "";

                        reader = cmd.ExecuteScalar().ToString();

                        //string word = SelectedState[0].ToString();
                        //string[] words = word.Split(',');

                        //string[] words = SelectedState.Split(' ');


                        foreach (string words in SelectedState)
                        {

                            strquery = "";
                            strquery = "insert into iem_mst_trolemenu (rolemenu_role_gid,rolemenu_menu_gid,rolemenu_access) values ('" + reader + "','" + words + "','Y')";

                            GetCon();
                            cmd = new SqlCommand(strquery, con);
                            da = new SqlDataAdapter(cmd);
                            cmd.ExecuteNonQuery();
                            //cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value = Convert.ToInt16(selctedstate);
                            //cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = 1;
                            //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDHOLIDAYSTATE";
                            //result = (string)cmd.ExecuteScalar();
                        }



                        foreach (string unselectbox in unselectedrole)
                        {
                            strquery = "";
                            strquery = " insert into iem_mst_trolemenu (rolemenu_role_gid,rolemenu_menu_gid,rolemenu_access) values ('" + reader + "','" + unselectbox + "','N')";
                            GetCon();
                            cmd = new SqlCommand(strquery, con);
                            da = new SqlDataAdapter(cmd);
                            cmd.ExecuteNonQuery();
                        }

                        result = "Successfully Saved";

                    }

                }
                else
                {
                    result = "Duplicate Record !";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;

        }

        public iem_mst_role menu(int id = 0)
        {
            List<menu> menu = new List<menu>();

            List<menu> all = new List<menu>();

            iem_mst_role obj = new iem_mst_role();
            obj.menu = new List<menu>();
            obj.RoleList = new List<iem_mst_role>();

            GetCon();

            DataTable dt = new DataTable();

            cmd = new SqlCommand("pr_iem_mst_trolemenu", con);
            cmd.Parameters.AddWithValue("@role_gid", id);
            cmd.Parameters.AddWithValue("@action", "select");
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);

            da.Fill(dt);
           

            menu mn;

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {

                mn = new menu();

                mn.menu_gid = Convert.ToInt32(dt.Rows[i]["menu_gid"].ToString());

                mn.menu_name = dt.Rows[i]["menu_name"].ToString();

                mn.menu_parent_gid = Convert.ToInt32(dt.Rows[i]["menu_parent_gid"].ToString());

                mn.menu_link = dt.Rows[i]["menu_link"].ToString();

                obj.menu.Add(mn);
            }

            dt.Rows.Clear();

            all = obj.menu.OrderBy(a => a.menu_parent_gid).ToList();

            //all = obj.OrderBy(a => a.menu_parent_gid).ToList();
            obj.RoleList = getrole().ToList();

            return obj;

            /*
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                all = dc.SiteMenus.OrderBy(a => a.ParentMenuID).ToList();
            }
             */


        }


        //public IEnumerable<iem_mst_role> getroledelete(int id)
        //{
        //    try
        //    {

        //        List<iem_mst_role> role = new List<iem_mst_role>();
        //        iem_mst_role rol;

        //        DataTable dt = new DataTable();
        //        string strquery = "";

        //        strquery = "select roleemployee_role_gid from iem_mst_trole as a inner join iem_mst_troleemployee as b on a.role_gid=b.roleemployee_role_gid where roleemployee_role_gid='" + id + "'";

        //        GetCon();

        //        cmd = new SqlCommand(strquery, con);

        //        da = new SqlDataAdapter(cmd);

        //        da.Fill(dt);

        //        if (dt.Rows.Count == 0)
        //        {

        //            strquery = "update iem_mst_trole set role_isremoved='Y' where role_gid='" + id + "'";

        //            GetCon();



        //            cmd = new SqlCommand(strquery, con);
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                rol = new iem_mst_role();
        //                rol.role_gid = Convert.ToInt32(dt.Rows[i]["role_gid"].ToString());
        //                rol.role_code = dt.Rows[i]["role_code"].ToString();
        //                rol.role_name = dt.Rows[i]["role_name"].ToString();
        //                rol.rolegroup_name = dt.Rows[i]["rolegroup_name"].ToString();

        //                role.Add(rol);
        //            }


        //        }
        //        else
        //        {
                   
        //        }
        //        return role;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public string getroledelete(int id)
        {
           try
           {

               List<iem_mst_role> role = new List<iem_mst_role>();
               iem_mst_role rol;

               DataTable dt = new DataTable();
               string strquery = "";

               strquery = "select roleemployee_role_gid from iem_mst_trole as a ";
               strquery += " inner join iem_mst_troleemployee as b on a.role_gid=b.roleemployee_role_gid ";
               strquery += " inner join iem_mst_temployee  on employee_gid=b.roleemployee_employee_gid ";
               strquery += " where b.roleemployee_role_gid='" + id + "' and b.roleemployee_isremoved='N' and role_isremoved='N' and employee_isremoved='N' ";
               GetCon();

               cmd = new SqlCommand(strquery, con);

               da = new SqlDataAdapter(cmd);

               da.Fill(dt);

               if (dt.Rows.Count == 0)
               {

                   strquery = "update iem_mst_trole set role_isremoved='Y' where role_gid='" + id + "'";

                   GetCon();



                   cmd = new SqlCommand(strquery, con);
                   da = new SqlDataAdapter(cmd);
                   da.Fill(dt);

                   for (int i = 0; i < dt.Rows.Count; i++)
                   {
                       rol = new iem_mst_role();
                       rol.role_gid = Convert.ToInt32(dt.Rows[i]["role_gid"].ToString());
                       rol.role_code = dt.Rows[i]["role_code"].ToString();
                       rol.role_name = dt.Rows[i]["role_name"].ToString();
                       rol.rolegroup_name = dt.Rows[i]["rolegroup_name"].ToString();
                       role.Add(rol);
                   }


               }
               else
               {
                   return "2";
               }
              
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return "success";
        }
            //  throw new NotImplementedException();


       
        
    }
}

