<template>
  <div class="relative md:grid md:grid-cols-3 md:gap-6">
    <div class="md:col-span-1">
      <div class="px-4 sm:px-0">
        <h3 class="text-lg font-medium leading-6 text-gray-900">Edit a Destination</h3>
        <p class="mt-1 text-sm text-gray-600">The "Create a destination" function allows users to add new trip details, such as destinations, dates, and activities, to the platform.</p>
      </div>
    </div>
    <div class="mt-5 md:col-span-2 md:mt-0">
      <form @submit.prevent="updateDestination">
        <div class="shadow sm:overflow-hidden sm:rounded-md">
          <div class="space-y-6 bg-white px-4 py-5 sm:p-6">
            <div>
              <div class="col-span-6 sm:col-span-3">
                <label for="trip-name" class="block text-sm font-medium text-gray-700">Start location</label>
                <input
                    required
                    v-model="destination.startlocation"
                    type="text"
                    name="start-location"
                    id="start-location"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="end-location" class="block text-sm font-medium text-gray-700">End location</label>
                <input
                    required
                    v-model="destination.endlocation"
                    type="text"
                    name="end-location"
                    id="end-location"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 text-right sm:px-6">
            <button
                type="submit"
                class="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50"
            >
              Edit destination
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import {useDestinationResources} from "~/composables/useDestinationResources";

const destination = ref({
  startlocation: '',
  endlocation: '',
});

const isAdmin = ref(false);

const updateDestination = async () => {

  try {
    if (typeof window !== "undefined") {
      const tripId = localStorage.getItem("TripId");
      const DestinationId = localStorage.getItem("DestinationId");
      const role = localStorage.getItem("Role");
      const { putDestination } = useDestinationResources(tripId);

      if (role === "Admin"){
        isAdmin.value = true;
      }

      const result = await putDestination(destination.value, DestinationId);
      if (result && !isAdmin) {
        window.location.href = '/myTrips';
      } else if (result && isAdmin) {
        window.location.href = '/adminTrips'
      }
    }
  } catch (error) {
    console.error('Error submitting the trip:', error);
  }
};
</script>
