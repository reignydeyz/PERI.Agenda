using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.BLL;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Rsvp")]
    public class RsvpController : Controller
    {
        private readonly IRsvp rsvpBusiness;

        public RsvpController(IRsvp rsvp)
        {
            this.rsvpBusiness = rsvp;
        }

        [BLL.ValidateModelState]
        [HttpPut]
        [Route("Add")]
        public async Task Add([FromBody] Models.Rsvp obj)
        {
            var bll_r = rsvpBusiness;

            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            obj.MemberId = obj.MemberId == 0 ? user.MemberId : obj.MemberId;

            var r = await bll_r.Get(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId });

            if (r == null)
                await bll_r.Add(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId, IsGoing = obj.IsGoing });
            else
                await bll_r.Update(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId, IsGoing = obj.IsGoing });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [BLL.ValidateModelState]
        [HttpPost("[action]")]
        [Route("Delete")]
        public async Task Delete([FromBody] Models.Rsvp obj)
        {
            var bll_r = rsvpBusiness;

            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            obj.MemberId = obj.MemberId == 0 ? user.Member.Id : obj.MemberId;

            await bll_r.Delete(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId });
        }

        [BLL.ValidateModelState]
        [HttpPost]
        [Route("Find")]
        public async Task<IActionResult> Find([FromBody] Models.Rsvp obj)
        {
            var bll_r = rsvpBusiness;

            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            obj.MemberId = obj.MemberId == 0 ? user.Member.Id : obj.MemberId;

            var res = from r in await bll_r.Find(obj.Member, obj.EventId, obj.IsGoing).ToListAsync()
                      select new
                      {
                          r.EventId,
                          r.MemberId,
                          Member = r.Member.Name,
                          r.IsGoing,
                          DateTimeResponded = r.DateModified
                      };

            return Json(res);
        }
    }
}