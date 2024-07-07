using AutoMapper;
using Learning.Entities;
using Learning.Tutor.ViewModel;
using Learning.ViewModel.Account;

namespace Learning.ViewModel.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountUserModel, AppUser>().ReverseMap();
            CreateMap<Student, StudentModel>()
     .ForMember(dest => dest.StudentUserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<StudentModel, Student>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.StudentUserName));
            CreateMap<TestViewModel, Entities.Test>().
                ForMember(d => d.GradeLevelsId, op => op.MapFrom(src => src.GradeID));
        }
    }
}
