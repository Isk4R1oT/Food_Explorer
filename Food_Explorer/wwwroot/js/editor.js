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
        const ingredientItem = document.createElement('span');
        ingredientItem.className = 'form-field-ingridients-item';
        ingredientItem.innerHTML = `
            <p class="roboto-regular">${ingredientName}</p>
            <img src="~/img/Close.svg" alt="" class="remove-ingredient">
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
