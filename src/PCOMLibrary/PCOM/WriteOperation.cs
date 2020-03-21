namespace Unitronics.PCOM
{

    public interface IWriteOperation
    {}
    public abstract class WriteOperation<T> : Operation<T>,IWriteOperation
    {

    }
}