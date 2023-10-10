namespace sti_sys_backend.DataImplementations;

public interface IPC
{
    public Guid Id { get; set; }
    public OperatingSys operatingSystem { get; set; }
    public string computerName { get; set; }
    public Guid comlabId { get; set; }
    public ComputerStatus computerStatus { get; set; } 
}

public enum OperatingSys
{
    WINDOWS,
    MACOS,
    LINUX
}

public enum ComputerStatus
{
    WORKING,
    NOT_WORKING,
    NO_NETWORK
}