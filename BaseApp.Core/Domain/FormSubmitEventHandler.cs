
using BaseApp.Core.Db;

namespace BaseApp.Core.Domain;


public delegate void FormSubmitEventHandler<T>(T entity) where T : BaseEntity;
