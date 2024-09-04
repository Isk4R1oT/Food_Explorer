function changeQuantity(button, change) {
    const form = button.closest('form');

    const countDisplay = form.querySelector('.catalog-section-slider-items-item-actions-count');

    const countInput = form.querySelector('.catalog-section-slider-items-item-actions-count-value');

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
}
