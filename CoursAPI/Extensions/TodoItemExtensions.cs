using CoursAPI.Models;

namespace CoursAPI.Extensions
{
    public static class TodoItemExtensions
    {
   
        public static TodoItemDTO ToDTO(this TodoItem item)
        {
            return new TodoItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                isComplete = item.isComplete
            };
        }

        public static TodoItem ToEntity(this TodoItemDTO dto)
        {
            return new TodoItem
            {
                Id = dto.Id,
                Name = dto.Name,
                isComplete = dto.isComplete
            };
        }
    }
}