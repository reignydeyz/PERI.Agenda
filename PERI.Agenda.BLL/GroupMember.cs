using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PERI.Agenda.BLL
{
    public class GroupMember
    {
        private readonly UnitOfWork unitOfWork;

        public GroupMember(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        /// <summary>
        /// Gets or finds the list of members in a group
        /// </summary>
        /// <param name="args">Member</param>
        /// <param name="id">Group id</param>
        /// <returns>List of members</returns>
        public IQueryable<EF.Member> Members(EF.Member args, int id)
        {
            return unitOfWork.GroupMemberRepository.Entities
                .Include(x => x.Member)
                .Where(x => x.GroupId == id
                && x.Member.Name.Contains(args.Name ?? ""))
                .Select(x => x.Member).OrderBy(x => x.Name);
        }

        public IQueryable<EF.GroupMember> Checklist(EF.Member args, int id)
        {
            return from m in unitOfWork.MemberRepository.Entities
                   join gm in unitOfWork.GroupMemberRepository.Entities on m.Id equals gm.MemberId into left
                   from gm in left.DefaultIfEmpty()
                   where gm.GroupId == id
                   && m.Id != gm.Group.GroupLeader
                   && m.Name.Contains(args.Name ?? "")
                   select new EF.GroupMember
                   {
                       Member = m,
                       Group = gm.Group,
                       GroupId = gm == null ? 0 : gm.GroupId,
                       MemberId = m.Id
                   };
        }
    }
}
