using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.DbContexts;
using CareerConnect.Infrastructure.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Infrastructure.Service
{
    public class ResumeService
    {
        private readonly UserInformation _userInformation;
        private readonly CoreDbContext _coreDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ResumeService(CoreDbContext coreDbContext, UserInformation userInformation, IWebHostEnvironment webHostEnvironment)
        {
            _coreDbContext = coreDbContext;
            _userInformation = userInformation;
            _webHostEnvironment = webHostEnvironment;
        }

        public Result<List<Resume>> GetMyResumes()
        {
            return new();
        }
        public async Task<Result<Resume>> GetResumeAsync(Guid id)
        {
            var resume = await _coreDbContext.Set<Resume>().FindAsync(id);
            if (resume == null)
            {
                return new();
            }

            return new()
            {
                Success = true,
                Data = resume
            };
        }

        public async Task<Result<Resume>> UpdateResumeAsync(Guid id ,string? name)
        {
            var resume = await _coreDbContext.Set<Resume>()
                .FindAsync(id);

            if (resume == null)
            {
                return new("Not found");
            }

            resume.Name = name;

            await _coreDbContext.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = resume
            };
        }

        public async Task<Result<Resume>> DeleteResumeAsync(Guid id)
        {
            var resume = await _coreDbContext.Set<Resume>()
                .FindAsync(id);

            if (resume == null)
            {
                return new("Not found");
            }

            _coreDbContext.Remove(resume);

            await _coreDbContext.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = resume
            };
        }

        public async Task<Result<Resume>> CreateResumeAsync(IFormFile? file, string? name)
        {
            var fileData = await file.GetDataAsync();

            if (!file.IsPdfFile() || fileData == null)
            {
                return new("Invalid file format. Only .pdf accepted!");
            }

            var id = Guid.NewGuid();
            var fullPath = $"{_webHostEnvironment.ContentRootPath}/Files/{id}.pdf";
            if (!Directory.Exists($"{_webHostEnvironment.ContentRootPath}/Files"))
            {
                Directory.CreateDirectory($"{_webHostEnvironment.ContentRootPath}/Files");
            }
            await File.WriteAllBytesAsync(fullPath, fileData);
            var resume = new Resume
            {
                Id = id,
                FilePath = fullPath,
                Name = name,
                CreatedDate = DateTime.Now,
                ApplicantId = _userInformation.ApplicantId,
            };

            _coreDbContext.Add(resume);
            await _coreDbContext.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = resume
            };
        }

        public async Task<Result<List<Resume>>> GetResumesAsync()
        {
            var resumes = new List<Resume>();

            switch (_userInformation.UserType)
            {
                case UserType.Admin:
                    resumes = await _coreDbContext.Set<Resume>()
                        .ToListAsync();
                    break;
                case UserType.Applicant:
                    resumes = await _coreDbContext.Set<Resume>()
                        .OrderByDescending(e => e.CreatedDate)
                        .Where(r => r.ApplicantId == _userInformation.ApplicantId)
                        .ToListAsync();
                    break;
            }

            return new()
            {
                Success = true,
                Data = resumes
            };
        }
    }
}
