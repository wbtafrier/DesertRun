mergeInto(LibraryManager.library, {
  QuitAnimation: function () {
   window.addEventListener("message onmessage", function(event) {
    if (event.origin === "http://localhost:5000" || event.origin === "https://melorelo-staging.herokuapp.com" || event.origin === "https://melorelo.com") {
        if (event.data === "Game Open") {
            // Call function to start the music and sound and any other proceduers necessary for when the game is back in display
        }
    }
});
  }
});
