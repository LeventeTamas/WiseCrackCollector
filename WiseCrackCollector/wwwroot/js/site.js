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
function SetWisecrackEditorData(id, saidBy, content, date) {
    document.getElementById('edit_wc_id').value = id;
    document.getElementById('edit_wc_saidBy').value = decodeHtml(saidBy);
    document.getElementById('edit_wc_content').value = decodeHtml(content);
    document.getElementById('edit_wc_date').value = date;
}
function SetWisecrackDeleteID(id) {
    document.getElementById('delete_wc_id').value = id;
}

function SetMembershipDeleteData(userId, userName) {
    document.getElementById('delete_member_user_id').value = userId;
    document.getElementById('delete_member_user_name').innerText = userName;
}

function BoolToChecked(boolValue) {
    return boolValue.toLowerCase() === "true";
}

function SetMembershipEditorData(userId, userName, permRead, permAdd, permUpdate, permDelete, permManageMembers) {
    document.getElementById('edit_membership_user_id').value = userId;
    document.getElementById('edit_membership_perm_read').checked = BoolToChecked(permRead);
    document.getElementById('edit_membership_perm_update').checked = BoolToChecked(permUpdate);
    document.getElementById('edit_membership_perm_add').checked = BoolToChecked(permAdd);
    document.getElementById('edit_membership_perm_delete').checked = BoolToChecked(permDelete);
    document.getElementById('edit_membership_perm_members').checked = BoolToChecked(permManageMembers);
    document.getElementById('edit_membership_user_name').innerText = userName;
}

function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}