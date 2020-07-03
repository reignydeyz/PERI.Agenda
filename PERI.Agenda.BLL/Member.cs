using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.BLL
{
    public class Member : IMember
    {
        private readonly IUnitOfWork _unitOfWork;

        public Member(IUnitOfWork unitOfWork)
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
            // Validate Email
            var m = await GetByEmail(String.IsNullOrEmpty(args.Email) ? "email" : args.Email.Trim());

            if (m == null)
            {
                // Validate Name
                m = await Find(new EF.Member
                {
                    Name = (args.Name).ToUpper(),
                    CommunityId = args.CommunityId
                }).FirstOrDefaultAsync();
            }

            if (m == null)
            {
                // Add to Member
                m = new EF.Member
                {
                    Name = (args.Name).ToUpper(),
                    NickName = args.NickName,
                    Address = args.Address,
                    Mobile = args.Mobile,
                    Email = args.Email,
                    BirthDate = args.BirthDate,
                    CommunityId = args.CommunityId,
                    IsActive = args.IsActive,
                    CreatedBy = args.CreatedBy,
                    DateCreated = args.DateCreated,
                    InvitedBy = args.InvitedBy,
                    Remarks = args.Remarks,
                    Gender = args.Gender
                };

                await _unitOfWork.MemberRepository.AddAsync(m);
                _unitOfWork.Commit();
            }
            else
            {
                // Is equal to New Member info Email?
                if (m.Email == null || m.Email == string.Empty)
                {
                    // Update Email when Existing Member Info has no Email
                    m.Email = args.Email.Trim();
                    await Edit(m);
                }
                else if (m.Email != args.Email)
                {
                    throw new ArgumentException("Oops. There's something wrong with your entry. Please contact admin.");
                }
            }

            return m.Id;
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
            // Validate Email
            var m = await GetByEmail(String.IsNullOrEmpty(args.Email) ? "email" : args.Email.Trim());

            if (m != null && args.Id != m.Id)
            {
                // Check if member is same
                throw new ArgumentException("The email you entered is being used by another member.");
            }

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
            user.InvitedBy = args.InvitedBy;
            user.Remarks = args.Remarks;

            await _unitOfWork.CommitAsync();
        }

        public IQueryable<EF.Member> Find(EF.Member args)
        {
            var res = _unitOfWork.MemberRepository.Entities
                .Include(x => x.EndUser).ThenInclude(x => x.Role)
                .Where(x => x.Name.Contains(args.Name ?? "")
            && ((x.Email ?? "").Contains(args.Email ?? ""))
            && x.CommunityId == args.CommunityId)
                .OrderBy(x => x.Name).AsQueryable();

            if (args.EndUser != null)
                res = res.Where(x => x.EndUser.RoleId == args.EndUser.RoleId);

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
            return await _unitOfWork.MemberRepository.Entities
                .Include(x => x.EndUser)
                .FirstOrDefaultAsync(x => x.Id == args.Id
            && x.CommunityId == args.CommunityId);
        }

        public async Task<EF.Member> GetById(int id)
        {
            return await _unitOfWork.MemberRepository.Entities
                .Include(x => x.EndUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsSelectedIdsOk(int[] ids, EF.EndUser user)
        {
            switch (user.RoleId)
            {
                case 1:
                case 3:
                    return await _unitOfWork.MemberRepository.Entities.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count();
                default:
                    return await _unitOfWork.MemberRepository.Entities.Where(x => ids.Contains(x.Id) && x.CommunityId == user.Member.CommunityId).CountAsync() == ids.Count()
                        && await _unitOfWork.MemberRepository.Entities.Where(x => ids.Contains(x.Id) && x.Id == user.MemberId).CountAsync() == ids.Count();
            }
        }

        public async Task<int?> GetIdByName(string name, int communityId)
        {
            var res = await _unitOfWork.MemberRepository.Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()
            && x.CommunityId == communityId);

            if (res == null)
                return null;
            else
                return res.Id;
        }

        public IQueryable<EF.Attendance> Activities(int id)
        {
            return _unitOfWork.AttendanceRepository.Entities
                .Include(x => x.Event).ThenInclude(x => x.EventCategory)
                .Where(x => x.MemberId == id);
        }


        /// <summary>
        /// Gets the total number of groups leading
        /// </summary>
        /// <param name="id">ID of the member</param>
        /// <returns>int</returns>
        public async Task<int> Leading(int id)
        {
            return await _unitOfWork.GroupRepository.Entities.CountAsync(x => x.GroupLeader == id);
        }

        /// <summary>
        /// Gets the total number of groups following
        /// </summary>
        /// <param name="id">ID of the member</param>
        /// <returns>int</returns>
        public async Task<int> Following(int id)
        {
            return await _unitOfWork.GroupMemberRepository.Entities.CountAsync(x => x.MemberId == id);
        }

        /// <summary>
        /// Gets the total number of invited members
        /// </summary>
        /// <param name="id">ID of the member</param>
        /// <returns>int</returns>
        public async Task<int> Invites(int id)
        {
            return await _unitOfWork.MemberRepository.Entities.CountAsync(x => x.InvitedBy == id);
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Member args)
        {
            throw new NotImplementedException();
        }

        public async Task<EF.Member> GetByEmail(string email)
        {
            return await _unitOfWork.MemberRepository.Entities.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
