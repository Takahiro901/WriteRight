window.renderDiff = (oldText, newText, targetElement) => {

    const diff = Diff.diffChars(oldText, newText);
    const hasDifference = diff.some(part => part.added || part.removed); //文章に差異があるかどうかを判断

    if (hasDifference) {
        // Unified Diff形式の文字列を生成
        var diffString = Diff.createPatch("diff", oldText, newText);

        const configuration = {
            drawFileList: false,
            matching: 'lines',
            highlight: true,
            outputFormat: 'side-by-side',
            fileContentToggle: false
        };
        const diff2htmlUi = new Diff2HtmlUI(targetElement, diffString, configuration);
        diff2htmlUi.draw();
        diff2htmlUi.highlightCode();
        return true;
    }
    else {
        return false;
    }
    
};