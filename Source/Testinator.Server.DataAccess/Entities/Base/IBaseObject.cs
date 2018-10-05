namespace Testinator.Server.DataAccess.Entities.Base
{
    public interface IBaseObject<T>
    {
        T Id { get; set; }
    }
}
