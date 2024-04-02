using Volo.Abp.Domain.Entities;

namespace ABPvNextOrangeAdmin.Data;

public class StaffPhotos:Entity<long>
{
    public Staff Staff { get; set; }
    public string PhotoUrl{ get; set; }
}