namespace sti_sys_backend.DataImplementations;

public interface IRecordJoinedParticipants
{
    int id { get; set; }
    int accountId { get; set; }
    Guid room_id { get; set; }
    Guid comlabId { get; set; }
    RecordJoinedStatus _RecordJoinedStatus { get; set; } 
    DateTime date_joined { get; set; }
    DateTime date_left { get; set; }
}

public enum RecordJoinedStatus
{
    JOINED,
    LEFT
}