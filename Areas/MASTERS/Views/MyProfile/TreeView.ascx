<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<script runat="server">

    TreeNode nodeToAdd = null, ParentNodeTofind = null;
    SortedList parrentnode = new SortedList();
    SortedList newparent = new SortedList();
    ArrayList parentnode = new ArrayList();
    TreeNode node, parent = null;
    TreeNode newnode;
    System.Data.DataTable dt = new System.Data.DataTable();
    //ASMS_TEMP.Models.RoleMasterModel role = new ASMS_TEMP.Models.RoleMasterModel();
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

        //cmd.CommandText = "SELECT menu_gid,menu_code,menu_parent_gid FROM iem_mst_tmenu WHERE menu_parent_gid=0 AND menu_isremoved='N'";
        cmd.CommandText = "SELECT employee_gid,employee_name,employee_supervisor FROM iem_mst_temployee where employee_supervisor=0";
        cmd.Connection = con;
        System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.SelectCommand.CommandTimeout = 0;
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
        if (con.State == System.Data.ConnectionState.Closed) 
        con.Open();
        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        cmd.CommandText = "SELECT employee_gid,employee_name,employee_supervisor FROM iem_mst_temployee WHERE employee_gid='" + node.Value + "'";
        //cmd.CommandText = "SELECT menu_gid,menu_code,menu_parent_gid FROM iem_mst_tmenu WHERE menu_parent_gid='" + node.Value + "' AND menu_isremoved='N'";
        cmd.Connection = con;
        System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.SelectCommand.CommandTimeout = 0;
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
    protected void Arivu()
    {
        TreeNode node, node1, node2;
        TreeView1.Nodes.Clear();
        node = new TreeNode();
        node.Text = "Parent";
        node.Value = "Parent";

        TreeView1.Nodes.Add(node);

        node1 = new TreeNode();
        node1.Text = "Child";
        node1.Value = "Child";
        node = TreeView1.FindNode("Parent");
        node.ChildNodes.Add(node1);
        node2 = new TreeNode();
        node2.Text = "Grand Child";
        node2.Value = "Grand Child";
        node1.ChildNodes.Add(node2);
        node1.Collapse();
        return;
    }

</script>

<form id="form1" runat="server">

    <div id="treebind" runat="server">
        <asp:TreeView ID="TreeView1" runat="server" ShowExpandCollapse="true" EnableClientScript="true">
        </asp:TreeView>
    </div>

</form>
