//логика файла
const fileField = document.querySelector('.form-field-file');
const hiddenInput = document.getElementById('imageInput');

fileField.addEventListener('click', function() {
    hiddenInput.click();
});

//логика ингридиентов
const ingredientsContainer = document.querySelector('.form-field-ingridients');
const addItemButton = document.querySelector('.form-field-ingridients-item-add');
const ingredientsInput = document.getElementById('ingredientsList');

addItemButton.addEventListener('click', function () {
    const ingredientName = prompt("Введите название ингредиента:"); 
    if (ingredientName) {
        const removeImage = "PHN2ZyB3aWR0aD0iMTgiIGhlaWdodD0iMTgiIHZpZXdCb3g9IjAgMCAxOCAxOCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZmlsbC1ydWxlPSJldmVub2RkIiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGQ9Ik0wLjI2MzYwNCAwLjI2MzgzM0MwLjYxNTA3NiAtMC4wODc2MzkxIDEuMTg0OTIgLTAuMDg3NjM5MSAxLjUzNjQgMC4yNjM4MzNMOSA3LjcyNzQ0TDE2LjQ2MzYgMC4yNjM4MzNDMTYuODE1MSAtMC4wODc2MzkxIDE3LjM4NDkgLTAuMDg3NjM5MSAxNy43MzY0IDAuMjYzODMzQzE4LjA4NzkgMC42MTUzMDUgMTguMDg3OSAxLjE4NTE1IDE3LjczNjQgMS41MzY2M0wxMC4yNzI4IDkuMDAwMjNMMTcuNzM2NCAxNi40NjM4QzE4LjA4NzkgMTYuODE1MyAxOC4wODc5IDE3LjM4NTIgMTcuNzM2NCAxNy43MzY2QzE3LjM4NDkgMTguMDg4MSAxNi44MTUxIDE4LjA4ODEgMTYuNDYzNiAxNy43MzY2TDkgMTAuMjczTDEuNTM2NCAxNy43MzY2QzEuMTg0OTIgMTguMDg4MSAwLjYxNTA3NiAxOC4wODgxIDAuMjYzNjA0IDE3LjczNjZDLTAuMDg3ODY4IDE3LjM4NTIgLTAuMDg3ODY4IDE2LjgxNTMgMC4yNjM2MDQgMTYuNDYzOEw3LjcyNzIxIDkuMDAwMjNMMC4yNjM2MDQgMS41MzY2M0MtMC4wODc4NjggMS4xODUxNSAtMC4wODc4NjggMC42MTUzMDUgMC4yNjM2MDQgMC4yNjM4MzNaIiBmaWxsPSJ3aGl0ZSIvPgo8L3N2Zz4K";
        const ingredientItem = document.createElement('span');
        ingredientItem.className = 'form-field-ingridients-item';
        ingredientItem.innerHTML = `
            <p class="roboto-regular">${ingredientName}</p>
            <img src="data:image/svg+xml;base64,${removeImage}" alt="" class="remove-ingredient">
        `;
        ingredientsContainer.insertBefore(ingredientItem, addItemButton);
        updateIngredientsList();

        ingredientItem.querySelector('.remove-ingredient').addEventListener('click', function () {
            ingredientItem.remove();
            updateIngredientsList(); 
        });
    }
});

function updateIngredientsList() {
    const ingredients = Array.from(ingredientsContainer.children)
        .filter(item => item.classList.contains('form-field-ingridients-item'))
        .map(item => item.querySelector('p').textContent);
        ingredientsInput.value = ingredients.join(',');
}

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.remove-ingredient').forEach(button => {
        button.addEventListener('click', function () {
            const ingredientItem = this.closest('.form-field-ingridients-item');
            if (ingredientItem) {
                ingredientItem.remove();
            }
            updateIngredientsList()
        });
    });
    updateIngredientsList()
});