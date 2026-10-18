import { test, expect } from '@playwright/test';
import { addProductToCart, emptyCart, removeFirstCartItem } from './cart-helpers';

// «ساعت مچی جی‌پی‌اس ادونچرر» (Id 99) با قیمت ۲۰٬۰۰۰٬۰۰۰ تومان در دادهٔ نمونهٔ فارسی.
const PRODUCT_NAME = 'ساعت مچی جی‌پی‌اس ادونچرر';
const UNIT_PRICE_TOMAN = 20_000_000;

test('add, update, and remove an item from the cart', async ({ page }) => {
  await emptyCart(page);
  await addProductToCart(page, PRODUCT_NAME);

  await page.goto('/cart');
  await expect(page.getByRole('heading', { name: 'سبد خرید', exact: true })).toBeVisible();
  await expect(page.getByText(PRODUCT_NAME)).toBeVisible();

  const quantity = page.locator('[data-cart-quantity-input]');
  await expect(quantity).toHaveValue('1');

  const updateRequest = page.waitForRequest(request =>
    request.method() === 'POST' && new URL(request.url()).pathname === '/cart');
  await Promise.all([
    updateRequest,
    page.getByRole('button', { name: /^افزایش تعداد/ }).first().click(),
  ]);
  await expect(quantity).toHaveValue('2', { timeout: 15_000 });
  // قیمت با ارقام فارسی و جداکنندهٔ هزارگان فارسی نمایش داده می‌شود.
  const expectedTotal = new Intl.NumberFormat('fa-IR').format(UNIT_PRICE_TOMAN * 2);
  await expect(page.locator('.cart-summary-total')).toContainText(expectedTotal, { timeout: 15_000 });

  await removeFirstCartItem(page);
  await expect(page.getByText('سبد خرید شما خالی است')).toBeVisible({ timeout: 15_000 });
});
