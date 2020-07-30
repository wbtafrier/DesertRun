mergeInto(LibraryManager.library, {
  GameLoaded: function () {
     window.parent.postMessage("Game Loaded", "*");
  }
});
