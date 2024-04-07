using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.Data;

public class StaffPhotos:AggregateRoot<long>
{
    private StaffPhotos(long staffId, string photoUrl)
    {
        StaffId = staffId;
        PhotoUrl = photoUrl;
    }

    public static StaffPhotos CreateInstance(long staffId, string photoUrl)
    {
        return new StaffPhotos(staffId, photoUrl);
    }
    public static List<StaffPhotos> CreateInstances(long staffId, string[] photoUrls)
    {
        List<StaffPhotos> staffPhotos = new List<StaffPhotos>();
        foreach (var photoUrl in photoUrls)
        {
            staffPhotos.Add(StaffPhotos.CreateInstance(staffId,photoUrl));
        }

        return staffPhotos;
    }

    public long StaffId { get; set; }
    public string PhotoUrl{ get; set; }
}