using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;


        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Project Find(int? id)
        {

            IQueryable<Project> obj = from project in _context.Project
                                      where project.Id == id
                                      select new Project()
                                      {
                                          Id = project.Id,
                                          Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                                          Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                                          Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                                          Active = project.Active == null ? 0 : project.Active,
                                          Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                                          Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                                      };

            return obj.FirstOrDefault();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindAllAsync()
        {

            var obj = from project in _context.Project
                      select new Project()
                      {
                          Id = project.Id,
                          Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                          Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                          Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                          Active = project.Active == null ? 0 : project.Active,
                          Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                          Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                      };

            return await obj
                .Where(x => x.Project_Name != "" && x.Project_Name != null && x.Active == 1)
                .OrderBy(x => x.Project_Name)
                .Distinct()
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindProjectAsync(int? id)
        {

            var obj = from project in _context.Project
                      select new Project()
                      {
                          Id = project.Id,
                          Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                          Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                          Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                          Active = project.Active == null ? 0 : project.Active,
                          Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                          Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                      };
            return await obj
                .Where(x => x.Id == id && x.Active == 1)
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindProjectAsync(int employeeId, int accessLevel)
        {
            if (accessLevel != 1)
            {
                var result = from project in _context.Project
                         join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                         join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                         where employee.Id == employeeId

                         select new Project()
                         {
                             Id = project.Id,
                             Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                             Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                             Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                             Active = project.Active == null ? 0 : project.Active,
                             Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                             Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                         };

                return await result
               .OrderBy(x => x.Project_Name)
               .Distinct()
               .ToListAsync();

            }
            else
            {
                var result = from project in _context.Project
                         join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                         join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id

                         select new Project()
                         {
                             Id = project.Id,
                             Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                             Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                             Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                             Active = project.Active == null ? 0 : project.Active,
                             Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                             Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                         };

                return await result
               .OrderBy(x => x.Project_Name)
               .Distinct()
               .ToListAsync();

            }
        }

        public async Task<List<Project>> FindProjecPerEmployeetAsync(int employeeId)
        {

                var result = from project in _context.Project
                             join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                             join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                             where employee.Id == employeeId

                             select new Project()
                             {
                                 Id = project.Id,
                                 Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                                 Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                                 Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                                 Active = project.Active == null ? 0 : project.Active,
                                 Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                                 Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                             };

                return await result
               .OrderBy(x => x.Project_Name)
               .Distinct()
               .ToListAsync();
        }

        public async Task<List<ListProject>> FindAllToListAsync(int? accessLevel, int? employeeId)
        {

            var obj = from project in _context.Project
                      join client in _context.Client on project.Client_Id equals client.Id
                      join manager in _context.Employee on project.Manager_Id equals manager.Id
                      into Managers
                      from employee in Managers.DefaultIfEmpty()

                      join managerProject in _context.Employee on project.Project_Manager_Id equals managerProject.Id
                      into ManagersProjects
                      from managersProjects in ManagersProjects.DefaultIfEmpty()



                      select new ListProject
                      {
                          Id = project.Id,
                          Project = project.Project_Name == null ? "" : project.Project_Name,
                          Client = client.Name == null ? "" : client.Name,
                          Status = project.Active == null ? "" : project.Active.ToString(),
                          CostCenter = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                          Manager_Id = employee.Id == null ? 0 : employee.Id,
                          Project_Manager_Id = managersProjects.Id == null ? 0 : managersProjects.Id,
                          Manager = employee.Name == null ? "" : employee.Name,
                          Project_Manager = managersProjects.Name == null ? "" : managersProjects.Name,
                          AcessLevel = employee.Access_LevelId == null ? 0 : employee.Access_LevelId
                      };

            if (accessLevel == 2)
            {
                obj = obj.Where(x => x.Project_Manager_Id == employeeId);
            }

            return await obj
                .OrderBy(x => x.Project)
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<ListProject>> FindAllToListAsync(int? accessLevel, int? employeeId, int? clients, int? projects, int? manager_project, int? manager)
        {

            var obj = from project in _context.Project
                      join client in _context.Client on project.Client_Id equals client.Id
                      join managers in _context.Employee on project.Manager_Id equals managers.Id
                      into Managers
                      from employee in Managers.DefaultIfEmpty()

                      join managerProject in _context.Employee on project.Project_Manager_Id equals managerProject.Id
                      into ManagersProjects
                      from managersProjects in ManagersProjects.DefaultIfEmpty()



                      select new ListProject
                      {
                          Id = project.Id,
                          Project_Id = project.Id == null ? 0 : project.Id,
                          Project = project.Project_Name == null ? "" : project.Project_Name,
                          Client_Id = client.Id == null ? 0 : client.Id,
                          Client = client.Name == null ? "" : client.Name,
                          Status = project.Active == null ? "" : project.Active.ToString(),
                          CostCenter = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                          Manager_Id = employee.Id == null ? 0 : employee.Id,
                          Project_Manager_Id = managersProjects.Id == null ? 0 : managersProjects.Id,
                          Manager = employee.Name == null ? "" : employee.Name,
                          Project_Manager = managersProjects.Name == null ? "" : managersProjects.Name,
                          AcessLevel = employee.Access_LevelId == null ? 0 : employee.Access_LevelId
                      };

            if (accessLevel == 2)
            {
                obj = obj.Where(x => x.Project_Manager_Id == employeeId);
            }

            if (clients.HasValue)
            {
                obj = obj.Where(x => x.Client_Id == clients);
}

            if (projects.HasValue)
            {
                obj = obj.Where(x => x.Project_Id == projects);
            }

            if (manager_project.HasValue)
            {
                obj = obj.Where(x => x.Project_Manager_Id == manager_project);
            }

            if (manager.HasValue)
            {
                obj = obj.Where(x => x.Manager_Id == manager);
            }

            return await obj
                .OrderBy(x => x.Project)
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

    }
}
