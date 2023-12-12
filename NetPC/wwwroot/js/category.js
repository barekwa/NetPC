function toggleSubcategoryFields() {
    var categorySelect = document.querySelector(".kategoria");
    var subcategoryLista = document.getElementById("PodkategoriaLista");
    var subcategoryInny = document.getElementById("PodkategoriaInny");

    var subcategoryListSelect = subcategoryLista.querySelector("select");
    var subcategoryInnyInput = subcategoryInny.querySelector("input");

    if (categorySelect.value == "Służbowy") {
        subcategoryLista.style.display = "block";
        subcategoryInny.style.display = "none";
        subcategoryInnyInput.disabled = true;
        subcategoryListSelect.disabled = false;
    } 
    else if(categorySelect.value == "Inny") {
        subcategoryLista.style.display = "none";
        subcategoryInny.style.display = "block";
        subcategoryInnyInput.disabled = false;
        subcategoryListSelect.disabled = true;
    }
    else {
        subcategoryLista.style.display = "none";
        subcategoryInny.style.display = "none";
        subcategoryInnyInput.disabled = true;
        subcategoryListSelect.disabled = true;
    }
}

toggleSubcategoryFields();
