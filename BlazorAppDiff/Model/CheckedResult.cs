using System.ComponentModel;

namespace BlazorAppDiff.Model
{
    /// <summary>
    /// Azure OpenAIからの回答を定義するためのクラス
    /// </summary>
    /// <param name="modifiedText"></param>
    /// <param name="comment"></param>
    public class CheckedResult(string modifiedText, string comment)
    {
        [Description("校正後のテキスト")]
        public string ModifiedText { get; set; } = modifiedText;

        [Description("校正についての説明")]
        public string Comment { get; set; } = comment;
    }
}
