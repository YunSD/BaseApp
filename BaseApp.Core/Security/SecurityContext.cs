using BaseApp.Core.Security.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BaseApp.Security
{

    public partial class SecurityContext : ObservableRecipient, IRecipient<LoginCompletedMessage>, IRecipient<LogoutMessage>, IRecipient<RefreshUserMessage>
    {

        private static readonly SecurityContext SECURITY_USER = new SecurityContext();

        private SecurityContext() { this.IsActive = true; }

        public static SecurityContext Singleton { get { return SECURITY_USER; } }

        public SecurityUser? GetUserInfo()
        {
            if (Sign) return SecurityUser;
            return null;
        }


        public static long? GetUserId() => Singleton.SecurityUser?.UserId;
        public static string? GetUserLoginName() => Singleton.SecurityUser == null ? "anonymous" : Singleton.SecurityUser.UserName;
        public static string? GetUserName() => Singleton.SecurityUser?.Name;



        #region field
        private volatile SecurityUser? SecurityUser;
        private volatile bool Sign = false;
        #endregion


        private bool RecycleStatement(LoginCompletedMessage message)
        {
            lock (this)
            {
                if (Sign == true) return false;
                this.Sign = true;
                this.SecurityUser = message.User;
                return true;
            }
        }

        private void Logout()
        {
            lock (this)
            {
                if (Sign == false) return;
                this.Sign = false;
                this.SecurityUser = null;
            }
        }

        public void Receive(LoginCompletedMessage message)
        {
            if (RecycleStatement(message)) Messenger.Send<LoginCompletedRedirectionMessage>();
        }

        public void Receive(LogoutMessage message)
        {
            this.Logout();
        }

        public void Receive(RefreshUserMessage message)
        {
            lock (this)
            {
                this.SecurityUser = message.User;
            }
        }
    }
}
