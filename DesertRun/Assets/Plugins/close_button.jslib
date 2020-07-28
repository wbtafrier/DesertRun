mergeInto(LibraryManager.library, {
  QuitAnimation: function () {
    /* var ifrm = document.getElementById('unityContainer');
    if (ifrm !== null) {
        console.log("POST: Game Exit");
        ifrm.contentWindow.postMessage("Game Exit", "*");
    }
    else {
        console.log('Iframe does not exist when executing JS.');
    } */
    onDesertRunMessage("Game Exit");
  }
});