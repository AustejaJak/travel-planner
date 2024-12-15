<template>
  <div class="relative md:grid md:grid-cols-3 md:gap-6">
    <div class="md:col-span-1">
      <div class="px-4 sm:px-0">
        <h3 class="text-lg font-medium leading-6 text-gray-900">Update a trip</h3>
        <p class="mt-1 text-sm text-gray-600">The "Update a Trip" function allows users to add new trip details, such as destinations, dates, and activities, to the platform.</p>
      </div>
    </div>
    <div class="mt-5 md:col-span-2 md:mt-0">
      <form @submit.prevent="updateTrip">
        <div class="shadow sm:overflow-hidden sm:rounded-md">
          <div class="space-y-6 bg-white px-4 py-5 sm:p-6">
            <div>
              <div class="col-span-6 sm:col-span-3">
                <label for="trip-name" class="block text-sm font-medium text-gray-700">Trip name</label>
                <input
                    required
                    v-model="trip.name"
                    type="text"
                    name="trip-name"
                    id="trip-name"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="description" class="block text-sm font-medium text-gray-700">Description</label>
                <input
                    required
                    v-model="trip.description"
                    type="text"
                    name="description"
                    id="description"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="photo-url" class="block text-sm font-medium text-gray-700">Photo URL</label>
                <input
                    required
                    v-model="trip.url"
                    type="text"
                    name="photo-url"
                    id="photo-url"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="trip-start-date" class="block text-sm font-medium text-gray-700">Trip start date</label>
                <input
                    required
                    v-model="trip.tripstart"
                    type="date"
                    name="trip-start-date"
                    id="trip-start-date"
                    :min="tomorrow"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
                <p v-if="errors.startDate" class="text-sm text-red-500 mt-1">{{ errors.startDate }}</p>
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="trip-end-date" class="block text-sm font-medium text-gray-700">Trip end date</label>
                <input
                    required
                    v-model="trip.tripend"
                    type="date"
                    name="trip-end-date"
                    id="trip-end-date"
                    :min="trip.tripstart || tomorrow"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
                <p v-if="errors.endDate" class="text-sm text-red-500 mt-1">{{ errors.endDate }}</p>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 text-right sm:px-6">
            <button
                :disabled="hasErrors"
                type="submit"
                class="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50"
            >
              Update a trip
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from "vue";
import { useTripResources } from "~/composables/useTripResources.js";

const { putTrip } = useTripResources();

const isAdmin = ref(false);

const trip = ref({
  name: '',
  description: '',
  url: '',
  tripstart: '',
  tripend: ''
});

const errors = ref({
  startDate: '',
  endDate: ''
});

const tomorrow = computed(() => {
  const date = new Date();
  date.setDate(date.getDate() + 1);
  return date.toISOString().split('T')[0];
});

const setStartAndEndTimeToNoon = (date: string) => {
  const currentDate = new Date(date);
  const today = new Date();
  if (
      currentDate.getFullYear() === today.getFullYear() &&
      currentDate.getMonth() === today.getMonth() &&
      currentDate.getDate() === today.getDate()
  ) {

    return currentDate.toISOString().split('T')[0];
  }
  return date;
};

const validateDates = () => {
  errors.value.startDate = '';
  errors.value.endDate = '';

  if (trip.value.tripstart && trip.value.tripstart < tomorrow.value) {
    errors.value.startDate = "The start date must be after today.";
  }

  if (trip.value.tripstart && trip.value.tripend && trip.value.tripend <= trip.value.tripstart) {
    errors.value.endDate = "The end date must be after the start date.";
  }
};

watch(() => trip.value.tripstart, (newStartDate) => {
  trip.value.tripstart = setStartAndEndTimeToNoon(newStartDate);
});

watch(() => trip.value.tripend, (newEndDate) => {
  trip.value.tripend = setStartAndEndTimeToNoon(newEndDate);
});

const hasErrors = computed(() => {
  return !!errors.value.startDate || !!errors.value.endDate;
});

const updateTrip = async () => {
  validateDates();

  if (hasErrors.value) {
    console.error("Fix the validation errors before submitting.");
    return;
  }

  try {
    if (typeof window !== "undefined"){
      const tripId = localStorage.getItem("TripId");
      const result = await putTrip(trip.value, tripId);
      const role = localStorage.getItem("Role");

      if (role === "Admin"){
        isAdmin.value = true;
      }

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
