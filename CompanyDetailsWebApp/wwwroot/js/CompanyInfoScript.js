function showDiv(targetDivID) {
    //document.getElementById('IdToHide').style.display = 'none';
    //$(`#${targetDivID}`).html("Changed");
    //$(`#${targetDivID}`).toggle();
    $(`#IdToHide`).toggle();

    document.getElementById(targetDivID).style.display = 'none';
}

