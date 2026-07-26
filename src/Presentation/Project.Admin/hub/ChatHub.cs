using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Project.Services.Catalog;
using Project.Services.Users;

namespace Project.Admin.Hubs
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserConnection> _connections = new();
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        #region ctor

        public ChatHub(IUserService userService,
                        IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }

        #endregion

        #region ===== Helpers (Claims + IP) =====

        protected long UserIdentifier
        {
            get
            {
                if (Context.User?.Identity?.IsAuthenticated ?? false)
                {
                    var claim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null && long.TryParse(claim.Value, out var id))
                        return id;
                }
                return 0;
            }
        }

        protected string UniqeCode
        {
            get
            {
                if (Context.User?.Identity?.IsAuthenticated ?? false)
                {
                    var claim = Context.User.FindFirst(ClaimTypes.UserData);
                    return claim?.Value ?? string.Empty;
                }
                return string.Empty;
            }
        }

        protected string UserRole
        {
            get
            {
                if (Context.User?.Identity?.IsAuthenticated ?? false)
                {
                    var claim = Context.User.FindFirst(ClaimTypes.Role);
                    return claim?.Value ?? "Unknown";
                }
                return "Anonymous";
            }
        }

        protected string GetClientIp()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
                return "Unknown";

            var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
                return forwarded.Split(',').FirstOrDefault()?.Trim();

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        #endregion

        #region ===== Connection Lifecycle =====

        public override async Task OnConnectedAsync()
        {
            //if (UserIdentifier == 0 || string.IsNullOrWhiteSpace(UniqeCode))
            //    throw new HubException("Unauthenticated user");

            var connection = new UserConnection
            {
                UserId = UserIdentifier,
                Role = UserRole,
                UniqeCode = UniqeCode,
                ConnectionId = Context.ConnectionId,
                IpAddress = GetClientIp(),
                LastSeenUtc = DateTime.UtcNow
            };

            _connections[Context.ConnectionId] = connection;

            // First connection for this UniqeCode = ONLINE
            //if (GetConnectionsByUniqeCode(UniqeCode).Count == 1)
            //    await NotifyStatus(UserIdentifier, UserRole, true);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryRemove(Context.ConnectionId, out var conn))
            {
                var stillOnline = _connections.Values
                    .Any(x => x.UniqeCode == conn.UniqeCode);

                if (!stillOnline)
                    await NotifyStatus(conn.UserId, conn.Role, false);
            }

            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region ===== Heartbeat =====

        // Client should call every 30–60 seconds
        public Task Heartbeat()
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var conn))
                conn.LastSeenUtc = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        #endregion

        #region ===== Messaging =====

        //public async Task SendMessage(string receiverId, string message)
        //{
        //    if (!_connections.TryGetValue(Context.ConnectionId, out var sender))
        //        return;

        //    var chat = new ChatMaster
        //    {
        //        FromUserId = sender.UniqeCode.ToString(),
        //        ToUserId = receiverId,
        //        MessageText = message,
        //        StartedOn = DateTime.UtcNow,
        //        IsRead = false
        //    };

        //    await _chatService.SaveChatMasterAsync(chat);

        //    var targets = _connections.Values
        //        .Where(x => x.UniqeCode == receiverId)
        //        .Select(x => x.ConnectionId)
        //        .ToList();

        //    await Clients.Clients(targets)
        //        .SendAsync("ReceiveMessage", sender.UniqeCode, chat);

        //    var targetsnew = _connections.Values
        //      .Where(x => x.UniqeCode == sender.UniqeCode)
        //      .Select(x => x.ConnectionId)
        //      .ToList();

        //    await Clients.Clients(targetsnew)
        //        .SendAsync("MessageSent", receiverId, chat);
        //}

        #endregion

        #region ===== Typing Indicator =====

        public async Task Typing(string receiverId, bool isTyping)
        {
            if (!_connections.TryGetValue(Context.ConnectionId, out var sender))
                return;

            var targets = _connections.Values
                .Where(x => x.UniqeCode == receiverId)
                .Select(x => x.ConnectionId)
                .ToList();

            await Clients.Clients(targets)
                .SendAsync("UserTyping", sender.UniqeCode, isTyping);
        }

        #endregion

        #region ===== Status / Presence =====

        private async Task NotifyStatus(long userId, string role, bool isOnline)
        {
            var evt = role == "Admin"
                ? (isOnline ? "AdminOnline" : "AdminOffline")
                : (isOnline ? "UserOnline" : "UserOffline");

            var targets = _connections.Values
                .Where(x => x.Role != role)
                .Select(x => x.ConnectionId)
                .Distinct()
                .ToList();

            await Clients.Clients(targets)
                .SendAsync(evt, userId);
        }

        private static List<UserConnection> GetConnectionsByUniqeCode(string uniqeCode)
        {
            return _connections.Values
                .Where(x => x.UniqeCode == uniqeCode)
                .ToList();
        }

        public static List<UserConnection> GetExpiredConnections(DateTime threshold)
        {
            return _connections.Values
                .Where(x => x.LastSeenUtc < threshold)
                .ToList();
        }

        public static async Task ForceOffline(
            UserConnection conn,
            IHubContext<ChatHub> hub)
        {
            _connections.TryRemove(conn.ConnectionId, out _);

            var stillOnline = _connections.Values
                .Any(x => x.UniqeCode == conn.UniqeCode);

            if (!stillOnline)
            {
                var evt = conn.Role == "Admin" ? "AdminOffline" : "UserOffline";

                var targets = _connections.Values
                    .Where(x => x.Role != conn.Role)
                    .Select(x => x.ConnectionId)
                    .ToList();

                await hub.Clients.Clients(targets)
                    .SendAsync(evt, conn.UserId);
            }
        }

        public static IReadOnlyCollection<string> onlineUsers()
        {
            return _connections.Values.Where(x => x.LastSeenUtc > DateTime.UtcNow.AddMinutes(-2))
                    .Select(x => x.UniqeCode)
                    .ToList();

        }
        #endregion

        #region ===== Models =====

        public class UserConnection
        {
            public long UserId { get; set; }
            public string Role { get; set; }
            public string UniqeCode { get; set; }
            public string ConnectionId { get; set; }
            public string IpAddress { get; set; }
            public DateTime LastSeenUtc { get; set; }
        }

        #endregion
    }
}
