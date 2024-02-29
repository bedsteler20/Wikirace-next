using System.ComponentModel.DataAnnotations;

namespace Wikirace.Data;

public enum GameType {
    [Display(Name = "First Past The Post")]
    FirstPassThePost,
    [Display(Name = "Shortest Path")]
    ShortestPath,
}
