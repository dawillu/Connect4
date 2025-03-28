// number of rows
var rows = 6;
var cols = 7;

// <div class="field"></div>
var field = document.createElement('div');
field.setAttribute('id', 'field');
field.setAttribute('class', 'field');
field.setAttribute('style', 'visibility: hidden');

for (let row = 0; row < rows; row++) {
    for (let col = 0; col < cols; col++) {
        // <div id="??" class="cell" onclick="PlayerMove(id)"></div>
        var cell = document.createElement('div');
        cell.setAttribute('id', `${row} ${col}`);
        cell.setAttribute('class', 'cell');
        cell.setAttribute('onclick', `PlayerMove('${row} ${col}')`);

        // insert the cell in the field
        field.appendChild(cell);
    }
}

// output
document.body.appendChild(field);