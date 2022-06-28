window.addEventListener('load', (event) => {
  var inspectorMenu = document.querySelector(".wy-menu ul li:nth-child(1)")

  if (!inspectorMenu.classList.contains("current")) {
    inspectorMenu.classList.add("current")
  }
});
