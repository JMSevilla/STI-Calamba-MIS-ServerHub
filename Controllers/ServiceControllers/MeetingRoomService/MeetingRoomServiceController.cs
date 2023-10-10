using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers.MeetingRoomBase;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.MeetingRoomService
{
    public class MeetingRoomServiceController : MeetingRoomBaseController<MeetingRoom, MeetingRoomConstructor>
    {
        public MeetingRoomServiceController(MeetingRoomConstructor repository) : base(repository)
        {
        }
    }
}
