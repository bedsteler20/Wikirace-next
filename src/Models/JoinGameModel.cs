﻿#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Wikirace.Models;

public class JoinGameModel {
    [Required]
    [Display(Name = "Join Code")]
    public string JoinCode { get; set; }

    [Required]
    [Display(Name = "Display Name")]
    [MaxLength(20)]
    public string DisplayName { get; set; }

    public bool HideJoinCode { get; set; } = false;

}
