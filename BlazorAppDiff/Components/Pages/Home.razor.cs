using Azure.AI.OpenAI;
using BlazorAppDiff.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Newtonsoft.Json.Schema.Generation;
using OpenAI.Chat;
using System.ClientModel;
using System.ComponentModel;
using System.Text.Json;

namespace BlazorAppDiff.Components.Pages
{
    public partial class Home
    {
        [Inject]
        IOptions<EnvironmentalVariables> Options { get; set; } = default!;

        private string endpoint = "";
        private string apikey = "";
        private string deployment = "";

        private string oldText = "私はソフトウェアエンジニアです。";
        private string tips = "誤字脱字チェックをお願いします。";

        private string comment = "";
        private string modifiedText = "";
        private ElementReference diffElement;

        private bool isDisplayDiff = false;
        private string displayString 
        {
            get
            {
                return isDisplayDiff ? "block" : "none";
            }
        }

        protected override void OnInitialized()
        {
            endpoint = Options.Value.AzureOpenAI.Endpoint;
            apikey = Options.Value.AzureOpenAI.ApiKey;
            deployment = Options.Value.AzureOpenAI.Deployment;
        }

        /// <summary>
        /// JS Interopを利用して、jsdiff と diff2html で差分を描画する
        /// </summary>
        private async Task ShowDiff()
        {
            //前回の結果を初期化
            comment = "";
            modifiedText = "";
            isDisplayDiff = false;

            var test = await AskOpenAiAsync(oldText);
            if(string.IsNullOrEmpty(test.modifiedText) is false)
                await JSRuntime.InvokeVoidAsync("renderDiff", oldText, test.modifiedText, diffElement);
            
            comment = test.comment;
            modifiedText = test.modifiedText;
            isDisplayDiff = true;
            StateHasChanged();
        }

        private async Task<(string modifiedText, string comment)> AskOpenAiAsync(string oldText)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            var jsonSchema = generator.Generate(typeof(CheckedResult)).ToString();


            AzureOpenAIClient azureClient = new(
                new Uri(endpoint),
                new ApiKeyCredential(apikey));
            ChatClient chatClient = azureClient.GetChatClient(deployment);
            ChatCompletion completion = await chatClient.CompleteChatAsync(
                [
                    // System messages represent instructions or other guidance about how the assistant should behave
                    new SystemChatMessage("あなたは文章校正が得意なアシスタントです。"),
                    // User messages represent user input, whether historical or the most recent input
                    new UserChatMessage($"{tips} /n/n {oldText}"),
                ],
                new ChatCompletionOptions()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                        "checkResult",
                        BinaryData.FromString(jsonSchema))
                }
                );

            var jsonString = completion.Content[0].Text;
            var result = JsonSerializer.Deserialize<CheckedResult>(jsonString);

            return (result?.ModifiedText ?? "", result?.Comment ?? "");
        }

        private async Task CopyAsync()
        {
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", modifiedText);
        }

        public class CheckedResult(string modifiedText, string comment)
        {
            [Description("校正後のテキスト")]
            public string ModifiedText { get; set; } = modifiedText;

            [Description("校正についての説明を必ず追加")]
            public string Comment { get; set; } = comment;
        }
    }
}
