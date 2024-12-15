<template>
  <ul role="list" class="flex flex-col items-center space-y-8">
    <li
        v-for="trip in trips"
        :key="trip.id"
        class="overflow-hidden rounded-md bg-white px-6 py-4 shadow w-8/12 flex flex-col transform transition-all duration-300 hover:scale-105 hover:shadow-lg cursor-pointer group"
        @click="selectTrip(trip.id)"
    >
      <div class="flex justify-between w-full">
        <div class="flex-1 pr-4 flex flex-col justify-between">
          <div>
            <h2 class="text-lg font-bold">{{ trip.name }}</h2>
            <p class="text-sm text-gray-600 pt-3">{{ trip.description }}</p>
            <div class="flex text-sm text-gray-500 pt-3 space-x-4">
              <p><span class="font-bold">Trip Start:</span> {{ formatDate(trip.tripStart) }}</p>
              <p><span class="font-bold">Trip End:</span> {{ formatDate(trip.tripEnd) }}</p>
            </div>
            <p class="text-sm text-gray-400 pt-5">
              Created on: {{ formatDate(trip.creationDate) }}
            </p>
          </div>

          <div class="flex justify-center pt-4">
            <ChevronDownIcon class="w-6 h-6 transition-all duration-300 text-gray-400 group-hover:text-gray-700" />
          </div>
        </div>
        <img
            :src="trip.url"
            alt="Trip Image"
            class="w-2/5 h-52 rounded-md transition-all duration-300 group-hover:grayscale-0"
            :class="{'grayscale': selectedTripId !== trip.id}"
        />

        <div class="ml-4 flex items-start">
          <ActionButtonComponent @click.stop="storeTripIdInLocalStorage(trip.id)" />
        </div>
      </div>

      <div v-if="selectedTripId === trip.id" class="mt-4 pb-24 bg-gray-50 p-4 rounded-md">
        <ul class="list-disc">
          <li v-for="destination in destinations" :key="destination.id" class="flex flex-col space-y-2">

            <div class="flex justify-between items-center w-full">
              <button
                  class="flex items-center cursor-pointer font-semibold text-gray-700 w-full transition-all duration-300 mt-3 p-2 border border-gray-300 hover:border-gray-400 rounded"
                  @click.stop="selectDestination(trip.id, destination.id)"
              >
                <span class="flex-1">{{ destination.startLocation }} - {{ destination.endLocation }}</span>
                <ChevronDownIcon class="w-6 h-6 text-gray-700 transition-all duration-300" />
              </button>

              <div class="ml-4">
                <ActionButtonForDestination @click.stop="storeDestinationIdInLocalStorage(destination.id)" />
              </div>
            </div>

            <div v-if="selectedDestinationId === destination.id" class="ml-4">
              <ul class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-3">
                <li v-for="activity in activities" :key="activity.id" class="bg-white p-4 rounded-md shadow-sm hover:shadow-lg">
                  <div class="flex flex-col space-y-2">
                    <div class="text-md font-semibold text-gray-800">{{ activity.name }}</div>
                    <div class="text-sm text-gray-500">
                      <p>Type: {{ activity.type }}</p>
                      <p>Start Time: <strong>{{ activity.startTime }}</strong></p>
                      <p>End Time: <strong>{{ activity.endTime }}</strong></p>
                    </div>
                  </div>
                  <div class="flex justify-end">
                    <ActionButtonForActivityComponent @click.stop="storeActivityIdInLocalStorage(activity.id)"/>
                  </div>
                </li>
              </ul>
            </div>
          </li>
        </ul>
      </div>
    </li>
  </ul>
</template>

<script setup>
import { onMounted, ref } from "vue";
import { useTripResources } from "~/composables/useTripResources.js";
import { ChevronDownIcon } from "@heroicons/vue/20/solid";
import { formatDate } from "compatx";
import { useDestinationResources } from "~/composables/useDestinationResources.js";
import { useActivityResources } from "~/composables/useActivityResources.js";
import { jwtDecode } from "jwt-decode";
import ActionButtonComponent from "~/components/actionButtons/ActionButtonComponent.vue";
import ActionButtonForDestination from "~/components/actionButtons/ActionButtonForDestination.vue";
import ActionButtonForActivityComponent from "~/components/actionButtons/ActionButtonForActivityComponent.vue";

const trips = ref([]);
const destinations = ref([]);
const activities = ref([]);

const selectedTripId = ref(null);
const selectedDestinationId = ref(null);

const { getTripByUserId } = useTripResources();

onMounted(async () => {
  try {
    if (typeof window !== "undefined") {
      const token = localStorage.getItem("AccessToken");
      const decodedToken = jwtDecode(token);
      const fetchedTrips = await getTripByUserId(decodedToken.sub);
      if (fetchedTrips && fetchedTrips.length) {
        trips.value = fetchedTrips;
      }
    }
  } catch (err) {
    console.error("Error fetching trips:", err);
  }
});

const selectTrip = async (tripId) => {
  try {
    if (selectedTripId.value === tripId) {
      selectedTripId.value = null;
      destinations.value = [];
      activities.value = [];
      return;
    }

    if (typeof window !== undefined){
      localStorage.setItem("TripId", tripId);
    }

    selectedTripId.value = tripId;
    selectedDestinationId.value = null;
    activities.value = [];

    const { getDestination } = useDestinationResources(tripId);
    destinations.value = await getDestination();
  } catch (err) {
    console.error("Error fetching destinations:", err);
  }
};

const selectDestination = async (tripId, destinationId) => {
  try {
    if (selectedTripId.value === tripId && selectedDestinationId.value === destinationId) {
      selectedTripId.value = null;
      selectedDestinationId.value = null;
      activities.value = [];
      return;
    }

    selectedTripId.value = tripId;
    selectedDestinationId.value = destinationId;

    if (typeof window !== undefined){
      localStorage.setItem("DestinationId", destinationId);
    }

    const { getActivity } = useActivityResources(tripId, destinationId);
    activities.value = await getActivity();
  } catch (err) {
    console.error("Error fetching activities:", err);
  }
};

const storeTripIdInLocalStorage = (tripId) => {
  try {
    if (typeof window !== undefined){
      localStorage.setItem("TripId", tripId);
    }
  } catch (err) {
    console.error("Error saving TripId to localStorage:", err);
  }
};

const storeDestinationIdInLocalStorage = (destinationId) => {
  try {
    if (typeof window !== undefined){
      localStorage.setItem("DestinationId", destinationId);
    }
  } catch (err) {
    console.error("Error saving TripId to localStorage:", err);
  }
};

const storeActivityIdInLocalStorage = (activityId) => {
  try {
    if (typeof window !== undefined){
      localStorage.setItem("ActivityId", activityId);
    }
  } catch (err) {
    console.error("Error saving TripId to localStorage:", err);
  }
};
</script>

<style scoped>
</style>
