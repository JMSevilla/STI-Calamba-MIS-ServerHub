namespace sti_sys_backend.DataImplementations;

public interface IComLaboratory
{
    public Guid Id { get; set; }
    public string comlabName { get; set; }
    public int totalComputers { get; set; }
    public int totalWorkingComputers { get; set; }
    public int totalNotWorkingComputers { get; set; }
    public int totalNoNetworkComputers { get; set; }
}