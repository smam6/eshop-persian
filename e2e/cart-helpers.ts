import { expect, Page } from '@playwright/test';

// رابط کاربری وب‌اپ فارسی است؛ انتخابگرها بر پایهٔ متن‌های فارسیِ resx (SharedResources.fa-IR) تنظیم شده‌اند.
export async function addProductToCart(page: Page, productName: string) {
  await page.goto('/');
  await page.getByRole('link', { name: productName }).click();
  await expect(page.getByRole('heading', { name: productName })).toBeVisible();
  await page.getByRole('button', { name: 'افزودن به سبد خرید' }).click();
  await expect(page.locator('.in-cart').getByRole('link', { name: 'سبد خرید' })).toBeVisible();
}

/** Remove the first cart line via its Remove control, waiting for the update to post. */
export async function removeFirstCartItem(page: Page) {
  const updateRequest = page.waitForRequest(request =>
    request.method() === 'POST' && new URL(request.url()).pathname === '/cart');

  await Promise.all([
    updateRequest,
    page.getByRole('button', { name: /^حذف/ }).first().click(),
  ]);
}

export async function emptyCart(page: Page) {
  await page.goto('/cart');
  await expect(page.getByRole('heading', { name: 'سبد خرید', exact: true })).toBeVisible();

  const quantities = page.locator('[data-cart-quantity-input]');
  while (await quantities.count() > 0) {
    const previousCount = await quantities.count();
    await removeFirstCartItem(page);
    await expect(quantities).toHaveCount(previousCount - 1, { timeout: 15_000 });
  }

  await expect(page.getByText('سبد خرید شما خالی است')).toBeVisible();
}
