using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Pkcs;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.MailDto;
using sti_sys_backend.Utilization.Meeting;
using Accounts = sti_sys_backend.Models.Accounts;

namespace sti_sys_backend.Core.ServiceImplementations
{
    public abstract class MeetingRoomImpl<TEntity, TContext> : IMeetingRoomService<TEntity>
        where TEntity : class, IMeetingRoom
        where TContext : DatabaseQueryable
    {
        private readonly TContext _context;
        private readonly MailSettings _mailSettings;
        public MeetingRoomImpl(TContext context, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mailSettings = mailSettings.Value;
        }

        public async Task<dynamic> createRoom(TEntity room)
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            if (!string.IsNullOrEmpty(room.room_password))
            {
                string hashedRoomPassword = BCrypt.Net.BCrypt.HashPassword(room.room_password);
                room.room_password = hashedRoomPassword;
            }
            room.pushNotifs = 1;
            room.created_at = currentDate;
            room.updated_at = currentDate;
            await _context.Set<TEntity>().AddAsync(room);
            await _context.SaveChangesAsync();
            dynamic obj = new ExpandoObject();
            obj.room_id = room.id;
            obj.status = 200;
            return obj;
        }

        public Task SendEmailNotification(string email, string? body)
        {
            throw new NotImplementedException();
        }
        class MultiSections
        {
            public string label { get; set; }
            public int value { get; set; }
        }
        public async Task<dynamic> getAllRooms(SectionsHelper sectionsHelper)
        {
            var roomsWithParticipants = _context.Set<TEntity>()
                .Where(x => x.sectionId != null)
                .AsEnumerable<TEntity>() // Explicitly specify the type
                .Where(x => JsonConvert.DeserializeObject<List<MultiSections>>(x.sectionId)
                    .Any(sect => sectionsHelper.section.Any(id => sect.value == id)))
                .Select(room => new
                {
                    Room = room,
                    Participants = _context.JoinedParticipantsEnumerable
                        .Where(p => p.room_id == room.id && p._joinedStatus == JoinedStatus.JOINED)
                        .ToList(),
                    RoomAuthorization = _context.MeetingActionsLogsEnumerable
                        .Where(x => x.room_id == room.id && x.logDateTime.Value.Date == DateTime.Today)
                        .ToList()
                })
                .OrderByDescending(item => item.Room.created_at)
                .ToList();

            return roomsWithParticipants;
        }

        public async Task<dynamic> ModeratorReceiver(JwtJitsiServe jwtJitsiServe)
        {
            
            var jwtgen = await ModeratorMechanism(
                jwtJitsiServe.userId,
                jwtJitsiServe.userName,
                jwtJitsiServe.userEmail,
                jwtJitsiServe.roomName
                );
            return jwtgen;
        }

        public async Task<dynamic> ParticipantsReceiver(JwtJitsiServe jwtJitsiServe)
        {
            var jwtgen = await ParticipantsMechanism(
                jwtJitsiServe.userId,
                jwtJitsiServe.userName,
                jwtJitsiServe.userEmail,
                jwtJitsiServe.roomName
            );
            return jwtgen;
        }

        public RSA LoadPkcs8PrivateKey(string privateKeyText)
        {
            RSA rsaPrivateKey = RSA.Create();
            rsaPrivateKey.ImportFromPem(privateKeyText);
            return rsaPrivateKey;
        }
        public const string BEGIN_PKCS1_PRIVATE_KEY = "-----BEGIN RSA PRIVATE KEY-----";
        public const string END_PKCS1_PRIVATE_KEY = "-----END RSA PRIVATE KEY-----";
        public const string BEGIN_PKCS8_PRIVATE_KEY = "-----BEGIN PRIVATE KEY-----";
        public const string END_PKCS8_PRIVATE_KEY = "-----END PRIVATE KEY-----";
        public static RSA ScanPrivateKey(string key, PKType pkType)
        {
            var rsa = RSA.Create();
            var privateKey = key;
            privateKey = privateKey.Replace(pkType == PKType.PKCS1 ? BEGIN_PKCS1_PRIVATE_KEY : BEGIN_PKCS8_PRIVATE_KEY,
                "");
            privateKey = privateKey.Replace(pkType == PKType.PKCS1 ? END_PKCS1_PRIVATE_KEY : END_PKCS8_PRIVATE_KEY, "");
            var privateKeyDecoded = Convert.FromBase64String(privateKey);
            if (pkType == PKType.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyDecoded, out _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyDecoded, out _);
            }

            return rsa;
        }
        public async Task<string> ModeratorMechanism(int userId, string userName, string userEmail, string roomName)
        {
            var rgetKey = await _context.MailGunSecuredApiKeys.Where(x => x.AuthenticationMechanisms == "jitsi-key")
                .FirstOrDefaultAsync();
            var jwtBuilder = JitsiServerSide.Builder();
            var privateKey = ScanPrivateKey(rgetKey.domain, PKType.PKCS8);
            jwtBuilder
                .WithAudienceAndIssuer()
                .WithApiKey("vpaas-magic-cookie-d912c09dfba74cf2b05fe117f76fafd1/33de9d")
                .WithUserName(userName)
                .WithUserEmail(userEmail)
                .WithUserAvatar("")
                .WithAppID("vpaas-magic-cookie-d912c09dfba74cf2b05fe117f76fafd1")
                .WithLiveStreamingEnabled(false)
                .WithRecordingEnabled(true)
                .WithOutboundCallEnabled(true)
                .WithTranscriptionEnabled(true)
                .WithModerator(true)
                .WithRoomName("*")
                .WithUserId(Convert.ToString(userId));
            
            // Sign the JWT token
            var jwtToken = jwtBuilder.SignWith(privateKey);
            return jwtToken;
            
        }

        public async Task<string> ParticipantsMechanism(int userId, string userName, string userEmail, string roomName)
        {
            var rgetKey = await _context.MailGunSecuredApiKeys.Where(x => x.AuthenticationMechanisms == "jitsi-key")
                .FirstOrDefaultAsync();
            var jwtBuilder = JitsiServerSide.Builder();
            var privateKey = ScanPrivateKey(rgetKey.domain, PKType.PKCS8);
            jwtBuilder
                .WithAudienceAndIssuer()
                .WithApiKey("vpaas-magic-cookie-d912c09dfba74cf2b05fe117f76fafd1/33de9d")
                .WithUserName(userName)
                .WithUserEmail(userEmail)
                .WithUserAvatar("")
                .WithAppID("vpaas-magic-cookie-d912c09dfba74cf2b05fe117f76fafd1")
                .WithLiveStreamingEnabled(false)
                .WithRecordingEnabled(true)
                .WithOutboundCallEnabled(true)
                .WithTranscriptionEnabled(true)
                .WithModerator(false)
                .WithRoomName("*")
                .WithUserId(Convert.ToString(userId));

            // Sign the JWT token
            var jwtToken = jwtBuilder.SignWith(privateKey);
            return jwtToken;
        }
        /*
         * @Deprecated LeaveMeeting
         */
        public async Task<dynamic> LeaveMeeting(LeaveMeetingParams leaveMeetingParams)
        {
            bool currentAccountJoined = await _context.Set<JoinedParticipants>()
                .AnyAsync(x => x.accountId == leaveMeetingParams.accountId
                && x.room_id == leaveMeetingParams.roomId);
            if (!currentAccountJoined)
            {
                // change logic to JoinedParticipants Model
                Models.LeaveMeeting leaveMeeting = new LeaveMeeting();
                
                leaveMeeting.firstname = leaveMeetingParams.firstname;
                leaveMeeting.lastname = leaveMeetingParams.lastname;
                leaveMeeting.accountId = leaveMeetingParams.accountId;
                leaveMeeting.leaveDate = DateTime.Now;
                await _context.Set<LeaveMeeting>().AddAsync(leaveMeeting);
                await _context.SaveChangesAsync();
            }

            return 400;
        }

        public async Task<dynamic> feedConferenceAuth(ConferenceAuth conferenceAuth)
        {
            conferenceAuth.isValid = 1;
            await _context.Set<ConferenceAuth>().AddAsync(conferenceAuth);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> JoinedParticipantsLogs(JoinedParticipants joinedParticipants)
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            RecordJoinedParticipants recordJoinedParticipants = new RecordJoinedParticipants();
            recordJoinedParticipants.room_id = joinedParticipants.room_id;
            recordJoinedParticipants.accountId = joinedParticipants.accountId;
            recordJoinedParticipants._RecordJoinedStatus = 0;
            recordJoinedParticipants.comlabId = joinedParticipants.comlabId;
            recordJoinedParticipants.date_joined = currentDate;
            joinedParticipants.date_joined = currentDate;
            await _context.Set<JoinedParticipants>().AddAsync(joinedParticipants);
            await _context.Set<RecordJoinedParticipants>().AddAsync(recordJoinedParticipants);
            await _context.SaveChangesAsync();
            return 200;
        }
        public async Task<dynamic> ListParticipants(Guid room_id)
        {
            bool participantsHasAny = await _context.Set<JoinedParticipants>()
                .AnyAsync(x => x.room_id == room_id);
            if (!participantsHasAny)
            {
                return 400;
            }
            else
            {
                var joined_room = await _context.Set<TEntity>()
                    .Where(x => x.id == room_id && x.room_status == 1)
                    .FirstOrDefaultAsync();
                if (joined_room != null)
                {
                    var list = await _context.JoinedParticipantsEnumerable
                        .Where(x => x.room_id == joined_room.id && x._joinedStatus == JoinedStatus.JOINED)
                        .OrderByDescending(x => x.date_joined)
                        .Join(_context.AccountsEnumerable,
                            joined => joined.accountId,
                            account => account.id,
                            ((participants, accounts) => new
                            {
                                Joined = participants,
                                Account = accounts
                            }))
                        .Join(_context.Set<TEntity>(),
                            meetings => meetings.Joined.room_id,
                            additional => additional.id,
                            (joined, additional) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = additional
                            })
                        .Join(_context.CoursesEnumerable,
                            courses => courses.Account.course_id,
                            account => account.id,
                            (joined, course) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = joined.Additional,
                                Course = course
                            }).ToListAsync();
                    return list;
                }
                else
                {
                    return 400;
                }
            }
        }

        public async Task<dynamic> RecordListParticipants(Guid room_id)
        {
              bool participantsHasAny = await _context.Set<RecordJoinedParticipants>()
                .AnyAsync(x => x.room_id == room_id);
            if (!participantsHasAny)
            {
                return 400;
            }
            else
            {
                var joined_room = await _context.Set<TEntity>()
                    .Where(x => x.id == room_id)
                    .FirstOrDefaultAsync();
                if (joined_room != null)
                {
                    var list = await _context.RecordJoinedParticipantsEnumerable
                        .Where(x => x.room_id == joined_room.id && x._RecordJoinedStatus == RecordJoinedStatus.JOINED)
                        .OrderByDescending(x => x.date_joined)
                        .Join(_context.AccountsEnumerable,
                            joined => joined.accountId,
                            account => account.id,
                            ((participants, accounts) => new
                            {
                                Joined = participants,
                                Account = accounts
                            }))
                        .Join(_context.Set<TEntity>(),
                            meetings => meetings.Joined.room_id,
                            additional => additional.id,
                            (joined, additional) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = additional
                            })
                        .Join(_context.CoursesEnumerable,
                            courses => courses.Account.course_id,
                            account => account.id,
                            (joined, course) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = joined.Additional,
                                Course = course
                            }).ToListAsync();
                    return list;
                }
                else
                {
                    return 400;
                }
            }
        }

        public async Task<dynamic> RemoveJoinedParticipants(int uuid)
        {
            bool findJoinedParticipants = await _context.Set<JoinedParticipants>()
                .AnyAsync(x => x.accountId == uuid && x._joinedStatus == JoinedStatus.JOINED);
            if (findJoinedParticipants)
            {
                var joinedParticipantToBeDeleted = await _context.Set<JoinedParticipants>()
                    .Where(x => x.accountId == uuid && x._joinedStatus == JoinedStatus.JOINED)
                    .FirstOrDefaultAsync();
               
                joinedParticipantToBeDeleted._joinedStatus = JoinedStatus.LEFT;
                joinedParticipantToBeDeleted.date_left = DateTime.Now;
                await _context.SaveChangesAsync();
                return 200;
            }
            else
            {
                return 403;
            }
        }

        public async Task<dynamic> WatchRoomStatus(Guid room_id)
        {
            bool watchroom = await _context.Set<TEntity>()
                .AnyAsync(x => x.room_status == 0 && x.id == room_id);
            return watchroom;
        }

        public async Task<dynamic> RevokeMeetingRoom(Guid room_id)
        {
            var roomToBeRevoked = await _context.Set<TEntity>()
                .Where(x => x.id == room_id)
                .FirstOrDefaultAsync();
            roomToBeRevoked.room_status = 0;
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> CurrentAccountLeftMeeting(int uuid)
        {
            var AccountFromJoinedMeeting = await _context.Set<JoinedParticipants>()
                .Where(x => x.accountId == uuid && x._joinedStatus == 0)
                .FirstOrDefaultAsync();
            if (AccountFromJoinedMeeting != null)
            {
                AccountFromJoinedMeeting._joinedStatus = JoinedStatus.LEFT;
                AccountFromJoinedMeeting.date_left = DateTime.Now;
            }

            return 400;
        }

        public async Task<dynamic> ListParticipantsLeft(Guid room_id)
        {
            bool participantsHasAny = await _context.Set<JoinedParticipants>()
                .AnyAsync(x => x.room_id == room_id);
            if (!participantsHasAny)
            {
                return 400;
            }
            else
            {
                var joined_room = await _context.Set<TEntity>()
                    .Where(x => x.id == room_id && x.room_status == 1)
                    .FirstOrDefaultAsync();
                if (joined_room != null)
                {
                    var list = await _context.JoinedParticipantsEnumerable
                        .Where(x => x.room_id == joined_room.id && x._joinedStatus == JoinedStatus.LEFT)
                        .OrderByDescending(x => x.date_left)
                        .Join(_context.AccountsEnumerable,
                            joined => joined.accountId,
                            account => account.id,
                            ((participants, accounts) => new
                            {
                                Joined = participants,
                                Account = accounts
                            }))
                        .Where(joinedAccount => joinedAccount.Account.access_level == 3)
                        .Join(_context.Set<TEntity>(),
                            meetings => meetings.Joined.room_id,
                            additional => additional.id,
                            (joined, additional) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = additional
                            })
                        .Join(_context.CoursesEnumerable,
                            courses => courses.Account.course_id,
                            account => account.id,
                            (joined, course) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = joined.Additional,
                                Course = course
                            }).ToListAsync();
                    return list;
                }
                else
                {
                    return 400;
                }
            }
        }

        public async Task<dynamic> RecordListParticipantsLeft(Guid room_id)
        {
             bool participantsHasAny = await _context.Set<RecordJoinedParticipants>()
                .AnyAsync(x => x.room_id == room_id);
            if (!participantsHasAny)
            {
                return 400;
            }
            else
            {
                var joined_room = await _context.Set<TEntity>()
                    .Where(x => x.id == room_id)
                    .FirstOrDefaultAsync();
                if (joined_room != null)
                {
                    var list = await _context.JoinedParticipantsEnumerable
                        .Where(x => x.room_id == joined_room.id && x._joinedStatus == JoinedStatus.LEFT)
                        .OrderByDescending(x => x.date_left)
                        .Join(_context.AccountsEnumerable,
                            joined => joined.accountId,
                            account => account.id,
                            ((participants, accounts) => new
                            {
                                Joined = participants,
                                Account = accounts
                            }))
                        .Join(_context.Set<TEntity>(),
                            meetings => meetings.Joined.room_id,
                            additional => additional.id,
                            (joined, additional) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = additional
                            })
                        .Join(_context.CoursesEnumerable,
                            courses => courses.Account.course_id,
                            account => account.id,
                            (joined, course) => new
                            {
                                Joined = joined.Joined,
                                Account = joined.Account,
                                Additional = joined.Additional,
                                Course = course
                            }).ToListAsync();
                    return list;
                }
                else
                {
                    return 400;
                }
            }
        }

        public async Task<dynamic> AttemptingToEnterPrivateRoom(Guid room_id, string room_password)
        {
            bool identifyRoom = await _context.Set<TEntity>()
                .AnyAsync(x => x.id == room_id && x.room_status == 1);
            if (identifyRoom)
            {
                var selectFromRoom = await _context.Set<TEntity>()
                    .Where(x => x.id == room_id && x.room_status == 1)
                    .FirstOrDefaultAsync();
                if (BCrypt.Net.BCrypt.Verify(room_password, selectFromRoom.room_password))
                {
                    return 200;
                }
                else
                {
                    return 401;
                }
            }
            else
            {
                return 403;
            }
        }

        public async Task<dynamic> MeetingActionsLogger(MeetingActionsLogs meetingActionsLogs)
        {
            bool existingMeeetingActionsLogger = await _context.Set<MeetingActionsLogs>()
                .AnyAsync(x => x.accountId == meetingActionsLogs.accountId 
                               && x.room_id == meetingActionsLogs.room_id
                               && x.logDateTime.Value.Date == DateTime.Today);
            if (existingMeeetingActionsLogger)
            {
                var findexistLogger = await _context.Set<MeetingActionsLogs>()
                    .Where(x => x.accountId == meetingActionsLogs.accountId && x.room_id == meetingActionsLogs.room_id)
                    .FirstOrDefaultAsync();
                if (findexistLogger.violations >= 3)
                {
                    findexistLogger._meetingAuthorization = MeetingAuthorization.UNAUTHORIZED;
                    findexistLogger.logDateTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return 401;
                }
                else
                {
                    findexistLogger.violations = findexistLogger.violations + 1;
                    findexistLogger.logDateTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return 200;
                }
            }
            else
            {
                meetingActionsLogs.violations = 1;
                meetingActionsLogs._meetingAuthorization = MeetingAuthorization.AUTHORIZED;
                meetingActionsLogs.logDateTime = DateTime.Now;
                await _context.Set<MeetingActionsLogs>().AddAsync(meetingActionsLogs);
                await _context.SaveChangesAsync();
                return 200;
            }
        }

        public async Task<dynamic> CheckStudentAuthorization(int accountId)
        {
            bool checkmeetAuthorization = await _context.MeetingActionsLogsEnumerable
                .AnyAsync(x => x.accountId == accountId &&
                               x.logDateTime.Value.Date == DateTime.Today);
            if (checkmeetAuthorization)
            {
                var foundMeetAuthorization = await _context.Set<MeetingActionsLogs>()
                    .Where(x => x.accountId == accountId
                                && x.logDateTime.Value.Date == DateTime.Today)
                    .FirstOrDefaultAsync();
                return foundMeetAuthorization._meetingAuthorization;
            }

            return 200;
        }

        public async Task<dynamic> InitializedSettings(Settings settings)
        {
            bool checkAnySettings = await _context.Set<Settings>()
                .AnyAsync();
            if (checkAnySettings)
            {
                return 200;
            }
            else
            {
                await _context.Set<Settings>().AddAsync(settings);
                await _context.SaveChangesAsync();
                return 200;
            }
        }

        public async Task<dynamic> GetAnySettings(Guid id)
        {
            var res = await _context.Set<Settings>()
                .Where(x => x.id == id).ToListAsync();
            return res;
        }

        public async Task<dynamic> UpdateAnySettings(Settings settings)
        {
            var res = await _context.Set<Settings>()
                .Where(x => x.id == settings.id)
                .FirstOrDefaultAsync();
            res.roomSettings = settings.roomSettings;
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> RemoveTicketIssuesCategory(Guid id)
        {
            var ticketIssues = await _context.Set<TicketIssues>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            _context.Set<TicketIssues>().Remove(ticketIssues);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> FetchAffectedAccountsOnCourseDeletion(int courseId)
        {
            bool checkAccountsUnderCourseId = await _context.AccountsEnumerable
                .AnyAsync(x => x.course_id == courseId);
            if (checkAccountsUnderCourseId)
            {
                var accountsAffected = await _context.Set<Accounts>()
                    .Where(x => x.course_id == courseId).CountAsync();
                return accountsAffected;
            }

            return 400;
        }

        public async Task<dynamic> LeftMeetingPermanentLogs(RecordJoinedParticipants recordJoinedParticipants)
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            recordJoinedParticipants.room_id = recordJoinedParticipants.room_id;
            recordJoinedParticipants.comlabId = recordJoinedParticipants.comlabId;
            recordJoinedParticipants.accountId = recordJoinedParticipants.accountId;
            recordJoinedParticipants.date_left = currentDate;
            recordJoinedParticipants._RecordJoinedStatus = RecordJoinedStatus.LEFT;
            await _context.Set<RecordJoinedParticipants>().AddAsync(recordJoinedParticipants);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> RemoveRoom(Guid id)
        {
            var result = await _context.Set<TEntity>()
                .Where(x => x.id == id)
                .FirstOrDefaultAsync();
            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
                return 200;
            }

            return 404;
        }
    }
}
