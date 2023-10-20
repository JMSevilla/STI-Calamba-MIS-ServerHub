using System.Security.Cryptography;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.Meeting;

namespace sti_sys_backend.Core.Services
{
    public interface IMeetingRoomService<T> where T : class, IMeetingRoom
    {
        public Task<dynamic> createRoom(T room);
        public Task SendEmailNotification(string email, string? body);
        public Task<dynamic> getAllRooms(SectionsHelper sectionsHelper);

        public Task<dynamic> ModeratorReceiver(JwtJitsiServe jwtJitsiServe);
        public Task<dynamic> ParticipantsReceiver(JwtJitsiServe jwtJitsiServe);
        public RSA LoadPkcs8PrivateKey(string privateKeyText);
        public Task<string> ModeratorMechanism(int userId, string userName, string userEmail, string roomName);
        public Task<string> ParticipantsMechanism(int userId, string userName, string userEmail, string roomName);
        public Task<dynamic> LeaveMeeting(LeaveMeetingParams leaveMeetingParams);
        public Task<dynamic> feedConferenceAuth(ConferenceAuth conferenceAuth);
        public Task<dynamic> JoinedParticipantsLogs(JoinedParticipants joinedParticipants);
        public Task<dynamic> ListParticipants(Guid room_id);
        public Task<dynamic> RecordListParticipants(Guid room_id);
        public Task<dynamic> RemoveJoinedParticipants(int uuid);
        public Task<dynamic> WatchRoomStatus(Guid room_id);
        public Task<dynamic> RevokeMeetingRoom(Guid room_id);
        public Task<dynamic> CurrentAccountLeftMeeting(int uuid);
        public Task<dynamic> ListParticipantsLeft(Guid room_id);
        public Task<dynamic> RecordListParticipantsLeft(Guid room_id);
        public Task<dynamic> AttemptingToEnterPrivateRoom(Guid room_id, string room_password);
        public Task<dynamic> MeetingActionsLogger(MeetingActionsLogs meetingActionsLogs);
        public Task<dynamic> CheckStudentAuthorization(int accountId);
        /* app settings temp place here */
        public Task<dynamic> InitializedSettings(Settings settings);
        public Task<dynamic> GetAnySettings(Guid id);
        public Task<dynamic> UpdateAnySettings(Settings settings);
        public Task<dynamic> RemoveTicketIssuesCategory(Guid id);
        public Task<dynamic> FetchAffectedAccountsOnCourseDeletion(int courseId);
        public Task<dynamic> LeftMeetingPermanentLogs(RecordJoinedParticipants recordJoinedParticipants);
        public Task<dynamic> RemoveRoom(Guid id);
    }
}
