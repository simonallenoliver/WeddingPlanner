#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class GuestList
{
    [Key]    
    public int GuestListId { get; set; } 
    // The IDs linking to the adjoining tables   
    public int UserId { get; set; }    
    public int WeddingId { get; set; }
    // Our navigation properties - don't forget the ?    
    public User? User { get; set; }    
    public Wedding? Wedding { get; set; }
}