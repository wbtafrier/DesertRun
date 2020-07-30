mergeInto(LibraryManager.library, {
  OpenGame: function () {
    window.addEventListener("message", function(event) {
        if (event.origin === "http://localhost:5000" || event.origin === "https://melorelo-staging.herokuapp.com" || event.origin === "https://melorelo.com") {
            if (event.data === "Game Open") {
                unityInstance.SendMessage("[Bridge]", "ReceiveMessageFromPage", "Game Open");
            }
        }
    });
  }
});
