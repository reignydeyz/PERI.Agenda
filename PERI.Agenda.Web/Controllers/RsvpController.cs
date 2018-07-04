﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PERI.Agenda.BLL;

namespace PERI.Agenda.Web.Controllers
{
    [BLL.VerifyUser]
    [Produces("application/json")]
    [Route("api/Rsvp")]
    public class RsvpController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public RsvpController()
        {
            unitOfWork = new UnitOfWork(new EF.AARSContext());
        }

        [BLL.ValidateModelState]
        [HttpPut("[action]")]
        [Route("Add")]
        public async Task Add([FromBody] Models.Rsvp obj)
        {
            var bll_r = new BLL.Rsvp(unitOfWork);

            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            obj.MemberId = obj.MemberId == 0 ? user.MemberId : obj.MemberId;

            var r = await bll_r.Get(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId });

            if (r == null)
                await bll_r.Add(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId, IsGoing = obj.IsGoing });
            else
                await bll_r.Update(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId, IsGoing = obj.IsGoing });
        }

        [BLL.ValidateModelState]
        [HttpPost("[action]")]
        [Route("Delete")]
        public async Task Delete([FromBody] Models.Rsvp obj)
        {
            var bll_r = new BLL.Rsvp(unitOfWork);

            var user = HttpContext.Items["EndUser"] as EF.EndUser;
            obj.MemberId = obj.MemberId == 0 ? user.Member.Id : obj.MemberId;

            await bll_r.Delete(new EF.Rsvp { EventId = obj.EventId, MemberId = obj.MemberId });
        }
    }
}