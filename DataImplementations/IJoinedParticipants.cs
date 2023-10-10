namespace sti_sys_backend.DataImplementations;

public interface IJoinedParticipants
{
    int id { get; set; }
    int accountId { get; set; }
    Guid room_id { get; set; }
    Guid comlabId { get; set; }
    JoinedStatus _joinedStatus { get; set; } 
    DateTime date_joined { get; set; }
    DateTime date_left { get; set; }
}
public enum JoinedStatus
{
    JOINED,
    LEFT
}