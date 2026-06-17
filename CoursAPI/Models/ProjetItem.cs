using System.ComponentModel.DataAnnotations;

namespace CoursAPI.Models
{
    public class ProjetItem
    {
        public int Id { get; set; }

        public required string NomProjet { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [RegularExpression(@".*github\.com.*", ErrorMessage = "The URL must be a GitHub link.")]
        public required string GitHubLink { get; set; }
    }
}