function changeQuantity(button, change) {
    const form = button.closest('form');

    const countDisplay = form.querySelector('.product-container-info-text-actions-count');

    const countInput = form.querySelector('.product-container-info-text-actions-count-value');

    let currentCount = parseInt(countDisplay.textContent, 10);
    currentCount += change;

    if (currentCount < 1) {
        currentCount = 1;
    }

    if (currentCount > 10) {
        currentCount = 10;
    }

    countDisplay.textContent = currentCount.toString().padStart(2, '0');
    countInput.value = currentCount;

    const pricePerOne = form.querySelector('.product-container-info-text-actions-price-perone').textContent;

    form.querySelector('.product-container-info-text-actions-button-total').textContent = Number(pricePerOne * currentCount).toFixed(2);

}
