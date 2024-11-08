﻿using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.PL.ViewModels.Departments;

namespace LinkDev.IKEA.PL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Employee
            CreateMap<EmployeeViewModel, CreatedEmployeeDto>().ReverseMap();
            CreateMap<EmployeeViewModel, UpdatedEmployeeDto>().ReverseMap();
            CreateMap<EmployeeDetailsDto, UpdatedEmployeeDto>().ReverseMap();
            CreateMap<EmployeeDetailsDto, EmployeeViewModel>();
            #endregion

            #region Department
            CreateMap<DepartmentDetailsDto, DepartmentViewModel>()
                /*.ForMember(des => des.Name,config => config.MapFrom(src => src.Name))*/
                .ReverseMap();
            CreateMap<DepartmentViewModel, UpdatedDepartmentDto>();
            CreateMap<DepartmentViewModel, CreatedDepartmentDto>();
            #endregion
        }
    }
}
