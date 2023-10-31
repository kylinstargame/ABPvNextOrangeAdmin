using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.User;
using ABPvNextOrangeAdmin.System.User.Dto;
using IdentityServer4.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectMapping;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.ObjectMapper;

public class UserOutObjectMapper : IObjectMapper<SysUser, SysUserOutput>, ITransientDependency
{
    public SysUserOutput Map(SysUser source)
    {
        return SysUserOutput.CreateInstance(source.Id, source.UserName, source.Password, source.NickName, source.Avatar,
            source.Sex, source.Email,
            source.PhoneNumber, 0, source.IsActive?"0":"1", source.LoginIP, source.LoginTime);
        
    }

    public SysUserOutput Map(SysUser source, SysUserOutput destination)
    {
        destination.Id = source.Id;
        destination.UserName = source.UserName;
        destination.NickName = source.ExtraProperties.ContainsKey("NickName")
            ? source.ExtraProperties["NickName"].ToString()
            : "";
        destination.userPassword = source.ExtraProperties.ContainsKey("Password")
            ? source.ExtraProperties["Password"].ToString()
            : "";
        destination.PhoneNumber = source.PhoneNumber;
        destination.Email = source.Email;
        destination.Sex = source.ExtraProperties.ContainsKey("Sex") ? source.ExtraProperties["Sex"].ToString() : "";
        return destination;
    }
}

public class DeptOutputObjectMapper : IObjectMapper<SysDept, SysDeptOutput>, ITransientDependency
{
    public SysDeptOutput Map(SysDept source)
    {
        return new SysDeptOutput();
    }

    public SysDeptOutput Map(SysDept source, SysDeptOutput destination)
    {
        destination.Id = source.Id;
        destination.Code = source.Code;
        destination.DeptName = source.DeptName;
        destination.Leader = source.ExtraProperties.ContainsKey("Leader")
            ? source.ExtraProperties["Leader"].ToString()
            : "";
        destination.Phone = source.ExtraProperties.ContainsKey("Phone")
            ? source.ExtraProperties["Phone"].ToString()
            : "";
        destination.Email = source.ExtraProperties.ContainsKey("Email")
            ? source.ExtraProperties["Email"].ToString()
            : "";
        return destination;
    }
}

public class DeptObjectMapper : IObjectMapper<SysDept, SysDeptTreeSelectOutput>, ITransientDependency
{
    public SysDeptTreeSelectOutput Map(SysDept source)
    {
        return new SysDeptTreeSelectOutput();
    }

    public SysDeptTreeSelectOutput Map(SysDept source, SysDeptTreeSelectOutput destination)
    {
        destination.Id = source.Id;
        destination.Code = source.Code;
        destination.DeptName = source.DeptName;
        destination.Label = source.DeptName;
        destination.Leader = source.ExtraProperties.ContainsKey("Leader")
            ? source.ExtraProperties["Leader"].ToString()
            : "";
        destination.Phone = source.ExtraProperties.ContainsKey("Phone")
            ? source.ExtraProperties["Phone"].ToString()
            : "";
        destination.Email = source.ExtraProperties.ContainsKey("Email")
            ? source.ExtraProperties["Email"].ToString()
            : "";
        return destination;
    }
}