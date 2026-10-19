import { test as setup, expect } from '@playwright/test';
import { STORAGE_STATE } from '../playwright.config';
import { assert } from 'console';

assert(process.env.USERNAME1, 'USERNAME1 is not set');
assert(process.env.PASSWORD, 'PASSWORD is not set');

setup('Login', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByRole('heading', { name: 'آمادهٔ یک ماجراجویی جدید هستید؟' })).toBeVisible();

  await page.getByLabel('ورود به حساب').click();
  await expect(page.getByRole('heading', { name: 'ورود' })).toBeVisible();

  await page.getByPlaceholder('نام کاربری').fill(process.env.USERNAME1!);
  await page.getByPlaceholder('گذرواژه').fill(process.env.PASSWORD!);
  await page.getByRole('button', { name: 'ورود' }).click();
  await expect(page.getByRole('heading', { name: 'آمادهٔ یک ماجراجویی جدید هستید؟' })).toBeVisible();
  await page.context().storageState({ path: STORAGE_STATE });
});
