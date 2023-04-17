// using System;
// using Volo.Abp.Domain.Entities.Auditing;
// using Volo.Abp.MultiTenancy;
//
// namespace ABPvNextOrangeAdmin.System.Dept;
//
// public class SysDeptRole : CreationAuditedEntity, IMultiTenant
// {
//     public SysDeptRole()
//     {
//     }
//
//     public SysDeptRole(long deptId, long roleId, Guid? tenantId)
//     {
//         DeptId = deptId;
//         RoleId = roleId;
//         TenantId = tenantId;
//     }
//
//     public long DeptId { get; protected set; }
//     public long RoleId { get; protected set; }
//
//     public Guid? TenantId { get; }
//
//     public override object[] GetKeys()
//     {
//         return new object[] { DeptId, RoleId };
//     }
// }