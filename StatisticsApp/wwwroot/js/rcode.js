function saveRScript() {
    var cm = $('.CodeMirror')[0].CodeMirror;
    var code = $(cm.getWrapperElement()).get
    var code = code.replace(/\n/g, "\r\n");
    var codeAsBlob = new Blob([code], { type: 'text/plain' });
    var fileName = "RScriptChanged.r";

    var downloadLink = document.createElement("a");
    downloadLink.download = fileName;
    downloadLink.innerHTML = "LINKTITLE";    
    downloadLink.href = window.URL.createObjectURL(codeAsBlob);
    downloadLink.onclick = destroyClickedElement;
    downloadLink.style.display = "none";
    document.body.appendChild(downloadLink);
    downloadLink.click();
}

function destroyClickedElement(event) {
    document.body.removeChild(event.target);
}
 