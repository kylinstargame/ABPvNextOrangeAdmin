using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.Dict;

public class DictSeedContributor : IDataSeedContributor, ITransientDependency
{
    IRepository<SysDictData> DictDataRepository;
    IRepository<SysDictType> DictTypeRepository;

    public DictSeedContributor(IRepository<SysDictData> dictDataRepository, IRepository<SysDictType> dictTypeRepository)
    {
        DictDataRepository = dictDataRepository;
        DictTypeRepository = dictTypeRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysDictType> sysDictTypes = new List<SysDictType>();
        sysDictTypes.Add(new SysDictType(1, "用户性别", "sys_user_sex", "0", "用户性别列表"));
        sysDictTypes.Add(new SysDictType(2, "菜单状态", "sys_show_hide", "0", "菜单状态列表"));
        sysDictTypes.Add(new SysDictType(3, "系统开关", "sys_normal_disable", "0", "系统开关列表"));
        sysDictTypes.Add(new SysDictType(4, "任务状态", "sys_job_status", "0", "任务状态列表"));
        sysDictTypes.Add(new SysDictType(5, "任务分组", "sys_job_group", "0", "任务分组列表"));
        sysDictTypes.Add(new SysDictType(6, "系统是否", "sys_yes_no", "0", "系统是否列表"));
        sysDictTypes.Add(new SysDictType(7, "通知类型", "sys_notice_type", "0", "通知类型列表"));
        sysDictTypes.Add(new SysDictType(8, "通知状态", "sys_notice_status", "0", "通知状态列表"));
        sysDictTypes.Add(new SysDictType(9, "操作类型", "sys_oper_type", "0", "操作类型列表"));
        sysDictTypes.Add(new SysDictType(10, "系统状态", "sys_common_status", "0", "登录状态列表"));
        int count = await DictTypeRepository.CountAsync();
        if (count <= 0)
        {
            await DictTypeRepository.InsertManyAsync(sysDictTypes);
        }
        // else
        // {
        //      await DictTypeRepository.UpdateManyAsync(sysDictTypes);           
        // }

        List<SysDictData> sysDictDatas = new List<SysDictData>();
        sysDictDatas.Add(new SysDictData(1, 1, "男", "0", "sys_user_sex", "", "Y", "0", "性别男"));
        sysDictDatas.Add(new SysDictData(2, 2, "女", "1", "sys_user_sex", "", "N", "0", "性别女"));
        sysDictDatas.Add(new SysDictData(3, 3, "未知", "2", "sys_user_sex", "", "N", "0", "性别未知"));
        sysDictDatas.Add(new SysDictData(4, 1, "显示", "0", "sys_show_hide", "primary", "Y", "0", "显示菜单"));
        sysDictDatas.Add(new SysDictData(5, 2, "隐藏", "1", "sys_show_hide", "danger", "N", "0", "隐藏菜单"));
        sysDictDatas.Add(new SysDictData(6, 1, "正常", "0", "sys_normal_disable", "primary", "Y", "0", "正常状态"));
        sysDictDatas.Add(new SysDictData(7, 2, "停用", "1", "sys_normal_disable", "danger", "N", "0", "停用状态"));
        sysDictDatas.Add(new SysDictData(8, 1, "正常", "0", "sys_job_status", "primary", "Y", "0", "正常状态"));
        sysDictDatas.Add(new SysDictData(9, 2, "暂停", "1", "sys_job_status", "danger", "N", "0", "停用状态"));
        sysDictDatas.Add(new SysDictData(10, 1, "默认", "DEFAULT", "sys_job_group", "", "Y", "0", "默认分组"));
        sysDictDatas.Add(new SysDictData(11, 2, "系统", "SYSTEM", "sys_job_group", "", "N", "0", "系统分组"));
        sysDictDatas.Add(new SysDictData(12, 1, "是", "Y", "sys_yes_no", "primary", "Y", "0", "系统默认是"));
        sysDictDatas.Add(new SysDictData(13, 2, "否", "N", "sys_yes_no", "danger", "N", "0", "系统默认否"));
        sysDictDatas.Add(new SysDictData(14, 1, "通知", "1", "sys_notice_type", "warning", "Y", "0", "通知"));
        sysDictDatas.Add(new SysDictData(15, 2, "公告", "2", "sys_notice_type", "success", "N", "0", "公告"));
        sysDictDatas.Add(new SysDictData(16, 1, "正常", "0", "sys_notice_status", "primary", "Y", "0", "正常状态"));
        sysDictDatas.Add(new SysDictData(17, 2, "关闭", "1", "sys_notice_status", "danger", "N", "0", "关闭状态"));
        sysDictDatas.Add(new SysDictData(18, 1, "新增", "1", "sys_oper_type", "info", "N", "0", "新增操作"));
        sysDictDatas.Add(new SysDictData(19, 2, "修改", "2", "sys_oper_type", "info", "N", "0", "修改操作"));
        sysDictDatas.Add(new SysDictData(20, 3, "删除", "3", "sys_oper_type", "danger", "N", "0", "删除操作"));
        sysDictDatas.Add(new SysDictData(21, 4, "授权", "4", "sys_oper_type", "primary", "N", "0", "授权操作"));
        sysDictDatas.Add(new SysDictData(22, 5, "导出", "5", "sys_oper_type", "warning", "N", "0", "导出操作"));
        sysDictDatas.Add(new SysDictData(23, 6, "导入", "6", "sys_oper_type", "warning", "N", "0", "导入操作"));
        sysDictDatas.Add(new SysDictData(24, 7, "强退", "7", "sys_oper_type", "danger", "N", "0", "强退操作"));
        sysDictDatas.Add(new SysDictData(25, 8, "生成代码", "8", "sys_oper_type", "warning", "N", "0", "生成操作"));
        sysDictDatas.Add(new SysDictData(26, 9, "清空数据", "9", "sys_oper_type", "danger", "N", "0", "清空操作"));
        sysDictDatas.Add(new SysDictData(27, 1, "成功", "0", "sys_common_status", "primary", "N", "0", "正常状态"));
        sysDictDatas.Add(new SysDictData(28, 2, "失败", "1", "sys_common_status", "danger", "N", "0", "停用状态"));
        count = await DictDataRepository.CountAsync();
        if (count <= 0)
        {
            await DictDataRepository.InsertManyAsync(sysDictDatas);
        }
        // else
        // {
        //     await DictDataRepository.UpdateManyAsync(sysDictDatas);           
        // }
    }
}