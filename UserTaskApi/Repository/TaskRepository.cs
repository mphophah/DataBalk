namespace UserTaskApi.Controllers
{
    public class TaskRepository : CrudRepository<Task, int>, ITaskRepository
    {
        private readonly UserDbContext _dbContext;

        public TaskRepository(UserDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Task> GetTasksForUser(int assigneeID)
        {
            return DbSet.Where(t => t.AssigneeID == assigneeID).ToList();
        }
    }
}