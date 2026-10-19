import { test, expect } from '@playwright/test';

// نام‌ها و قیمت‌ها با دادهٔ نمونهٔ فارسی (src/Catalog.API/Setup/catalog.json) هم‌خوان‌اند.
const PRODUCT_NAME = 'ساعت مچی جی‌پی‌اس ادونچرر'; // Id 99

test('browse catalog pages and view product details', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByRole('heading', { name: 'آمادهٔ یک ماجراجویی جدید هستید؟' })).toBeVisible();
  await page.getByRole('link', { name: PRODUCT_NAME }).click();
  await expect(page.getByRole('heading', { name: PRODUCT_NAME })).toBeVisible();

  await page.goto('/');
  await page.getByRole('link', { name: '۲', exact: true }).click();
  await expect(page).toHaveURL(/\?page=2$/);
  await expect(page.locator('.catalog-product')).toHaveCount(9);
});
