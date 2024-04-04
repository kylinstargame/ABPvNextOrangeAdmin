namespace ABPvNextOrangeAdmin.Dto;

public class StaffListInput : PagedInput
{
   public string Name { get; set; }
   public int Years { get; set; }
   public string Dept { get; set; }
}
