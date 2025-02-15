//window.renderDiff = (diffText) => {
//    // Diff2Html を使って、Unified Diff テキストから HTML を生成
//    var diffHtml = Diff2Html.html(diffText, {
//        inputFormat: 'diff',
//        drawFileList: false,
//        matching: 'lines',
//        outputFormat: 'side-by-side' // 'side-by-side' も選択可能
//    });
//    // 生成した HTML を、id="diffContainer" の要素にセット
//    document.getElementById("diffContainer").innerHTML = diffHtml;
//};

window.renderDiff = (oldText, newText) => {
    // jsdiff の createTwoFilesPatch 関数を使い、Unified Diff形式の文字列を生成
    // ※ グローバル変数は「Diff」として読み込まれている
    var diff = Diff.createPatch("testtext", oldText, newText);

    //// diff2html を使って、Unified Diff文字列からHTMLを生成
    //var diffHtml = Diff2Html.html(diff, {
    //    drawFileList: false,         // ファイル一覧を非表示（文章差分の場合は不要）
    //    matching: 'words',           // 行単位の比較
    //    outputFormat: 'side-by-side' // サイドバイサイド形式の場合は 'side-by-side' も選択可能
    //});

    //// 結果を、id="diffContainer" の要素に挿入
    //document.getElementById("diffContainer").innerHTML = diffHtml;


    const targetElement = document.getElementById('diffContainer');
    const configuration = {
        drawFileList: false,
        matching: 'lines',
        highlight: true,
        outputFormat: 'side-by-side',
        fileContentToggle: false
    };
    const diff2htmlUi = new Diff2HtmlUI(targetElement, diff, configuration);
    diff2htmlUi.draw();
    diff2htmlUi.highlightCode();
};