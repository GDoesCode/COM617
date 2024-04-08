// Interops for dropdown functions.
var Dropdown = Dropdown || {};

/**
 * Prevent default propagation on click when dropdown is in use,
 * contains logic for changing dropdown classnames at the right time,
 * for opening and closing animations. Will hide the dropdown once
 * the 'animationend' callback is fired.
 * @param {any} dropdownId
 */
Dropdown.register = function (dropdownId, resultsContainer, callbackReference) {
    const elem = dropdownId + " " + resultsContainer;
    const trigger = dropdownId + " button";
    const animated = document.querySelector(dropdownId);

    /**
     * Closes the dropdown when clicks outside of it are detected.
     */
    $(document).click(function (e) {
        var container = $(dropdownId);

        //check if the clicked area is dropDown or not
        if (container.has(e.target).length === 0) {
            if (!$(elem).hasClass("hidden")) {
                $(elem).addClass("closing");
            }
        }
    })

    /**
     * Fires when the trigger is clicked.
     * Toggles the visibility of the results.
     */
    $(trigger).click(function (e) {
        if ($(elem).hasClass("hidden")) {
            $(elem).removeClass("hidden");
        } else {
            $(elem).addClass("closing");
        }
    });

    /** 
     * Listen for 'animationend' event, and hide the element
     * when the closing animation is done.
     */
    animated.addEventListener('animationend', (event) => {
        if (event.animationName.startsWith("closing")) {
            $(elem).addClass("hidden");
            $(elem).removeClass("closing");
            callbackReference.invokeMethodAsync('OnCloseEventReceived')
        }
    });
}