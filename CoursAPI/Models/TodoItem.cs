namespace CoursAPI.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public bool isComplete { get; set; }
    }

    public class TodoItemDTO
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public bool isComplete { get; set; }
    }
}
