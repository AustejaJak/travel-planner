<template>
  <Menu as="div" class="relative inline-block text-left">
    <div>
      <MenuButton class="inline-flex w-full justify-center gap-x-1.5 rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">
        Options
        <ChevronDownIcon class="-mr-1 size-5 text-gray-400" aria-hidden="true" />
      </MenuButton>
    </div>

    <transition enter-active-class="transition ease-out duration-100" enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100" leave-active-class="transition ease-in duration-75" leave-from-class="transform opacity-100 scale-100" leave-to-class="transform opacity-0 scale-95">
      <MenuItems class="absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 focus:outline-none">
        <div class="py-1">
          <MenuItem v-slot="{ active }">
            <a href="#" :class="[active ? 'bg-gray-100 text-gray-900 outline-none' : 'text-gray-700', 'block px-4 py-2 text-sm']" @click="changeToAdmin">Change to admin</a>
          </MenuItem>
          <MenuItem v-slot="{ active }">
            <a href="#" :class="[active ? 'bg-gray-100 text-gray-900 outline-none' : 'text-gray-700', 'block px-4 py-2 text-sm']" @click="removeAllRoles">Remove all roles</a>
          </MenuItem>
          <MenuItem v-slot="{ active }">
            <a href="#" :class="[active ? 'bg-gray-100 text-gray-900 outline-none' : 'text-gray-700', 'block px-4 py-2 text-sm']" @click="deleteUsers">Delete user</a>
          </MenuItem>
        </div>
      </MenuItems>
    </transition>
  </Menu>
</template>

<script setup>
import { defineProps } from 'vue';
import { useUserResources } from '~/composables/useUserResources';
import { Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/vue';
import { ChevronDownIcon } from '@heroicons/vue/20/solid';

const props = defineProps({
  userId: {
    type: String,
    required: true
  }
});

const { removeUserRoles, assignAdminRole, deleteUser } = useUserResources()

const changeToAdmin = async () => {
  try {
    const result = await assignAdminRole(props.userId)
    alert(result.message);
    window.location.reload();
  } catch (error) {
    console.error('Error assigning admin role:', error)
    alert('Failed to assign admin role.')
  }
}

const removeAllRoles = async () => {
  try {
    const result = await removeUserRoles(props.userId)
    alert(result.message);
    window.location.reload();
  } catch (error) {
    console.error('Error removing roles:', error)
    alert('Failed to remove user roles.')
  }
}

const deleteUsers = async () => {
  try {
    const result = await deleteUser(props.userId)
    alert(result.message)
    window.location.reload();
  } catch (error) {
    console.error('Error deleting user:', error)
    alert('Failed to delete user.')
  }
}
</script>
