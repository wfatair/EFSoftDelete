using System.ComponentModel.DataAnnotations;

namespace EFSoftDelete.Contracts.Post
{
    public class CreatePostRequest
    {
        [Required]
        public string Title { get; set; } = null!;
    }
}
