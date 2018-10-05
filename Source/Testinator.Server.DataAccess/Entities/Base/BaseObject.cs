namespace Testinator.Server.DataAccess.Entities.Base
{
    public abstract class BaseObject<T> : IBaseObject<T>
    {
        public T Id { get; set; }
    }
}
