using BaseApp.App.Services;
using BaseApp.App.Views;
using BaseApp.Core.Domain;
using BaseApp.Core.Enums;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using BaseApp.Security;
using BaseApp.Upms.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using log4net.Repository.Hierarchy;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BaseApp.App.ViewModels
{

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        private ILog logger = LogManager.GetLogger(nameof(LoginViewModel));

        private readonly IUnitOfWork _unitOfWork;

        private readonly BaseUserDetailsService userDetailsService;


        [ObservableProperty]
        public string? username;

        [ObservableProperty]
        public LoginType loginType = LoginType.PASSWORD;

        public LoginViewModel(IUnitOfWork unitOfWork, BaseUserDetailsService userDetailsService)
        {
            this._unitOfWork = unitOfWork;
            this.userDetailsService = userDetailsService;
        }

        public async Task<bool> LoginByPassword(string password)
        {
            SysUser? user = new SysUser();
            bool status = await Task.Delay(1500).ContinueWith((t) =>
            {
                if (Username == null) return false;
                user = LoadUserByUsername(Username);
                if (user == null || !SecurityUtil.Verify(password, user.Password)) return false;
                if (user.IsLocked()) return false;
                return true;
            });

            if (status) WeakReferenceMessenger.Default.Send(new LoginCompletedMessage(userDetailsService.LoadSecurityUser(user)));
            return status;
        }
        private SemaphoreSlim IsProcess = new SemaphoreSlim(1, 1);
        public void LoginByFaceRecognition(OpenCvSharp.Mat mat)
        {
            OpenCvSharp.Mat frame = mat.Clone();
            if (!IsProcess.Wait(0)) return;
            Task.Run(() =>
            {
                try
                {
                    float[]? emb = FaceRecognitionService.GenerateEmbedding(frame);
                    if (emb == null) return;
                    long? userId = FaceRecognitionService.KNNSearch(emb);
                    if (userId == null) return;
                    SysUser? user = LoadUserByUserId((long)userId);
                    if (user == null) return;
                    WeakReferenceMessenger.Default.Send(new LoginCompletedMessage(userDetailsService.LoadSecurityUser(user)));
                    frame.Dispose();
                }
                finally { IsProcess.Release(); }
            });
        }



        private SysUser LoadUserByUsername(string username)
        {
            return _unitOfWork.GetRepository<SysUser>().GetFirstOrDefault(predicate: u => u.Username != null && username.Equals(u.Username));
        }

        private SysUser LoadUserByUserId(long userId)
        {
            return _unitOfWork.GetRepository<SysUser>().GetFirstOrDefault(predicate: u => u.UserId != null && userId.Equals(u.UserId));
        }

        [RelayCommand]
        private void SwitchLoginType(string key)
        {
            LoginType type = (LoginType)Enum.Parse(typeof(LoginType), key);
            if (this.LoginType != type)
            {
                this.LoginType = type;
                WeakReferenceMessenger.Default.Send(new SwitchLoginMessage(type));
            }
        }



    }
}
