namespace sti_sys_backend.Utilization.Meeting;

public class LeaveMeetingParams
{
    public Guid roomId { get; set; }
    public int accountId { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
}