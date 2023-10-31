using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.User;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Dept;

public class PostSeedContributor : IDataSeedContributor, ITransientDependency
{
    private IPostRepository PostRepository { get; set; }


    public PostSeedContributor(IPostRepository postRepository)
    {
        PostRepository = postRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysPost> sysPosts = new List<SysPost>();


        sysPosts.Add(new SysPost(1, "CEO", "董事长"));
        sysPosts.Add(new SysPost(2, "SE", "项目经理"));
        sysPosts.Add(new SysPost(3, "HR", "人力资源"));
        sysPosts.Add(new SysPost(4, "USER", "普通员工"));
        for (int i = sysPosts.Count-1; i >= 0; i--)
        {
            var post = sysPosts[i];
            var posts = await PostRepository.GetPostsById(post.Id);
            if (posts.Count > 0)
            {
                sysPosts.Remove(post);
            }
        }


        await PostRepository.InsertManyAsync(sysPosts);
    }
}