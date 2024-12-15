<template>
  <div class="relative md:grid md:grid-cols-3 md:gap-6">
    <div class="md:col-span-1">
      <div class="px-4 sm:px-0">
        <h3 class="text-lg font-medium leading-6 text-gray-900">Add activity</h3>
        <p class="mt-1 text-sm text-gray-600">The "Create an activity" function allows users to add new trip details, such as destinations, dates, and activities, to the platform.</p>
      </div>
    </div>
    <div class="mt-5 md:col-span-2 md:mt-0">
      <form @submit.prevent="submitActivity">
        <div class="shadow sm:overflow-hidden sm:rounded-md">
          <div class="space-y-6 bg-white px-4 py-5 sm:p-6">
            <div>
              <div class="col-span-6 sm:col-span-3">
                <label for="trip-name" class="block text-sm font-medium text-gray-700">Activity name</label>
                <input
                    required
                    v-model="Activity.name"
                    type="text"
                    name="trip-name"
                    id="trip-name"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="description" class="block text-sm font-medium text-gray-700">Type</label>
                <input
                    required
                    v-model="Activity.type"
                    type="text"
                    name="description"
                    id="description"
                    autocomplete="given-name"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="start-time" class="block text-sm font-medium text-gray-700">Start time</label>
                <input
                    required
                    v-model="Activity.startTime"
                    type="time"
                    name="start-time"
                    id="start-time"
                    min="00:00"
                    max="23:59"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
              </div>
              <div class="col-span-6 sm:col-span-3 mt-5">
                <label for="end-time" class="block text-sm font-medium text-gray-700">End time</label>
                <input
                    required
                    v-model="Activity.endTime"
                    type="time"
                    name="end-time"
                    id="end-time"
                    min="00:00"
                    max="23:59"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                />
                <p v-if="errors.startDate" class="text-sm text-red-500 mt-1">{{ errors.startDate }}</p>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 text-right sm:px-6">
            <button
                :disabled="hasErrors"
                type="submit"
                class="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50"
            >
              Add Activity
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, computed } from "vue";
import { useActivityResources } from "~/composables/useActivityResources";

const { putActivity } = useActivityResources();

const Activity = ref({
  name: '',
  type: '',
  startTime: '',
  endTime: '',
});

const errors = ref({
  startDate: '',
  endDate: ''
});

const hasErrors = computed(() => {
  return !!errors.value.startDate || !!errors.value.endDate;
});

const formatTime = (time: string) => {
  if (!time) return "00:00:00";
  const [hours, minutes] = time.split(":");
  return `${hours}:${minutes}:00`;
};

const submitActivity = async () => {
  if (hasErrors.value) {
    console.error("Fix the validation errors before submitting.");
    return;
  }

  Activity.value.startTime = formatTime(Activity.value.startTime);
  Activity.value.endTime = formatTime(Activity.value.endTime);

  try {
    if (typeof window !== "undefined") {
      const tripId = localStorage.getItem("TripId");
      const destinationId = localStorage.getItem("DestinationId");
      const activityId = localStorage.getItem("ActivityId");

      const result = await putActivity(Activity.value, tripId, destinationId, activityId);
      if (result) {
        window.location.href = '/myTrips';
      }
    }
  } catch (error) {
    console.error('Error submitting the trip:', error);
  }
};
</script>

