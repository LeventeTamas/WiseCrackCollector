// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SetGroupDeleteID(id, name) {
    document.getElementById('delete_group_id').value = id;
    document.getElementById('delete_group_name').innerHTML = decodeHtml(name);
}
function SetGroupEditorData(id, name, owner, createdAt) {
    document.getElementById('edit_group_id').value = id;
    document.getElementById('edit_group_name').value = decodeHtml(name);
    document.getElementById('edit_group_owner').value = decodeHtml(owner);
    document.getElementById('edit_group_date').value = createdAt;
}
function SetWisecrackEditorData(id, author, content, date) {
    document.getElementById('wc_edit_id').value = id;
    document.getElementById('wc_edit_author').value = decodeHtml(author);
    document.getElementById('wc_edit_content').value = decodeHtml(content);
    document.getElementById('wc_edit_date').value = date;
}
function SetWisecrackDeleteID(id) {
    document.getElementById('wc_delete_id').value = id;
}

function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}