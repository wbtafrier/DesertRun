mergeInto(LibraryManager.library, {
  QuitAnimation: function () {
     window.parent.postMessage("Game Exit", "*");
  }
});
