

using BaseApp.Core.Enums;

namespace BaseApp.Core.Security.Messages
{
    public class SwitchLoginMessage
    {
        public LoginType LoginType;
        public SwitchLoginMessage(LoginType loginType)
        {
            this.LoginType = loginType;
        }
    }
}
