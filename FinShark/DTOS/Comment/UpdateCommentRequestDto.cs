using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOS.Comment;

public class UpdateCommentRequestDto
{

    [Required]
    [MinLength(1, ErrorMessage = "Title must be at least 1 character")]
    [MaxLength(50, ErrorMessage = "Title cannot be over 50 characters")]
    public string Title { get; set; } = string.Empty;


    [Required]
    [MinLength(1, ErrorMessage = "Content must be at least 1 character")]
    [MaxLength(50, ErrorMessage = "Content cannot be over 50 characters")]
    public string Content { get; set; } = string.Empty;
}
