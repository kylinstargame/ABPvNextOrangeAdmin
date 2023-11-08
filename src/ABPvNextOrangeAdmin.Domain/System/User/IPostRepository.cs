using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Organization;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.User;

public interface IPostRepository: IBasicRepository<SysPost>
{
   Task<List<long>> GetPostsByUserId(Guid userId);
   Task<List<SysPost>> GetPostsById(long postId);
}