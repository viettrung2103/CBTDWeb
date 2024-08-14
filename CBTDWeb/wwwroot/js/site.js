// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
import DataTable from 'datatables.net-dt';
import 'datatables.net-responsive-dt';

let table = new DataTable('#myTable', {
    responsive: true,
    search: {
        return true
    }
});

$(document).ready(function () {
    $('#myTable').DataTable();
});