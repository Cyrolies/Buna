namespace DALBase
{
    public interface IDALFactory
    {
        /// <summary>
        /// Gets the current uo W.
        /// </summary>
        /// <value>The current uo W.</value>
        IUnitOfWork CurrentUoW { get; }
    }
}