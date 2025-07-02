namespace CRUD.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public DateOnly DeadLine { get; set; }
        public string Description { get; set; }
    }
}