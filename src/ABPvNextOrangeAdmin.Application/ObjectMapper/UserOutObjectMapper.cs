using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.User;
using ABPvNextOrangeAdmin.System.User.Dto;
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
        return new SysUserOutput();
    }

    public SysUserOutput Map(SysUser source, SysUserOutput destination)
    {
        destination.Id = source.Id;
        destination.UserName = source.UserName;
        destination.NickName = source.ExtraProperties.ContainsKey("NickName")? source.ExtraProperties["NickName"].ToString():""; 
        destination.Password = source.ExtraProperties.ContainsKey("Password")?source.ExtraProperties["Password"].ToString():"";
        destination.PhoneNumber = source.PhoneNumber;
        destination.Email = source.Email;
        destination.Sex = source.ExtraProperties.ContainsKey("Sex")?source.ExtraProperties["Sex"].ToString():"";
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
        destination.Leader =source.ExtraProperties.ContainsKey("Leader")? source.ExtraProperties["Leader"].ToString():""; 
        destination.Phone  =source.ExtraProperties.ContainsKey("Phone")? source.ExtraProperties["Phone"].ToString():""; 
        destination.Email =source.ExtraProperties.ContainsKey("Email")? source.ExtraProperties["Email"].ToString():""; 
        return destination;
    }
}