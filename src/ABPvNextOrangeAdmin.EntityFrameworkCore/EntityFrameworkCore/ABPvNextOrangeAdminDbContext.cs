using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ABPvNextOrangeAdmin.Data;
using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.System.Dept;
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
using Volo.Abp.Identity.Settings;

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
    public DbSet<SysUser> Users { get; set; }
    public DbSet<SysRole> Roles { get; set; }
    public DbSet<SysDept> Depts { get; set; }
    public DbSet<SysPost> Posts { get; set; }

    public DbSet<SysUserRole> UserRoles { get; set; }
    // public DbSet<SysUserDept> UserDepts { get; set; }
    public DbSet<SysRoleDept> RoleDepts { get; set; }
    // public DbSet<SysUserPost> UserPosts { get; set; }
    public DbSet<SysUserLogin> UserLogins { get; set; }

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
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<StaffPhotos> StaffPhos { get; set; }
    public DbSet<StaffRecord> StaffRecords { get; set; }

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
        builder.Entity<SysUser>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        builder.Entity<SysDept>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "dept", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        
        builder.Entity<SysDept>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Depts
            )
            .UsingEntity(
                "SysUserDept",
                l => l.HasOne(typeof(SysUser)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(SysUser.Id)),
                r => r.HasOne(typeof(SysDept)).WithMany().HasForeignKey("DeptId").HasPrincipalKey(nameof(SysDept.Id)),
                
                j => j.ToTable("sys_user_dept").HasKey("UserId","DeptId"));
      
        builder.Entity<SysPost>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Posts
            )
            .UsingEntity(
                      "SysUserPost",
                      r => r.HasOne(typeof(SysUser)).WithMany().HasForeignKey("UserId")
                          .HasPrincipalKey(nameof(SysUser.Id)),
                      l => l.HasOne(typeof(SysPost)).WithMany().HasForeignKey("PostId").HasPrincipalKey(nameof(SysPost.Id)),
                      
                j => j.ToTable("sys_user_post").HasKey("UserId","PostId"));
        
        builder.Entity<SysUserLogin>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user_login", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SysRole>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "role", ABPvNextOrangeAdminConsts.DbSchema);
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
                new { x.UserId, x.RoleId }
            );
            b.ConfigureByConvention();
        });


        builder.Entity<SysRoleDept>(b =>
        {
            b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "role_dept", ABPvNextOrangeAdminConsts.DbSchema);
            b.HasKey(x =>
                new { x.RoleId, x.DeptId }
            );
            b.ConfigureByConvention();
        });

        // builder.Entity<SysUserPost>(b =>
        // {
        //     b.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "user_post", ABPvNextOrangeAdminConsts.DbSchema);
        //     b.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserId);
        //     b.HasOne(x => x.Post).WithMany(x => x.UserPosts).HasForeignKey(x => x.PostId);
        //     b.HasKey(x =>
        //         new { x.UserId, x.PostId }
        //     );
        //     b.ConfigureByConvention();
        // });
        
        builder.Entity<SysMenu>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Menus)
            .UsingEntity<SysRoleMenu>(
                r => r.HasOne<SysRole>().WithMany().HasForeignKey(e => e.RoleId),
                l => l.HasOne<SysMenu>().WithMany().HasForeignKey(e => e.MenuId),
           j => j.ToTable(ABPvNextOrangeAdminConsts.DbTablePrefix + "role_menu")
               .HasKey("RoleId","MenuId")
                );

       
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
        
        builder.Entity<Staff>(b =>
        {
            b.ToTable(ABPvNextDataConsts.DbTablePrefix + "staff", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasMany<StaffPhotos>(s => s.Photos).WithOne().HasForeignKey(p=>p.StaffId);
        });
        
        builder.Entity<StaffPhotos>(b =>
        {
            b.ToTable(ABPvNextDataConsts.DbTablePrefix + "staff_photo", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
        builder.Entity<StaffRecord>(b =>
        {
            b.ToTable(ABPvNextDataConsts.DbTablePrefix + "staff_record", ABPvNextOrangeAdminConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}