namespace UserTaskApi.Controllers
{
    public interface ITaskRepository : ICrudRepository<Task, int>
    {
        IEnumerable<Task> GetTasksForUser(int assigneeID);
    }
}