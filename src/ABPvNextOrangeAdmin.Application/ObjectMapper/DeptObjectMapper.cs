// using System.Diagnostics.Tracing;
// using ABPvNextOrangeAdmin.System.Dept;
// using ABPvNextOrangeAdmin.System.Organization.Dto;
// using ABPvNextOrangeAdmin.System.User;
// using AutoMapper.Internal.Mappers;
// using IdentityServer4.Models;
// using Volo.Abp.DependencyInjection;
// using Volo.Abp.ObjectMapping;
// using NotImplementedException = System.NotImplementedException;
//
// namespace ABPvNextOrangeAdmin.ObjectMapper;
//
// public class DeptObjectMapperr : IObjectMapper<SysDept,SysDeptTreeSelectOutput>,ITransientDependency{
//
//
//     public SysDeptTreeSelectOutput Map(SysDept source)
//     {
//         return new SysDeptTreeSelectOutput();
//     }
//
//     public SysDeptTreeSelectOutput Map(SysDept source, SysDeptTreeSelectOutput destination)
//     {
//         destination.Id = destination.Id;
//         destination.Label = source.DeptName;
//         destination.DeptName = source.DeptName;
//         destination.ParentId = source.ParentId;
//         destination.Leader = source.Leader;
//         destination.Code = source.Code;
//         destination.Email = source.Email;
//         destination.Phone = source.PhoneNumber;
//         return destination;
//     }
// }