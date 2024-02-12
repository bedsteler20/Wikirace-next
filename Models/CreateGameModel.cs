#nullable disable
using System.ComponentModel.DataAnnotations;
using Wikirace.Data;

namespace Wikirace.Models;

public class CreateGameModel {
    [Required]
    [Display(Name = "Start Page")]
    public string StartPage { get; set; }
    
    [Required]
    [Display(Name = "End Page")]
    public string EndPage { get; set; }

    [Required]
    [Display(Name = "Max Players")]
    [Range(2, 20, ErrorMessage = "Max players must be between 2 and 20")]
    public int MaxPlayers { get; set; } = 20;

    [Required]
    [Display(Name = "Game Type")]
    public GameType GameType { get; set; }

    [Required]
    [Display(Name = "Your Display Name")]
    public string DisplayName { get; set; }

}
