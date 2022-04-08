mergeInto(LibraryManager.library, {
  OpenGame: function () {
    window.addEventListener("message", function(event) {
      if (event.data === "Game Open") {
        SendMessage("[Bridge]", "ReceiveMessageFromPage", "Game Open");
      }
    });
  }
});
