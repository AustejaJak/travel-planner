<template>
  <ul role="list" class="flex flex-col items-center space-y-3">
    <li
        v-for="trip in trips"
        :key="trip.id"
        class="overflow-hidden rounded-md bg-white px-6 py-4 shadow w-8/12 flex transform transition-all duration-300 hover:scale-105 hover:shadow-lg cursor-pointer group"
    >
      <!-- Content section on the left -->
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

      <img :src="trip.url" alt="Trip Image" class="w-2/5 h-auto rounded-md transition-all duration-300 grayscale group-hover:grayscale-0"/>
    </li>
  </ul>
</template>


<script setup>
import {ref, onMounted} from "vue";
import {useTripResources} from "~/composables/useTripResources.js";
import {ChevronDownIcon} from "@heroicons/vue/20/solid";
import {formatDate} from "compatx";

const trips = ref([]);

const {getTrip} = useTripResources();

onMounted(async () => {
  try {
    const fetchedTrips = await getTrip();
    if (fetchedTrips && fetchedTrips.length) {
      trips.value = fetchedTrips;
    }
  } catch (err) {
    console.error('Error fetching trips:', err);
  }
});
</script>

<style scoped>
</style>
