<template>
  <div v-if="loading"></div>

  <Disclosure v-else as="nav" class="bg-gray-900" v-slot="{ open }">
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div class="absolute z-20 w-full max-w-7xl">
        <div class="flex h-16 justify-between">
          <div class="flex">
            <div class="-ml-2 mr-2 flex items-center md:hidden">
              <!-- Mobile menu button -->
              <DisclosureButton
                  class="inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white">
                <span class="sr-only">Open main menu</span>
                <Bars3Icon v-if="!open" class="block h-6 w-6" aria-hidden="true"/>
                <XMarkIcon v-else class="block h-6 w-6" aria-hidden="true"/>
              </DisclosureButton>
            </div>
            <div class="hidden md:ml-6 md:flex md:items-center md:space-x-4">
              <a v-for="item in filteredNavigation" :key="item.name" :href="item.href"
                 :class="[item.current ? 'bg-gray-800 text-white' : 'transition text-gray-300 hover:bg-gray-700 hover:text-white', 'px-3 py-2 rounded-md text-sm font-medium']"
                 :aria-current="item.current ? 'page' : undefined">{{ item.name }}</a>
            </div>
          </div>
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <NuxtLink to="/login"
                        class="relative inline-flex transition items-center rounded-md border border-transparent bg-gray-800 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-gray-700 hover:border-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-offset-2 focus:ring-offset-gray-800"
                        v-if="!isLoggedIn">
                <span>Login</span>
                <ArrowLongRightIcon class="ml-2 h-5 w-5" aria-hidden="true"/>
              </NuxtLink>
            </div>
            <div class="flex-shrink-0">
              <nuxt-link to="/signup"
                         class="relative inline-flex transition items-center rounded-md border border-transparent bg-indigo-500 ml-5 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-indigo-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 focus:ring-offset-gray-800"
                         v-if="!isLoggedIn">
                <span>Sign up</span>
              </nuxt-link>
            </div>
            <div v-if="isLoggedIn" class="hidden md:ml-4 md:flex md:flex-shrink-0 md:items-center">
              <!-- Profile dropdown -->
              <Menu as="div" class="relative ml-3">
                <div>
                  <MenuButton
                      class="flex rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800">
                    <span class="sr-only">Open user menu</span>
                    <img class="h-8 w-8 rounded-full" :src="user.imageUrl" alt=""/>
                  </MenuButton>
                </div>
                <transition enter-active-class="transition ease-out duration-200"
                            enter-from-class="transform opacity-0 scale-95"
                            enter-to-class="transform opacity-100 scale-100"
                            leave-active-class="transition ease-in duration-75"
                            leave-from-class="transform opacity-100 scale-100"
                            leave-to-class="transform opacity-0 scale-95">
                  <MenuItems
                      class="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                    <MenuItem v-for="item in userNavigation" :key="item.name" v-slot="{ active }">
                      <a :href="item.href"
                         :class="[active ? 'bg-gray-100' : '', 'block px-4 py-2 text-sm text-gray-700']"
                         @click="handleLogout(item.name)">{{
                          item.name
                        }}</a>
                    </MenuItem>
                  </MenuItems>
                </transition>
              </Menu>
            </div>
          </div>
        </div>
      </div>
    </div>

    <DisclosurePanel class="md:hidden">
      <div class="space-y-1 px-2 pt-2 pb-3 sm:px-3">
        <DisclosureButton v-for="item in filteredNavigation" :key="item.name" as="a" :href="item.href"
                          :class="[item.current ? 'bg-gray-900 text-white' : 'text-gray-300 hover:bg-gray-700 hover:text-white', 'block px-3 py-2 rounded-md text-base font-medium']"
                          :aria-current="item.current ? 'page' : undefined">{{ item.name }}
        </DisclosureButton>
      </div>
      <div class="border-t border-gray-700 pt-4 pb-3" v-if="isLoggedIn">
        <div class="flex items-center px-5 sm:px-6">
          <div class="flex-shrink-0">
            <img class="h-10 w-10 rounded-full" :src="user.imageUrl" alt=""/>
          </div>
          <div class="ml-3">
            <div class="text-base font-medium text-white">{{ user.name }}</div>
            <div class="text-sm font-medium text-gray-400">{{ user.email }}</div>
          </div>
        </div>
        <div class="mt-3 space-y-1 px-2 sm:px-3">
          <DisclosureButton
              v-for="item in userNavigation"
              :key="item.name"
              class="block w-full rounded-md px-3 py-2 text-base font-medium text-gray-400 hover:bg-gray-700 hover:text-white"
              @click="handleLogout(item.name)">{{ item.name }}
          </DisclosureButton>
        </div>
      </div>
    </DisclosurePanel>
  </Disclosure>
</template>

<script setup>
import {Disclosure, DisclosureButton, DisclosurePanel, Menu, MenuButton, MenuItem, MenuItems} from '@headlessui/vue';
import {Bars3Icon, XMarkIcon} from '@heroicons/vue/24/outline';
import {ArrowLongRightIcon} from "@heroicons/vue/20/solid";
import {useNavigation} from '~/utils/navigation/useNavigation.js';
import {useAuth} from '~/composables/useAuth.js';
import {onMounted, ref} from "vue";

const {logout} = useAuth();
const {navigation} = useNavigation();

const isAdmin = ref(false);

const user = ref({
  imageUrl: 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png',
});

const userNavigation = computed(() => {
  return [
    {name: isAdmin.value ? 'Change permissions' : 'All my trips', href: isAdmin.value ? '/changePermissions' : '/myTrips'},
    {name: 'Sign out', href: '#'},
  ];
});

const isLoggedIn = ref(false);
const loading = ref(true);

onMounted(() => {
  if (typeof window !== "undefined") {
    const accessToken = localStorage.getItem('AccessToken');
    const role = localStorage.getItem('Role');
    isAdmin.value = role === "Admin";
    if (accessToken) {
      isLoggedIn.value = true;
    }
  }
  loading.value = false;
});

const handleLogout = async (name) => {
  if (name === "Sign out") {
    await logout();
    isLoggedIn.value = false;
    localStorage.removeItem("Role");
    window.location.href = "/";
  }
};

const filteredNavigation = computed(() => {
  return navigation.value.filter(item => !item.requiresAuth || isLoggedIn.value);
});
</script>
