


namespace UserTaskApi.Controllers
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeID { get; set; } // Reference to User
        public DateTime DueDate { get; set; }
    }

}