using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PERI.Agenda.BLL
{
    public class GroupReport
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int GroupId { get; set; }
        public int ReportId { get; set; }
        public int CommunityId { get; set; }
                
        public DataTable GetTable()
        {
            var connectionString = Core.Setting.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlCommand cmd = new SqlCommand("RPT_GroupActivityPivot", new SqlConnection(connectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@datestart", this.DateFrom);
            cmd.Parameters.AddWithValue("@dateend", this.DateTo);
            cmd.Parameters.AddWithValue("@groupid", this.GroupId);
            cmd.Parameters.AddWithValue("@reportid", this.ReportId);
            cmd.Parameters.AddWithValue("@communityid", this.CommunityId);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
    }
}
