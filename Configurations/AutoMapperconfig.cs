using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace Routing.Configurations;

public class AutoMapperconfig  : Profile
{
    public AutoMapperconfig(){
        // CreateMap<Student,StudentDTO>();
        // CreateMap<StudentDTO,Student>();
        CreateMap<StudentDTO,Student>().ReverseMap();

        // CreateMap<Student,StudentDTO>()
        //     .ForMember(n=>n.Name  , opt=> opt.MapFrom(src=>src.StudentName));
        // student.studentName = StudentDTO.Name

        // CreateMap<Student, StudentDTO>()
        //     .ForMember(dest => dest.Name, opt => opt.Ignore()); 
            // NAme will be null/empty in StudentDTo

        // CreateMap<Student,StudentDTO>()
        //     .ForMember(dest => dest.FullAddress,
        //    opt => opt.MapFrom(src => src.City + ", " + src.Country));

    }
}
