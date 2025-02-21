using System.ComponentModel.DataAnnotations;

namespace BlazorAppDiff.Model
{
    public class TextInput
    {
        [Required(ErrorMessage ="テキストを入力してください。")]
        [StringLength(1000, ErrorMessage = "入力できるのは1000文字までです")]
        public string OriginalText { get; set; } = string.Empty;

        [Required(ErrorMessage = "テキストを入力してください。")]
        [StringLength(1000, ErrorMessage = "入力できるのは1000文字までです")]
        public string Hint { get; set; } = string.Empty;
    }
}
