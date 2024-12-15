<template>
  <Menu as="div" class="relative inline-block text-left">
    <div>
      <MenuButton class="flex items-center rounded-full bg-gray-100 text-gray-400 hover:text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 focus:ring-offset-gray-100">
        <span class="sr-only">Open options</span>
        <EllipsisVerticalIcon class="h-5 w-5" aria-hidden="true" />
      </MenuButton>
    </div>

    <transition enter-active-class="transition ease-out duration-100" enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100" leave-active-class="transition ease-in duration-75" leave-from-class="transform opacity-100 scale-100" leave-to-class="transform opacity-0 scale-95">
      <MenuItems class="absolute right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
        <div class="py-1">
          <MenuItem v-if="!isAdmin" v-slot="{ active }">
            <a href="/createDestinations" :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700', 'block px-4 py-2 text-sm']">Add destination</a>
          </MenuItem>
          <MenuItem v-slot="{ active }">
            <a href="/editTrips" :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700', 'block px-4 py-2 text-sm']">Edit trip</a>
          </MenuItem>
          <MenuItem v-slot="{ active }">
            <a href="#" @click="deleteTripFunction" :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700', 'block px-4 py-2 text-sm']">Delete trip</a>
          </MenuItem>
        </div>
      </MenuItems>
    </transition>
  </Menu>
</template>

<script setup>
import { Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/vue'
import { EllipsisVerticalIcon } from '@heroicons/vue/20/solid'
import { useTripResources} from "~/composables/useTripResources.js";
import {onMounted} from "vue";

const {deleteTrip} = useTripResources();

const isAdmin = ref(false);

const getUserRole = () => {
  if (typeof window !== "undefined") {
    const role = localStorage.getItem('Role');
    if (role === "Admin") {
      isAdmin.value = true;
    }
  }
};

onMounted(() => {
  getUserRole();
});

const deleteTripFunction = async () => {
  try {
    if (typeof window !== undefined) {
      const tripId = localStorage.getItem("TripId");
      await deleteTrip(tripId);
      window.location.reload();
    }
  } catch (err) {
    console.error("Error deleting trip:", err);
  }
};

</script>