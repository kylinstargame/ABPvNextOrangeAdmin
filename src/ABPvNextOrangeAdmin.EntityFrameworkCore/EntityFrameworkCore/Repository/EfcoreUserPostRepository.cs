using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.User;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfcoreUserPostRepository:EfCoreRepository<ABPvNextOrangeAdminDbContext, SysPost>,IPostRepository
{
    public EfcoreUserPostRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }



    public async Task<List<long>> GetPostsByUserId(Guid userId)
    {
        var dbContext = await GetDbContextAsync();
         var Posts= from Post in dbContext.Set<SysPost>()
             join UserPost in dbContext.Set<SysUserPost>() on Post.Id equals UserPost.PostId
             where UserPost.UserId==userId
             select Post.Id;
                
        return  Posts.ToList();
    }

    public async Task<List<long>> GetPostsById(long postId)
    {
        var dbContext = await GetDbContextAsync();
        var Posts= from Post in dbContext.Set<SysPost>()
            where Post.Id ==postId
            select Post.Id;
        return Posts.ToList();
    }
    //
    // public Task<List<SysUserPost>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<List<SysUserPost>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<SysUserPost> InsertAsync(SysUserPost entity, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task InsertManyAsync(IEnumerable<SysUserPost> entities, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<SysUserPost> UpdateAsync(SysUserPost entity, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task UpdateManyAsync(IEnumerable<SysUserPost> entities, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task DeleteAsync(SysUserPost entity, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task DeleteManyAsync(IEnumerable<SysUserPost> entities, bool autoSave = false,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }

}