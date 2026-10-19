import { test, expect } from '@playwright/test';
import { addProductToCart, emptyCart } from './cart-helpers';

// نشانی‌های زیر با دادهٔ نمونهٔ کاربران در src/Identity.API/UsersSeed.cs هم‌خوان‌اند.
const PRODUCT_NAME = 'ساعت مچی جی‌پی‌اس ادونچرر';
const DEMO_STREET = 'خیابان آزادی، پلاک ۱۲۳';
const DEMO_CITY = 'تهران';

test('place an order through checkout', async ({ page }) => {
  await emptyCart(page);

  await page.goto('/user/orders');
  await expect(page.getByRole('heading', { name: 'سفارش‌ها', exact: true })).toBeVisible();
  const existingOrderCount = await page.locator('.order-number').count();

  await addProductToCart(page, PRODUCT_NAME);
  await page.goto('/cart');
  await expect(page.getByRole('heading', { name: 'سبد خرید', exact: true })).toBeVisible();
  await page.getByRole('link', { name: 'تسویه حساب' }).click();

  await expect(page.getByRole('heading', { name: 'تسویه حساب' })).toBeVisible();
  await expect(page.getByLabel('نشانی')).toHaveValue(DEMO_STREET);
  await expect(page.getByLabel('شهر')).toHaveValue(DEMO_CITY);
  await page.getByRole('button', { name: 'ثبت سفارش' }).click();

  await expect(page).toHaveURL(/\/user\/orders$/);
  await expect(page.getByRole('heading', { name: 'سفارش‌ها', exact: true })).toBeVisible();
  await expect(page.locator('.order-number')).toHaveCount(existingOrderCount + 1);
});
