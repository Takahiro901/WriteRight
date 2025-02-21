using Azure.AI.OpenAI;
using BlazorAppDiff.Model;
using BlazorAppDiff.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Schema.Generation;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace BlazorAppDiff.Services
{
    public class OpenAIService
    {
        private readonly ChatClient chatClient;

        public OpenAIService(IOptions<EnvironmentalVariables> options) 
        {
            var endpoint = options.Value.AzureOpenAI.Endpoint;
            var apikey = options.Value.AzureOpenAI.ApiKey;
            var deployment = options.Value.AzureOpenAI.Deployment;

            AzureOpenAIClient azureClient = new(
                new Uri(endpoint),
                new ApiKeyCredential(apikey));
            chatClient = azureClient.GetChatClient(deployment);
        }
        
        /// <summary>
        /// 文章を構成する
        /// </summary>
        /// <param name="oldText"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public async Task<(string modifiedText, string comment)> ProofReadText(string oldText, string tips)
        {
            JSchemaGenerator generator = new();
            var jsonSchema = generator.Generate(typeof(CheckedResult)).ToString();

            ChatCompletion completion = await chatClient.CompleteChatAsync(
                [
                    // System messages represent instructions or other guidance about how the assistant should behave
                    new SystemChatMessage("あなたは文章校正が得意なアシスタントです。"),
                    // User messages represent user input, whether historical or the most recent input
                    new UserChatMessage($"#お願い\n文章の校正をお願いします。\n\n #ルール\n{tips} \n\n #情報\n元の文章:\n{oldText}"),
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
    }
}
