document.addEventListener('DOMContentLoaded', function () {
    var priceRange = document.getElementById('priceRange');
    var priceDisplay = document.getElementById('priceDisplay');

    priceDisplay.textContent = formatPrice(priceRange.value);

    priceRange.addEventListener('input', function () {
        priceDisplay.textContent = formatPrice(priceRange.value);
    });

    function formatPrice(price) {
        return 'VNĐ: ' + (price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' ₫');
    }
});