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

        [HttpPost("[action]")]
        public async Task<int> New([FromBody] EF.Member obj)
        {
            var context = new EF.aarsdbContext();
            var bll_member = new BLL.Member(context);

            return await bll_member.Add(obj);
        }
        
        [HttpGet("[action]")]
        public async Task<EF.Member> FindById()
        {
            var id = Request.Query["id"].ToString();

            var context = new EF.aarsdbContext();
            var bll_member = new BLL.Member(context);

            return await bll_member.Get(new EF.Member { Id = Convert.ToInt32(id) });
        }

        [HttpPost("[action]")]
        public async Task Edit([FromBody] EF.Member obj)
        {
            var context = new EF.aarsdbContext();
            var bll_member = new BLL.Member(context);

            await bll_member.Edit(obj);
        }
    }
}