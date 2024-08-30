function updatePrice() {
    let total = 0;

    document.querySelectorAll('.basket-container-dishes-items-item').forEach(item => {
        const pricePerOne = parseFloat(item
            .querySelector('.basket-container-dishes-items-item-text-price-perone')
            .textContent);

        const quantity = parseInt(item
            .querySelector('.basket-container-dishes-items-item-text-quantity')
            .textContent);

        item.querySelector('.basket-container-dishes-items-item-text-price')
            .textContent = "$" + (pricePerOne * quantity).toFixed(2);

        total += Number(pricePerOne * quantity);
    });
    document.querySelector('.basket-container-dishes-items-total-price')
        .textContent = total.toFixed(2);
}

document.addEventListener("DOMContentLoaded", updatePrice);

function selectPay(value) {
    switch (value) {
        case 1:
            document.querySelector('.basket-container-payment-block-pay-types-one').style.backgroundColor = "#0D161B";
            document.querySelector('.basket-container-payment-block-pay-types-two').style.backgroundColor = "#000A0F";
            document.querySelector('.basket-container-payment-block-pay-form').style.zIndex = -1;
            break;
        case 2:
            document.querySelector('.basket-container-payment-block-pay-types-one').style.backgroundColor = "#000A0F";
            document.querySelector('.basket-container-payment-block-pay-types-two').style.backgroundColor = "#0D161B";
            document.querySelector('.basket-container-payment-block-pay-form').style.zIndex = 1;
            break;
    }
}
