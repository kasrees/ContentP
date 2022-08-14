using System.ComponentModel.DataAnnotations;

namespace FullTextExtranetSearch.Api.Models
{
    public class SearchRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "minimal length is 3 symbols")]
        [MaxLength(30, ErrorMessage = "maximal length is 30 symbols")]
        public string Query { get; set; }

        [Required]
        [Range(1, 100)]
        public int Limit { get; set; }
    }
}