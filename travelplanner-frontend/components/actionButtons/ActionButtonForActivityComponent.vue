<template>
  <Menu as="div" class="relative inline-block text-left">
    <div>
      <MenuButton class="flex items-center rounded-full bg-gray-100 text-gray-400 hover:text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 focus:ring-offset-gray-100">
        <span class="sr-only">Open options</span>
        <EllipsisVerticalIcon class="h-5 w-5" aria-hidden="true" />
      </MenuButton>
    </div>

    <transition enter-active-class="transition ease-out duration-100" enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100" leave-active-class="transition ease-in duration-75" leave-from-class="transform opacity-100 scale-100" leave-to-class="transform opacity-0 scale-95">
      <MenuItems class="absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
        <div class="py-1">
          <MenuItem v-slot="{ active }">
            <a href="/editActivity" :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700', 'block px-4 py-2 text-sm']">Edit activity</a>
          </MenuItem>
          <MenuItem v-slot="{ active }">
            <a href="#" @click="deleteActivityFunction" :class="[active ? 'bg-gray-100 text-gray-900' : 'text-gray-700', 'block px-4 py-2 text-sm']">Delete activity</a>
          </MenuItem>
        </div>
      </MenuItems>
    </transition>
  </Menu>
</template>

<script setup>
import { Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/vue'
import { EllipsisVerticalIcon } from '@heroicons/vue/20/solid'
import { useActivityResources } from "~/composables/useActivityResources.js";

const deleteActivityFunction = async () => {
  try {
    if (typeof window !== undefined) {
      const tripId = localStorage.getItem("TripId");
      const destinationId = localStorage.getItem("DestinationId");
      const activityId = localStorage.getItem("ActivityId");
      const {deleteActivity} = useActivityResources(tripId, destinationId);
      await deleteActivity(tripId, destinationId, activityId);
      window.location.reload();
    }
  } catch (err) {
    console.error("Error deleting trip:", err);
  }
};

</script>