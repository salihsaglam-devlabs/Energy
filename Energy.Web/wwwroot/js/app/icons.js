(function (window) {
    "use strict";

    // Canonical list of icon names available in the active DevExtreme Fluent
    // theme (dx.fluent.energy-custom-scheme.css). Used to populate the icon
    // picker on the Menus screen so administrators can only choose icons that
    // actually render. Keep in sync with the theme's dx-icon-* glyphs.
    var icons = [
        "accountbox", "activefolder", "add", "addcirclefilled", "addcircleoutline", "addcolumnleft", "addcolumnright", "addrowabove",
        "addrowbelow", "addtable", "addtableheader", "airplane", "aligncenter", "alignjustify", "alignleft", "alignright",
        "apply", "arrowback", "arrowdown", "arrowleft", "arrowright", "arrowsortdown", "arrowsortup", "arrowup",
        "attach", "back", "background", "bell", "belloutline", "blockquote", "bmpfile", "bold",
        "bookmark", "botfilled", "botoutline", "box", "bulletlist", "calendardateendfilled", "calendardateendoutline", "calendardatestartfilled",
        "calendardatestartoutline", "car", "card", "cardcontent", "cart", "cellproperties", "chart", "chat",
        "chatadd", "chatsparklefilled", "chatsparkleoutline", "check", "checklist", "checkmarkcircle", "checkmarkcirclefilled", "chevrondoubleleft",
        "chevrondoubleright", "chevrondown", "chevronleft", "chevronnext", "chevronprev", "chevronright", "chevronup", "clear",
        "clearcircle", "clearformat", "clearsquare", "clipboardpastesparkle", "clipboardtasklist", "clock", "close", "codeblock",
        "coffee", "collapse", "color", "colordismiss", "column", "columnchooser", "columnfield", "columnproperties",
        "comment", "conferenceroomfilled", "conferenceroomoutline", "contains", "contentlayout", "context", "copy", "copyfilled",
        "csv", "cursormove", "cursorprohibition", "cut", "dataarea", "dataareafilled", "dataareaoutline", "databarfilled",
        "databaroutline", "datadoughnutfilled", "datadoughnutoutline", "datafield", "datalinefilled", "datalineoutline", "datapie", "datapiefilled",
        "datapieoutline", "datastackedbarfilled", "datastackedbaroutline", "datatrending", "datausage", "daterangepicker", "decreaseindent", "decreaselinespacing",
        "deletecolumn", "deleterow", "deletetable", "description", "detailslayout", "diagram", "doc", "docfile",
        "docxfile", "doesnotcontain", "download", "dragvertical", "dropzone", "edit", "edittableheader", "email",
        "endswith", "equal", "errorcircle", "event", "eventall", "expand", "expandform", "export",
        "exportpdf", "exportselected", "exportxlsx", "eyeclose", "eyeopen", "favorites", "fieldchooser", "fields",
        "file", "fill", "filter", "find", "fix", "fixcolumn", "fixcolumnleft", "fixcolumnright",
        "floppy", "folder", "font", "fontsize", "food", "formula", "fullscreen", "gift",
        "globe", "greater", "greaterorequal", "group", "groupbycolumn", "growfont", "handlehorizontal", "handlevertical",
        "header", "help", "hidepanel", "hierarchy", "home", "image", "imagethumbnail", "imgarlock",
        "imgarunlock", "import", "importselected", "inactivefolder", "increaseindent", "increaselinespacing", "indent", "indeterminatestate",
        "info", "insertcolumnleft", "insertcolumnright", "insertrowabove", "insertrowbelow", "inserttable", "isblank", "isnotblank",
        "italic", "jpgfile", "key", "less", "lessorequal", "like", "link", "lock",
        "login", "map", "mediumiconslayout", "mention", "menu", "mergecells", "message", "micfilled",
        "micoutline", "minus", "money", "moon", "more", "movetofolder", "music", "newfolder",
        "notequal", "optionsfilled", "optionsgear", "optionsoutline", "orderedlist", "ordersbox", "overflow", "packagebox",
        "palette", "panelleft", "panelright", "parentfolder", "paste", "pasteplaintext", "pdffile", "percent",
        "photo", "photooutline", "pin", "pinleft", "pinmap", "pinright", "plus", "pptfile",
        "pptxfile", "preferences", "print", "product", "pulldown", "range", "ratingfilled", "ratingoutline",
        "redo", "refresh", "remove", "removecolumn", "removerow", "removetable", "rename", "repeat",
        "restore", "return", "revert", "right", "rotation", "rowfield", "rowproperties", "rtffile",
        "runner", "save", "search", "selectall", "send", "sendfilled", "servicebell", "share",
        "showpanel", "shrinkfont", "smalliconslayout", "sortdown", "sortdowntext", "sorted", "sortup", "sortuptext",
        "sparkle", "spindown", "spinleft", "spinnext", "spinprev", "spinright", "spinup", "splitcells",
        "square", "startswith", "stick", "stickcolumn", "stopfilled", "stopoutline", "strike", "subscript",
        "sun", "superscript", "svgfile", "tableproperties", "tags", "taskcomplete", "taskhelpneeded", "taskinprogress",
        "taskrejected", "taskstop", "tel", "textdocument", "tips", "to", "today", "todo",
        "toggle", "toolbox", "trash", "triangledown", "triangleleft", "triangleright", "triangleup", "txtfile",
        "underline", "undo", "unfix", "unfixcolumn", "ungroupallcolumns", "ungroupcolumn", "unlock", "unpin",
        "unselectall", "upload", "user", "variable", "verticalalignbottom", "verticalaligncenter", "verticalaligntop", "video",
        "warning", "with", "xlsfile", "xlsxfile", "zoominfilled", "zoominoutline", "zoomoutfilled", "zoomoutoutline"
    ];

    window.AppIcons = icons;
})(window);

