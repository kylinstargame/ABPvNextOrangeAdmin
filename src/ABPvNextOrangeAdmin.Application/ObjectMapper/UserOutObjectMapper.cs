using ABPvNextOrangeAdmin.System.User.Dto;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectMapping;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.ObjectMapper;

public class UserOutObjectMapper : IObjectMapper<IdentityUser, UserOutput>, ITransientDependency
{
    public UserOutput Map(IdentityUser source)
    {
        return new UserOutput();
    }

    public UserOutput Map(IdentityUser source, UserOutput destination)
    {
        destination.UserId = source.Id;
        destination.UserName = source.UserName;
        destination.NickName = source.ExtraProperties.ContainsKey("NickName")? source.ExtraProperties["NickName"].ToString():""; 
        destination.Password = source.ExtraProperties.ContainsKey("Password")?source.ExtraProperties["Password"].ToString():"";
        destination.PhoneNumber = source.PhoneNumber;
        destination.Email = source.Email;
        destination.Sex = source.ExtraProperties.ContainsKey("Sex")?source.ExtraProperties["Sex"].ToString():"";
        return destination;
    }
}