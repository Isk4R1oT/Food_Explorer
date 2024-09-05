let currentIndexFood = 0;
let currentIndexDessert = 0;
let currentIndexDrinks = 0;

function moveFoodSlide(direction) {
    const slider = document.getElementById('food-slider');
    const totalSlides = slider.querySelectorAll('.catalog-section-slider-items-item').length;

    currentIndexFood += direction;

    if (currentIndexFood < -1) {
        currentIndexFood = totalSlides - 2;
    } else if (currentIndexFood >= totalSlides - 1) {
        currentIndexFood = 0;
    }

    const offset = -currentIndexFood * 304;
    slider.style.transform = `translateX(${offset}px)`;
}

function moveDessertSlide(direction) {
    const slider = document.getElementById('desserts-slider');
    const totalSlides = slider.querySelectorAll('.catalog-section-slider-items-item').length;

    currentIndexDessert += direction;

    if (currentIndexDessert < -1) {
        currentIndexDessert = totalSlides - 2;
    } else if (currentIndexDessert >= totalSlides - 1) {
        currentIndexDessert = 0;
    }

    const offset = -currentIndexDessert * 304;
    slider.style.transform = `translateX(${offset}px)`;
}

function moveDrinksSlide(direction) {
    const slider = document.getElementById('drinks-slider');
    const totalSlides = slider.querySelectorAll('.catalog-section-slider-items-item').length;

    currentIndexDrinks += direction;

    if (currentIndexDrinks < -1) {
        currentIndexDrinks = totalSlides - 2;
    } else if (currentIndexDrinks >= totalSlides - 1) {
        currentIndexDrinks = 0;
    }

    const offset = -currentIndexDrinks * 304;
    slider.style.transform = `translateX(${offset}px)`;
}