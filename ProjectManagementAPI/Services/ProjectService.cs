using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Enums;
using ProjectManagementAPI.Services.Exceptions;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementAPI.Services
{
    public class ProjectService
    {
        private AppDBContext _context;
        public ProjectService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IntegerIdDTO> CreateAsync(CreateProjectDTO dto)
        {
            Project project = new Project(dto.Title, dto.Description, dto.StartDate, dto.Deadline, dto.Status);
            await _context.Projects.AddAsync(project);

            if (await _context.SaveChangesAsync() == 0)
                throw new DatabaseException("Error when writing to database");

            return new IntegerIdDTO { Id = project.Id };
        }

        public async Task UpdateAsync(int id, PatchDTO dto)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null)
                throw new ProjectNotFoundException("Project with id " + id.ToString() + " not found.");

            foreach (PatchDTO.Patch p in dto.Patches)
            {
                switch (p.Field)
                {
                    case "Title":
                        project.Title = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "Description":
                        project.Description = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "StartDate":
                        project.StartDate = ((JsonElement)p.Value).Deserialize<DateTime>();
                        break;
                    case "EndDate":
                        project.EndDate = ((JsonElement)p.Value).Deserialize<DateTime>();
                        break;
                    case "Deadline":
                        project.Deadline = ((JsonElement)p.Value).Deserialize<DateTime>();
                        break;
                    case "Status":
                        project.Status = ((JsonElement)p.Value).Deserialize<Status>();
                        break;
                    default:
                        throw new FieldUpdateNotAllowedException("Field " + p.Field + " can't be modified");
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null)
                throw new ProjectNotFoundException("Project with the id " + id.ToString() + " does not exist");

            _context.Projects.Remove(project);
            if (await _context.SaveChangesAsync() == 0)
                throw new DatabaseException("Error when deleting from a database");
        }

        public async Task<List<GetProjectDTO>> FetchAllAsync()
        {
            return await _context.Projects.Select(p => new GetProjectDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                Deadline = p.Deadline,
                EndDate = p.EndDate,
                Status = p.Status
            }).ToListAsync();
        }

        public async Task<GetProjectDTO> FetchAsync(int id)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null)
                throw new ProjectNotFoundException("Project with id " + id.ToString() + " does not exist.");

            return new GetProjectDTO
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                Deadline = project.Deadline,
                EndDate = project.EndDate,
                Status = project.Status
            };
        }
    }
}
