window.renderDiff = (oldText, newText, targetElement) => {
    // Unified Diff形式の文字列を生成
    var diff = Diff.createPatch("diff", oldText, newText);

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