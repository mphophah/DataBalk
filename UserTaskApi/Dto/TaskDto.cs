using System.ComponentModel.DataAnnotations;

namespace UserTaskApi.Controllers
{
    public class TaskDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int AssigneeID { get; set; }

        public DateTime DueDate { get; set; }
    }
}