namespace sti_sys_backend.DataImplementations;

public interface ILeaveMeeting
{
    Guid id { get; set; }
    Guid roomId { get; set; }
    int accountId { get; set; }
    string firstname { get; set; }
    string lastname { get; set; }
    DateTime leaveDate { get; set; }
}