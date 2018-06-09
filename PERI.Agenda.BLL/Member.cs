using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Member
    {
        private UnitOfWork _unitOfWork;

        public Member(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            foreach (var id in ids)
            {
                (await _unitOfWork.MemberRepository.Entities.FirstAsync(x => x.Id == id)).IsActive = true;
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.Member args)
        {
            args.IsActive = true;
            await _unitOfWork.MemberRepository.AddAsync(args);
            await _unitOfWork.CommitAsync();
            return args.Id;
        }

        public async Task<List<EF.Member>> Add(List<EF.Member> args)
        {
            foreach (var r in args)
                r.IsActive = true;

            await _unitOfWork.MemberRepository.AddRangeAsync(args);
            await _unitOfWork.CommitAsync();

            return args;
        }

        public async Task Deactivate(int[] ids)
        {
            foreach (var id in ids)
            {
                (await _unitOfWork.MemberRepository.Entities.FirstAsync(x => x.Id == id)).IsActive = false;
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task Delete(int[] ids)
        {
            _unitOfWork.MemberRepository.RemoveRange(_unitOfWork.MemberRepository.Entities.Where(x => ids.Contains(x.Id)));
            await _unitOfWork.CommitAsync();
        }

        public async Task Edit(EF.Member args)
        {
            var user = await _unitOfWork.MemberRepository.Entities.FirstAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);

            user.Name = args.Name;
            user.NickName = args.NickName;
            user.BirthDate = args.BirthDate;
            user.Gender = args.Gender;
            user.Email = args.Email;
            user.Address = args.Address;
            user.Mobile = args.Mobile;
            user.IsActive = args.IsActive;

            await _unitOfWork.CommitAsync();
        }

        public IQueryable<EF.Member> Find(EF.Member args)
        {
            var res = _unitOfWork.MemberRepository.Entities.Where(x => x.Name.Contains(args.Name ?? "")
            && ((x.Email ?? "").Contains(args.Email ?? ""))
            && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();

            return res;
        }

        public IQueryable<EF.Member> Search(EF.Member args)
        {
            var res = _unitOfWork.MemberRepository.Entities.Where(x => x.Name.Contains(args.Name ?? "")
            || ((x.Email ?? "").Contains(args.Email ?? ""))
            && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();

            return res;
        }

        public IQueryable<EF.Member> Search(EF.Member[] args)
        {
            var res = _unitOfWork.MemberRepository.Entities.Where(x => (args.Select(y => y.Name ?? "").Contains(x.Name ?? "")
            || args.Select(y => y.Email ?? "").Contains(x.Email ?? ""))            
            && args.Select(y => y.CommunityId).Contains(x.CommunityId))
                .OrderBy(x => x.Name).AsQueryable();

            return res;
        }

        public async Task<EF.Member> Get(EF.Member args)
        {
            return await _unitOfWork.MemberRepository.Entities.FirstOrDefaultAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            return await _unitOfWork.MemberRepository.Entities.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
        }
    }
}
