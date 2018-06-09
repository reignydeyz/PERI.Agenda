﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Agenda.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Agenda.BLL
{
    public class GroupCategory
    {

        private readonly UnitOfWork unitOfWork;

        public GroupCategory(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.GroupCategory args)
        {
            await unitOfWork.GroupCategoryRepository.AddAsync(args);
            unitOfWork.Commit();
            return args.Id;
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.GroupCategory args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.GroupCategory args)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EF.GroupCategory> Find(EF.GroupCategory args)
        {
            return unitOfWork.GroupCategoryRepository.Entities
                .Where(x => x.Name.Contains(args.Name ?? x.Name)
                && x.CommunityId == args.CommunityId)
                .Include(x => x.Group).ThenInclude(x => x.GroupMember)
                .OrderBy(x => x.Name).AsQueryable();
        }

        public async Task<EF.GroupCategory> Get(EF.GroupCategory args)
        {
            return await unitOfWork.GroupCategoryRepository.Entities
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == args.Id
                && x.CommunityId == args.CommunityId);
        }
    }
}
