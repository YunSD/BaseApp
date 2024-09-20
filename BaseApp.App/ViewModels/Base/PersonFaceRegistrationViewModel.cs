using BaseApp.App.Utils;
using BaseApp.App.Views;
using BaseApp.Core.Domain;
using BaseApp.Core.Security;
using BaseApp.Core.UnitOfWork;
using BaseApp.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseApp.App.ViewModels.Base
{
    public class PersonFaceRegistrationViewModel
    {
        private ILog logger = LogManager.GetLogger(nameof(PersonFaceRegistrationViewModel));
        private readonly IUnitOfWork _unitOfWork;

        public PersonFaceRegistrationViewModel(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }


        public bool FaceRegistration(OpenCvSharp.Mat frame) {
            try
            {
                float[]? embedding = FaceUtil.GenerateEmbedding(frame);
                if (embedding == null) return false;

                string em_str = JsonSerializer.Serialize(embedding);

                SecurityUser? user = SecurityContext.Singleton.GetUserInfo();
                if (user == null) return false;

                IRepository<SysUser> repository = _unitOfWork.GetRepository<SysUser>();

                SysUser cur_user = repository.Find(user.UserId);
                if (cur_user == null) return false;

                cur_user.InfoFace = em_str;
                cur_user.InfoFaceFlag = Core.Enums.BaseStatusEnum.NORMAL;
                repository.Update(cur_user);
                repository.ignoreOtherEntityField(cur_user, [nameof(SysUser.InfoFace), nameof(SysUser.InfoFaceFlag)]);
                _unitOfWork.SaveChanges();
                _unitOfWork.TrackClear();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
            
        }



    }
}
