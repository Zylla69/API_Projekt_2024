document.addEventListener("DOMContentLoaded", function () {
    // Selektieren der benötigten DOM-Elemente
    const addNoteButton = document.querySelector(".add-note-button");
    const noteForm = document.querySelector(".note-form");
    const noteList = document.querySelector(".note-list");
    const deleteNoteButton = document.getElementById("delete-note-button");
    let currentNoteId = null;

    // Funktion zum Laden aller Notizen beim Start des Servers
    loadAllNotes();

    // Eventlistener für den Hinzufügen-Knopf
    addNoteButton.addEventListener("click", function () {
        showNoteForm();
        resetForm();
    });

    // Eventlistener für den Löschen-Knopf
    deleteNoteButton.addEventListener("click", function () {
            deleteNote();
    });

    // Eventlistener für das Absenden des Formulars
    noteForm.addEventListener("submit", function (event) {
        event.preventDefault();
        const noteTitle = document.getElementById("note-title").value;
        const noteContent = document.getElementById("note-content").value;
        if (currentNoteId) {
            updateNote(currentNoteId, noteTitle, noteContent);
        } else {
            saveNote(noteTitle, noteContent);
        }
    });

    // Event Listener für das Klicken des Abbrechen-Buttons
    const cancelButton = document.querySelector(".note-form button[type='button']");
    cancelButton.addEventListener("click", function () {
        hideNoteForm();
        resetForm();
    });

    // Funktionen für die Anzeige, das Ausblenden und das Zurücksetzen des Formulars
    function showNoteForm() {
        noteForm.style.display = "block";
    }

    function hideNoteForm() {
        noteForm.style.display = "none";
    }

    function resetForm() {
        currentNoteId = null;
        document.getElementById("note-title").value = "";
        document.getElementById("note-content").value = "";
        deleteNoteButton.style.display = "none";
    }

    // Funktion zum Speichern einer neuen Notiz über AJAX
    function saveNote(title, content) {
        const xhr = new XMLHttpRequest();
        xhr.open("POST", "http://localhost:8080/api/notiz", true);
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 201) {
                    const response = JSON.parse(xhr.responseText);
                    displayNote(response.id, response.title, response.text);
                    hideNoteForm();
                    resetForm();
                    loadAllNotes(); // Hier wird loadAllNotes aufgerufen
                } else {
                    console.error("Fehler beim Speichern der Notiz.");
                }
            }
        };

        const data = JSON.stringify({ title: title, text: content });
        xhr.send(data);
    }

    // Funktion zum Aktualisieren einer vorhandenen Notiz über AJAX
    function updateNote(id, title, content) {
        const xhr = new XMLHttpRequest();
        xhr.open("PUT", `http://localhost:8080/api/notiz`, true);
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    const response = JSON.parse(xhr.responseText);
                    const noteItem = document.getElementById(id);
                    noteItem.querySelector("h3").textContent = response.title;
                    hideNoteForm();
                    resetForm();
                    loadAllNotes(); // Hier wird loadAllNotes aufgerufen
                } else {
                    console.error("Fehler beim Aktualisieren der Notiz.");
                }
            }
        };

        const data = JSON.stringify({ id: id, title: title, text: content });
        xhr.send(data);
    }

    // Funktion zum Löschen einer Notiz über AJAX
    function deleteNote() {
        if (!currentNoteId) return;

        const xhr = new XMLHttpRequest();
        xhr.open("DELETE", `http://localhost:8080/api/notiz/${currentNoteId}`, true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    const noteItem = document.getElementById(currentNoteId);
                    noteItem.remove();
                    hideNoteForm();
                    resetForm();
                    loadAllNotes(); // Hier wird loadAllNotes aufgerufen
                } else {
                    console.error("Fehler beim Löschen der Notiz.");
                }
            }
        };

        xhr.send();
    }

    // Funktion zum Anzeigen einer einzelnen Notiz in der Notizliste
    function displayNote(id, title, text) {
        const noteItem = document.createElement("li");
        noteItem.id = id;
        noteItem.innerHTML = `
        <div class="note" style="border: 1px solid white; border-radius: 10px; margin: 0 auto; width: 50%; text-align: center;" onclick="editNote('${id}', '${title}', \`${text.replace(/`/g, '\\`')}\`)">
            <h3>${title}</h3>
        </div>
    `;
        noteList.appendChild(noteItem);
    }


    // Funktion zum Laden aller Notizen über AJAX beim Start des Servers
    function loadAllNotes() {
        const xhr = new XMLHttpRequest();
        xhr.open("GET", "http://localhost:8080/api/notizen", true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    const notes = JSON.parse(xhr.responseText);
                    noteList.innerHTML = ""; // Clear existing notes
                    notes.forEach(note => {
                        displayNote(note.id, note.title, note.text);
                    });
                } else {
                    console.error("Fehler beim Laden der Notizen.");
                }
            }
        };

        xhr.send();
    }

    // Globale Funktion für das Bearbeiten einer Notiz
    window.editNote = function (id, title, text) {
        currentNoteId = id;
        document.getElementById("note-title").value = title;
        document.getElementById("note-content").value = text;
        showNoteForm();
        deleteNoteButton.style.display = "inline";
    };
});
