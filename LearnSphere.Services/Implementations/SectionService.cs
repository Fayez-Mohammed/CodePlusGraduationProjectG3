using LearnSphere.DAL.Models.SystemModels;
using LearnSphere.Repo.Interfaces;
using LearnSphere.Repo.Specifications;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.SectionDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class SectionService : ISectionService
    {
        private readonly IUnitOfWork _unit;
        public SectionService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<SectionResponseDTO>> CreateAsync(CreateSectionDTO sectionDTO)
        {
            if (sectionDTO == null)
                return Result<SectionResponseDTO>.Failure("Please fill the form");
            var course = await _unit.Repository<Course>().GetByIdAsync(sectionDTO.CourseId);
            if (course == null)
                return Result<SectionResponseDTO>.Failure("Course not found");
            var spec = new BaseSpecification<Section>(s=>s.CourseId==course.Id);
            var maxDisplayOrder = await _unit.Repository<Section>().MaxAsync(spec, s => (int?)s.DisplayOrder)??0;
            var section = new Section
            {
                CourseId=course.Id,
                Title = sectionDTO.Title,
                DisplayOrder = maxDisplayOrder + 1,

            };
            await _unit.Repository<Section>().AddAsync(section);
            await _unit.CompleteAsync();
            var result = new SectionResponseDTO
            {
                Id = section.Id,
                Title = section.Title,
                CourseId = section.CourseId,
                DisplayOrder = section.DisplayOrder,
                CreatedAt = section.CreatedAt

            };
            return Result<SectionResponseDTO>.Success(result);
        }

        public async Task<Result<IReadOnlyList<SectionDTO>>> GetAllAsync(string courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId))
                return Result<IReadOnlyList<SectionDTO>>.Failure("Course id is required");
            var spec = new BaseSpecification<Section>(s=>s.CourseId==courseId);
            spec.ApplyNoTracking();
            spec.AddOrderBy(s => s.DisplayOrder);
            var sections = await _unit.Repository<Section>().SelectAsync(spec,s=>new SectionDTO
            {
                Id = s.Id,
                Title = s.Title,
                DisplayOrder = s.DisplayOrder,
                HasQuiz=s.Quiz!=null,
                TotalDurationInMinutes=s.Lessons.Sum(l=>l.DurationMinutes),
                LessonsCount=s.Lessons.Count(),
                Lessons=s.Lessons.OrderBy(o => o.DisplayOrder).Select(l=>new SectionLessonsDTO
                {
                    Id=l.Id,
                    Title=l.Title,
                    Type=l.Type,
                    DisplayOrder=l.DisplayOrder,
                    DurationMinutes=l.DurationMinutes,
                    IsFreePreview=l.IsFreePreview,
                }).ToList()
            });
            return Result<IReadOnlyList<SectionDTO>>.Success(sections);
        }

        public async Task<Result<SectionResponseDTO>> UpdateAsync(UpdateSectionDTO sectionDTO)
        {
            if (sectionDTO == null)
                return Result<SectionResponseDTO>.Failure("Please fill the form");
            if (string.IsNullOrWhiteSpace(sectionDTO.SectionId))
                return Result<SectionResponseDTO>.Failure("Sectionid is required");
            var section = await _unit.Repository<Section>().GetByIdAsync(sectionDTO.SectionId);
            if (section == null) return Result<SectionResponseDTO>.Failure("Section not found");
            section.Title = sectionDTO.Title;
            await _unit.CompleteAsync();
            var result = new SectionResponseDTO
            {
                CourseId = section.CourseId,
                Id = section.Id,
                Title = section.Title,
                DisplayOrder = section.DisplayOrder,
                CreatedAt = section.CreatedAt,

            };
            return Result<SectionResponseDTO>.Success(result);


        }
        public async Task<Result> RemoveAsync(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return Result.Failure("id is required");
            var section = await _unit.Repository<Section>().GetByIdAsync(id);
            if (section == null)
                return Result.Failure("Section Not Found");
           await _unit.Repository<Section>().RemoveAsync(section);

            await _unit.CompleteAsync();
            return Result.Success();
        }

        public async Task<Result<List<ReOrderDTO>>> ReOrderSectionsAsync(List<ReOrderDTO> reOrderDTOs)
        {
            if(reOrderDTOs==null||!reOrderDTOs.Any())
                return Result<List<ReOrderDTO>>.Success(new List<ReOrderDTO>());
            var dic = reOrderDTOs.ToDictionary(x => x.Id, x => x.DisplayOrder);
            var ids = dic.Keys.ToList();
            var spec = new BaseSpecification<Section>(s => ids.Contains(s.Id));
            var sections = await _unit.Repository<Section>().SelectAsync(spec);
            var updateSections = sections.ToList();

            foreach (var section in sections)
            {
                if (section == null) continue;
                if (dic.TryGetValue(section.Id, out int displayOrder))
                    section.DisplayOrder = displayOrder;
            }
            await _unit.Repository<Section>().UpdateRangeAsync(updateSections);
            await _unit.CompleteAsync();
            var result = sections.Select(s => new ReOrderDTO
            {
                Id = s.Id,
                DisplayOrder = s.DisplayOrder,
            }).ToList();
           
            return Result<List<ReOrderDTO>>.Success(result);

        }
    }
}
