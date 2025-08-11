using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Services.Exceptions;

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

        public async Task<bool> UpdateAsync(int id, ProjectDTO pDTO)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            project.Title = pDTO.Title;
            project.Description = pDTO.Description;
            project.StartDate = pDTO.StartDate;
            project.Deadline = pDTO.Deadline;
            project.EndDate = pDTO.EndDate;
            project.Status = pDTO.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProjectDTO>> FetchAllAsync()
        {
            return await _context.Projects.Select(p => new ProjectDTO
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

        public async Task<ProjectDTO?> FetchAsync(int id)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null)
                return null;

            return new ProjectDTO
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
