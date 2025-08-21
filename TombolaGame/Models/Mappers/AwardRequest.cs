using System.ComponentModel.DataAnnotations;
public class AwardRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}