// using System.Collections.Generic;
// using System.Threading.Tasks;
// using ABPvNextOrangeAdmin.System.Roles;
// using ABPvNextOrangeAdmin.System.User;
// using Volo.Abp.Data;
// using Volo.Abp.DependencyInjection;
// using Volo.Abp.Domain.Repositories;
// using NotImplementedException = System.NotImplementedException;
//
// namespace ABPvNextOrangeAdmin.System.Dept;
//
// public class UserPostDataSeedContributor : IDataSeedContributor, ITransientDependency
// {
//     public UserPostDataSeedContributor(IRepository<SysUserPost> userPostRepository, IUserRepository userRepository)
//     {
//         UserPostRepository = userPostRepository;
//         UserRepository = userRepository;
//     }
//
//     private IRepository<SysUserPost> UserPostRepository { get; set; }
//     private IUserRepository UserRepository { get; set; }
//
//     public async Task SeedAsync(DataSeedContext context)
//     {
//         SysUser user = await UserRepository.FindByNormalizedUserNameAsync("admin");
//         List<SysUserPost> sysUserPosts = new List<SysUserPost>();
//
//         sysUserPosts.Add(SysUserPost.CreateInstance(user.Id, 1));
//
//         SysUser user1 = await UserRepository.FindByNormalizedUserNameAsync("liliang");
//         sysUserPosts.Add(SysUserPost.CreateInstance(user1.Id, 2));
//         await UserPostRepository.InsertManyAsync(sysUserPosts);
//     }
// }