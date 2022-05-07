using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class SetupRepository : ISetupRepository
    {
        private PhotoAppDbContext _ctx=null;
        

        public SetupRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx ;
        }
        
        public SetUp ReadSetupById(int id)
        {
            return _ctx.SetUps
                .Include(s=>s.Logo)   
                .Include(s=>s.NeededProfileQuestions)
                .ThenInclude(q=>(q as GroupMCProfileQuestion).GroupProfileOptions)
                .Include(s=>s.NeededProfileQuestions)
                .ThenInclude(q=>(q as StudentMCProfileQuestion).StudentProfileOptions)
                .Include(s=>s.SetTasks)
                .ThenInclude(t => t.Questions)
                .Single(s => s.SetUpId == id);
        }

        public SetUp ReadSetupWithUsageAndGroupsById(int id)
        {
            return _ctx.SetUps
                // .Include(s=>s.Teachers)
                // .ThenInclude(t=>t.Groups)
                // .Include(s=>s.Teachers)
                // .ThenInclude(t=>t.UserAccount)
                .Include(s => s.Usage)
                .ThenInclude(u => u.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.UserAccount)
                .SingleOrDefault(s => s.SetUpId == id);
        }

        public SetUp ReadSetupWithGroupsById(int id)
        {
            return _ctx.SetUps
                .Include(s => s.Teachers)
                .ThenInclude(t => t.Groups)
                .SingleOrDefault(s => s.SetUpId == id);
        }

        public IEnumerable<SetUp> ReadAllSetUps()
        {
            return _ctx.SetUps;
        }

        public IEnumerable<Admin> ReadAdminsOfSetUp(int id)
        {
            return _ctx.SetUpAdmins.Where(sa => sa.SetUp.SetUpId == id).Select(sa => sa.Admin);
        }

        public void AddSetup(SetUp setUp)
        {
            _ctx.SetUps.Add(setUp);
            _ctx.SaveChanges();
        }

        public SetUp UpdateSetup(SetUp setUp)
        {
            _ctx.SetUps.Update(setUp);
            _ctx.SaveChanges();
            return setUp;
        }

        public void RemoveSetup(SetUp setUp)
        {
            _ctx.SetUps.Remove(setUp);
            _ctx.SaveChanges();
        }

        public int NumberOfSetups()
        {
            return _ctx.SetUps.Count();
        }

        public void ClearSetups()
        {
            throw new System.NotImplementedException();
        }

        public void AddSetUpAdmin(SetUpAdmin setUpAdmin)
        {
            _ctx.SetUpAdmins.Add(setUpAdmin);
            _ctx.SetUps.Update(setUpAdmin.SetUp);
            _ctx.Admins.Update(setUpAdmin.Admin);
            _ctx.SaveChanges();
        }

        public SetUp ReadSetUpWithSetTasks(int setupId)
        {
            return _ctx.SetUps
                .Include(s => s.Logo)
                .Include(s => s.SetTasks)
                .FirstOrDefault(s => s.SetUpId == setupId);
        }

        public string ReadSetupIdentifier(string identifier)
        {
            string i= _ctx.SetUps.FirstOrDefault(s => s.loginIndentifier == identifier)?.loginIndentifier;
            return i;
        }

        public SetUp ReadSimpleSetupById(int id)
        {
            return _ctx.SetUps.Find(id);
        }

        public void ClearEmptySetUps()
        {
            var emptySetups = _ctx.SetUps.Where(s => s.Name == "").ToList();
            foreach (var emptySetup in emptySetups)
            {
                _ctx.SetUps.Remove(emptySetup);
            }
            _ctx.SaveChanges();
        }

        public SetUp ReadSetupByIdentifier(string identifier)
        {
            return _ctx.SetUps.FirstOrDefault(s => s.loginIndentifier == identifier);
        }

        public List<StudentProfileQuestion> ReadSetUpStudentProfileQuestions(int setUpId)
        {
            return _ctx.StudentProfileQuestions.Where(sq => sq.SetUp.SetUpId == setUpId).ToList();
        }
    }
}