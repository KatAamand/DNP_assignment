using System.ComponentModel.DataAnnotations;

namespace DTO;

public class CreateCommentDTO
{
    [Required(ErrorMessage = "No empty comments allowed! :)")]
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int PostId { get; set; }
}