mergeInto(LibraryManager.library, {
  QuitAnimation: function () {
    // Call this code in order when the user clicks the close button
    /* $("#background-image, #desert_run, .navbar-nav").css({"animation": "fadeOutBackground 1s ease-in-out none", "-webkit-animation": "fadeOutBackground 1s ease-in-out none"});
    $("#background-image, #desert_run, .navbar-nav").on("webkitAnimationEnd oAnimationEnd", function() {
      $("#background-image, #desert_run, .navbar-nav").off("webkitAnimationEnd oAnimationEnd");
      $("#desert_run").css({"display": "none", "animation": "none"});
      $("#relo_character").css({"animation": "fallDown 1s forwards", "-webkit-animation": "fallDown 1s forwards"});
      $("#background-image, .navbar-nav").css({"animation": "fadeInBackground 1s ease-in-out none", "-webkit-animation": "fadeInBackground 1s ease-in-out none"});
    });
    $("#relo_character").on("webkitAnimationEnd oAnimationEnd", function() {
      $("#relo_character").attr("src", "/static/images/relo-character.jpg");
      $("#relo_character").on("load", function() {
        $("#relo_character").css({"animation": "bounce 0.6s none infinite alternate", "-webkit-animation": "bounce 0.6s none infinite alternate"});
        $("#relo_character").off("webkitAnimationEnd oAnimationEnd");
      });
    }); */
    window.parent.postMessage("Game Exit", "*");
  }
});