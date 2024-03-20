#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;



public class Wedding
{    
    [Key]
    public int WeddingId { get; set; }    
    public string WedderOneName { get; set; }
    public string WedderTwoName { get; set; }  
    public DateTime CreatedAt {get; set; }
    public DateTime UpdatedAt {get; set; }
    public DateTime DateOfWedding { get; set; }
    // This is the ID we will use to know which User made the post
    // This name should match the name of the key from the User table (UserId)
    public int UserId { get; set; }
    // Our navigation property to our GuestList class
    // Notice there is NO reference to the User class   
    public List<GuestList> Users { get; set; } = new List<GuestList>();
}