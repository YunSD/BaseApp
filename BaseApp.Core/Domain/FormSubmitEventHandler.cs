
using BaseApp.Core.Db;

namespace MaterialDemo.Domain;


public delegate void FormSubmitEventHandler<T>(T entity) where T : BaseEntity;
