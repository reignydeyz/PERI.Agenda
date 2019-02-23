using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/ActivityReport")]
    public class ActivityReportController : Controller
    {
        /// <summary>
        /// Generates activity report
        /// <see cref="https://stackoverflow.com/questions/4959722/c-sharp-datatable-to-csv"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns>CSV</returns>
        [HttpPost("[action]")]
        [BLL.ValidateModelState]
        public IActionResult GenerateReport([FromBody] Models.ActivityReport args)
        {
            var user = HttpContext.Items["EndUser"] as EF.EndUser;

            var bll_gr = new BLL.GroupReport();
            bll_gr.GroupId = args.GroupId;
            bll_gr.ReportId = args.ReportId;
            bll_gr.CommunityId = user.Member.CommunityId.Value;
            bll_gr.DateFrom = args.DateTimeStart;
            bll_gr.DateTo = args.DateTimeEnd;

            var dt = bll_gr.GetTable();

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());

            var result = new FileContentResult(bytes, "text/csv");
            result.FileDownloadName = "my-csv-file.csv";
            return result;
        }
    }
}