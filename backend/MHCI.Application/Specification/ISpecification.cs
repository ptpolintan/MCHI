namespace MHCI.Application.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity, ref List<string> error);
    }
}
