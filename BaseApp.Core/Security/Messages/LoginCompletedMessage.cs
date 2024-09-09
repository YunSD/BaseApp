namespace BaseApp.Core.Security.Messages
{
    public sealed class LoginCompletedMessage
    {
        public LoginCompletedMessage(SecurityUser user) => User = user;

        public SecurityUser User { get; set; }
    }
}
