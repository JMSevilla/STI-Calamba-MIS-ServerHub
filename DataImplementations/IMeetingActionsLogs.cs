namespace sti_sys_backend.DataImplementations;

public interface IMeetingActionsLogs
{
    Guid id { get; set; }
    string log_message { get; set; }
    int accountId { get; set; }
    Guid room_id { get; set; }
    int? violations { get; set; }
    MeetingAuthorization? _meetingAuthorization { get; set; }
    DateTime? logDateTime { get; set; }
}

public enum MeetingAuthorization
{
    AUTHORIZED,
    UNAUTHORIZED
}