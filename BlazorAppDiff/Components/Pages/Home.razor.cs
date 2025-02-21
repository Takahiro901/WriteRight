using BlazorAppDiff.Model;
using BlazorAppDiff.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorAppDiff.Components.Pages
{
    public partial class Home
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        OpenAIService OpenAIService { get; set; } = default!;

        private string comment = "";
        private string modifiedText = "";
        private ElementReference diffElement;
        private bool isDisplayDiff = false;
        private bool isWaiting = false;

        [SupplyParameterFromForm]
        private TextInput TextInput { get; set; } = new();

        private string DisplayString
        {
            get
            {
                return isDisplayDiff ? "block" : "none";
            }
        }

        /// <summary>
        /// JS Interopを利用して、jsdiff と diff2html で差分を描画する
        /// </summary>
        private async Task ShowDiff()
        {
            isWaiting = true;
            //前回の結果を初期化
            comment = "";
            modifiedText = "";
            isDisplayDiff = false;

            var oldText = TextInput.OriginalText;
            var tips = TextInput.Hint;

            var test = await OpenAIService.ProofReadText(oldText, tips);
            if (string.IsNullOrEmpty(test.modifiedText) is false)
                isDisplayDiff = await JSRuntime.InvokeAsync<bool>("renderDiff", oldText, test.modifiedText, diffElement);

            comment = test.comment;
            modifiedText = test.modifiedText;
            StateHasChanged();
            isWaiting = false;
        }

        private async Task CopyAsync()
        {
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", modifiedText);
        }
    }
}
