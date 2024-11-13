function showDiv(targetDivID) {
    //document.getElementById('IdToHide').style.display = 'none';
    //$(`#${targetDivID}`).html("Changed");
    //$(`#${targetDivID}`).toggle();
    $(`#IdToHide`).toggle();

    document.getElementById(targetDivID).style.display = 'none';
}

function toggleDiv(identifier) {

    $("#targetDiv_" + identifier).slideToggle(400); // transition with sliding effect
    //$("#targetDiv_" + identifier).toggle(); // normal toggle without transition effect
}