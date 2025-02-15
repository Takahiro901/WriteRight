#nullable disable
using System.ComponentModel.DataAnnotations;

namespace BlazorAppDiff.Options
{
    /// <summary>
    /// 環境変数を格納するクラス
    /// </summary>
    public class EnvironmentalVariables
    {
        public AOAI AzureOpenAI { get; set; }

        public class AOAI
        {
            [Required]
            public string Endpoint { get; set; }

            [Required]
            public string ApiKey { get; set; }

            [Required]
            public string Deployment { get; set; }
        }
    }
}
