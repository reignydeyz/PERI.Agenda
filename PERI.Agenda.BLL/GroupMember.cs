using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class GroupMember
    {
        private readonly UnitOfWork unitOfWork;

        public GroupMember(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<int> Add(EF.GroupMember args)
        {
            await unitOfWork.GroupMemberRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

            return args.Id;
        }

        public async Task Delete(EF.GroupMember args)
        {
            var a = await unitOfWork.GroupMemberRepository.Entities.FirstAsync(x => x.MemberId == args.MemberId && x.GroupId == args.GroupId);
            unitOfWork.GroupMemberRepository.Remove(a);
            unitOfWork.Commit();
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
            var bll_g = new BLL.Group(unitOfWork);
            var g = bll_g.Get(new EF.Group { Id = id }).Result;

            var res = from m in unitOfWork.MemberRepository.Entities
                        .Where(x => x.IsActive == true 
                        && x.CommunityId == args.CommunityId
                        && x.Id != g.GroupLeader)
                   join gm in unitOfWork.GroupMemberRepository.Entities
                        .Where(x => x.GroupId == id) on m.Id equals gm.MemberId into left
                    from rec in left.DefaultIfEmpty()
                    where m.Name.Contains(args.Name ?? "")
                    && m.Id != rec.Group.GroupLeader
                   select new EF.GroupMember
                   {
                       Member = m,
                       Group = rec.Group,
                       GroupId = rec == null ? 0 : rec.GroupId,
                       MemberId = m.Id
                   };

            return res.OrderBy(x => x.Member.Name);
        }
    }
}
