<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEM.Areas.EOW.Models.SupClassificationModel>" %>

<script runat="server">

    TreeNode nodeToAdd = null, ParentNodeTofind = null;
    SortedList parrentnode = new SortedList();
    SortedList newparent = new SortedList();
    ArrayList parentnode = new ArrayList();
    TreeNode node, parent = null;
     TreeNode newnode;
    System.Data.DataTable dt = new System.Data.DataTable();
    IEM.Areas.EOW.Models.RoleMasterModel role = new IEM.Areas.EOW.Models.RoleMasterModel();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        TreeView1.Nodes.Clear();
        parentnode.Clear();
        findparent();
        addparent();
          //Arivu();
        
    }
    private void findparent()
    {
        TreeNode node;
        string strConnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(strConnString);
        if (con.State == System.Data.ConnectionState.Closed) con.Open();
        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        cmd.CommandText = "SELECT menu_gid,menu_code,menu_parent_gid FROM iem_mst_tmenu WHERE menu_parent_gid=0 AND menu_isremoved='N'";
        cmd.Connection = con;
        System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
        sda.SelectCommand = cmd;
        System.Data.DataTable dt = new System.Data.DataTable();
        sda.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            node = new TreeNode(dt.Rows[0][1].ToString(), dt.Rows[0][0].ToString());
            CheckBox chk = new CheckBox();
            parentnode.Add(node);
            if (dt.Rows[0][2].ToString() == "0")
            {
                return;
            }
            else
            {
                findparent();
            }
        }

        dt.Rows.Clear();
        con.Close();
    }

    private void addchild(TreeNode node)
    {
        TreeNode newnode;
        TreeNode parent;
        string strConnString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(strConnString);
        if (con.State == System.Data.ConnectionState.Closed) con.Open();
        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        cmd.CommandText = "SELECT menu_gid,menu_code,menu_parent_gid FROM iem_mst_tmenu WHERE menu_parent_gid='" + node.Value + "' AND menu_isremoved='N'";
        cmd.Connection = con;
        System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
        sda.SelectCommand = cmd;
        System.Data.DataTable dt = new System.Data.DataTable();
        sda.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                newnode = new TreeNode(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString());
               
                node.ChildNodes.Add(newnode);
                addchild(newnode);
            }
        }

        dt.Rows.Clear();
        con.Close();
    }
    protected void addparent()
    {

        for (int i = parentnode.Count - 1; i >= 0; i--)
        {
            node = (TreeNode)parentnode[i];
            //node.CollapseAll();
            //node.SelectAction = TreeNodeSelectAction.Select;
            if (i == parentnode.Count - 1)
            {
                TreeView1.Nodes.Clear();
                TreeView1.Nodes.Add(node);
                if (parentnode.Count != 1)
                    parent = TreeView1.FindNode(node.Value);
                    node.Text = string.Format("<span style='color:green'>{0}</span>", node.Text);
                    
                    addchild(node);
                   
            }
            else
            {
                node.Text = string.Format("<span style='color:green'>{0}</span>", node.Text);
                parent.ChildNodes.Add(node);
                parent = node;               
                parent.ChildNodes.Add(node);
                parent = node;
                if (i == 0)
                addchild(node);
            }
        }
    }
</script>



<style>
    .tblstyle tr td {
        padding: 10px;
    }

</style>
<form id="form1" runat="server">
<div class="panel panel-default">
    <table class="tblstyle">
        <tr>
            <td >
                <div>
                    Role Code
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="input-append">
                    <input type="text"
                           id="RoleCode"
                           name="txtEmpcode"
                           <%--value="@ViewBag.filter"--%>
                           class="form-control"
                           style="display: block"
                           placeholder="Enter RoleCode" />
                </div>
            </td>
        </tr>
        <tr>
            <td >
                <div>
                    Role Name
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="input-append">
                    <input type="text"
                           id="RoleName"
                           name="txtEmpcode"
                          <%-- value="@ViewBag.filter"--%>
                           class="form-control"
                           style="display: block"
                           placeholder="Enter RoleName" />
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="treebind" runat="server">
    <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ShowExpandCollapse="true" EnableClientScript="true">
        
    </asp:TreeView>
</div>
 <script>
    
     
 </script>
        
</form>
