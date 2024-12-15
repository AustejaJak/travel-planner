<template>
  <div class="relative bg-gray-900">
    <div class="flex min-h-full flex-col justify-center py-32 sm:px-6 lg:px-8 z-0">
      <svg
          class="absolute inset-0 z-10 h-full w-full stroke-white/10 [mask-image:radial-gradient(100%_100%_at_top_right,white,transparent)]"
          aria-hidden="true">
        <defs>
          <pattern id="983e3e4c-de6d-4c3f-8d64-b9761d1534cc" width="200" height="200" x="50%" y="-1"
                   patternUnits="userSpaceOnUse">
            <path d="M.5 200V.5H200" fill="none"/>
          </pattern>
        </defs>
        <svg x="50%" y="-1" class="overflow-visible fill-gray-800/20">
          <path d="M-200 0h201v201h-201Z M600 0h201v201h-201Z M-400 600h201v201h-201Z M200 800h201v201h-201Z"
                stroke-width="0"/>
        </svg>
        <rect width="100%" height="100%" stroke-width="0" fill="url(#983e3e4c-de6d-4c3f-8d64-b9761d1534cc)"/>
      </svg>
      <svg viewBox="0 0 1108 632" aria-hidden="true"
           class="absolute top-10 left-[calc(50%-4rem)] z-10 w-[69.25rem] max-w-none transform-gpu blur-3xl sm:left-[calc(50%-18rem)] lg:left-48 lg:top-[calc(50%-30rem)] xl:left-[calc(50%-24rem)]">
        <path fill="url(#175c433f-44f6-4d59-93f0-c5c51ad5566d)" fill-opacity=".2"
              d="M235.233 402.609 57.541 321.573.83 631.05l234.404-228.441 320.018 145.945c-65.036-115.261-134.286-322.756 109.01-230.655C968.382 433.026 1031 651.247 1092.23 459.36c48.98-153.51-34.51-321.107-82.37-385.717L810.952 324.222 648.261.088 235.233 402.609Z"/>
        <defs>
          <linearGradient id="175c433f-44f6-4d59-93f0-c5c51ad5566d" x1="1220.59" x2="-85.053" y1="432.766" y2="638.714"
                          gradientUnits="userSpaceOnUse">
            <stop stop-color="#4F46E5"/>
            <stop offset="1" stop-color="#80CAFF"/>
          </linearGradient>
        </defs>
      </svg>

      <div class="sm:mx-auto sm:w-full sm:max-w-md z-20">
        <!-- Dynamic Title -->
        <h2 class="mt-6 text-center text-3xl font-bold tracking-tight text-white">
          {{ isLogin ? 'Login to your account' : 'Sign up for a new account' }}
        </h2>
      </div>

      <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md z-20">
        <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
          <form @submit.prevent="submitForm" class="space-y-6">
            <!-- Username Field -->
            <div v-if="isLogin || !isLogin">
              <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
              <div class="mt-1">
                <input id="username" name="username" type="text" autocomplete="username" placeholder="Enter your username" required="" v-model="userName"
                       class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"/>
              </div>
            </div>

            <div v-if="!isLogin">
              <label for="email" class="block text-sm font-medium text-gray-700">Email address</label>
              <div class="mt-1">
                <input id="email" name="email" type="email" autocomplete="email" placeholder="Enter your email" required="" v-model="email"
                       class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"/>
              </div>
            </div>

            <div>
              <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
              <div class="mt-1">
                <input id="password" name="password" type="password" autocomplete="current-password" placeholder="Enter your password" required="" v-model="password"
                       class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"/>
              </div>
              <p v-if="error" class="text-red-500 text-sm mt-1">{{ error }}</p>
            </div>

            <div>
              <button type="submit"
                      class="flex w-full justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2">
                {{ isLogin ? 'Login' : 'Sign up' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
    <div v-if="!isLogin">
      <CenteredWithSingleAction v-if="isSuccess" class="z-50" :title="'Registered succesfully'" :text-link="'Go to login page'" :href="'http://localhost:3000/login'"/>
    </div>

  </div>
</template>

<script setup>
import CenteredWithSingleAction from "~/components/application-ui/overlays/modals/centered_with_single_action.vue";
import {checkCurrentRoute} from "~/utils/navigation/checkCurrentRoute.js";
import {useRoute} from 'vue-router';
import {useAuth} from "~/composables/useAuth.js";

const route = useRoute();
const currentRoute = checkCurrentRoute(route);

const isLogin = currentRoute.currentRouteName.value === 'login';
const isSuccess = ref(false);

const userName = ref('');
const email = ref('');
const password = ref('');

const error = ref("");

const validatePassword = () => {
  const formatSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
  const formatUpper = /[A-Z]/;

  if (password.value.length < 8) {
    error.value = "Password must be at least 8 characters long.";
    return false;
  }
  if (!formatSpecialChar.test(password.value) || !formatUpper.test(password.value)) {
    error.value =
        "Password must include at least one special character and one uppercase letter.";
    return false;
  }
  error.value = "";
  return true;
};

const submitForm = async () => {
  if (!isLogin && !validatePassword()) {
    console.error("Password validation failed.");
    return;
  }

  const payload = isLogin
      ? {
        userName: userName.value,
        password: password.value,
      }
      : {
        userName: userName.value,
        email: email.value,
        password: password.value,
      };

  try {
    let response;
    if (isLogin) {
      response = await useAuth().login(payload);
    } else {
      response = await useAuth().register(payload);
    }
    if (response.status === 200) {
      window.location.href = "http://localhost:3000/";
      isSuccess.value = true;
    } else if (!isLogin){
      isSuccess.value = true;
    } else if (response.status === 422) {
      error.value = "Incorrect username or password";
    } else {
      error.value = response.message || "An error occurred.";
    }
  } catch (err) {
    console.error("API call failed:", err);
  }
};
</script>
