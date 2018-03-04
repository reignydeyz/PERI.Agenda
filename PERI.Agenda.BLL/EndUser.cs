using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Agenda.EF;

namespace PERI.Agenda.BLL
{
    public class EndUser : IEndUser
    {
        EF.AARSContext context;

        public EndUser(AARSContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            var res = context.EndUser.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await context.SaveChangesAsync();
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
            args.ConfirmationCode = guidString;
            args.ConfirmationExpiry = DateTime.Now.AddHours(12);

            context.EndUser.Add(args);
            await context.SaveChangesAsync();

            return args.UserId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = context.EndUser.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.EndUser.RemoveRange(context.EndUser.Where(x => ids.Contains(x.UserId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.EndUser args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.EndUser args)
        {
            var rec = context.EndUser.First(x => x.UserId == args.UserId);

            if (args.PasswordHash != rec.PasswordHash)
            {
                var salt = Core.Crypto.GenerateSalt();
                var enc = Core.Crypto.Hash(args.PasswordHash, salt);

                rec.PasswordHash = enc;
                rec.PasswordSalt = Convert.ToBase64String(salt);
            }
            rec.RoleId = args.RoleId;
            rec.LastPasswordChanged = args.LastPasswordChanged;
            rec.DateConfirmed = args.DateConfirmed;
            rec.ConfirmationCode = args.ConfirmationCode;
            rec.ConfirmationExpiry = args.ConfirmationExpiry;
            rec.DateInactive = args.DateInactive;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.EndUser>> Find(EF.EndUser args)
        {
            return await context.EndUser
            .Include(x => x.Role)
            .Where(x => x.FirstName.Contains(args.FirstName ?? x.FirstName)
            && x.LastName.Contains(args.LastName ?? x.LastName)
            && x.Email == (args.Email ?? x.Email)).ToListAsync();
        }

        public async Task<EF.EndUser> Get(EF.EndUser args)
        {
            var rec = await context.EndUser.Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == (args.UserId == 0 ? x.UserId : args.UserId)
            && x.Email == (args.Email ?? x.Email));

            return rec;
        }
    }
}
