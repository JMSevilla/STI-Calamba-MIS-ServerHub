using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.Meeting;

namespace sti_sys_backend.Controllers.BaseControllers.MeetingRoomBase
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(KeyAuthFilter))]
    public abstract class MeetingRoomBaseController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IMeetingRoom
        where TRepository : IMeetingRoomService<TEntity>
    {
        private readonly TRepository _repository;
        public MeetingRoomBaseController(TRepository repository)
        {
            _repository = repository;
        }

        [Route("create-room"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateRoom([FromBody] TEntity entity)
        {
            var result = (await _repository.createRoom(entity));
            return Ok(result);
        }
        
        [Route("get-all-rooms"), HttpGet, HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRooms([FromBody] SectionsHelper sectionsHelper)
        {
            var list = await _repository.getAllRooms(sectionsHelper);
            return Ok(list);
        }

        [Route("after-room-creation-jwt"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateJWTRoom([FromBody] JwtJitsiServe jwtJitsiServe)
        {
            var result = await _repository.ModeratorReceiver(jwtJitsiServe);
            return Ok(result);
        }

        [Route("participants-join-room"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> JoinMeetingRoomWithJWT([FromBody] JwtJitsiServe jwtJitsiServe)
        {
            var result = await _repository.ParticipantsReceiver(jwtJitsiServe);
            return Ok(result);
        }

        [Route("leave-meeting"), HttpPut, HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LeaveMeeting([FromBody] LeaveMeetingParams leaveMeetingParams)
        {
            var result = (await _repository.LeaveMeeting(leaveMeetingParams));
            return Ok(result);
        }

        [Route("conference-feed"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> FeedConferenceAuth([FromBody] ConferenceAuth conferenceAuth)
        {
            var result = await _repository.feedConferenceAuth(conferenceAuth);
            return Ok(result);
        }

        [Route("joined-participants"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> joinedParticipants([FromBody] JoinedParticipants joinedParticipants)
        {
            var result = (await _repository.JoinedParticipantsLogs(joinedParticipants));
            return Ok(result);
        }

        [Route("current-joined-list/{room_id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> joinedParticipantsList([FromRoute] Guid room_id)
        {
            var result = await _repository.ListParticipants(room_id);
            return Ok(result);
        }

        [Route("record-current-joined-list/{room_id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> recordJoinedList([FromRoute] Guid room_id)
        {
            var result = await _repository.RecordListParticipants(room_id);
            return Ok(result);
        }

        [Route("delete-joined-participants/{uuid}"), HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> deleteParticipants([FromRoute] int uuid)
        {
            var result = await _repository.RemoveJoinedParticipants(uuid);
            return Ok(result);
        }

        [Route("watch-room-status/{room_id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> WatchRoom([FromRoute] Guid room_id)
        {
            var result = await _repository.WatchRoomStatus(room_id);
            return Ok(result);
        }

        [Route("revoke-room/{room_id}"), HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeRoom([FromRoute] Guid room_id)
        {
            var result = await _repository.RevokeMeetingRoom(room_id);
            return Ok(result);
        }

        [Route("leave-meeting-v2/{uuid}"), HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> LeaveMeetingV2([FromRoute] int uuid)
        {
            var result = await _repository.CurrentAccountLeftMeeting(uuid);
            return Ok(result);
        }

        [Route("current-left-list/{room_id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CurrentLeftList([FromRoute] Guid room_id)
        {
            var result = await _repository.ListParticipantsLeft(room_id);
            return Ok(result);
        }

        [Route("private-room-password-identifier/{room_id}/{room_password}"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PrivateRoomPasswordIdentifier([FromRoute] Guid room_id,
            [FromRoute] string room_password)
        {
            var result = await _repository.AttemptingToEnterPrivateRoom(room_id, room_password);
            return Ok(result);
        }

        [Route("meeting-actions-logger"), HttpPost, HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> MeetingActionsLogger([FromBody] MeetingActionsLogs meetingActionsLogs)
        {
            var result = await _repository.MeetingActionsLogger(meetingActionsLogs);
            return Ok(result);
        }

        [Route("check-student-authorization/{accountId}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckStudentAuthorization([FromRoute] int accountId)
        {
            var result = await _repository.CheckStudentAuthorization(accountId);
            return Ok(result);
        }
        
        /* app settings api temp place here. */
        [Route("initialized-settings"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InitializedAppSettings([FromBody] Settings settings)
        {
            var result = await _repository.InitializedSettings(settings);
            return Ok(result);
        }

        [Route("find-app-settings/{id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindAppSettings([FromRoute] Guid id)
        {
            var res = await _repository.GetAnySettings(id);
            return Ok(res);
        }

        [Route("update-any-settings"), HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSettings([FromBody] Settings settings)
        {
            var res = await _repository.UpdateAnySettings(settings);
            return Ok(res);
        }
        
        [Route("remove-ticket-issues/{id}"), HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveTicketIssues([FromRoute] Guid id)
        {
            var res = await _repository.RemoveTicketIssuesCategory(id);
            return Ok(res);
        }

        [Route("check-affected-accounts-on-course/{courseId}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AffectedAccountsOnCourse([FromRoute] int courseId)
        {
            var result = await _repository.FetchAffectedAccountsOnCourseDeletion(courseId);
            return Ok(result);
        }

        [Route("left-meeting-permanent-logs"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostLeftMeetingPermanentLogs(
            [FromBody] RecordJoinedParticipants recordJoinedParticipants)
        {
            var result = await _repository.LeftMeetingPermanentLogs(recordJoinedParticipants);
            return Ok(result);
        }

        [Route("record-left-list-participants/{room_id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RecordLeftListParticipants([FromRoute] Guid room_id)
        {
            var result = await _repository.RecordListParticipantsLeft(room_id);
            return Ok(result);
        }

        [Route("remove-room/{id}"), HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveRevokedRoom([FromRoute] Guid id)
        {
            var result = await _repository.RemoveRoom(id);
            return Ok(result);
        }
    }
}
