namespace WorkLife.Data
{
    public interface IRepository<T> where T : class
    {
        public T Get(int id);
        public ICollection<T> GetAll();
        public T Create(T entity);
        public T Update(T entity);
        public void Delete(T entity);
    }
}
