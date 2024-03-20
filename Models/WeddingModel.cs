#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;



public class Wedding
{    
    [Key]

    public int WeddingId { get; set; }    

    [Required]
    [Display(Name = "Wedder One: ")]
    public string WedderOneName { get; set; }

    [Required]
    [Display(Name = "Wedder Two: ")]
    public string WedderTwoName { get; set; }  

    [Required]
    [Display(Name = "Address ")]
    public string Address { get; set; }

    public DateTime CreatedAt {get; set; }
    public DateTime UpdatedAt {get; set; }


    [DateValidator]
    [Display(Name = "Date: ")]
    public DateTime DateOfWedding { get; set; }
    // This is the ID we will use to know which User made the post
    // This name should match the name of the key from the User table (UserId)
    public int UserId { get; set; }
    // Our navigation property to our GuestList class
    // Notice there is NO reference to the User class    NOTE Users WAS A DUMB CHOICE HERE IT SHOULD BE WEDDINGSINVTTED TO OR SOMETHING LOL
    public List<GuestList> Users { get; set; } = new List<GuestList>();
}

public class DateValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Though we have Required as a validation, sometimes we make it here anyways
        // In which case we must first verify the value is not null before we proceed
        if (value == null)
        {
            // If it was, return the required error
            return new ValidationResult("Date is required");
        }
        // casting value to be a DateTime
        DateTime WedDate = (DateTime)value;
        if (WedDate <= DateTime.Today)
        {
            return new ValidationResult("Date must be in the future.");
        }
        return ValidationResult.Success;

    }
}