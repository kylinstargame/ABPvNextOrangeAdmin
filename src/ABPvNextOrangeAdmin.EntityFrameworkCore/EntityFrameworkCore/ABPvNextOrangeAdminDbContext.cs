using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.System.Dict;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

// using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore;

[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ABPvNextOrangeAdminDbContext :
    AbpDbContext<ABPvNextOrangeAdminDbContext>,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<SysUser> User { get; set; }
    public DbSet<SysRole> Role { get; set; }
    public DbSet<SysDept> Dept { get; set; }
    public DbSet<SysPost> Post { get; set; }
    public DbSet<SysUserRole> UserRole { get; set; }
    public DbSet<SysUserPost> UserPost { get; set; }
    
    public DbSet<SysRoleMenu> RoleMenu { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    // SysConfig
    public DbSet<SysConfig> Configs { get; set; }

    //Menu
    public DbSet<SysMenu> Menus { get; set; }
    public DbSet<SysRoleMenu> RoleMenus { get; set; }

    //Dict
    public DbSet<SysDictData> DictDatas { get; set; }
    public DbSet<SysDictType> DictTypes { get; set; }

    #endregion

    public ABPvNextOrangeAdminDbContext(DbContextOptions<ABPvNextOrangeAdminDbContext> options)
        : base(options)
    {
        AbpCommonDbProperties.DbTablePrefix = "sys_";
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /* Include modules to your migration db context */

        // builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        // builder.ConfigureIdentity();
        // builder.ConfigureIdentityServer();
        // builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "YourEntities", ABPvNextOrangeAdminConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        
        builder.Entity<SysUser>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysRole>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "role", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysDept>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "dept", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysPost>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "post", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysUserRole>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user_role", ABPvNextOrangeAdminConsts.DbSchema);
            b.HasKey(x =>
                new {x.UserId, x.RoleId}
            );
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysUserPost>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user_post", ABPvNextOrangeAdminConsts.DbSchema);
            b.HasKey(x =>
                new {x.UserId, x.PostId}
            );
            b.ConfigureByConvention();
        });

        builder.Entity<SysRoleMenu>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "role_menu", ABPvNextOrangeAdminConsts.DbSchema);
            b.HasKey(x =>
                new {x.MenuId, x.RoleId}
            );
            b.ConfigureByConvention();
        });

        builder.Entity<SysMenu>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "menu", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SysConfig>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "config", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SysDictData>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "dict_data", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SysDictType>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "dict_type", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}