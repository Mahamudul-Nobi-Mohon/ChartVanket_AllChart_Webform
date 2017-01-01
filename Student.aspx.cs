using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

public partial class Student : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetChartTypes();
            GetChartData();
        }
    }

    private void GetChartTypes()
    {
        foreach (int chartType in Enum.GetValues(typeof(SeriesChartType)))
        {
            ListItem li = new ListItem(Enum.GetName(typeof(SeriesChartType),chartType),chartType.ToString());
            DropDownList1.Items.Add(li);
        }
    }

    void GetChartData()
    {
        string cs = ConfigurationManager.ConnectionStrings["Chart"].ConnectionString;
        Series series = Chart1.Series["Series1"];
        using (SqlConnection con = new SqlConnection(cs))
        {
             SqlCommand cmd = new SqlCommand("Select studentName,TotalMarks from Students",con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                series.Points.AddXY(reader["StudentName"].ToString(), reader["TotalMarks"]);
            }
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetChartData();
        Chart1.Series["Series1"].ChartType =(SeriesChartType)Enum.Parse(typeof (SeriesChartType), DropDownList1.SelectedValue);
    }
}