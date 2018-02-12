using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IEnumerable<EF.Member>> Find([FromBody] EF.Member obj)
        {
            var context = new EF.aarsdbContext();
            var bll_member = new BLL.Member(context);

            return await bll_member.Find(obj);
        }
    }
}