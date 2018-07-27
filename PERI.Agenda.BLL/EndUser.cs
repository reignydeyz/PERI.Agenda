using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class EndUser
    {
        private readonly UnitOfWork unitOfWork;

        public EndUser(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.EndUserRepository.Entities.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.EndUser args)
        {
            var salt = Core.Crypto.GenerateSalt();
            var enc = Core.Crypto.Hash(args.PasswordHash ?? Guid.NewGuid().ToString(), salt);

            // default RoleId
            // User
            var roleId = 2;

            // Generate ConfirmationCode
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            args.PasswordHash = enc;
            args.PasswordSalt = Convert.ToBase64String(salt);
            args.RoleId = args.RoleId == 0 ? roleId : args.RoleId;
            args.LastSessionId = Guid.NewGuid().ToString();
            args.LastLoginDate = DateTime.Now;
            args.DateCreated = args.LastLoginDate.Value;
            args.ConfirmationCode = args.ConfirmationCode ?? guidString;
            args.ConfirmationExpiry = DateTime.Now.AddHours(12);

            await unitOfWork.EndUserRepository.AddAsync(args);
            unitOfWork.Commit();

            return args.UserId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.EndUserRepository.Entities.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public async Task Delete(int id)
        {
            unitOfWork.EndUserRepository.Remove(unitOfWork.EndUserRepository.Entities.First(x => x.UserId == id));
            await unitOfWork.CommitAsync();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.EndUserRepository.RemoveRange(unitOfWork.EndUserRepository.Entities.Where(x => ids.Contains(x.UserId)));
            await unitOfWork.CommitAsync();
        }

        public async Task Delete(EF.EndUser args)
        {
            unitOfWork.EndUserRepository.Remove(args);
            await unitOfWork.CommitAsync();
        }

        public async Task Edit(EF.EndUser args)
        {
            var rec = unitOfWork.EndUserRepository.Entities.First(x => x.UserId == args.UserId);
            
            rec.PasswordHash = args.PasswordHash;
            rec.PasswordSalt = args.PasswordSalt;
            rec.RoleId = args.RoleId;
            rec.LastPasswordChanged = args.LastPasswordChanged;
            rec.DateConfirmed = args.DateConfirmed;
            rec.ConfirmationCode = args.ConfirmationCode;
            rec.ConfirmationExpiry = args.ConfirmationExpiry;
            rec.DateInactive = args.DateInactive;

            await unitOfWork.CommitAsync();
        }

        public IQueryable<EF.EndUser> Find(EF.EndUser args)
        {
            throw new NotImplementedException();
        }

        public async Task<EF.EndUser> Get(EF.EndUser args)
        {
            args.Member = args.Member ?? new EF.Member();

            var rec = await unitOfWork.EndUserRepository.Entities
                .Include(x => x.Member)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserId == (args.UserId == 0 ? x.UserId : args.UserId)
                && x.Member.Email == (args.Member.Email ?? x.Member.Email));

            return rec;
        }
    }
}
